using CSLib.Lib;
using KazApi.Controller.Service;
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
        /// 初期処理
        /// </summary>
        [HttpPost("/api/common/runtime")]
        public ActionResult<string> Init()
        {
            return UEnvironment.IsRuntime();
        }
    }
}
