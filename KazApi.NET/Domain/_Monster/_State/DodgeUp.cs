//using KazApi.Common._Const;

//namespace KazApi.Domain._Monster._State
//{
//    /// <summary>
//    /// 回避率アップクラス
//    /// </summary>
//    public class DodgeUp : IState
//    {
//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        public DodgeUp(string name, int stateType, int maxDuration)
//                : base(name, stateType, maxDuration)
//        {
//            base.StateType = ((int)CStateType.DODGEUP);
//        }

//        public override IState DeepCopy()
//            => new DodgeUp(base.Name, base.StateType, base.MaxDuration);

//        public override void DisabledLogging(IMonster monster)
//            => throw new NotImplementedException();

//        /// <summary>
//        /// 
//        /// </summary>
//        public override void Impact(IMonster monster)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
