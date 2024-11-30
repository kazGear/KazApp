using CSLib.Lib;
using KazApi.Domain._User;
using KazApi.Domain.DTO;
using KazApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KazApi.Controller
{
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabase _posgre;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _posgre = new PostgreSQL(configuration);
        }

        /// <summary>
        /// ログイン実行
        /// </summary>
        [HttpPost("api/login")]
        public IActionResult Login([FromQuery] string? loginId, [FromQuery] string? password)
        {
            loginId = loginId.Trim();
            if (loginId == null || password == null) return Unauthorized();

            // ユーザの認証
            Auth auth = new Auth(_posgre);
            UserDTO? user = auth.AuthenticateUser(loginId, password);
            
            // 認証失敗
            if (user == null) return Unauthorized();
            
            // トークン発行
            string token = UJwt.GenerateJwtToken(user.LoginId, _configuration);

            return Ok(JsonConvert.SerializeObject(token));
        }

        /// <summary>
        /// トークンが有効か確認する
        /// </summary>
        [HttpPost("api/checkToken")]
        public IActionResult IsValidToken([FromQuery] string token)
        {
            if (token == null || token == "null") return Ok(false);

            bool isValid = UJwt.IsValidToken(token);
            return Ok(isValid);
        }
    }
}
