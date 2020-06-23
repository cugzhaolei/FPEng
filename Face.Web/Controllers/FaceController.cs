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
                    // result = faceCompare.FaceIdentifyByBuffer(image,groupIdList,userId);
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

        /// <summary>
        /// 1:N比较，传入图片文件路径
        /// </summary>
        /// <param name="file">图片信息，数据大小小于10M，传入图片文件路径</param>
        /// <param name="user_group">组id列表。默认至少填写一个group_id，从指定的group中进行查找。需要同时查询多个group，用逗号分隔，上限10个</param>
        /// <param name="user_id">用户id，若指定了某个user，则只会与指定group下的这个user进行对比；若user_id传空字符串” ”，则会与此group下的所有user进行1：N识别</param>
        /// <returns></returns>
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

        /// <summary>
        /// 1:N比较，传入提取的人脸特征值
        /// </summary>
        /// <param name="file_name">传入图片文件路径</param>
        /// <returns></returns>
        /// <param name="user_group">传入用户组Id</param>
        /// <param name="user_id">输入用户Id</param>
        /// <returns></returns>
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

        /// <summary>
        /// 1:N比较，传入图片文件二进制buffer
        /// </summary>
        /// <param name="file">二进制图片信息，数据大小小于10M</param>
        /// <param name="user_group">组id列表。默认至少填写一个group_id，从指定的group中进行查找。需要同时查询多个group，用逗号分隔，上限10个</param>
        /// <param name="usr_id">用户id，若指定了某个user，则只会与指定group下的这个user进行对比；若user_id传空字符串” ”，则会与此group下的所有user进行1：N识别</param>
        /// <returns></returns>
        [Route("FaceIdentifyByBuffer")]
        [HttpPost]
        public string FaceIdentifyByBuffer(byte[] file, string user_group, string user_id)
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

        /// <summary>
        /// 通过特征值比对（1:1） 对人脸特征值进行比较，可返回人脸特征相似分值（百分制）
        /// </summary>
        /// <param name="file1">2048个byte数组的特征值(传图片路径)</param>
        /// <param name="file2">2048个byte数组的特征值（传图片路径）</param>
        /// <returns></returns>
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

        /// <summary>
        /// 1:N比较，传入提取的人脸特征值和已加载的内存中整个库比较
        /// </summary>
        /// <param name="file">传入人脸文件特征值</param>
        /// <returns></returns>
        [Route("FaceIdentifyByFeatureWithAll")]
        [HttpPost]
        public string FaceIdentifyByFeatureWithAll(byte[] file)
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
        public string FaceIdentifyByBufferWithAll(byte[] file)
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

        /// <summary>
        /// 人脸对比接口（传入二进制图片buffer）
        /// </summary>
        /// <param name="file1">需要对比的第一张图片，小于10M</param>
        /// <param name="file2">需要对比的第二张图片，小于10M</param>
        /// <returns></returns>
        //[Route("FaceMatchByBuffer")]
        //[HttpPost]
        //public string FaceMatchByBuffer(byte[] file1,byte[] file2)
        //{
        //    try
        //    {
        //        string result = faceCompare.FaceMatchByBuffer(file1,file2);
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        /// <summary>
        /// 人脸对比接口(传入二进制图片buffer)
        /// </summary>
        /// <param name="file1">需要对比的特征值</param>
        /// <param name="file2">需要对比的第二张图片，小于10M</param>
        /// <returns></returns>
        //[Route("FaceMatchByFeature")]
        //[HttpPost]
        //public string FaceMatchByFeature(byte[] file1,byte[] file2)
        //{
        //    try
        //    {
        //        string result = faceCompare.FaceMatchByFeature(file1,file2);
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        /// <summary>
        /// 获取用户组下面的用户
        /// </summary>
        /// <param name="user_id">用户id（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <returns></returns>
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

        /// <summary>
        /// 用户注册，该接口支持传入本地图片文件地址。
        /// </summary>
        /// <param name="file">	图片信息，须小于10M，传入图片的本地文件地址</param>
        /// <param name="user_id">用户id，字母、数字、下划线组成，最多128个字符</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B。用户组和user_id之间，仅为映射关系。
        /// 如传入的groupid并未事先创建完毕，则注册用户的同时，直接完成group的创建</param>
        /// <returns></returns>
        // POST: api/Face
        [Route("Add")]
        [HttpPost]
        public string FaceAdd(string file,string user_id,string group_id,string user_info)
        {
            string rand = Guid.NewGuid().ToString().Substring(0, 8);
            // string path = "G:\\Development\\Application\\testface\\img\\beckham\\" + file + ".jpg";
            // user_id = "beckham";
            // group_id = "beckham";
            string result = faceManager.UserAdd(user_id, group_id, file, rand); //G:\Development\Application\testface\img\beckham
            return result;
        }

        /// <summary>
        /// 人脸图片及信息更新
        /// </summary>
        /// <param name="value">图片信息，数据大小应小于10M，传入本地图片文件地址，每次仅支持单张图片</param>
        /// <param name="user_id">用户id（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public string FaceUpdate(string value, string user_id, string group_id,string user_info)
        {
            string rand = Guid.NewGuid().ToString().Substring(0, 8);
            // string path = "G:\\Development\\Application\\testface\\img\\beckham\\" + value + ".jpg";
            string result = faceManager.UserUpdate(user_id, group_id, value, user_info); //G:\Development\Application\testface\img\beckham
            return result;
        }

        // PUT: api/Face/5
        [Route("Put")]
        [HttpPut]
        public void Put(string id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 人脸删除
        /// </summary>
        /// <param name="userId">用户id（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="groupId">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="faceToken">人脸id（由数字、字母、下划线组成）长度限制128B</param>
        // DELETE: api/ApiWithActions/5
        [Route("DeleteFace")]
        [HttpDelete]
        public string DeleteFace(string userId, string groupId, string faceToken)
        {
            try
            {
                var result = faceManager.UserFaceDelete(userId, groupId, faceToken);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="userId">用户id（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="groupId">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <returns></returns>
        [Route("DeleteUser")]
        [HttpDelete]
        public string DeleteUser(string userId, string groupId)
        {
            try
            {
                var result = faceManager.UserDelete(userId, groupId);
                return result;
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
