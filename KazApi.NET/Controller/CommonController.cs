using CSLib.Lib;
using KazApi.Controller.Service;
using Microsoft.AspNetCore.Mvc;


namespace KazApi.Controller
{
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly CommonService _serviceCommon;
        private readonly BattleReportService _serviceReport;

        public CommonController(IConfiguration configuration)
        {
            _serviceCommon = new CommonService(configuration);
            _serviceReport = new BattleReportService(configuration);
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        [HttpPost("api/common/runtime")]
        public ActionResult<string> Init()
        {
            return UEnvironment.IsRuntime();
        }

        /// <summary>
        /// 画像ファイルアップロード
        /// </summary>
        [HttpPost("api/common/imgUpload")]
        public async Task<IActionResult> UploadImage(
            IFormFile image,
            [FromQuery] string loginId)
        {
            if (image == null || image.Length == 0) 
                return BadRequest("No file uploaded.");

            // 画像のバイナリ化
            using (MemoryStream ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                byte[] imageByte = ms.ToArray();
                string imageBASE64 = Convert.ToBase64String(imageByte);

                _serviceCommon.UpdateImage(loginId, imageBASE64);
            }
        
            return Ok(new { message = "Image uploaded successfully" });
        }
    }
}
