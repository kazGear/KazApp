using KazApi.DTO;

namespace KazApi.Common._Const
{
    /// <summary>
    /// 状態定数
    /// </summary>
    public enum CStateType
    {
        /// <summary>
        /// 無し
        /// </summary>
        NONE,
        /// <summary>
        /// 毒状態
        /// </summary>
        POISON,
        /// <summary>
        /// 睡眠
        /// </summary>
        SLEEP,
        /// <summary>
        /// 魅了
        /// </summary>
        CHARM,
        /// <summary>
        /// スロー、遅い
        /// </summary>
        SLOW,
        /// <summary>
        /// 攻撃力UP
        /// </summary>
        POWERUP,
        /// <summary>
        /// 回避率UP
        /// </summary>
        DODGEUP,
        /// <summary>
        /// クリティカル率UP
        /// </summary>
        CRITICALUP,
        /// <summary>
        /// 自動回復
        /// </summary>
        AUTOHEAL
    }

    /// <summary>
    /// 状態名定数
    /// </summary>
    public static class CStateName
    {
        /// <summary>
        /// 無し
        /// </summary>
        public static readonly string NONE = "通常";
        /// <summary>
        /// 毒状態
        /// </summary>
        public static readonly string POISON = "毒";
        /// <summary>
        /// 睡眠
        /// </summary>
        public static readonly string SLEEP = "睡眠";
        /// <summary>
        /// 魅了
        /// </summary>
        public static readonly string CHARM = "魅了";
        /// <summary>
        /// スロー、遅い
        /// </summary>
        public static readonly string SLOW = "スロー";
        /// <summary>
        /// 攻撃力UP
        /// </summary>
        public static readonly string POWERUP = "攻撃力UP";
        /// <summary>
        /// 回避率UP
        /// </summary>
        public static readonly string DODGEUP = "回避率UP";
        /// <summary>
        /// クリティカルUP
        /// </summary>
        public static readonly string CRITICALUP = "クリティカルUP";
        /// <summary>
        /// 自動回復
        /// </summary>
        public static readonly string AUTOHEAL = "自動回復";

        /// <summary>
        /// 状態名の詰め合わせを取得
        /// </summary>
        public static IEnumerable<string> ConvertTypeToName(IEnumerable<StateDTO> status)
        {
            IList<string> result = [];
            foreach (StateDTO state in status)
            {
                result.Add(state.Name);
            }
            return result;
        }
    }
}
