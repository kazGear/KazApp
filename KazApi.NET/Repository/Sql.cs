
namespace KazApi.Repository
{
    /// <summary>
    /// SQL文格納クラス
    /// </summary>
    public static class SQL
    {
        public static class AuthSQL
        {
            public static string SelectLoginUser()
            {
                string SQL = @"
                    SELECT login_id   AS LoginId
                         , login_pass AS LoginPass
                      FROM m_user
                     WHERE login_id          = @login_id
                       AND login_pass        = @login_pass
                       AND is_login_disabled = FALSE ;
                ";
                return SQL;
            }
        }
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
        }

        public static class CodeSQL
        {
            public static string SelectCode()
            {
                string SQL = @"
                    SELECT category AS Category
                         , value    AS Value
                         , name     AS Name
                         , param1   AS Param1
                         , param2   AS Param2
                         , param3   AS Param3
                         , remarks  AS Remarks
                      FROM m_code ;
                ";
                return SQL;
            }
        }

        public static class MonsterSQL
        {
            public static string SelectMonsters()
            {
                string SQL = @"
                    SELECT m.monster_id   AS MonsterId 
                         , m.monster_name AS MonsterName
                         , m.monster_type AS MonsterType
                         , m.hp           AS Hp
                         , m.hp           AS MaxHp
                         , m.attack       AS Attack
                         , m.speed        AS Speed
                         , m.week         AS Week
                         , max(m.hp) * 10
                             + max(m.attack) * 10
                             + max(m.speed) * 5
                             + sum(s.weight) * 20
                             + sum(s.critical * 100) AS BetScore
                      FROM m_monster AS m 
                INNER JOIN m_monster_skill AS ms
                        ON ms.monster_id = m.monster_id 
                INNER JOIN m_skill AS s
                        ON s.skill_id = ms.skill_id
                  GROUP BY m.monster_id
                  ORDER BY m.monster_id ASC ;
                ";
                return SQL;
            } 
        }
        
        public static class SkillSQL
        {
            public static string SelectSkill()
            {
                string SQL = @"
                    SELECT skill_id     AS SkillId
                         , skill_name   AS SkillName
                         , skill_type   AS SkillType
                         , element_type AS ElementType
                         , state_type   AS StateType
                         , target_type  AS TargetType
                         , attack       AS Attack
                         , weight       AS Weight
                         , critical     AS Critical
                         , remarks      AS Remarks
                      FROM m_skill ;
                ";
                return SQL;
            } 
        }

        public static class MonsterSkillSQL
        {
            public static string SelectMonsterSkill()
            {
                string SQL = @"
                    SELECT monster_id AS MonsterId
                         , skill_id   AS SkillId 
                         , disabled   AS Disabled
                      FROM m_monster_skill ;
                ";
                return SQL;
            }
        }

        public static class BattleResultSQL
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
        }

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
                            )                AS BattleId
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
            public static string PartialBattleReportFromTo(DateTime? from , DateTime? to)
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
}
