using KazApi.Common._Log;
using KazApi.DTO;

namespace KazApi.Domain.monster._State
{
    /// <summary>
    /// 状態異常インターフェイス
    /// </summary>
    public abstract class IState
    {
        protected readonly ILog<BattleMetaData> _Log = new BattleLogger();

        public string Name { get; protected set; }
        public int StateType { get; protected set; }
        public int MaxDuration { get; protected set; }
        public int DurationCount { get; protected set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IState(string name, int stateType, int maxDuration)
        {
            Name = name;
            StateType = stateType;
            MaxDuration = maxDuration;
            DurationCount = 0;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IState(StateDTO dto)
        {
            Name = dto.Name;
            StateType = dto.StateType;
            MaxDuration = dto.MaxDuration;
            DurationCount = dto.DurationCount;
        }

        /// <summary>
        /// 状態タイプを取得
        /// </summary>
        public int GetStateType() => StateType;

        /// <summary>
        /// 新しくオブジェクトを生成
        /// </summary>
        public abstract IState DeepCopy();

        /// <summary>
        /// 状態解除時のログ
        /// </summary>
        public abstract void DisabledLogging(IMonster monster);

        /// <summary>
        /// 状態の影響をモンスターに与える
        /// </summary>
        public abstract void Impact(IMonster monster);

        /// <summary>
        /// 状態名を取得
        /// </summary>
        public string StateName() => Name;

        /// <summary>
        /// 状態が有効であるか
        /// </summary>
        public bool IsDisable() => DurationCount >= MaxDuration;

    }
}
