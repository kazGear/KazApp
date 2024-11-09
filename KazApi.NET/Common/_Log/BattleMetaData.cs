using KazApi.Domain._Monster;

namespace KazApi.Common._Log
{
    /// <summary>
    /// 戦闘ログクラス
    /// </summary>
    public class BattleMetaData
    {
        private static readonly int NONE = 0;
        public readonly int TargetMonsterId = NONE;
        public readonly int BeforeHp = NONE;
        public readonly int ImpactPoint = NONE;
        public readonly string StateName = string.Empty;
        public readonly bool EnableState = false;
        public readonly bool DisableState = false;
        public readonly int SkillId = NONE;
        public readonly string Message = string.Empty;
        public readonly bool IsStop = false;
        public readonly bool AllLoser = false;
        public readonly bool ExistWinner = false;
        public readonly int WinnerMonsterId = NONE;
        public readonly string WinnerMonsterName = NONE.ToString();

        /// <summary>
        /// コンストラクタ（ログ区切りのマーカー）
        /// </summary>
        public BattleMetaData(bool isStop = true)
        {
            IsStop = isStop;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleMetaData(string message, bool isStop = false)
        {
            Message = message;
            IsStop = isStop;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleMetaData(
            int targetMonsterId,
            string message,
            bool isStop = false)
        {
            TargetMonsterId = targetMonsterId;
            Message = message;
            IsStop = isStop;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleMetaData(
            int targetMonsterId,
            bool disableState,
            string stateName,
            string message,
            bool isStop = false)
        {
            TargetMonsterId = targetMonsterId;
            DisableState = disableState;
            StateName = stateName;
            Message = message;
            IsStop = isStop;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleMetaData(
            int targetMonsterId,
            int skillId,
            string message,
            bool isStop = false)
        {
            TargetMonsterId = targetMonsterId;
            SkillId = skillId;
            Message = message;
            IsStop = isStop;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleMetaData(
            int targetMonsterId,
            int skillId,
            string stateName,
            bool enableState,
            string message,
            bool isStop = false)
        {
            TargetMonsterId = targetMonsterId;
            SkillId = skillId;
            StateName = stateName;
            EnableState = enableState;
            Message = message;
            IsStop = isStop;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleMetaData(
            int targetMonsterId,
            int beforeHp,
            int impactPoint,
            string message,
            bool isStop = false)
        {
            TargetMonsterId = targetMonsterId;
            BeforeHp = beforeHp;
            ImpactPoint = impactPoint;
            Message = message;
            IsStop = isStop;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleMetaData(
            int targetMonsterId,
            int beforeHp,
            int impactPoint,
            int skillId,
            string message,
            bool isStop = false)
        {
            TargetMonsterId = targetMonsterId;
            BeforeHp = beforeHp;
            ImpactPoint = impactPoint;
            SkillId = skillId;
            Message = message;
            IsStop = isStop;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleMetaData(bool existWinner, bool allLoser, IMonster? alive)
        {
            ExistWinner = existWinner;
            AllLoser = allLoser;

            if (alive != null)
            {
                TargetMonsterId = alive.MonsterId;
                WinnerMonsterId = alive.MonsterId;
                WinnerMonsterName = alive.MonsterName;
            }
            IsStop = true;
        }

        public override string ToString() => Message;
    }
}
