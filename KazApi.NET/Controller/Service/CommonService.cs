using KazApi.Repository;
using KazApi.Repository.sql;

namespace KazApi.Controller.Service
{
    public class CommonService
    {
        private readonly IDatabase _posgre;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonService(IConfiguration configuration)
        {
            _posgre = new PostgreSQL(configuration);
        }

        public void UpdateImage(string loginId, string image)
        {
            var param = new
            {
                login_id = loginId,
                image = image,
            };
            _posgre.Execute(UserSQL.UpdateUserImage(), param);
        }
    }
}
