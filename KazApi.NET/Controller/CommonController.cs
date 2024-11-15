using CSLib.Lib;
using KazApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace KazApi.Controller
{
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly BattleReportService _service;

        public CommonController(IConfiguration configuration)
        {
            _service = new BattleReportService(configuration);
        }

        /// <summary>
        /// CORS対策
        /// </summary>
        [HttpOptions("api/*")]
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
        [HttpPost("/api/common/runtime")]
        public ActionResult<string> Init()
        {
            return UEnvironment.IsRuntime();
        }
    }
}
