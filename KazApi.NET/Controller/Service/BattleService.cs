using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain.monster;
using KazApi.DTO;
using KazApi.Repository;
using KazApi.Repository.sql;
using System.Transactions;


namespace KazApi.Controller.Service
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
            => _posgre.Select<MonsterDTO>(MonsterSQL.SelectMonsters());

        /// <summary>
        /// スキルーデータの読込み
        /// </summary>
        public IEnumerable<SkillDTO> SelectSkills()
            => _posgre.Select<SkillDTO>(SkillSQL.SelectSkill());

        /// <summary>
        /// スキルマップデータの読込み
        /// </summary>
        public IEnumerable<MonsterSkillDTO> SelectMonsterSkills()
            => _posgre.Select<MonsterSkillDTO>(MonsterSQL.SelectMonsterSkill());

        public IEnumerable<CodeDTO> SelectStateCode()
            => _posgre.Select<CodeDTO>(CodeSQL.SelectCode())
                       .Where(e => e.Category == CCodeType.STATE.VALUE);

        /// <summary>
        /// 勝敗結果を記録
        /// </summary>
        public bool InsertBattleResult(
            IEnumerable<MonsterDTO> monsters, DateTime endDate, TimeSpan endTime)
        {
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

                        string sql = BattleSQL.InsertBattleResult();
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
    }
}
