using KazApi.Common._Const;
using KazApi.Domain._Monster._Skill;
using KazApi.Domain._Monster._State;
using KazApi.DTO;

namespace KazApi.Domain._Factory
{
    /// <summary>
    /// スキル生成クラス
    /// </summary>
    public class StateFactory
    {
        private readonly IList<ISkill> _skills = new List<ISkill>();
        private IEnumerable<CodeDTO> _codeEntities;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StateFactory(IEnumerable<CodeDTO> codeEntities)
        {
            _skills = new List<ISkill>();

            // 状態コード取得
            _codeEntities = codeEntities;
        }

        /// <summary>
        /// 状態オブジェクトを作成
        /// </summary>
        public IState Create(int stateCode)
        {
            CodeDTO param = _codeEntities.Where(e => e.Value == stateCode).Single();

            if (stateCode == CStateType.NONE.VALUE)
                return new None(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.POISON.VALUE)
                return new Poison(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.SLEEP.VALUE)
                return new Sleep(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.CHARM.VALUE)
                return new Charm(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.SLOW.VALUE)
            {
                Console.WriteLine($"対応していないコードです：{CStateType.SLOW}");
                return new None(param.Name, param.Value, param.Param2);
            }

            //    return new Slow(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.POWERUP.VALUE)
            {
                Console.WriteLine($"対応していないコードです：{CStateType.POWERUP}");
                return new None(param.Name, param.Value, param.Param2);
            }
            //    return new PowerUp(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.DODGEUP.VALUE)
            {
                Console.WriteLine($"対応していないコードです：{CStateType.DODGEUP}");
                return new None(param.Name, param.Value, param.Param2);
            }
            //    return new DodgeUp(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.CRITICALUP.VALUE)
            {
                Console.WriteLine($"対応していないコードです：{CStateType.CRITICALUP}");
                return new None(param.Name, param.Value, param.Param2);
            }
            //    return new CriticalUp(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.AUTOHEAL.VALUE)
                return new AutoHeal(param.Name, param.Value, param.Param2);
            else
                throw new Exception("存在しない状態コードです。");
        }

        /// <summary>
        /// 状態オブジェクトを作成（DTOより）
        /// </summary>
        public IState Create(int stateCode, StateDTO dto)
        {
            CodeDTO param = _codeEntities.Where(e => e.Value == stateCode).Single();

            if (stateCode == CStateType.NONE.VALUE)
                return new None(dto);
            else if (stateCode == CStateType.POISON.VALUE)
                return new Poison(dto);
            else if (stateCode == CStateType.SLEEP.VALUE)
                return new Sleep(dto);
            else if (stateCode == CStateType.CHARM.VALUE)
                return new Charm(dto);
            //else if (stateCode == ((int)CStateType.SLOW))
            //    return new Slow(param.Name, param.Value, param.Param2);
            //else if (stateCode == ((int)CStateType.POWERUP))
            //    return new PowerUp(param.Name, param.Value, param.Param2);
            //else if (stateCode == ((int)CStateType.DODGEUP))
            //    return new DodgeUp(param.Name, param.Value, param.Param2);
            //else if (stateCode == ((int)CStateType.CRITICALUP))
            //    return new CriticalUp(param.Name, param.Value, param.Param2);
            else if (stateCode == CStateType.AUTOHEAL.VALUE)
                return new AutoHeal(dto);
            else
                throw new Exception("存在しない状態コードです。");
        }
    }
}
