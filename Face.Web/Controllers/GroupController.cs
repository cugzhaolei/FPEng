using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Face.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Face.Web.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        public FaceUtil _faceUtil;
        public GroupController(FaceUtil faceUtil)
        {
            faceUtil = _faceUtil;
        }

        // GET: api/Group
        //[Route("api/[controller]/GetGroupList")]
        [HttpGet]
        public IEnumerable<string> GetGroupList()
        {
            var result = _faceUtil._faceManager.GetGroupList();
            return new string[] { result };
        }

        // GET: api/Group/5
        //[Route("api/[controller]/GetGroupUsers/{id}")]
        [HttpGet("{id}")]
        public string GetGroupUsers(string id)
        {
            var result = _faceUtil._faceManager.GetGroupList();
            foreach(var face in result)
            {
            }
            return result.ToString();
        }

        [HttpPost]
        //[Route("api/[controller]/Add")]
        public string Add(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _faceUtil._faceManager.GroupAdd(value);
            }
            else
            {
                return "no group id";
            }
            return "add group success";
        }
        // POST: api/Group
        [HttpPost]
        public string Post([FromBody] string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _faceUtil._faceManager.GroupAdd(value);
            }
            else
            {
                return "no group id";
            }
            return "add group success";
        }

        // PUT: api/Group/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        //[Route("api/[controller]/Delete/{id}")]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
           var result = _faceUtil._faceManager.GroupDelete(id);
            Console.WriteLine("groupDelete",result);
        }
    }
}
