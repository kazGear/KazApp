using KazApi.Common._Const;
using KazApi.Domain.monster;
using KazApi.Domain.monster._Skill;
using KazApi.Domain.monster._State;
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

        /// <summary>
        /// モンスターとスキルのマッピング（DTO）
        /// </summary>
        public IEnumerable<MonsterDTO> MappingToMonsterDTO(
            IEnumerable<MonsterDTO> monstersDTO,
            IEnumerable<SkillDTO> skillsDTO,
            IEnumerable<MonsterSkillDTO> monsterSkillsDTO
            )
        {
            IList<MonsterDTO> result = [];

            foreach (MonsterDTO m in monstersDTO)
            {
                // モンスターのスキル対応表を取得
                IEnumerable<MonsterSkillDTO> skillMap =
                    monsterSkillsDTO.Where(e => e.MonsterId == m.MonsterId);

                IList<SkillDTO> bindSkills = [];

                foreach (MonsterSkillDTO ms in skillMap)
                {
                    // 対応表を元にスキルを設定
                    SkillDTO skill = skillsDTO.Where(e => e.SkillId == ms.SkillId).Single();
                    bindSkills.Add(skill);
                }
                m.Skills = bindSkills;
                m.Status = [];
                result.Add(m);
            }
            return result;
        }

        /// <summary>
        /// 戦闘用モンスターオブジェクトを構築
        /// </summary>
        public IEnumerable<IMonster> CreateBattleMonsters(
            IEnumerable<MonsterDTO> monsters,
            IEnumerable<CodeDTO> codes)
        {
            SkillFactory skillFactory = new SkillFactory(codes);
            StateFactory stateFactory = new StateFactory(codes);

            // バトルモンスター構築
            IList<IMonster> battleMonsters = [];
            foreach (MonsterDTO m in monsters)
            {
                IEnumerable<ISkill> skills = skillFactory.Create(m.Skills);

                ISet<IState> status = new HashSet<IState>();
                foreach (StateDTO state in m.Status)
                {
                    // 同じ状態は追加しない
                    status.Add(stateFactory.Create(state.StateType, state));
                }
                // スキル、ステータスを設定
                IMonster battleMonster = new Monster(m, skills, status);
                battleMonsters.Add(battleMonster);
            }
            return battleMonsters;
        }

        /// <summary>
        /// モデルからDTOへ変換
        /// </summary>
        public IEnumerable<MonsterDTO> ConvertToDTO(IEnumerable<IMonster> battleMonsters)
        {
            IList<MonsterDTO> monstersDTO = [];
            foreach (IMonster m in battleMonsters)
            {
                IEnumerable<SkillDTO> skillsDTO = SkillDTO.ModelToDTO(m.CurrentSkills());
                IEnumerable<StateDTO> statusDTO = StateDTO.ModelToDTO(m.CurrentStatus());

                MonsterDTO monsterDTO = new MonsterDTO(m);
                monsterDTO.Skills = skillsDTO;
                monsterDTO.Status = statusDTO;
                monsterDTO.StatusName = CStateName.ConvertTypeToName(statusDTO);

                monstersDTO.Add(monsterDTO);
            }
            return monstersDTO;
        }

    }
}
