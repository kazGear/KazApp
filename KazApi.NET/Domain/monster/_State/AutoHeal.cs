using CSLib.Lib;
using KazApi.Common._Log;
using KazApi.DTO;

namespace KazApi.Domain.monster._State
{
    /// <summary>
    /// 自動回復状態クラス
    /// </summary>
    public class AutoHeal : IState
    {
        private static readonly double HEAL_RATE = 0.15;
        private static readonly double ADJUST_RATE = 0.25;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoHeal(StateDTO dto) : base(dto) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoHeal(string name, int stateType, int maxDuration)
                 : base(name, stateType, maxDuration) { }

        public override IState DeepCopy()
            => new AutoHeal(Name, StateType, MaxDuration);

        public override void DisabledLogging(IMonster monster)
        {
            bool disableState = true;

            _Log.Logging(new BattleMetaData(
                monster.MonsterId,
                disableState,
                Name,
                $"{monster.MonsterName}の自然治癒力がなくなった。")
                );

        }

        /// <summary>
        /// 自ターンに自動回復する
        /// </summary>
        public override void Impact(IMonster me)
        {
            if (IsDisable()) return;

            int healPoint1 = (int)(me.MaxHp * HEAL_RATE);
            int healPoint2 = URandom.RandomChangeInt(healPoint1, ADJUST_RATE);
            int healLimit = me.MaxHp - me.Hp;
            int healPointFix = healPoint2 >= healLimit ? healLimit : healPoint2;

            _Log.Logging(new BattleMetaData(me.MonsterId, $"{me.MonsterName}の自然治癒！"));
            _Log.Logging(new BattleMetaData(
                me.MonsterId,
                me.Hp,
                healPointFix * -1,
                $"{me.MonsterName}のHPが{healPointFix}回復した！")
                );

            me.AcceptDamage(healPointFix * -1); // マイナスダメージを加えてHP増加

            DurationCount++;
        }

    }
}
