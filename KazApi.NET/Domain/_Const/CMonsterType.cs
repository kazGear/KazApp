namespace KazApi.Domain._Const
{
    /// <summary>
    /// モンスタータイプ定数
    /// </summary>
    public class CMonsterType : Enumeration<int>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private CMonsterType(int value, string name) : base(value, name) { }

        public static readonly CMonsterType キラービー = new(1, "キラービー");
        public static readonly CMonsterType カーミラ = new(2, "カーミラ");
        public static readonly CMonsterType デーモン = new(3, "デーモン");
        public static readonly CMonsterType ゴブリン = new(4, "ゴブリン");
        public static readonly CMonsterType マシンゴーレム = new(5, "マシンゴーレム");

        public static readonly CMonsterType ハーピー = new(6, "ハーピー");
        public static readonly CMonsterType アーマーナイト = new(7, "アーマーナイト");
        public static readonly CMonsterType マジシャン = new(8, "マジシャン");
        public static readonly CMonsterType マイコニド = new(9, "マイコニド");
        public static readonly CMonsterType ニードルバード = new(10, "ニードルバード");

        public static readonly CMonsterType プチドラゴン = new(11, "プチドラゴン");
        public static readonly CMonsterType ポト = new(12, "ポト");
        public static readonly CMonsterType プリースト = new(13, "プリースト");
        public static readonly CMonsterType ラビ = new(14, "ラビ");
        public static readonly CMonsterType グリーンスライム = new(15, "グリーンスライム");

        public static readonly CMonsterType イビルソード = new(16, "イビルソード");
        public static readonly CMonsterType ウルフ = new(17, "ウルフ");
        public static readonly CMonsterType ダック = new(18, "ダック");
        public static readonly CMonsterType モールベア = new(19, "モールベア");
        public static readonly CMonsterType ギャルビー = new(20, "ギャルビー");

        public static readonly CMonsterType サハギン = new(21, "サハギン");
        public static readonly CMonsterType クロウラー = new(22, "クロウラー");
        public static readonly CMonsterType パックン = new(23, "パックン");
        public static readonly CMonsterType チビデビル = new(24, "チビデビル");
        public static readonly CMonsterType オーガボックス = new(25, "オーガボックス");

        public static readonly CMonsterType バレッテ = new(26, "バレッテ");
        public static readonly CMonsterType バシリスク = new(27, "バシリスク");
        public static readonly CMonsterType スペクター = new(28, "スペクター");
        public static readonly CMonsterType ユニコーンヘッド = new(29, "ユニコーンヘッド");
        public static readonly CMonsterType シェイプシフター = new(30, "シェイプシフター");

        public static readonly CMonsterType ボルダー = new(31, "ボルダー");
        public static readonly CMonsterType パンプキンボム = new(32, "パンプキンボム");

        // ユーザ登録初期から登場するモンスターたち
        public static readonly IList<CMonsterType> START_UP = new List<CMonsterType>
        {
            ラビ,
            カーミラ,
            キラービー,
            ゴブリン,
            マシンゴーレム,
            ハーピー,
            アーマーナイト,
            マジシャン,
            マイコニド,
            ニードルバード
        };

    }
}
