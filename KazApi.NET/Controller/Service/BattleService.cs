using KazApi.Common._Log;
using KazApi.Domain._Const;
using KazApi.Domain._Monster;
using KazApi.Domain.DTO;
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
        /// モンスターデータ取得
        /// </summary>
        public IEnumerable<MonsterDTO> SelectMonsters(string? loginId = null)
        {
            var param = new　{ login_id = loginId };
            return _posgre.Select<MonsterDTO>(MonsterSQL.SelectMonsters(), param);
        }

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
                       .Where(e => e.CodeId == CCodeType.STATE.VALUE);

        /// <summary>
        /// 勝敗結果を記録（モンスター）
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

        /// <summary>
        /// 勝敗結果を記録（ユーザー）
        /// </summary>
        public bool UpdateUserResults(bool hit, int betGil, decimal betRate, string loginId)
        {
            try
            {
                var param = new
                {
                    login_id         = loginId,
                    wins             = hit ? 1 : 0,
                    losses           = hit ? 0 : 1,
                    cash             = hit ? Math.Floor(betGil * betRate) : (-1 * betGil),
                    wins_get_cash    = hit ? Math.Floor(betGil * betRate) : 0,
                    losses_lost_cash = hit ? 0 : betGil,
                };
                _posgre.Execute(BattleSQL.InsertUserResult(), param);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
