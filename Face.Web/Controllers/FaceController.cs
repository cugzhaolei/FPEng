using System;
using System.Collections.Generic;
using Dapper;
using Face.Web.Core.FaceAI;
using Face.Web.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Face.Web.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// face management
    /// </summary>
    public class FaceController : ControllerBase
    {
        FaceUtil _faceUtil;
        private readonly FaceManager faceManager = new FaceManager();
        private readonly FaceCompare faceCompare = new FaceCompare();
        private readonly FaceTrack faceTrack = new FaceTrack();
        /// <summary>
        /// face management controller
        /// </summary>
        /// <param name="faceUtil"></param>
        public FaceController()
        {
            FaceUtil faceUtil = new FaceUtil();
            _faceUtil = faceUtil;
        }
        private static string ConnString = "server=127.0.0.1;user id=root;pwd=159357;database=`face-app`;SslMode=none;allowuservariables=True;";

        // GET: api/Face
        [Route("GetUserList")]
        [HttpGet]
        public IEnumerable<string> GetUserList(string groupId)
        {
            // FaceManager faceManager = new FaceManager();
            var result = faceManager.GetUserList(groupId);
            return new string[] { result };
        }
        /// <summary>
        /// 1:N 人脸搜索接口
        /// </summary>
        /// <param name="image">图片信息(**总数据大小应小于10M**)，图片上传方式根据image_type来判断</param>
        /// <param name="imageType">图片类型     <br> **BASE64**:图片的base64值，base64编码后的图片数据，编码后的图片大小不超过2M； <br>**URL**:图片的 URL地址( 可能由于网络等原因导致下载图片时间过长)； <br>**FACE_TOKEN**: 人脸图片的唯一标识，调用人脸检测接口时，会为每个人脸图片赋予一个唯一的FACE_TOKEN，同一张图片多次检测得到的FACE_TOKEN是同一个。</param>
        /// <param name="groupIdList">从指定的group中进行查找 用逗号分隔，**上限20个**</param>
        /// <param name="options"> 可选参数对象，key: value都为string类型，可选的参数包括
        ///     <list type="bullet">
        ///           <item>  <c>max_face_num</c>: 最多处理人脸的数目<br>**默认值为1(仅检测图片中面积最大的那个人脸)** **最大值10** </item>
        ///           <item>  <c>match_threshold</c>: 匹配阈值（设置阈值后，score低于此阈值的用户信息将不会返回） 最大100 最小0 默认80 <br>**此阈值设置得越高，检索速度将会越快，推荐使用默认阈值`80`** </item>
        ///           <item>  <c>quality_control</c>: 图片质量控制  **NONE**: 不进行控制 **LOW**:较低的质量要求 **NORMAL**: 一般的质量要求 **HIGH**: 较高的质量要求 **默认 NONE** </item>
        ///           <item>  <c>liveness_control</c>: 活体检测控制  **NONE**: 不进行控制 **LOW**:较低的活体要求(高通过率 低攻击拒绝率) **NORMAL**: 一般的活体要求(平衡的攻击拒绝率, 通过率) **HIGH**: 较高的活体要求(高攻击拒绝率 低通过率) **默认NONE** </item>
        ///           <item>  <c>user_id</c>: 当需要对特定用户进行比对时，指定user_id进行比对。即人脸认证功能。 </item>
        ///           <item>  <c>max_user_num</c>: 查找后返回的用户数量。返回相似度最高的几个用户，默认为1，最多返回50个。 </item>
        ///     </list>
        /// </param>
        /// <return>JObject</return>
        ///
        [Route("FaceSearch")]
        [HttpPost]
        public string FaceSearch(string image, string imageType, string groupIdList, string userId)
        {
            string result = "";
            switch (imageType)
            {
                case "base64":
                    result = faceCompare.FaceIdentifyByBuffer(image,groupIdList,userId);
                    break;
                case "url":
                    result = faceCompare.FaceIdentify(image, groupIdList, userId);
                    break;
                case "token":
                    result = faceCompare.FaceIdentifyByFeature(image,groupIdList,userId);
                    break;
                default:
                    break;
            }
            return result;
        }
        /// <summary>
        /// 1:N比较，传入图片文件路径和已加载的内存中整个库比较
        /// </summary>
        /// <param name="file">图片信息，数据大小小于10M，传入图片文件路径</param>
        /// <returns></returns>
        [Route("FaceIdentifyWithAll")]
        [HttpPost]
        public string FaceIdentifyWithAll(string file)
        {
            try
            {
               string result =  faceCompare.FaceIndentifyWithAll(file);
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [Route("FaceIdentify")]
        [HttpPost]
        public string FaceIdentify(string file, string user_group, string user_id)
        {
            try
            {
                string result = faceCompare.FaceIdentify(file, user_group, user_id);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("FaceIdentifyByFeature")]
        [HttpPost]
        public string FaceIdentifyByFeature(string file, string user_group, string user_id)
        {
            try
            {
                string result = faceCompare.FaceIdentifyByFeature(file, user_group, user_id);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("FaceIdentifyByBuffer")]
        [HttpPost]
        public string FaceIdentifyByBuffer(string file, string user_group, string user_id)
        {
            try
            {
                string result = faceCompare.FaceIdentifyByBuffer(file, user_group, user_id);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("FaceCompareFeature")]
        [HttpPost]
        public string FaceCompareFeature(string file1, string file2)
        {
            try
            {
                string result = faceCompare.FaceCompareFeature(file1, file2);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("FaceIdentifyByFeatureWithAll")]
        [HttpPost]
        public string FaceIdentifyByFeatureWithAll(string file)
        {
            try
            {
                string result = faceCompare.FaceIdentifyByFeatureWithAll(file);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("FaceIdentifyByBufferWithAll")]
        [HttpPost]
        public string FaceIdentifyByBufferWithAll(string file)
        {
            try
            {
                string result = faceCompare.FaceIdentifyByBufferWithAll(file);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("FaceMatchByMat")]
        [HttpPost]
        public string FaceMatchByMat(string file1, string file2)
        {
            try
            {
                string result = faceCompare.FaceMatchByMat(file1, file2);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("GetFaceFeature")]
        [HttpPost]
        public string GetFaceFeature(string file)
        {
            try
            {
                string result = faceCompare.GetFaceFeature(file);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("GetFaceFeatureByBuffer")]
        [HttpPost]
        public string GetFaceFeatureByBuffer(string file)
        {
            try
            {
                string result = faceCompare.GetFaceFeatureByBuffer(file);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 人脸对比接口（传入图片文件路径）
        /// </summary>
        /// <param name="file1">需要对比的第一张图片，小于10M，传入图片文件路径</param>
        /// <param name="file2">需要对比的第二张图片，小于10M，传入图片文件路径</param>
        /// <returns></returns>
        [Route("FaceMatch")]
        [HttpPost]
        public string FaceMatch(string file1,string file2)
        {
            try
            {
                string result = faceCompare.FaceMatch(file1,file2);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("FaceMatchByBuffer")]
        [HttpPost]
        public string FaceMatchByBuffer(string file1,string file2)
        {
            try
            {
                string result = faceCompare.FaceMatchByBuffer(file1,file2);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Route("FaceMatchByFeature")]
        [HttpPost]
        public string FaceMatchByFeature(string file1,string file2)
        {
            try
            {
                string result = faceCompare.FaceMatchByFeature(file1,file2);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/Face/5
        [Route("GetUser")]
        [HttpGet]
        public IActionResult GetUser(string user_id,string group_id)
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection(ConnString);
                dynamic data = new DynamicParameters();
                //if (id != null)
                //{
                //    data = mySqlConnection.Query("select * from `face-app`.faceinfo where FaceId = " + id);
                //}
                //else
                //{
                //    data = mySqlConnection.Query("select * from `face-app`.faceinfo Limit 0,100");
                //}
                data = faceManager.GetUserInfo(user_id, group_id);
                return data;// Json(data);
            }
            catch (Exception err)
            {
                throw err;
            }

            // return "value";
        }

        // POST: api/Face
        [Route("Add")]
        [HttpPost]
        public string FaceAdd(string value,string user_id,string group_id)
        {
            string rand = Guid.NewGuid().ToString().Substring(0, 8);
            string path = "G:\\Development\\Application\\testface\\img\\beckham\\" + value + ".jpg";
            user_id = "beckham";
            group_id = "beckham";
            string result = faceManager.UserAdd(user_id, group_id, path, rand); //G:\Development\Application\testface\img\beckham
            return result;
        }

        [Route("Update")]
        [HttpPost]
        public string FaceUpdate(string value, string user_id, string group_id)
        {
            string rand = Guid.NewGuid().ToString().Substring(0, 8);
            string path = "G:\\Development\\Application\\testface\\img\\beckham\\" + value + ".jpg";
            string result = faceManager.UserUpdate(user_id, group_id, path, rand); //G:\Development\Application\testface\img\beckham
            return result;
        }

        // PUT: api/Face/5
        [Route("Put")]
        [HttpPut]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [Route("Delete")]
        [HttpDelete]
        public void Delete(string userId, string groupId, string faceToken)
        {
            var result = faceManager.UserFaceDelete(userId, groupId, faceToken);
        }

        /// <summary>
        /// 人脸检测，返回人脸信息
        /// </summary>
        /// <param name="file">人脸图片信息，传入图片文件路径</param>
        /// <param name="num">最多检测人脸数量，默认为1，最大不超过5</param>
        /// <returns></returns>
        [Route("FaceTrack")]
        [HttpGet]
        public JObject FaceTrack(string file,int num)
        {
            try
            {
                var result = faceTrack.Track(file, num);
                var data = JsonConvert.DeserializeObject<JObject>(result);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 人脸检测，返回人脸信息
        /// </summary>
        /// <param name="file">人脸图片信息，传入图片文件路径</param>
        /// <param name="num">最多检测人脸数量，默认为1，最大不超过5</param>
        /// <returns></returns>
        [Route("FaceTrackByBuffer")]
        [HttpGet]
        public JObject FaceTrackByBuffer(byte[] file,int num)
        {
            try
            {
                var result = faceTrack.TrackByBuf(file, num);
                var data = JsonConvert.DeserializeObject<JObject>(result);
                return data;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
