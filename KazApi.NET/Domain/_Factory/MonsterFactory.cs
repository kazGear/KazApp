using KazApi.Domain._Monster;
using KazApi.Domain._Monster._Skill;
using KazApi.DTO;

namespace KazApi.Domain._Factory
{
    /// <summary>
    /// モンスター生成クラス
    /// </summary>
    public class MonsterFactory
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MonsterFactory()
        {

        }
        /// <summary>
        /// モンスターオブジェクト（スキルなし）を構築
        /// </summary>
        public IEnumerable<IMonster> CreateNoSkillModel(IEnumerable<MonsterDTO> monsters)
        {
            IList<IMonster> result = [];

            foreach (MonsterDTO dto in monsters)
            {
                // スキル無しのモンスター
                IMonster monster = new Monster(dto, [], []);
                result.Add(monster);
            }
            return result;
        }
                
        /// <summary>
        /// モンスターオブジェクト（スキル付き）を構築
        /// </summary>
        public IEnumerable<IMonster> CreateModel(
            IEnumerable<MonsterDTO> monsters,
            IEnumerable<ISkill> skills,
            IEnumerable<MonsterSkillDTO> monsterSkills
            )
        {
            IList<IMonster> result = [];

            foreach (MonsterDTO monster in monsters)
            {
                // モンスターデフォルトのスキル
                IEnumerable<MonsterSkillDTO> targetSkill =
                    monsterSkills.Where(e => e.MonsterId == monster.MonsterId);

                // デフォルトスキルをスキル群から探す
                IList<ISkill> skillsForMonster = [];
                foreach (MonsterSkillDTO dto in targetSkill)
                {
                    ISkill skill = skills.Where(e => e.SkillId == dto.SkillId).Single();
                    skillsForMonster.Add(skill);
                }

                IMonster monsterModel = new Monster(monster, skillsForMonster, []);
                result.Add(monsterModel);
            }
            return result;
        }
    }
}
