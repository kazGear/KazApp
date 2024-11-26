namespace KazApi.Repository.sql
{
    /// <summary>
    /// SQL文格納クラス
    /// </summary>
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
}
