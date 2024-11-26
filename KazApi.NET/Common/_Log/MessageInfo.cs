using KazApi.Domain.monster;

namespace KazApi.Common._Log
{
    public class MessageInfo
    {
        private static readonly ILog<BattleMetaData> LOG = new BattleLogger();

        /// <summary>
        /// 誰のターンか表示
        /// </summary>
        public static void WhoseTurn(IMonster monster)
        {
            LOG.Logging(new BattleMetaData($"\n============================================"));
            LOG.Logging(new BattleMetaData($">>> {monster.MonsterName}のターン"));
            LOG.Logging(new BattleMetaData($"============================================\n"));
        }

        /// <summary>
        /// 戦闘結果表示
        /// </summary>
        public static void BattleResult(IEnumerable<IMonster> monsters)
        {
            bool existWinner = false;
            bool allLoser = false;

            IEnumerable<IMonster> alives = monsters.Where(e => e.Hp > 0);
            IMonster? alive = alives.Count() == 1 ? alives.Single() : null;

            if (alives.Count() == 1)
            {
                existWinner = true;

                LOG.Logging(new BattleMetaData($"\n*************************************************"));
                LOG.Logging(new BattleMetaData($"*************************************************"));
                LOG.Logging(new BattleMetaData($"  Winner {alives.Single().MonsterName} !!"));
                LOG.Logging(new BattleMetaData($"*************************************************"));
                LOG.Logging(new BattleMetaData($"*************************************************\n"));
                LOG.Logging(new BattleMetaData(existWinner, allLoser, alive));
            }
            else if (alives.Count() <= 0)
            {
                allLoser = true;
                LOG.Logging(new BattleMetaData($"... 勝者なし。"));
                LOG.Logging(new BattleMetaData(existWinner, allLoser, alive));
            }
        }
    }
}
