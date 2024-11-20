using KazApi.Domain._Monster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KazApi.Common._Log;
using KazApi.Common._Const;
using KazApi.Domain._ViewModel;
using KazApi.DTO;
using KazApi.Controller.Service;



namespace KazApi.Controller
{
    [ApiController]
    public class BattleController : ControllerBase
    {
        private readonly ILog<BattleMetaData> _logger;
        private readonly BattleService _service;

        public BattleController(IConfiguration configuration)
        {
            _logger = new BattleLogger();
            _service = new BattleService(configuration);
        }

        /// <summary>
        /// CORS対策
        /// </summary>

        //[HttpOptions("api/*")]
        //public IActionResult Preflight()
        //{
        //    Response.Headers.Add("Access-Control-Allow-Origin", "*");
        //    Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
        //    Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        //    return NoContent();
        //}

        /// <summary>
        /// 初期処理
        /// </summary>
        [HttpPost("api/battle/init")]
        public ActionResult<string> Init([FromQuery] int selectMonstersCount)
        {
            try
            {
                // モンスターデータ等の読込み
                IEnumerable<MonsterDTO> monstersFromDB = _service.SelectMonsters();
                IEnumerable<SkillDTO> skillsFromDB = _service.SelectSkills();
                IEnumerable<MonsterSkillDTO> monsterSkillFromDB = _service.SelectMonsterSkills();

                // モンスターDTO構築
                IEnumerable<MonsterDTO> monstersDTO =
                    _service.MappingToMonsterDTO(monstersFromDB, skillsFromDB, monsterSkillFromDB);

                // 参加モンスター（ランダム）
                IEnumerable<MonsterDTO> battleMonsters
                    = _service.MonsterSelector(monstersDTO, selectMonstersCount);

                // 賭けレート算出
                _service.CalcBetRate(battleMonsters);

                // テスト用モンスターで対戦
                //IEnumerable<MonsterDTO> testMonsters = new List<MonsterDTO>()
                //{
                //    monstersDTO.Where(e => e.MonsterId == ((int)CMonster.イビルシャーマン)).Single(),
                //    monstersDTO.Where(e => e.MonsterId == ((int)CMonster.エレメントソード)).Single(),
                //    monstersDTO.Where(e => e.MonsterId == ((int)CMonster.グリーンスライム)).Single(),
                //    monstersDTO.Where(e => e.MonsterId == ((int)CMonster.カーミラクイーン)).Single(),
                //    monstersDTO.Where(e => e.MonsterId == ((int)CMonster.セイレーン)).Single(),
                //};
                //battleMonsters = testMonsters;

                return JsonConvert.SerializeObject(battleMonsters); ;
            }
            catch (Exception e)
            {
                return e.Message;
            }           
        }

        /// <summary>
        /// モンスターたちの行動
        /// </summary>
        [HttpPost("api/battle/nextTurn")]
        public ActionResult<string> NextTurn([FromBody] IEnumerable<MonsterDTO> monsters)
        {
            // 戦闘用モンスターを構築
            IEnumerable<IMonster> battleMonsters = _service.CreateBattleMonsters(monsters);

            // TODO 未実装 チーム決め
            ((List<IMonster>)battleMonsters).ForEach(e => e.DefineTeam(CTeam.A.VALUE));
            if (battleMonsters.Where(e => e.Team == CTeam.UNKNOWN.VALUE).Count() > 0)
            {
                throw new Exception("チーム決めが完了していません。");
            }
            // 行動順決め
            IEnumerable<IMonster> orderedMonsters = _service.ActionOrder(battleMonsters);

            // モンスターの行動
            foreach (IMonster me in orderedMonsters)
            {
                if (me.Hp <= 0)
                {
                    continue;
                }

                // 誰のターンか
                _service.WhoseTurn(me);
               
                // 状態異常の影響
                me.StateImpact();

                // モンスターの行動
                IList<IMonster> otherMonsters = orderedMonsters.Where(e => e.MonsterId != me.MonsterId).ToList();
                if (me.IsMoveAble())
                    me.Move(otherMonsters);

                // 状態異常解除
                _service.DisabledStatus(me);
            }

            // 勝敗判定
            _service.BattleResult(battleMonsters);

            // DTOへ変換
            IEnumerable<MonsterDTO> monstersDTO = _service.ConvertToDTO(battleMonsters);

            BattleViewModel model = new BattleViewModel();
            model.Monsters = monstersDTO;
            model.BattleLog = _logger.DumpMemory(); Console.WriteLine();

            Console.WriteLine($"ログ数：{model.BattleLog.Count()}");
            foreach (var e in model.BattleLog) Console.WriteLine(e); // tmp


            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// 勝敗結果を記録
        /// </summary>
        [HttpPost("api/battle/recordResults")]
        public ActionResult<bool> RecordResults([FromBody] IEnumerable<MonsterDTO> monsters)
        {
            DateTime endDate = DateTime.Now;
            TimeSpan endTime = new TimeSpan(endDate.Ticks);

            return _service.InsertBattleResult(monsters, endDate, endTime);
        }
    }
}
