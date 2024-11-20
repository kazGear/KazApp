using CSLib.Lib;
using KazApi.Common._Const;
using KazApi.DTO;
using KazApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace KazApi.Controller.Service
{
    public class UserService
    {
        private readonly IDatabase _posgre;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserService(IConfiguration configuration)
        {
            _posgre = new PostgreSQL(configuration);
        }

        /// <summary>
        /// 登録済ユーザーを取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserDTO> RegistedSelectUsers()
            => _posgre.Select<UserDTO>(SQL.UserSQL.SelecUsers());

        // ユーザー登録
        public bool UserRegist(
            string LoginId,
            string Password,
            string DispName,
            string DispShortName)
        {
            try
            {
                // 暗号化
                string encryptPass = UAes.AesEncrypt(Password);

                var param = new
                {
                    login_id = LoginId,
                    login_pass = encryptPass,
                    disp_name = DispName,
                    disp_short_name = DispShortName
                };

                _posgre.Execute(SQL.UserSQL.InsertUser(), param);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
