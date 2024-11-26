namespace KazApi.Repository.sql
{
    /// <summary>
    /// SQL文格納クラス
    /// </summary>
    public static class ReportSQL
    {
        public static string SelectMonsterTypes()
        {
            string SQL = $@"
                SELECT value AS MonsterTypeId
                     , name  AS MonsterTypeName
                  FROM m_code
                 WHERE category = @category
              ORDER BY MonsterTypeName ASC
            ";
            return SQL;
        }

        public static string SelectMonsterReport(dynamic param)
        {
            string WHERE = PartialMonsterReport(param);

            string SQL = $@"
                SELECT m.monster_id          AS MonsterId
                     , max(m.monster_name)   AS MonsterName
                     , count(*)              AS BattleCount
                     , sum(CASE WHEN is_win = TRUE
                                THEN 1
                                ELSE 0 END ) AS Wins
                  FROM m_monster AS m
            INNER JOIN t_battle_result AS br
                    ON m.monster_id = br.monster_id
                {WHERE}
              GROUP BY m.monster_id
              ORDER BY MonsterName ASC ;
            ";

            return SQL;
        }
        public static string PartialMonsterReport(int monsterType)
        {
            return monsterType > 0
                ? $"WHERE monster_type = @monster_type"
                : "";
        }

        public static string SelectBattleReport(int battleScale, DateTime? from, DateTime? to)
        {
            string HAVING = PartialBattleReportHaving(battleScale);
            string AND_fromTo = PartialBattleReportFromTo(from, to);

            string SQL = $@"
                SELECT DENSE_RANK() OVER (
                           ORDER BY battle_end_date ASC, battle_end_time ASC
                       )                 AS BattleId
                     , b.battle_end_date AS BattleEndDate 
                     , b.battle_end_time AS BattleEndTime
                     , b.serial          AS Serial
                     , b.monster_id      AS MonsterId
                     , m.monster_name    AS MonsterName 
                     , b.is_win          AS IsWin
                  FROM t_battle_result AS b
            INNER JOIN m_monster AS m
                    ON b.monster_id = m.monster_id
                 WHERE EXISTS 
                    (
                        SELECT
                          FROM t_battle_result AS br
                         WHERE b.battle_end_date = br.battle_end_date
                           AND b.battle_end_time = br.battle_end_time
                      GROUP BY battle_end_date
                             , battle_end_time 
                       {HAVING}
                    )
                  {AND_fromTo}
              ORDER BY BattleEndDate ASC 
                     , BattleEndTime ASC 
                     , Serial ASC ;
            ";

            return SQL;
        }
        public static string PartialBattleReportHaving(int battleScale)
        {
            return battleScale != 0 ? "HAVING count(*) = @battle_scale"
                                    : "";
        }
        public static string PartialBattleReportFromTo(DateTime? from, DateTime? to)
        {
            string fromTo = string.Empty;
            if (from != null && to != null)
            {
                fromTo = " AND battle_end_date >= @from "
                       + " AND battle_end_date <= @to ";
            }
            else if (from != null)
            {
                fromTo = " AND battle_end_date >= @from ";
            }
            else if (to != null)
            {
                fromTo = " AND battle_end_date <= @to ";
            }
            return fromTo;
        }
    }
}
