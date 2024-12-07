﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KazApi.Controller.Service;
using KazApi.Repository;
using KazApi.Repository.sql;
using KazApi.Domain.DTO;

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
        /// ユーザ情報取得
        /// </summary>
        [HttpPost("api/user/userInfo")]
        public ActionResult<string> SelectUserOne([FromQuery] string loginId)
        {
            UserDTO? user = _service.SelectUserOne(loginId);
            return JsonConvert.SerializeObject(user);
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

                bool result = _service.InsertUser(LoginId, Password, DispName, DispShortName);
                result = _service.InsertStartUpMonsters(LoginId);
                return true;
            }
            catch (Exception)
            {
                throw new Exception(/* TODO */"エラー画面に伝搬したい");
            }
        }

        /// <summary>
        /// ログインユーザ情報を取得
        /// </summary>
        [HttpPost("api/user/loginUser")]
        public ActionResult<string?> SelectUser([FromQuery] string? loginId)
        {
            var param = new { login_id = loginId };
            
            UserDTO? user = _posgre.Select<UserDTO>(UserSQL.SelectLoginUser(), param)
                                   .SingleOrDefault();

            return JsonConvert.SerializeObject(user);
        }

        /// <summary>
        /// 自己破産（所持金初期化）
        /// </summary>
        [HttpPost("api/user/restartAsPlayer")]
        public ActionResult<string> RestartAsPlayer([FromQuery] string loginId)
        {
            try
            {
                _service.RestartAsPlayer(loginId);

                UserDTO? user = _service.SelectUserOne(loginId);
                return JsonConvert.SerializeObject(user);

            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        /// <summary>
        /// 使用可能なモンスター数を取得
        /// </summary>
        [HttpPost("api/user/getMonsterCount")]
        public ActionResult<string> SelectMonsterCount([FromQuery] string loginId)
        {
            LittleDTO<int> result = _service.SelectMonsterCount(loginId);
            return JsonConvert.SerializeObject($"{result.Param1} / {result.Param2}");
        }
    }
}
