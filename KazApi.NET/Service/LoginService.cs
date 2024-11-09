using KazApi.Domain._User;
using KazApi.Repository;

namespace KazApi.Service
{
    public class LoginService
    {
        private readonly IConfiguration _appsettings;

        public LoginService(IConfiguration configuration)
        {
            _appsettings = configuration;
        }

        // ユーザの一覧を取得
        public IEnumerable<string> FetchLoginUsers(string name, string pass)
        {
            string select = @"
                SELECT username
                  FROM users
                 WHERE username = @username
	               AND password = @password
	               AND is_invalid = FALSE;
                ";

            object parameters = new
            {
                username = name,
                password = pass
            };

            IDatabase postgre = new PostgreSQL(_appsettings);
            IEnumerable<IUser> users = postgre.Select<IUser>(select, parameters); // Select<IUser> がだめ。具象クラスを使用する

            IList<string> userNames = new List<string>();
            foreach (IUser user in users) userNames.Add(user.UserName);

            return userNames;
        }
    }
}
