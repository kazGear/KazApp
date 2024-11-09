//using KazApi.Common._Const;
//using KazApi.Common._Log;

//namespace KazApi.Domain._Monster._State
//{
//    /// <summary>
//    /// 攻撃力UP状態クラス
//    /// </summary>
//    public class PowerUp : IState
//    {
//        private static readonly double ATTACK_GAIN = 1.0;
//        private bool _firstTime = true;

//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        public PowerUp(string name, int stateType, int maxDuration)
//                : base(name, stateType, maxDuration) 
//        {
//            base.StateType = ((int)CStateType.POWERUP);
//        }

//        public override IState DeepCopy()
//            => new PowerUp(base.Name, base.StateType, base.MaxDuration);

//        public override void DisabledLogging(IMonster monster)
//            => base._Log.Logging(
//                new BattleMetaData(
//                    monster.MonsterId,
//                    $"{monster.MonsterName}の攻撃力が元に戻った。"));

//        /// <summary>
//        /// 攻撃力を上昇させる
//        /// </summary>
//        public override void Impact(IMonster monster)
//        {
//            //monster.InitAttack();
//            //if (IsDisable()) return;

//            //double gain = (double)monster.Attack * ATTACK_GAIN;
//            //monster.ChangeAttack(monster.Attack + (int)gain);
//            //_firstTime = false;

//            //base._Log.Logging($"{monster.Name}の攻撃力は上昇している！");

//            //base._durationCount++;

//        }

//    }
//}
