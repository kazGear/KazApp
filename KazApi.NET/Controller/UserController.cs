using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KazApi.Controller.Service;
using KazApi.DTO;

namespace KazApi.Controller
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(IConfiguration configuration)
        {
            _service = new UserService(configuration);
        }

        /// <summary>
        /// CORS対策
        /// </summary>

        //[HttpOptions("api/*")]
        //public IActionResult Preflight()
        //{
        //    Response.Headers.Add("Access-Control-Allow-Origin", "*");
        //    Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
        //    Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        //    return NoContent();
        //}

        /// <summary>
        /// 初期処理
        /// </summary>
        [HttpPost("api/user/init")]
        public ActionResult<string> Init()
        {
            // 検証用に登録済みユーザを取得
            IEnumerable<UserDTO> users = _service.RegistedSelectUsers();
            return JsonConvert.SerializeObject(users);
        }

        /// <summary>
        /// ユーザー登録
        /// </summary>
        [HttpPost("api/user/userRegist")]
        public ActionResult<bool> UserRegist(
            [FromQuery] string LoginId,
            [FromQuery] string Password,
            [FromQuery] string DispName,
            [FromQuery] string DispShortName)
        {
            try
            {
                // 空白除去
                LoginId = LoginId.Trim();
                Password = Password.Trim();
                DispName = DispName.Trim();
                DispShortName = DispShortName.Trim();

                /*
                TODO 引数検証
                error >>> エラーページへ
                 */

                bool result = _service.UserRegist(LoginId, Password, DispName, DispShortName);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(/* TODO */"エラー画面に伝搬したい");
            }
        }

    }
}
