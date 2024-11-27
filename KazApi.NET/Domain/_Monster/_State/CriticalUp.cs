//using KazApi.Common._Const;

//namespace KazApi.Domain._Monster._State
//{
//    /// <summary>
//    /// クリティカル率アップ状態クラス
//    /// </summary>
//    public class CriticalUp : IState
//    {
//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        public CriticalUp(string name, int stateType, int maxDuration)
//                   : base(name, stateType, maxDuration)
//        {
//            base.StateType = ((int)CStateType.CRITICALUP);
//        }

//        public override IState DeepCopy()
//            => new CriticalUp(base.Name, base.StateType, base.MaxDuration);

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
