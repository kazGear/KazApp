using KazApi.Controller.Service;
using KazApi.Domain._Monster;
using System.Text.Json.Serialization;

namespace KazApi.Common._Log
{
    /// <summary>
    /// 戦闘ログクラス
    /// </summary>
    public class BattleMetaData
    {
        private static readonly int NONE = 0;
        [JsonPropertyName("TargetMonsterId")]
        public readonly string TargetMonsterId = string.Empty;
        [JsonPropertyName("BeforeHp")]
        public readonly int BeforeHp = NONE;
        [JsonPropertyName("ImpactPoint")]
        public readonly int ImpactPoint = NONE;
        [JsonPropertyName("StateName")]
        public readonly string StateName = string.Empty;
        [JsonPropertyName("EnableState")]
        public readonly bool EnableState = false;
        [JsonPropertyName("DisableState")]
        public readonly bool DisableState = false;
        [JsonPropertyName("SkillId")]
        public readonly string SkillId = string.Empty;
        [JsonPropertyName("Message")]
        public readonly string Message = string.Empty;
        [JsonPropertyName("IsStop")]
        public readonly bool IsStop = false;
        [JsonPropertyName("AllLoser")]
        public readonly bool AllLoser = false;
        [JsonPropertyName("ExistWinner")]
        public readonly bool ExistWinner = false;
        [JsonPropertyName("WinnerMonsterId")]
        public readonly string WinnerMonsterId = string.Empty;
        [JsonPropertyName("WinnerMonsterName")]
        public readonly string WinnerMonsterName = NONE.ToString();

        /// <summary>
        /// コンストラクタ（ログ区切りのマーカー）
        /// </summary>
        public BattleMetaData()
        {
            IsStop = true;
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
            string targetMonsterId,
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
            string targetMonsterId,
            string skillId,
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
            string targetMonsterId,
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
            string targetMonsterId,
            string skillId,
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
            string targetMonsterId,
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
            string targetMonsterId,
            int beforeHp,
            int impactPoint,
            string skillId,
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
