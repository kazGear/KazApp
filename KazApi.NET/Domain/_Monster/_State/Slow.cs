//using KazApi.Common._Const;

//namespace KazApi.Domain._Monster._State
//{
//    /// <summary>
//    /// スロー状態クラス
//    /// </summary>
//    public class Slow : IState
//    {
//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        public Slow(string name, int stateType, int maxDuration) 
//             : base(name, stateType, maxDuration)
//        {
//            base.StateType = ((int)CStateType.SLOW);
//        }

//        public override IState DeepCopy()
//            => new Slow(base.Name, base.StateType, base.MaxDuration);

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
