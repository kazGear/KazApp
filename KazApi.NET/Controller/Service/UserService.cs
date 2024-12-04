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
        /// ユーザ情報取得
        /// </summary>
        public UserDTO? SelectUserOne(string loginId)
        {
            var param = new { login_id = loginId };

            return _posgre.Select<UserDTO>(UserSQL.SelecUsers(loginId), param)
                          .SingleOrDefault();
        }

        /// <summary>
        /// 登録済ユーザーを取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserDTO> RegistedSelectUsers()
            => _posgre.Select<UserDTO>(UserSQL.SelecUsers());


        /// <summary>
        /// ユーザ登録
        /// </summary>

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

        /// <summary>
        /// ユーザが初期か使えるモンスタを設定する
        /// </summary>
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

        /// <summary>
        /// 自己破産（所持金初期化）
        /// </summary>
        /// <returns></returns>
        public void RestartAsPlayer(string loginId)
        {
            var param = new { login_id = loginId };
            _posgre.Execute(UserSQL.RestartAsPlayer(), param);
        }
    }
}
