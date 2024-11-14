using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KazApi.Service;
using KazApi.DTO;

namespace KazApi.Controller
{
    [ApiController]
    public class BattleReportController : ControllerBase
    {
        private readonly BattleReportService _service;

        public BattleReportController(IConfiguration configuration)
        {
            Console.WriteLine("コントローラ起動 BattleReportController");
            _service = new BattleReportService(configuration);
        }

        [HttpOptions("/api/*")]
        public IActionResult Preflight()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            return NoContent();
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
        public ActionResult<string> SelectMonsterReport([FromQuery] string monsterTypeId)
        {
            try
            {
                Console.WriteLine("リクエスト到達 api/battleReport/monsterReport");
                IEnumerable<MonsterRepostDTO> monsterReports = _service.SelectMonsterReport(monsterTypeId);
                return JsonConvert.SerializeObject(monsterReports);
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
            [FromQuery] string battleScale,
            [FromQuery] string? from,
            [FromQuery] string? to)
        {
            try
            {
                DateTime? dateFrom = from == null ? null : DateTime.Parse(from);
                DateTime? dateTo = to == null ? null : DateTime.Parse(to);

                IEnumerable<BattleReportDTO> battleReports 
                    = _service.SelectBattleReport(battleScale, dateFrom, dateTo);

                return JsonConvert.SerializeObject(battleReports);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
