using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Face.Web.Net.Controllers
{
    public class FaceController : ApiController
    {
        public FaceUtil _faceUtil;

        public FaceController(FaceUtil faceUtil)
        {
            _faceUtil = faceUtil;
        }
        private static string ConnString = "server=127.0.0.1;user id=root;pwd=159357;database=`face-app`;SslMode=none;allowuservariables=True;";

        // GET: api/Face
        //[Route("api/[controller]/GetUserList/{groupid}")]
        [HttpGet]
        public IEnumerable<string> GetUserList(string groupId)
        {
            var result = _faceUtil._faceManager.GetUserList(groupId);
            return new string[] { result };
        }

        // GET: api/Face/5
        //[Route("api/[controller]/GetUser/{userid}")]
        [HttpGet]
        public IHttpActionResult GetUser(string id)
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
                data = _faceUtil._faceManager.GetUserInfo(id, "testGroup");
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
        [HttpPut]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        //[Route("api/[controller]/Delete/{id}")]
        [HttpDelete]
        public void Delete(string userId, string groupId, string faceToken)
        {

            var result = _faceUtil._faceManager.UserFaceDelete(userId, groupId, faceToken);

        }

    }
}
