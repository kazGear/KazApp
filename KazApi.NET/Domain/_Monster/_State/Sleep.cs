using CSLib.Lib;
using KazApi.Common._Const;
using KazApi.Common._Log;

using KazApi.DTO;

namespace KazApi.Domain._Monster._State
{
    /// <summary>
    /// 睡眠状態クラス
    /// </summary>
    public class Sleep : IState, IDisableMove
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sleep(StateDTO dto) : base(dto) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sleep(string name, int stateType, int maxDuration)
              : base(name, stateType, maxDuration)
        {
            StateType = CStateType.SLEEP.VALUE;
        }

        public override IState DeepCopy()
            => new Sleep(Name, StateType, MaxDuration);

        public override void DisabledLogging(IMonster monster)
        {
            bool disableState = true;

            _Log.Logging(new BattleMetaData(
                monster.MonsterId,
                disableState,
                Name,
                $"{monster.MonsterName}は目覚めた！"));

        }

        /// <summary>
        /// 自ターンは行動不能
        /// </summary>
        public override void Impact(IMonster monster)
        {
            if (IsDisable()) return;
            _Log.Logging(new BattleMetaData(monster.MonsterId, $"{monster.MonsterName}は眠っている Zzz ..."));

            // 早く回復することがある                        
            DurationCount += URandom.durationCountUp();
        }
    }
}
