namespace KazApi.Lib
{
    /// <summary>
    /// 乱数ユーティリティ
    /// </summary>
    public class URandom
    {
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        private URandom()
        {

        }

        /// <summary>
        /// int型の乱数を取得（範囲指定）
        /// minValue: 下限を含む
        /// maxValue: 排他的な上限（その数値は含まない）
        /// </summary>
        public static int RandomInt(int minValue, int maxValue)
        {
            Random r = new Random();
            return r.Next(minValue, maxValue);
        }

        /// <summary>
        /// double型の乱数を取得（範囲指定） 
        /// minValue: 下限を含む
        /// maxValue: 排他的な上限（その数値は含まない）
        /// </summary>
        public static double RandomDouble(double minValue, double maxValue)
        {
            Random r = new Random();
            return r.NextDouble() * ( maxValue - minValue) + minValue;
        }

        /// <summary>
        /// 対象の数値を rate(%) に応じて増減させる 
        /// </summary>
        public static int RandomChangeInt(int target, double rate)
        {
            double adjust = target * RandomDouble(0, rate);
            bool randBool = RandomBool();

            // 負数に変換
            if (!randBool) adjust *= -1;

            return target + (int)adjust;
        }

        /// <summary>
        /// true / false のいずれかを生成 
        /// </summary>
        public static bool RandomBool()
        {
            Random r = new Random();
            int oneOrzero = r.Next(0, 2); // 0 or 1

            return 1 == oneOrzero ? true : false;
        }

        /// <summary>
        /// 状態異常の経過数を追加
        /// </summary>
        /// <returns></returns>
        public static int durationCountUp()
        {
            bool doubleCount = URandom.RandomBool();

            // 早く目覚めることがある
            if (doubleCount)
                return 2;
            else
                return 1;
        }
    }
}
