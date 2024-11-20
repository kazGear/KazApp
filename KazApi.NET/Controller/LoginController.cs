using CSLib.Lib;
using KazApi.Controller.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace KazApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService LoginService;

        public LoginController(IConfiguration configuration)
        {
            LoginService = new LoginService(configuration);
        }

        // ユーザ一覧を取得する
        [HttpPost("FetchLoginUsers")]
        public ActionResult<string> FetchLoginUsers([FromBody] User? request)
        {
            // パスワード暗号化
            request.Password = UAes.AesEncrypt(request.Password);

            IEnumerable<string> userNames = LoginService.FetchLoginUsers(request.UserName, request.Password);
            return JsonConvert.SerializeObject(userNames);
        }

        public new class User
        {
            [JsonPropertyName("userName")]
            public string UserName { get; set; }
            [JsonPropertyName("password")]
            public string Password { get; set; }
        }
    }
}
