namespace KazApi.Repository.sql
{
    /// <summary>
    /// SQL文格納クラス
    /// </summary>
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
}
