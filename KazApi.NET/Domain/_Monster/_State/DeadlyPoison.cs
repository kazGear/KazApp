using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Lib;

namespace KazApi.Domain._Monster._State
{
    /// <summary>
    /// 猛毒状態クラス
    /// </summary>
    public class DeadlyPoison : IState
    {
        private static readonly double POISON_DAMAGE_RATE = 0.25;
        private static readonly double ADJUST_RATE = 0.2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeadlyPoison(string name, int stateType, int maxDuration)
                     : base(name, stateType, maxDuration)
        {
            base.StateType = CStateType.POISON.VALUE;
        }

        public override IState DeepCopy()
            => new DeadlyPoison(base.Name, base.StateType, base.MaxDuration);

        public override void DisabledLogging(IMonster monster)
        {
            bool disableState = true;

            base._Log.Logging(new BattleMetaData(
                monster.MonsterId,
                disableState,
                base.Name,
                $"{monster.MonsterName}の猛毒が消えたようだ。"));

        }

        /// <summary>
        /// 強めの毒ダメージ
        /// </summary>
        public override void Impact(IMonster monster)
        {
            if (IsDisable()) return;

            // 毒ダメージ算出
            int poisonDamage = (int)(monster.MaxHp * POISON_DAMAGE_RATE);
            poisonDamage = URandom.RandomChangeInt(poisonDamage, ADJUST_RATE);


            base._Log.Logging(new BattleMetaData(monster.MonsterId, $"猛毒に侵されている　..."));
            base._Log.Logging(new BattleMetaData(
                monster.MonsterId,
                monster.Hp,
                poisonDamage,
                $"{monster.MonsterName}は{poisonDamage}ダメージを受けた。"
                ));

            // 被ダメージ
            monster.AcceptDamage(poisonDamage);

            // 早く回復することがある                        
            base.DurationCount += URandom.durationCountUp();

        }
    }
}
