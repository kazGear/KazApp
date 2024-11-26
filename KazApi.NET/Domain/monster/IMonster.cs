using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain.monster._Skill;
using KazApi.Domain.monster._State;
using KazApi.DTO;

namespace KazApi.Domain.monster
{
    /// <summary>
    /// モンスターインターフェイス
    /// </summary>
    public abstract class IMonster
    {
        protected readonly ILog<BattleMetaData> _Log = new BattleLogger();
        protected ISet<IState> _status = new HashSet<IState>();
        protected IList<ISkill> _skills = new List<ISkill>();
        protected int _defaultAttack;

        public int MonsterId { get; protected set; }
        public string MonsterName { get; protected set; }
        public int MonsterType { get; protected set; }
        public int Hp { get; protected set; }
        public int MaxHp { get; protected set; } = 0;
        public int Attack { get; protected set; }
        public int Speed { get; protected set; }
        public int Team { get; protected set; }
        public int Week { get; protected set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IMonster(MonsterDTO dto, IEnumerable<ISkill> skills, IEnumerable<IState> status)
        {
            MonsterId = dto.MonsterId;
            MonsterName = dto.MonsterName;
            Hp = dto.Hp;
            if (MaxHp == 0) MaxHp = dto.MaxHp;
            Attack = dto.Attack;
            _defaultAttack = dto.Attack;
            Speed = dto.Speed;
            foreach (IState state in status) _status.Add(state);
            foreach (ISkill skill in skills) _skills.Add(skill);
            Team = CTeam.UNKNOWN.VALUE;
            Week = dto.Week;
        }
        /// <summary>
        /// 行動する
        /// </summarys>
        public void Move(IList<IMonster> monsters)
        {
            // 戦闘不能中は攻撃不可
            if (Hp <= 0) return;
            // 攻撃対象がい
            if (monsters.Where(e => e.Hp > 0).Count() <= 0) return;

            ISkill skill = SelectSkill();

            if (skill is IPositiveSkill)
            {
                PositiveMove(monsters, skill);
            }
            else
            {
                AttackMove(monsters, skill);
            }
        }

        /// <summary>
        /// 攻撃する
        /// </summary>
        protected abstract void AttackMove(IEnumerable<IMonster> monsters, ISkill skill);

        /// <summary>
        /// 有利な行動
        /// </summary>
        protected abstract void PositiveMove(IEnumerable<IMonster> monsters, ISkill skill);

        /// <summary>
        /// スキルを選択
        /// </summary>
        public abstract ISkill SelectSkill();

        /// <summary>
        /// 現在のスキルを見る
        /// </summary>
        public IEnumerable<ISkill> CurrentSkills() => new List<ISkill>(_skills);

        /// <summary>
        /// 現在のステータスを見る
        /// </summary>
        public IEnumerable<IState> CurrentStatus() => new List<IState>(_status);

        /// <summary>
        /// ステータスを更新する
        /// </summary>
        public void UpdateStatus(ISet<IState> changedStatus) => _status = changedStatus;

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        public void AcceptDamage(int damage) => Hp -= damage;

        /// <summary>
        /// スキルを設定する
        /// </summary>
        public void SetSkill(IList<ISkill> skills) => _skills = skills;

        /// <summary>
        /// チームを決定する
        /// </summary>
        public void DefineTeam(int team) => Team = team;

        // 攻撃力を変更する
        //public void ChangeAttack(int attack) => Attack = attack;

        // 攻撃力を戻す
        //public void InitAttack() => Attack = _defaultAttack;

        /// <summary>
        /// 状態異常になる
        /// </summary>
        public void AcceptState(IState state, ISkill skill)
        {
            // 状態異常は重複しない
            if (_status.Where(e => e.GetStateType() == state.GetStateType()).Count() <= 0)
            {
                bool enableState = true;

                _status.Add(state);
                _Log.Logging(new BattleMetaData(
                    MonsterId,
                    skill.SkillId,
                    state.Name,
                    enableState,
                    $"{MonsterName}は{state.StateName()}状態になった。")
                    ); ;
            }
            else
            {
                _Log.Logging(new BattleMetaData(MonsterId, $"{MonsterName}は既に{state.StateName()}状態になっている。"));
            }
        }

        /// <summary>
        /// 状態異常が消滅する
        /// </summary>
        public void LostState(IState state) => ((IList<IState>)_status).Remove(state);

        /// <summary>
        /// 状態異常の効果を受ける
        /// </summary>
        public void StateImpact()
        {
            // 変化したコレクションの操作は例外となる > 変化していないコレクションを操作して回避
            IEnumerable<IState> copyStatus = new List<IState>(_status);
            foreach (IState state in copyStatus) state.Impact(this);
        }

        /// <summary>
        /// 行動できる状態か判定
        /// </summary>
        public bool IsMoveAble()
        {
            int notMove = _status.Where(e => e is IDisableMove)
                                 .Count();
            return notMove <= 0;
        }
    }
}