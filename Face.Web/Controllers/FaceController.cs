using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Face.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        private static string ConnString = "server=127.0.0.1;user id=root;pwd=159357;database=`face-app`;SslMode=none;allowuservariables=True;";

        // GET: api/Face
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Face/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection(ConnString);
                dynamic data = new DynamicParameters();
                if (id != null)
                {
                    data = mySqlConnection.Query("select * from `face-app`.faceinfo where FaceId = " + id);
                }
                else
                {
                    data = mySqlConnection.Query("select * from `face-app`.faceinfo Limit 0,100");
                }

                return data;// Json(data);
            }
            catch (Exception err)
            {
                throw err;
            }

            // return "value";
        }

        // POST: api/Face
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Face/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}
