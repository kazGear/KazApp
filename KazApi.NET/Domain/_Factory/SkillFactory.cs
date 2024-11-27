using KazApi.Common._Const;
using KazApi.Domain._Monster._Skill;
using KazApi.DTO;


namespace KazApi.Domain._Factory
{
    /// <summary>
    /// スキル生成クラス
    /// </summary>
    public class SkillFactory
    {
        private IList<ISkill> _skills = [];
        private StateFactory _stateFactory;
    
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SkillFactory(IEnumerable<CodeDTO> codeEntities)
        {         
            _skills = new List<ISkill>();
            _stateFactory = new StateFactory(codeEntities);
        }
        /// <summary>
        /// スキルを生成
        /// </summary>
        public IEnumerable<ISkill> Create(IEnumerable<SkillDTO> entities)
        {
            foreach (SkillDTO skill in entities)
            {
                CreateSkill(skill);
            }
            IEnumerable<ISkill> result = new List<ISkill>(_skills);
            _skills = [];
            
            return result;
        }
        /// <summary>
        /// 各種スキルを生成
        /// </summary>
        private void CreateSkill(SkillDTO skill)
        {
            if (skill.StateType != CStateType.NONE.VALUE)
                // 状態スキル
                CreateStateSkill(skill);
            else if (skill.SkillType == CSkillType.HEAL.VALUE)
                // 回復スキル
                CreateHealSkill(skill);
            else if (skill.SkillType == CSkillType.ATTACK_RATE.VALUE)
                // 割合ダメージスキル
                CreateRatioAttackSkill(skill);
            else if (skill.SkillType == CSkillType.DEAD.VALUE)
                // 即死攻撃スキル
                CreateDeadSkill(skill);
            else if (skill.SkillType == CSkillType.NOT_MOVE.VALUE)
                // 行動しないスキル
                CreateNotMoveSkill(skill);
            else if (   skill.TargetType == CTarget.ENEMY_RANDOM.VALUE
                     || skill.TargetType == CTarget.ENEMY_ALL.VALUE
                     || skill.TargetType == CTarget.ENEMY_RANDOM_OR_ALL.VALUE)
                // 攻撃スキル
                CreateDamageSkill(skill);
            else
                throw new Exception($"{skill.SkillName}: スキルがどのタイプにも属していません。");
        }
        /// <summary>
        /// 攻撃スキル生成
        /// </summary>
        private void CreateDamageSkill(SkillDTO skill)
        {
            ISkill result = new DamageSkill(skill, ""); // TODO エフェクト画像のファイルパス 
            _skills.Add(result);
        }
        /// <summary>
        /// 状態スキルを生成
        /// </summary>
        private void CreateStateSkill(SkillDTO skill)
        {
            ISkill result = new StateSkill(skill, "", _stateFactory.Create(skill.StateType));
            // TODO エフェクト画像のファイルパス
            _skills.Add(result);
        }
        /// <summary>
        /// 回復スキルを生成
        /// </summary>
        private void CreateHealSkill(SkillDTO skill)
        {
            ISkill result = new HealSkill(skill, ""); // TODO エフェクト画像のファイルパス
            _skills.Add(result);
        }
        /// <summary>
        /// 無害なスキルを生成
        /// </summary>
        private void CreateNotMoveSkill(SkillDTO skill)
        {
            ISkill result = new NoMoveSkill(skill, ""); // TODO エフェクト画像のファイルパス
            _skills.Add(result);
        }
        /// <summary>
        /// 割合ダメージスキルを生成
        /// </summary>
        private void CreateRatioAttackSkill(SkillDTO skill)
        {
            ISkill result = new RateDamageSkill(skill, ""); // TODO エフェクト画像のファイルパス
            _skills.Add(result);
        }
        /// <summary>
        /// 即死スキルを生成
        /// </summary>
        private void CreateDeadSkill(SkillDTO skill)
        {
            ISkill result = new DeadSkill(skill, ""); // TODO エフェクト画像のファイルパス
            _skills.Add(result);
        }
    }
}
