namespace KazApi.Common._Log
{
    /// <summary>
    /// 戦闘料用ログクラス
    /// </summary>
    public class BattleLogger : ILog<BattleMetaData>
    {
        private static IList<BattleMetaData> _cache = [];
        private static IList<BattleMetaData> _storage = [];

        /// <summary>
        /// ログを全て出力
        /// </summary>
        public IList<BattleMetaData> DumpAll() => _storage;
        /// <summary>
        /// ログを記録する
        /// </summary>
        public void Logging(BattleMetaData log)
        {
            _cache.Add(log);
            _storage.Add(log);
        }
        /// <summary>
        /// 一時的に溜めたログを出力
        /// </summary>
        public IList<BattleMetaData> DumpMemory()
        {
            IList<BattleMetaData> result = new List<BattleMetaData>(_cache);
            _cache = []; // 初期化

            return result;
        }
    }
}
