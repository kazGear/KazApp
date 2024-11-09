namespace CSLib.Lib
{
    /// <summary>
    /// 定数ユーティリティ
    /// </summary>
    public class UConstSet<T>
    {
        /// <summary>
        /// enumの定数リストを取得 
        /// </summary>
        public static IList<T> EnumValues()
        {
            IList<T> elements = Enum.GetValues(typeof(T))
                                    .Cast<T>()
                                    .ToList();
            return elements;
        }

    }
}
