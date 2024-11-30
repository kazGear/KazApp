namespace KazApi.Repository.sql
{
    /// <summary>
    /// SQL文格納クラス
    /// </summary>
    public static class UserSQL
    {

        public static string SelecUsers()
        {
            string SQL = @"
                SELECT login_id        AS LoginId
                     , disp_name       AS DispName
                     , disp_short_name AS DispShortName
                  FROM m_user ;
            ";
            return SQL;
        }

        public static string SelectLoginUser()
        {
            string SQL = @"
                SELECT disp_name AS DispName
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
    }
}
