using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KazApi.DTO;
using KazApi.Controller.Service;
using KazApi.Common._Filter;
using KazApi.Domain._GameSystem;

namespace KazApi.Controller
{
    [SkipAuthActionFilter]
    [ApiController]
    public class BattleReportController : ControllerBase
    {
        private readonly BattleReportService _service;

        public BattleReportController(IConfiguration configuration)
        {
            _service = new BattleReportService(configuration);
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        [HttpPost("api/battleReport/init")]
        public ActionResult<string> Init()
        {
            try
            {
                IEnumerable<MonsterTypeDTO> monsterTypes = _service.SelectMonsterTypes();
                return JsonConvert.SerializeObject(monsterTypes);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// モンスターのレポートを取得
        /// </summary>
        [HttpPost("api/battleReport/monsterReport")]
        public ActionResult<string> SelectMonsterReport(
            [FromQuery] int monsterTypeId,
            [FromQuery] int sortType,
            [FromQuery] bool isAscOrder)
        {
            try
            {
                IEnumerable<MonsterReportDTO> report 
                    = _service.SelectMonsterReport(monsterTypeId, sortType, isAscOrder);

                // 勝率を算出
                IEnumerable<MonsterReportDTO> editedReport
                    = BattleSystem.ResultsOfMonster(report);

                return JsonConvert.SerializeObject(editedReport);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 戦闘のレポートを取得
        /// </summary>
        [HttpPost("api/battleReport/battleReport")]
        public ActionResult<string> SelectBattleReport(
            [FromQuery] int battleScale,
            [FromQuery] string? from,
            [FromQuery] string? to)
        {
            try
            {
                DateTime? dateFrom = from == null ? null : DateTime.Parse(from);
                DateTime? dateTo = to == null ? null : DateTime.Parse(to);

                IEnumerable<BattleReportDTO> battleReports
                    = _service.SelectBattleReport(battleScale, dateFrom, dateTo);
                
                IEnumerable<BattleReportDTO> editedReport = BattleSystem.ResultsOfBattle(battleReports);
                     
                return JsonConvert.SerializeObject(battleReports);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
