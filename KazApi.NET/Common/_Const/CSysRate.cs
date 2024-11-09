namespace KazApi.Common._Const
{
    /// <summary>
    /// 補正率クラス
    /// </summary>
    public static class CSysRate
    {
        /// <summary>
        /// クリティカル補正率
        /// </summary>
        public static readonly double CRITICAL_DAMAGE = 1.6;
        /// <summary>
        /// 行動順補正率
        /// </summary>
        public static readonly double MOVE_SPEED = 0.3;
        /// <summary>
        /// 弱点属性ダメージ
        /// </summary>
        public static readonly double WEEK_DAMAGE = 1.8;
        /// <summary>
        /// 物理スキルダメージ補正率
        /// </summary>
        public static readonly double PHYSICAL_SKILL_DAMAGE = 0.10;
        /// <summary>
        /// 魔法スキルダメージ補正率
        /// </summary>
        public static readonly double MAGIC_SKILL_DAMAGE = 0.05;
        /// <summary>
        /// 全体攻撃調整・下限
        /// </summary>
        public static readonly double ALL_ATTACK_ADJUST_MIN = 0.45;
        /// <summary>
        /// 全体攻撃調整・上限
        /// </summary>
        public static readonly double ALL_ATTACK_ADJUST_MAX = 0.65;
    }
}