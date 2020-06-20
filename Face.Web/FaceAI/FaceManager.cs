using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Face.Web.Core.FaceAI
{
    public class FaceManager
    {
        // 人脸注册(传入图片文件路径)
        /// <summary>
        /// 用户注册，该接口支持传入本地图片文件地址。
        /// </summary>
        /// <param name="user_id">用户id，字母、数字、下划线组成，最多128个字符</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B。用户组和user_id之间，仅为映射关系。如传入的groupid并未事先创建完毕，则注册用户的同时，直接完成group的创建</param>
        /// <param name="file_name">图片信息，须小于10M，传入图片的本地文件地址</param>
        /// <param name="user_info">? 用户资料，256个字符以内</param>
        /// <returns></returns>
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_add", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_add(string user_id, string group_id, string file_name,
            string user_info="");
        // 人脸注册(传入图片二进制buffer)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_add_by_buf", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_add_by_buf(string user_id, string group_id, byte[] image,
           int size, string user_info = "");

        // 人脸注册(传入特征值)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_add_by_feature", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_add_by_feature(string user_id, string group_id, byte[] fea,
           int fea_len, string user_info = "");

        // 人脸更新(传入图片文件路径)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_update", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_update(string user_id, string group_id, string file_name,
            string user_info = "");
        // 人脸更新(传入图片二进制buffer)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_update_by_buf", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_update_by_buf(string user_id, string group_id, byte[] image,
           int size, string user_info = "");
        // 人脸删除
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_face_delete", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_face_delete(string user_id, string group_id, string face_token);
        // 用户删除
        [DllImport("BaiduFaceApi.dll", EntryPoint = "user_delete", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr user_delete(string user_id, string group_id);
        // 组添加
        [DllImport("BaiduFaceApi.dll", EntryPoint = "group_add", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr group_add(string group_id);
        // 组删除
        [DllImport("BaiduFaceApi.dll", EntryPoint = "group_delete", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr group_delete(string group_id);
        // 查询用户信息
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_user_info", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_user_info(string user_id, string group_id);
        // 用户组列表查询
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_user_list", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_user_list(string group_id, int start = 0, int length = 100);
        // 组列表查询
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_group_list", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_group_list(int start = 0, int length = 100);

        //// 组获取用户
        //[DllImport("BaiduFaceApi.dll", EntryPoint = "get_group_users", CharSet = CharSet.Ansi
        //   , CallingConvention = CallingConvention.Cdecl)]
        //private static extern IntPtr get_group_users(string groupId, Dictionary<string, object> options = null);
        // 测试人脸注册
        public void test_user_add()
        {
            // 人脸注册
            string user_id = "test_user";
            string group_id = "test_group";
            string file_name = "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg";
            string user_info = "user_info";
            IntPtr ptr = user_add(user_id, group_id, file_name, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_add res is:" + buf);
        }
        /// <summary>
        /// add user face
        /// </summary>
        /// <param name="user_id">用户id，字母、数字、下划线组成，最多128个字符</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B。用户组和user_id之间，仅为映射关系。
        /// 如传入的groupid并未事先创建完毕，则注册用户的同时，直接完成group的创建</param>
        /// <param name="file_path">图片信息，须小于10M，传入图片的本地文件地址</param>
        /// <param name="user_info">用户资料，256个字符以内 可选择</param>
        /// <returns></returns>
        public string UserAdd(string user_id, string group_id, string file_path,string user_info = "")
        {
            IntPtr ptr = user_add(user_id, group_id, file_path, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_add res is:" + buf);
            return buf;
        }

        // 测试人脸注册(传入二进制图片buffer)
        public void test_user_add_by_buf()
        {
            // 人脸注册
            string user_id = "test_user";
            string group_id = "test_group";
            System.Drawing.Image img = System.Drawing.Image.FromFile("G:\\Development\\Application\\testface\\img\\beckham\\4.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);
            string user_info = "user_info";
            IntPtr ptr = user_add_by_buf(user_id, group_id, img_bytes, img_bytes.Length, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_add_by_buf res is:" + buf);
        }
        /// <summary>
        /// 人脸注册(传入二进制图片buffer)
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <param name="file_path"></param>
        /// <param name="user_info"></param>
        /// <returns></returns>
        public string UserAddByBuffer(string user_id, string group_id,string file_path, string user_info ="")
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(file_path);
            byte[] img_bytes = ImageUtil.img2byte(img);
            IntPtr ptr = user_add_by_buf(user_id, group_id, img_bytes, img_bytes.Length, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine ("user_add_by_buf res is:" + buf);
            return buf;

        }

        // 测试人脸注册(传入特征值)
        public void test_user_add_by_feature()
        {
            // 人脸注册
            string user_id = "test_user";
            string group_id = "test_group";
            // 传入人脸特征值，提取特征值demo，可参考FaceCompare文件中
            byte[] feature = new byte[2048];
            string user_info = "user_info";
            IntPtr ptr = user_add_by_feature(user_id, group_id, feature, feature.Length, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_add_by_feature res is:" + buf);
        }
        /// <summary>
        /// 人脸注册(传入特征值)
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <param name="fea"></param>
        /// <param name="fea_len"></param>
        /// <param name="user_info"></param>
        /// <returns></returns>
        public string UserAddByFeature(string user_id, string group_id, byte[] fea,int fea_len, string user_info = "")
        {
            // 传入人脸特征值，提取特征值demo，可参考FaceCompare文件中
            byte[] feature = new byte[2048];
            IntPtr ptr = user_add_by_feature(user_id, group_id, feature, feature.Length, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine ("user_add_by_feature res is:" + buf);
            return buf;

        }

        // 测试人脸更新
        public void test_user_update()
        {
            string user_id = "test_user";
            string group_id = "test_group";
            string file_name = "G:\\Development\\Application\\testface\\img\\beckham\\4.jpg";
            string user_info = "user_info";
            IntPtr ptr = user_update(user_id, group_id, file_name, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_update res is:" + buf);
        }
        /// <summary>
        /// 人脸更新(传入图片文件路径)
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <param name="file_name"></param>
        /// <param name="user_info"></param>
        /// <returns></returns>
        public string UserUpdate(string user_id, string group_id, string file_name,string user_info = "")
        {
            IntPtr ptr = user_update(user_id, group_id, file_name, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_update res is:" + buf);
            return buf;
        }

        // 测试人脸更新(传入二进制图片buffer)
        public void test_user_update_by_buf()
        {
            // 人脸更新
            string user_id = "test_user";
            string group_id = "test_group";
            System.Drawing.Image img = System.Drawing.Image.FromFile("G:\\Development\\Application\\testface\\img\\beckham\\8.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);
            string user_info = "user_info";
            IntPtr ptr = user_update_by_buf(user_id, group_id, img_bytes, img_bytes.Length, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_update_by_buf res is:" + buf);
        }
        /// <summary>
        /// 人脸更新(传入二进制图片buffer)
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <param name="user_info"></param>
        /// <returns></returns>
        public string UserUpdateByBuf(string file,string user_id, string group_id, string user_info = "")
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(file);
            byte[] img_bytes = ImageUtil.img2byte(img);
            IntPtr ptr = user_update_by_buf(user_id, group_id, img_bytes, img_bytes.Length, user_info);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_update_by_buf res is:" + buf);
            return buf;
        }

        // 测试人脸删除
        public void test_user_face_delete()
        {
            string user_id = "test_user";
            string group_id = "test_group";
            string face_token = "b6d8e657b5acd4dbae98efed64ea7c4b";
            IntPtr ptr = user_face_delete(user_id, group_id, face_token);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_face_delete res is:" + buf);
        }
        /// <summary>
        /// 人脸删除
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <param name="face_token"></param>
        /// <returns></returns>
        public string UserFaceDelete(string user_id, string group_id, string face_token)
        {
            IntPtr ptr = user_face_delete(user_id, group_id, face_token);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_face_delete res is:" + buf);
            return buf;
        }

        // 测试用户删除
        public void test_user_delete()
        {
            string user_id = "test_user";
            string group_id = "test_group";
            IntPtr ptr = user_delete(user_id, group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_delete res is:" + buf);
        }
        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public string UserDelete(string user_id, string group_id)
        {
            IntPtr ptr = user_delete(user_id, group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("user_delete res is:" + buf);
            return buf;
        }
        // 组添加
        public void test_group_add()
        {
            string group_id = "test_group2";
            IntPtr ptr = group_add(group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("group_add res is:" + buf);
        }
        /// <summary>
        /// 组添加
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public string GroupAdd(string group_id)
        {
            IntPtr ptr = group_add(group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("group_add res is:" + buf);
            return buf;
        }
        // 组删除
        public void test_group_delete()
        {
            string group_id = "test_group2";
            IntPtr ptr = group_delete(group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("group_delete res is:" + buf);
        }
        /// <summary>
        /// 组删除
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public string GroupDelete(string group_id)
        {
            IntPtr ptr = group_delete(group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("group_delete res is:" + buf);
            return buf;
        }

        // 查询用户信息
        public void test_get_user_info()
        {
            string user_id = "test_user";
            string group_id = "test_group";
            IntPtr ptr = get_user_info(user_id , group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("get_user_info res is:" + buf);
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="user_id">用户id（由数字、字母、下划线组成），长度限制128B</param>
        /// <param name="group_id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <returns></returns>
        public string GetUserInfo(string user_id,string group_id)
        {
            IntPtr ptr = get_user_info(user_id, group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("get_user_info res is:" + buf);
            return buf;
        }

        // 用户组列表查询
        public void test_get_user_list()
        {
            string group_id = "test_group";
            IntPtr ptr = get_user_list(group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("get_user_list res is:" + buf);
        }
        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <param name="group_id">用户组id</param>
        /// <param name="start">默认值为0</param>
        /// <param name="length">默认值100，最大值1000</param>
        /// <returns></returns>
        public string GetUserList(string group_id)
        {
            IntPtr ptr = get_user_list(group_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("get_user_list res is:" + buf);
            return buf;
        }

        // 组列表查询
        public void test_get_group_list()
        {
            IntPtr ptr = get_group_list();
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("get_group_list res is:" + buf);
        }
        /// <summary>
        /// 组列表查询
        /// </summary>
        /// <returns></returns>
        public string GetGroupList()
        {
            IntPtr ptr = get_group_list();
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("get_group_list res is:" + buf);
            return buf;
        }

        /// <summary>
        /// 获取组用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public string GetGroupUsers(string id)
        //{
        //    IntPtr ptr = get_group_users(id);
        //    string buf = Marshal.PtrToStringAnsi(ptr);
        //    Console.WriteLine("get_group_users res is:" + buf);
        //    return buf;
        //}
    }
}
