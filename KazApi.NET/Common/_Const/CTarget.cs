namespace KazApi.Common._Const
{
    public enum CTarget
    {
        /// <summary>
        /// 無し
        /// </summary>
        NONE,
        /// <summary>
        /// 敵ランダム
        /// </summary>
        ENEMY_RANDOM,
        /// <summary>
        /// 敵全体
        /// </summary>
        ENEMY_ALL,
        /// <summary>
        /// 敵ランダム・敵全体
        /// </summary>
        ENEMY_RANDOM_OR_ALL,
        /// <summary>
        /// 敵ランダム・複数回
        /// </summary>
        ENEMY_RANDOM_SOME_TIMES,
        /// <summary>
        /// 自身
        /// </summary>
        ME,
        /// <summary>
        /// 味方ランダム
        /// </summary>
        FRIEND_RANDOM,
        /// <summary>
        /// 味方全体
        /// </summary>
        FRIEND_ALL,
        /// <summary>
        /// 味方ランダム・味方全体
        /// </summary>
        FRIEND_RANDOM_OR_ALL
    }
}
