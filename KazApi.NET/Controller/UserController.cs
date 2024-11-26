using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KazApi.Controller.Service;
using KazApi.DTO;
using KazApi.Repository;
using KazApi.Repository.sql;

namespace KazApi.Controller
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IDatabase _posgre;

        public UserController(IConfiguration configuration)
        {
            _service = new UserService(configuration);
            _posgre = new PostgreSQL(configuration);
        }

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
            catch (Exception)
            {
                throw new Exception(/* TODO */"エラー画面に伝搬したい");
            }
        }

        [HttpPost("api/user/loginUser")]
        public UserDTO? SelectUser([FromQuery] string? userName)
        {
            if (userName == null) return null;

            var param = new { disp_name = userName };
            return _posgre.Select<UserDTO>(UserSQL.SelectLoginUser(), param)
                          .SingleOrDefault();
        }
    }
}
