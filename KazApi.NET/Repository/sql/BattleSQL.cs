namespace KazApi.Repository.sql
{
    /// <summary>
    /// SQL文格納クラス
    /// </summary>
    public static class BattleSQL
    {
        public static string InsertBattleResult()
        {
            string SQL = $@"
                INSERT INTO t_battle_result VALUES 
                (                  
                      @battle_end_date
                    , @battle_end_time
                    , @serial
                    , @monster_id
                    , @is_win
                );
            ";
            return SQL;
        }

        public static string InsertUserResult()
        {
            string SQL = $@"
                UPDATE m_user
                   SET cash             = cash + @cash
                     , wins             = wins + @wins
                     , wins_get_cash    = wins_get_cash + @wins_get_cash
                     , losses           = losses + @losses
                     , losses_lost_cash = losses_lost_cash + @losses_lost_cash
                 WHERE login_id = @login_id ;
            ";
            return SQL;
        }
    }
}
