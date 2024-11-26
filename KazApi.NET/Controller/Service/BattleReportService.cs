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
        public IEnumerable<MonsterRepostDTO> SelectMonsterReport(string monsterTypeId)
        {
            try
            {
                var param = new
                {
                    monster_type = int.Parse(monsterTypeId)
                };

                IEnumerable<MonsterRepostDTO> report
                    = _posgre.Select<MonsterRepostDTO>(
                        ReportSQL.SelectMonsterReport(param.monster_type), param);

                // 勝率を算出
                IEnumerable<MonsterRepostDTO> editedReport
                    = report.Select(e => new MonsterRepostDTO
                    {
                        MonsterId = e.MonsterId,
                        MonsterName = e.MonsterName,
                        BattleCount = e.BattleCount,
                        Wins = e.Wins,
                        WinRate = (e.Wins / (double)e.BattleCount * 100).ToString("N2") + "%"
                    });

                return editedReport;
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
                        ReportSQL.SelectBattleReport(param.battle_scale,
                                                         param.from,
                                                         param.to
                                                         ), param);

                IEnumerable<BattleReportDTO> editedReport
                    = report.Select(e => new BattleReportDTO
                    {
                        BattleId = e.BattleId,
                        BattleEndDateStr = e.BattleEndDate.ToString().Substring(0, 10),
                        BattleEndTimeStr = e.BattleEndTime.ToString().Substring(0, 8),
                        Serial = e.Serial,
                        MonsterId = e.MonsterId,
                        MonsterName = e.MonsterName,
                        IsWin = e.IsWin
                    });

                return editedReport;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
