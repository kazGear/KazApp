using KazApi.Common._Const;
using KazApi.Domain._GameSystem;
using KazApi.Domain.monster._State;
using KazApi.DTO;

namespace KazApi.Domain.monster._Skill
{
    /// <summary>
    /// 状態スキルクラス
    /// </summary>
    public class StateSkill : ISkill
    {
        private static readonly IEnumerable<CStateType> POSITIVE_SKILLS = [
            CStateType.AUTOHEAL, CStateType.POWERUP, CStateType.DODGEUP, CStateType.CRITICALUP
            ];
        private IState _state;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StateSkill(SkillDTO dto, string effectFilePath, IState state)
                   : base(dto, effectFilePath)
        {
            _state = state;
        }

        public override void Use(IEnumerable<IMonster> monsters, IMonster me)
        {
            bool isPositive = POSITIVE_SKILLS.Where(e => e.VALUE == _state.StateType)
                                             .Count() >= 1;

            if (isPositive) // 有利スキル
            {
                GivePositiveState(me);
            }
            else // 状態異常スキル
            {
                GiveNegativeState(monsters, me);
            }
        }

        /// <summary>
        /// 良い状態を与える
        /// </summary>
        private void GivePositiveState(IMonster me)
        {
            me.AcceptState(_state!.DeepCopy(), this);
        }

        /// <summary>
        /// 悪い状態を与える
        /// </summary>
        private void GiveNegativeState(IEnumerable<IMonster> monsters, IMonster me)
        {
            if (TargetType == CTarget.ENEMY_RANDOM.VALUE)
            {
                // 単体付与
                IMonster enemy = BattleSystem.SelectOneEnemy(monsters.ToList());
                enemy.AcceptState(_state.DeepCopy(), this);
            }
            else
            {
                foreach (IMonster enemy in monsters)
                    enemy.AcceptState(_state.DeepCopy(), this);
            }
        }

    }
}
