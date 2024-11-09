using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.DTO;
using KazApi.Lib;

namespace KazApi.Domain._Monster._State
{
    /// <summary>
    /// 毒状態クラス
    /// </summary>
    public class Poison : IState
    {
        private static readonly double POISON_DAMAGE_RATE = 0.1;
        private static readonly double ADJUST_RATE = 0.4;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Poison(StateDTO dto) : base(dto) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Poison(string name, int stateType, int maxDuration) 
               : base(name, stateType, maxDuration)
        {
            base.StateType = ((int)CStateType.POISON);
        }

        public override IState DeepCopy()
            => new Poison(base.Name, base.StateType, base.MaxDuration);

        public override void DisabledLogging(IMonster monster)
        {
            bool disableState = true;

            base._Log.Logging(new BattleMetaData(
                monster.MonsterId,
                disableState,
                base.Name,
                $"{monster.MonsterName}の毒が消えたようだ。"));

        }

        /// <summary>
        /// 毒ダメージを受ける
        /// </summary>
        public override void Impact(IMonster monster)
        {
            if (IsDisable()) return;

            // 毒ダメージ算出
            int poisonDamage = (int)(monster.MaxHp * POISON_DAMAGE_RATE);
            poisonDamage = URandom.RandomChangeInt(poisonDamage, ADJUST_RATE);


            base._Log.Logging(new BattleMetaData(monster.MonsterId, $"毒がまわってきた。。。"));
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
