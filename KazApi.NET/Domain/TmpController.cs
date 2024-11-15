using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace KazApi.Domain
{
    [Route("api/[controller]")]
    [ApiController]
    public class TmpController : ControllerBase
    {
        private readonly IConfiguration _appsettings;

        public TmpController(IConfiguration configuration)
        {
            _appsettings = configuration;
        }

        // api/Tmp
        [HttpGet]
        public ActionResult<string> GetTemp()
        {
            Console.WriteLine($"GetTemp. アクセスあり at {DateTime.Now}");

            //string url = Request.GetDisplayUrl();
            //QueryString query = Request.QueryString;

            //string? connectionStr = _appsettings["ConnectionStrings:DefaultConnection"];

            //string select = @"
            //    SELECT
            //           username
            //         , password
            //         , money
            //         , hp
            //         , mp
            //         , attack
            //         , defense
            //         , is_invalid as IsInvalid 
            //      FROM
            //           users ;
            //";

            //IDatabase postgre = new PostgreSQL(_appsettings);
            //IEnumerable<IUser> users = postgre.Select<IUser>(select);

            ////users = users.Where(e => e.IsInvalid == false);

            //string userNames = "";
            //foreach (IUser user in users)
            //{
            //    userNames += $"{user}, ";
            //}

            //string json2 = JsonConvert.SerializeObject(users);
            //Console.WriteLine(json2);
            //return json2;
            return JsonConvert.SerializeObject("{ \"a\": 1, \"b\": 2}");
        }

        
        // api/Tmp/{id}
        
        // <param name="id"></param>
        // <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> GetTemp(int id)
        {
            return $"tmp controller products: id:{id}.";
        }

        
        // api/Tmp/person/{name}
        
        // <param name="name"></param>
        // <returns></returns>
        [HttpGet("person/{name}")]
        public ActionResult<string> GetPerson(string name)
        {
            return $"person method : name:{name}.";
        }


        [HttpPost("post/{param}")]
        public string Post(string param)
        {
            string url = Request.GetDisplayUrl();
            QueryString query = Request.QueryString;
            return
                $"post method param:{param}\n"
                + $"url:{url}\n"
                + $"query:{query}";
        }
    }
}
