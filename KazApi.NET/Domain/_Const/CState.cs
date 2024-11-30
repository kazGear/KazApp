using KazApi.Domain.DTO;

namespace KazApi.Domain._Const
{
    /// <summary>
    /// 状態定数
    /// </summary>
    public class CStateType : Enumeration<int>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private CStateType(int id, string name) : base(id, name) { }

        /// <summary>
        /// 無し
        /// </summary>
        public static readonly CStateType NONE = new(0, "通常");
        /// <summary>
        /// 毒状態
        /// </summary>
        public static readonly CStateType POISON = new(1, "毒");
        /// <summary>
        /// 睡眠
        /// </summary>
        public static readonly CStateType SLEEP = new(2, "睡眠");
        /// <summary>
        /// 魅了
        /// </summary>
        public static readonly CStateType CHARM = new(3, "魅了");
        /// <summary>
        /// スロー、遅い
        /// </summary>
        public static readonly CStateType SLOW = new(4, "スロー");
        /// <summary>
        /// 攻撃力UP
        /// </summary>
        public static readonly CStateType POWERUP = new(5, "攻撃力UP");
        /// <summary>
        /// 回避率UP
        /// </summary>
        public static readonly CStateType DODGEUP = new(6, "回避率UP");
        /// <summary>
        /// クリティカル率UP
        /// </summary>
        public static readonly CStateType CRITICALUP = new(7, "クリティカルUP");
        /// <summary>
        /// 自動回復
        /// </summary>
        public static readonly CStateType AUTOHEAL = new(8, "自動回復");

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
