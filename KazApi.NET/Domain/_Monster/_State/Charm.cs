using CSLib.Lib;
using KazApi.Common._Log;
using KazApi.Domain._Const;
using KazApi.Domain._Monster._Skill;
using KazApi.Domain.DTO;

namespace KazApi.Domain._Monster._State
{
    /// <summary>
    /// 魅了状態クラス
    /// </summary>
    public class Charm : IState, IDisableMove
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Charm(StateDTO dto) : base(dto) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Charm(string name, int stateType, int maxDuration)
              : base(name, stateType, maxDuration) { }

        public override IState DeepCopy()
            => new Charm(Name, StateType, MaxDuration);

        public override void DisabledLogging(IMonster monster)
        {
            bool disableState = true;

            _Log.Logging(new BattleMetaData(
                monster.MonsterId,
                disableState,
                Name,
                $"{monster.MonsterName}は我に返った！")
                );

        }

        /// <summary>
        /// 自身に攻撃
        //  有利な効果は使用しない
        /// </summary>
        public override void Impact(IMonster me)
        {
            if (IsDisable()) return;

            // 睡眠時は発動しない
            int sleepCnt = me.CurrentStatus()
                             .Where(e => e.StateType == CStateType.SLEEP.VALUE)
                             .Count();
            if (sleepCnt >= 1)
            {
                DurationCount++;
                return;
            }

            ISkill skill = me.SelectSkill();
            while (skill is HealSkill || skill is NoMoveSkill) // 選び直し
                skill = me.SelectSkill();

            // 自傷
            _Log.Logging(new BattleMetaData(me.MonsterId, $"{me.MonsterName}は自分に攻撃！"));
            _Log.Logging(new BattleMetaData(me.MonsterId, $"{me.MonsterName}は {skill.SkillName} を放った！"));
            skill.Use([me], me);

            // 魅了回数減少
            DurationCount += URandom.durationCountUp();
        }
    }
}
