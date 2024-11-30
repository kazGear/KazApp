using CSLib.Lib;
using KazApi.Domain.DTO;
using KazApi.Repository;
using KazApi.Repository.sql;

namespace KazApi.Domain._User
{
    /// <summary>
    /// 認証クラス
    /// </summary>
    public class Auth
    {
        private readonly IDatabase _posgre;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Auth(IDatabase posgre)
        {
            _posgre = posgre;
        }

        /// <summary>
        /// 認証実行 ユーザー検索・パスワード検証
        /// </summary>

        internal UserDTO? AuthenticateUser(string loginId, string password)
        {
            string encryptPass = UAes.AesEncrypt(password);

            var param = new
            {
                login_id = loginId,
                login_pass = encryptPass
            };

            // ユーザー検索
            UserDTO? user = _posgre.Select<UserDTO>(AuthSQL.SelectLoginUser(), param)
                                   .SingleOrDefault();
            return user;
        }
    }
}
