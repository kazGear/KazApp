namespace KazApi.Domain._Const
{
    /// <summary>
    /// コードタイプ定数
    /// </summary>
    public class CCodeType : Enumeration<int>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private CCodeType(int id, string name) : base(id, name) { }

        /// <summary>
        /// 自然属性
        /// </summary>
        public static readonly CCodeType ELEMENT = new(1, "ELEMENT");
        ///// <summary>
        ///// 状態
        ///// </summary>
        public static readonly CCodeType STATE = new(2, "STATE");
        ///// <summary>
        ///// 対象
        ///// </summary>
        public static readonly CCodeType TARGET = new(3, "TARGET");
        ///// <summary>
        ///// スキル
        ///// </summary>
        public static readonly CCodeType SKILL = new(4, "SKILL");
        ///// <summary>
        ///// システム補正率
        ///// </summary>
        public static readonly CCodeType SYS_RATE = new(5, "SYS_RATE");
        ///// <summary>
        ///// モンスター
        ///// </summary>
        public static readonly CCodeType MONSTER = new(6, "MONSTER");
    }
}
