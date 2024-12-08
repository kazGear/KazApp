namespace KazApi.Repository.sql
{
    /// <summary>
    /// SQL文格納クラス
    /// </summary>
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
            INNER JOIN m_my_skills AS ms
                    ON ms.monster_id = m.monster_id 
            INNER JOIN m_skill AS s
                    ON s.skill_id = ms.skill_id
                 WHERE EXISTS 
                    (
                        SELECT *
                          FROM m_usable_monster_types AS u
                         WHERE login_id = @login_id
                           AND u.monster_type_id = m.monster_type  
                    )
              GROUP BY m.monster_id
              ORDER BY m.monster_id ASC ;
            ";
            return SQL;
        }

        public static string SelectMonsterSkill()
        {
            string SQL = @"
                    SELECT monster_id AS MonsterId
                         , skill_id   AS SkillId 
                         , disabled   AS Disabled
                      FROM m_my_skills ;
                ";
            return SQL;
        }
    }
}
