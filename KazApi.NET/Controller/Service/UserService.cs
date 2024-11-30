using CSLib.Lib;
using KazApi.Domain._Const;
using KazApi.Domain.DTO;
using KazApi.Repository;
using KazApi.Repository.sql;

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
            => _posgre.Select<UserDTO>(UserSQL.SelecUsers());

        // ユーザー登録
        public bool InsertUser(
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

                _posgre.Execute(UserSQL.InsertUser(), param);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool InsertStartUpMonsters(string loginId)
        {
            try
            {
                DateTime now = DateTime.Now;
                               
                foreach (CMonsterType monsterType in CMonsterType.START_UP)
                {
                    var param = new
                    {
                        user_id = loginId,
                        monster_type_id = monsterType.VALUE,
                        acquired_date = now,
                        not_use_this = false
                    };
                    _posgre.Execute(UserSQL.InsertStartUpMonsters(), param);
                }
            } 
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
