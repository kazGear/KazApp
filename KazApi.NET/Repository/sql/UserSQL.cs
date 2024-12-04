using KazApi.Domain._Const;

namespace KazApi.Repository.sql
{
    /// <summary>
    /// SQL文格納クラス
    /// </summary>
    public static class UserSQL
    {

        public static string SelecUsers(string? loginId = null)
        {
            string WHERE = loginId != null ? " WHERE login_id = @login_id " : "";

            string SQL = @$"
                SELECT u.login_id          AS LoginId
                     , u.failed_login_cnt  AS FailedLoginCnt
                     , u.is_login_disabled AS IsLoginDisabled
                     , u.disp_name         AS DispName
                     , u.disp_short_name   AS DispShortName
                     , u.role              AS Role
                     , c.name              AS RoleName
                     , u.cash              AS Cash
                     , u.bankruptcy_cnt    AS BankruptcyCnt
                     , u.wins              AS Wins
                     , u.wins_get_cash     AS WinsGetCash
                     , u.losses            AS Losses
                     , u.losses_lost_cash  AS LossesLostCash
                     , u.user_image        AS UserImage
                  FROM m_user AS u
            INNER JOIN m_code AS c
                    ON u.role = c.value
                   AND c.category = {CCodeType.ROLE.VALUE}
                {WHERE} ;
            ";
            return SQL;
        }

        public static string SelectLoginUser()
        {
            string SQL = @"
                SELECT disp_name  AS DispName
                     , user_image AS UserImage
                  FROM m_user
                 WHERE login_id = @login_id ;
            ";
            return SQL;
        }
        public static string InsertUser()
        {
            string SQL = @"
                INSERT INTO m_user VALUES 
                (
                    @login_id,
                    @login_pass,
                    0,                  -- failed_login_cnt
                    false,              -- is_login_disabled
                    @disp_name,
                    @disp_short_name,
                    1,                  -- role
                    5000,               -- cash
                    0,                  -- bankruptcy_cnt
                    0,                  -- wins
                    0,                  -- wins_get_cash
                    0,                  -- losses
                    0                   -- losses_lost_cash
                ) ;
                ";
            return SQL;
        }

        public static string InsertStartUpMonsters()
        {
            string SQL = @"
                INSERT INTO m_usable_monster_types VALUES
                (
                    @user_id,
                    @monster_type_id,
                    @acquired_date,
                    @not_use_this
                ) ;
            ";
            return SQL;
        }

        public static string RestartAsPlayer()
        {
            string SQL = @"
                UPDATE m_user
                   SET cash = 5000
                     , bankruptcy_cnt = bankruptcy_cnt + 1
                WHERE login_id = @login_id ;
            ";
            return SQL;
        }

        public static string UpdateUserImage()
        {
            string SQL = @"
                UPDATE m_user
                   SET user_image = @image
                WHERE login_id = @login_id ;
            ";
            return SQL;
        }
    }
}
