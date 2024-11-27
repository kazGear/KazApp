using KazApi.Common._Const;
using KazApi.DTO;
using KazApi.Repository;
using KazApi.Repository.sql;

namespace KazApi.Controller.Service
{
    public class BattleReportService
    {
        private readonly IDatabase _posgre;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleReportService(IConfiguration configuration)
        {
            _posgre = new PostgreSQL(configuration);
        }

        /// <summary>
        /// モンスター種族を取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MonsterTypeDTO> SelectMonsterTypes()
        {
            try
            {
                object parameter = new
                {
                    category = CCodeType.MONSTER.VALUE
                };
                string sql = ReportSQL.SelectMonsterTypes();

                return _posgre.Select<MonsterTypeDTO>(sql, parameter); ;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// モンスター毎のレポートを取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MonsterReportDTO> SelectMonsterReport(string monsterTypeId)
        {
            try
            {
                var param = new
                {
                    monster_type = int.Parse(monsterTypeId)
                };
                IEnumerable<MonsterReportDTO> report
                    = _posgre.Select<MonsterReportDTO>(
                        ReportSQL.SelectMonsterReport(param.monster_type), param);
                                
                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<BattleReportDTO> SelectBattleReport(
            string battleScale, DateTime? dateFrom, DateTime? dateTo
        )
        {
            try
            {
                var param = new
                {
                    battle_scale = int.Parse(battleScale),
                    from = dateFrom,
                    to = dateTo
                };

                IEnumerable<BattleReportDTO> report
                    = _posgre.Select<BattleReportDTO>(
                        ReportSQL.SelectBattleReport(
                            param.battle_scale,
                            param.from,
                            param.to
                            ), param);
                           
                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
