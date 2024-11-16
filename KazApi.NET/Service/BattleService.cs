using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain._Factory;
using KazApi.Domain._Monster;
using KazApi.Domain._Monster._Skill;
using KazApi.Domain._Monster._State;
using KazApi.DTO;
using KazApi.Lib;
using KazApi.Repository;
using System.Transactions;


namespace KazApi.Service
{
    public class BattleService
    {
        private readonly ILog<BattleMetaData> _log;
        private readonly IDatabase _posgre;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleService() 
        {
            _posgre = new PostgreSQL();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration"></param>
        public BattleService(IConfiguration configuration)
        {   
            _log = new BattleLogger();                 
            _posgre = new PostgreSQL(configuration!);
        }

        /// <summary>
        /// モンスターデータの読込み
        /// </summary>
        public IEnumerable<MonsterDTO> SelectMonsters()
            => _posgre.Select<MonsterDTO>(SQL.MonsterSQL.SelectMonsters());

        /// <summary>
        /// スキルーデータの読込み
        /// </summary>
        public IEnumerable<SkillDTO> SelectSkills()
            => _posgre.Select<SkillDTO>(SQL.SkillSQL.SelectSkill());

        /// <summary>
        /// スキルマップデータの読込み
        /// </summary>
        public IEnumerable<MonsterSkillDTO> SelectMonsterSkills()
            => _posgre.Select<MonsterSkillDTO>(SQL.MonsterSkillSQL.SelectMonsterSkill());

        /// <summary>
        /// モンスターとスキルのマッピング（DTO）
        /// </summary>
        public IEnumerable<MonsterDTO> MappingToMonsterDTO(
            IEnumerable<MonsterDTO> monstersDTO,
            IEnumerable<SkillDTO> skillsDTO,
            IEnumerable<MonsterSkillDTO> monsterSkillsDTO
            )
        {
            IList<MonsterDTO> result = [];

            foreach (MonsterDTO m in monstersDTO)
            {
                // モンスターのスキル対応表を取得
                IEnumerable<MonsterSkillDTO> skillMap =
                    monsterSkillsDTO.Where(e => e.MonsterId == m.MonsterId);

                IList<SkillDTO> bindSkills = [];

                foreach (MonsterSkillDTO ms in skillMap)
                {
                    // 対応表を元にスキルを設定
                    SkillDTO skill = skillsDTO.Where(e => e.SkillId == ms.SkillId).Single();
                    bindSkills.Add(skill);
                }
                m.Skills = bindSkills;
                m.Status = [];
                result.Add(m);
            }
            return result;
        }

        /// <summary>
        /// モンスターをランダムに選出する
        /// </summary>
        public IEnumerable<T> MonsterSelector<T>(IEnumerable<T> monsters, int needAmount)
        {
            if (monsters.Count() < needAmount) throw new Exception("モンスターの選択数が多すぎます。");
            if (monsters.Count() < 2) throw new Exception("バトルは２体以上必要です。");

            IList<T> result = [];
            IList<int> usedMonsterId = [];

            // 必要数のモンスタを用意
            for (int i = 0; i < needAmount; i++)
            {
                int monsterId = URandom.RandomInt(0, monsters.Count());

                // 同じモンスターは選べない
                while (usedMonsterId.Contains(monsterId))
                    // ランダムに選出
                    monsterId = URandom.RandomInt(0, monsters.Count());

                usedMonsterId.Add(monsterId);

                T monster = monsters.ElementAt(monsterId);
                result.Add(monster);
            }
            return result;
        }

        /// <summary>
        /// 戦闘賞モンスターオブジェクトを構築
        /// </summary>
        public IEnumerable<IMonster> CreateBattleMonsters(IEnumerable<MonsterDTO> monsters)
        {
            IEnumerable<CodeDTO> stateCodeFromDB =
                _posgre.Select<CodeDTO>(SQL.CodeSQL.SelectCode())
                       .Where(e => e.Category == CCodeType.STATE.VALUE);

            SkillFactory skillFactory = new SkillFactory(stateCodeFromDB);
            StateFactory stateFactory = new StateFactory(stateCodeFromDB);

            // バトルモンスター構築
            IList<IMonster> battleMonsters = [];
            foreach (MonsterDTO m in monsters)
            {
                IEnumerable<ISkill> skills = skillFactory.Create(m.Skills);

                ISet<IState> status = new HashSet<IState>();
                foreach (StateDTO state in m.Status)
                {
                    // 同じ状態は追加しない
                    status.Add(stateFactory.Create(state.StateType, state));
                }
                // スキル、ステータスを設定
                IMonster battleMonster = new Monster(m, skills, status);
                battleMonsters.Add(battleMonster);
            }
            return battleMonsters;
        }

        /// <summary>
        /// 行動順を決定する
        /// </summary>
        public IEnumerable<IMonster> ActionOrder(IEnumerable<IMonster> monsters)
        {
            // スピードを乱数調整した上で順番決め
            IList<IMonster> result =
                monsters.Where(e => e.Hp > 0)
                        .OrderByDescending(
                            e => URandom.RandomChangeInt(e.Speed, 0.5))
                        .ToList();

            return result;
        }

        /// <summary>
        /// 誰のターンか表示
        /// </summary>
        public void WhoseTurn(IMonster monster)
        {
            _log.Logging(new BattleMetaData($"\n============================================"));
            _log.Logging(new BattleMetaData($">>> {monster.MonsterName}のターン"));
            _log.Logging(new BattleMetaData($"============================================\n"));
        }

        /// <summary>
        /// 状態異常解除
        /// </summary>
        /// <param name="me"></param>
        public void DisabledStatus(IMonster me)
        {
            IEnumerable<IState> currentStatus = me.CurrentStatus();
            ISet<IState> changedStatus = new HashSet<IState>();
            foreach (IState state in currentStatus)
            {
                if (!state.IsDisable())
                    changedStatus.Add(state);
                else
                    state.DisabledLogging(me);
            }
            me.UpdateStatus(changedStatus);
            _log.Logging(new BattleMetaData());
        }

        /// <summary>
        /// 戦闘結果表示
        /// </summary>
        public void BattleResult(IEnumerable<IMonster> monsters)
        {
            bool existWinner = false;
            bool allLoser = false;

            IEnumerable<IMonster> alives = monsters.Where(e => e.Hp > 0);
            IMonster? alive = alives.Count() == 1 ? alives.Single() : null;

            if (alives.Count() == 1)
            {
                existWinner = true;

                _log.Logging(new BattleMetaData($"\n*************************************************"));
                _log.Logging(new BattleMetaData($"*************************************************"));
                _log.Logging(new BattleMetaData($"  Winner {alives.Single().MonsterName} !!"));
                _log.Logging(new BattleMetaData($"*************************************************"));
                _log.Logging(new BattleMetaData($"*************************************************\n"));
                _log.Logging(new BattleMetaData(existWinner, allLoser, alive));
            }
            else if (alives.Count() <= 0)
            {
                allLoser = true;
                _log.Logging(new BattleMetaData($"... 勝者なし。"));
                _log.Logging(new BattleMetaData(existWinner, allLoser, alive));
            }
        }

        /// <summary>
        /// モデルからDTOへ変換
        /// </summary>
        public IEnumerable<MonsterDTO> ConvertToDTO(IEnumerable<IMonster> battleMonsters)
        {
            IList<MonsterDTO> monstersDTO = [];
            foreach (IMonster m in battleMonsters)
            {
                IEnumerable<SkillDTO> skillsDTO = SkillDTO.ModelToDTO(m.CurrentSkills());
                IEnumerable<StateDTO> statusDTO = StateDTO.ModelToDTO(m.CurrentStatus());

                MonsterDTO monsterDTO = new MonsterDTO(m);
                monsterDTO.Skills = skillsDTO;
                monsterDTO.Status = statusDTO;
                monsterDTO.StatusName = CStateName.ConvertTypeToName(statusDTO);

                monstersDTO.Add(monsterDTO);
            }
            return monstersDTO;
        }

        /// <summary>
        /// 勝敗結果を記録
        /// </summary>
        /// <param name="monsters"></param>
        public bool InsertBattleResult(
            IEnumerable<MonsterDTO> monsters,
            DateTime endDate,
            TimeSpan endTime
        ) {
            try
            {
                using (var transaciton = new TransactionScope())
                {
                    int seq = 1;
                    foreach (MonsterDTO m in monsters)
                    {
                        object parameters = new
                        {
                            battle_end_date = endDate,
                            battle_end_time = endTime,
                            serial = seq,
                            monster_id = m.MonsterId,
                            is_win = m.Hp > 0 ? true : false
                        };

                        string sql = SQL.BattleResultSQL.InsertBattleResult();
                        _posgre.Execute(sql, parameters);

                        seq++;
                    }
                    transaciton.Complete();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 賭け金レートを算出
        /// </summary>
        /// <param name="monsters"></param>
        public void CalcBetRate(IEnumerable<MonsterDTO> monsters)
        {
            int monsterCount = monsters.Count() - 1; // モンスター数が多いほど倍率UP
            double maxScore = monsters.Max(e => e.BetScore);

            foreach (MonsterDTO monster in monsters)
            {
                if (maxScore == monster.BetScore)
                {
                    monster.BetRate = double.Parse(
                        (maxScore / monster.BetScore * monsterCount * 1.15).ToString("F2")
                        );
                }
                else
                {
                    monster.BetRate = double.Parse(
                        (maxScore / monster.BetScore * monsterCount).ToString("F2")
                        );
                }
            }

        }
    }
}
