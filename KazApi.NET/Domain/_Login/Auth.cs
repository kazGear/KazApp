using CSLib.Lib;
using KazApi.DTO;
using KazApi.Repository;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace KazApi.Domain._Login
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

            var param = new {
                login_id = loginId,
                login_pass = encryptPass
            };
            
            // ユーザー検索
            UserDTO? user = _posgre.Select<UserDTO>(SQL.AuthSQL.SelectLoginUser(), param)
                                   .SingleOrDefault();
            return user; 
        }
    }
}
