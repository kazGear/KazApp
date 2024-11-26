using KazApi.Common._Const;
using KazApi.DTO;

namespace KazApi.Domain.monster._State
{
    /// <summary>
    /// 状態異常なしクラス
    /// </summary>
    public class None : IState
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public None(StateDTO dto) : base(dto) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public None(string name, int stateType, int maxDuration)
             : base(name, stateType, maxDuration)
        {
            StateType = CStateType.NONE.VALUE;
        }


        public override IState DeepCopy()
            => new None(Name, StateType, MaxDuration);

        public override void DisabledLogging(IMonster monster)
            => throw new NotImplementedException();

        /// <summary>
        /// /
        /// </summary>
        public override void Impact(IMonster monster)
        {
            throw new NotImplementedException();
        }
    }
}
