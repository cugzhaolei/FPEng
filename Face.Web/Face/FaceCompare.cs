using System;
using System.Runtime.InteropServices;
using System.IO;
using OpenCvSharp;

namespace Face.Web.Face
{
    // 人脸比较1:1、1:N、抽取人脸特征值、按特征值比较等
    public class FaceCompare
    {
        // 提取人脸特征值(传图片文件路径)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_face_feature", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_face_feature(string file_name, ref int length);
        // 提取人脸特征值(传二进制图片buffer）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_face_feature_by_buf", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_face_feature_by_buf(byte[] buf,int size, ref int length);
        // 获取人脸特征值（传入opencv视频帧及人脸信息，适应于多人脸）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "get_face_feature_by_face", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_face_feature_by_face(IntPtr mat, ref TrackFaceInfo info, ref IntPtr feaptr);
        // 人脸1:1比对(传图片文件路径)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "match", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr match(string file_name1, string file_name2);
        // 人脸1:1比对（传二进制图片buffer）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "match_by_buf", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr match_by_buf(byte[] buf1, int size1, byte[] buf2, int size2);
        // 人脸1:1比对（传opencv视频帧）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "match_by_mat", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr match_by_mat(IntPtr img1, IntPtr img2);// byte[] buf1, int size1, byte[] buf2, int size2);
        // 人脸1:1比对（传人脸特征值和二进制图片buffer)
        [DllImport("BaiduFaceApi.dll", EntryPoint = "match_by_feature", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr match_by_feature(byte[] feature, int fea_len, byte[] buf2, int size2);
        // 特征值比对（传2个人脸的特征值）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "compare_feature", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern float compare_feature(byte[] f1, int f1_len, byte[] f2, int f2_len);
        // 1:N人脸识别（传图片文件路径和库里的比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify(string image, string group_id_list, string user_id, int user_top_num = 1);
        // 1:N人脸识别（传图片二进制文件buffer和库里的比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_buf", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_buf(byte[] buf, int size, string group_id_list,
            string user_id, int user_top_num = 1);
        // 1:N人脸识别（传人脸特征值和库里的比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_feature", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_feature(byte[] feature, int fea_len, string group_id_list, 
            string user_id, int user_top_num = 1);

        // 提前加载库里所有数据到内存中
        [DllImport("BaiduFaceApi.dll", EntryPoint = "load_db_face", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        public static extern bool load_db_face();

        // 1:N人脸识别（传人脸图片文件和内存已加载的整个库数据比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_with_all", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_with_all(string image, int user_top_num = 1);

        // 1:N人脸识别（传人脸图片文件和内存已加载的整个库数据比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_buf_with_all", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_buf_with_all(byte[] image, int size, int user_top_num = 1);

        // 1:N人脸识别（传人脸特征值和内存已加载的整个库数据比对）
        [DllImport("BaiduFaceApi.dll", EntryPoint = "identify_by_feature_with_all", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr identify_by_feature_with_all(byte[] feature, int fea_len, int user_top_num = 1);

        public FaceCompare()
        {
           Auth auth = new Auth();
        }

        // 测试获取人脸特征值(2048个byte）
        public void test_get_face_feature()
        {
            byte[] fea = new byte[2048];
            string file_name = "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg";
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if(ptr==IntPtr.Zero)
            {
                Console.WriteLine("get face feature error");
            }
            else
            {
                if (len == 2048)
                {
                    Console.WriteLine("get face feature success");
                    Marshal.Copy(ptr, fea, 0, 2048);
                    // 可保存特征值2048个字节的fea到文件中
                   // FileUtil.byte2file("G:\\Development\\Application\\testface\\img\\beckham\\fea1.txt",fea, 2048);
                }
                else
                {
                    Console.WriteLine("get face feature error");
                }
            }
        }
        /// <summary>
        /// 获取人脸特征值(2048个byte）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFaceFeature(string fileName)
        {
            try
            {
                byte[] fea = new byte[2048];
                string file_name = fileName == null ? fileName : "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg";
                int len = 0;
                IntPtr ptr = get_face_feature(file_name, ref len);
                if (ptr == IntPtr.Zero)
                {
                    Console.WriteLine("get face feature error");
                    return "error";
                }
                else
                {
                    if (len == 2048)
                    {
                        Console.WriteLine("get face feature success");
                        Marshal.Copy(ptr, fea, 0, 2048);
                        return fea.ToString();
                        // 可保存特征值2048个字节的fea到文件中
                        // FileUtil.byte2file("G:\\Development\\Application\\testface\\img\\beckham\\fea1.txt",fea, 2048);
                    }
                    else
                    {
                        Console.WriteLine("get face feature error");
                        return "error";
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 获取人脸特征值 2048Byte
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetFaceFeature(string file_name, ref int length)
        {
            byte[] fea = new byte[2048];
            int len = length==0?0:length; //defalut=0
            string result = "";
            IntPtr ptr = get_face_feature(file_name, ref len);
            if (ptr == IntPtr.Zero)
            {
                result = ("get face feature error");
            }
            else
            {
                if (len == 2048)
                {
                    result = ("get face feature success");
                    Marshal.Copy(ptr, fea, 0, 2048);
                    // 可保存特征值2048个字节的fea到文件中
                    // FileUtil.byte2file("G:\\Development\\Application\\testface\\img\\beckham\\fea1.txt",fea, 2048);
                }
                else
                {
                   result = ("get face feature error");
                }
            }
            return result;
        }


        // 测试获取人脸特征值(2048个byte）
        public void test_get_face_feature_by_buf()
        {
            byte[] fea = new byte[2048];
            System.Drawing.Image img = System.Drawing.Image.FromFile("G:\\Development\\Application\\testface\\img\\beckham\\2.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);
            int len = 0;
            IntPtr ptr = get_face_feature_by_buf(img_bytes, img_bytes.Length, ref len);
            if (ptr == IntPtr.Zero)
            {
                Console.WriteLine("get face feature error");
            }
            else
            {
                if (len == 2048)
                {
                    Console.WriteLine("get face feature success");
                    Marshal.Copy(ptr, fea, 0, 2048);
                    // 可保存特征值2048个字节的fea到文件中
                    //  FileUtil.byte2file("G:\\Development\\Application\\testface\\img\\beckham\\fea2.txt",fea, 2048);
                }
                else
                {
                    Console.WriteLine("get face feature error");
                }
            }
        }

        /// <summary>
        /// 获取人脸特征值 2048byte
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetFaceFeatureByBuffer(string filePath)
        {
            byte[] fea = new byte[2048];
            string result = "";
            System.Drawing.Image img = System.Drawing.Image.FromFile(filePath);
            byte[] img_bytes = ImageUtil.img2byte(img);
            int len = 0;
            IntPtr ptr = get_face_feature_by_buf(img_bytes, img_bytes.Length, ref len);
            if (ptr == IntPtr.Zero)
            {
                result = ("get face feature error");
            }
            else
            {
                if (len == 2048)
                {
                    result = ("get face feature success");
                    Marshal.Copy(ptr, fea, 0, 2048);
                    // 可保存特征值2048个字节的fea到文件中
                    //  FileUtil.byte2file("G:\\Development\\Application\\testface\\img\\beckham\\fea2.txt",fea, 2048);
                }
                else
                {
                    result = ("get face feature error");
                }
            }
            return result;
        }
        // 测试1:1比较，传入图片文件路径
        public void test_match()
        {
            string file1 = "G:\\Development\\Application\\testface\\img\\beckham\\1.jpg";
            string file2 = "G:\\Development\\Application\\testface\\img\\beckham\\9.jpg";
            IntPtr ptr = match(file1, file2);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("match res is:" + buf);
        }
        /// <summary>
        /// 1:1比较 传入图片文件路径
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public string FaceMatch(string file1,string file2)
        {
            IntPtr ptr = match(file1, file2);
            string buf = Marshal.PtrToStringAnsi(ptr);
            return ("match res is:" + buf);
        }
        // 测试1:1比较，传入图片文件二进制buffer
        public void test_match_by_buf()
        {
            System.Drawing.Image img1 = System.Drawing.Image.FromFile("d:\\444.bmp");
            byte[] img_bytes1 = ImageUtil.img2byte(img1);

            System.Drawing.Image img2 = System.Drawing.Image.FromFile("d:\\555.png");
            byte[] img_bytes2 = ImageUtil.img2byte(img2);
            Console.WriteLine("IntPtr ptr = match_by_buf");
            IntPtr ptr = match_by_buf(img_bytes1, img_bytes1.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("match_by_buf res is:" + buf);
        }
        /// <summary>
        /// 1:1比较 传图片二进制Buffer
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public string FaceMatchByBuffer(string file1, string file2)
        {
            System.Drawing.Image img1 = System.Drawing.Image.FromFile(file1);
            byte[] img_bytes1 = ImageUtil.img2byte(img1);

            System.Drawing.Image img2 = System.Drawing.Image.FromFile(file2);
            byte[] img_bytes2 = ImageUtil.img2byte(img2);
            Console.WriteLine("IntPtr ptr = match_by_buf");
            IntPtr ptr = match_by_buf(img_bytes1, img_bytes1.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            return ("match_by_buf res is:" + buf);
        }
        // 测试1:1比较，传入opencv视频帧
        public void test_match_by_mat()
        {
            Mat img1 = Cv2.ImRead("d:\\444.bmp");
            Mat img2 = Cv2.ImRead("d:\\555.png");
            IntPtr ptr = match_by_mat(img1.CvPtr, img2.CvPtr);// img_bytes1, img_bytes1.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("match_by_buf res is:" + buf);
        }
        /// <summary>
        /// 1:1比较，传入opencv视频帧
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public string FaceMatchByMat(string file1,string file2)
        {
            Mat img1 = Cv2.ImRead(file1);
            Mat img2 = Cv2.ImRead(file2);
            IntPtr ptr = match_by_mat(img1.CvPtr, img2.CvPtr);// img_bytes1, img_bytes1.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            return ("match_by_buf res is:" + buf);

        }
        // 测试根据特征值和图片二进制buf比较
        public void test_match_by_feature()
        {
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];
            string file_name = "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg";
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if( len!=2048)
            {
                Console.WriteLine("get face feature error!" );
                return;
            }
            Marshal.Copy(ptr, fea, 0, 2048);
            // 获取图片二进制buffer
            System.Drawing.Image img2 = System.Drawing.Image.FromFile("G:\\Development\\Application\\testface\\img\\beckham\\8.jpg");
            byte[] img_bytes2 = ImageUtil.img2byte(img2);

            IntPtr ptr_res = match_by_feature(fea, fea.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            Console.WriteLine("match_by_feature res is:" + buf);

        }
        /// <summary>
        /// 根据特征值和图片二进制buf比较
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="file_buffer"></param>
        /// <returns></returns>
        public string FaceMatchByFeature(string file_name,string file_buffer)
        {
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if (len != 2048)
            {
                return ("get face feature error!");
            }
            Marshal.Copy(ptr, fea, 0, 2048);
            // 获取图片二进制buffer
            System.Drawing.Image img2 = System.Drawing.Image.FromFile(file_buffer);
            byte[] img_bytes2 = ImageUtil.img2byte(img2);

            IntPtr ptr_res = match_by_feature(fea, fea.Length, img_bytes2, img_bytes2.Length);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            return ("match_by_feature res is:" + buf);

        }

        // 测试1:N比较，传入图片文件路径
        public /*void*/string test_identify(string str, string usr_grp, string usr_id)
        {
            string file1 = str;//"G:\\Development\\Application\\testface\\img\\beckham\\6.jpg";
            string user_group = usr_grp;//"test_group";
            string user_id = usr_id;//"test_user";
            IntPtr ptr = identify(file1, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify res is:" + buf);
            return buf;
        }
        /// <summary>
        /// 1:N比较，传入图片文件路径
        /// </summary>
        /// <param name="str"></param>
        /// <param name="user_group"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public string FaceIdentify(string str,string user_group,string user_id)
        {
            string file1 = str;
            IntPtr ptr = identify(file1, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify res is:" + buf);
            return buf;
        }

        // 测试1:N比较，传入图片文件二进制buffer
        public void test_identify_by_buf(string str, string usr_grp, string usr_id)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(str);//"G:\\Development\\Application\\testface\\img\\beckham\\2.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);

            string user_group = usr_grp;//"test_group";
            string user_id = usr_id;// "test_user";
            IntPtr ptr = identify_by_buf(img_bytes, img_bytes.Length, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify_by_buf res is:" + buf);
        }
        /// <summary>
        /// 1:N比较，传入图片文件二进制buffer
        /// </summary>
        /// <param name="str"></param>
        /// <param name="usr_grp"></param>
        /// <param name="usr_id"></param>
        /// <returns></returns>
        public string FaceIdentifyByBuffer(string str, string usr_grp, string usr_id)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(str);
            byte[] img_bytes = ImageUtil.img2byte(img);

            string user_group = usr_grp;//"test_group";
            string user_id = usr_id;// "test_user";
            IntPtr ptr = identify_by_buf(img_bytes, img_bytes.Length, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr);
            return ("identify_by_buf res is:" + buf);
        }
        
        // 测试1:N比较，传入提取的人脸特征值
        public void test_identify_by_feature()
        {
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];
            string file_name = "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg";
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if (len != 2048)
            {
                Console.WriteLine("get face feature error!");
                return;
            }
            Marshal.Copy(ptr, fea, 0, 2048);

            string user_group = "test_group";
            string user_id = "test_user";
            IntPtr ptr_res = identify_by_feature(fea, fea.Length, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            Console.WriteLine("identify_by_feature res is:" + buf);
        }
        /// <summary>
        /// 1:N比较，传入提取的人脸特征值
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public string FaceIdentifyByFeature(string file_name)
        {
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if (len != 2048)
            {
                return ("get face feature error!");
            }
            Marshal.Copy(ptr, fea, 0, 2048);

            string user_group = "test_group";
            string user_id = "test_user";
            IntPtr ptr_res = identify_by_feature(fea, fea.Length, user_group, user_id);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            return ("identify_by_feature res is:" + buf);
        }

        // 通过特征值比对（1:1）
        public void test_compare_feature()
        {
            // 获取特征值1   共2048个字节
            byte[] fea1 = new byte[2048];
            string file_name1 = "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg";
            int len1 = 0;
            IntPtr ptr1 = get_face_feature(file_name1, ref len1);
            if (len1 != 2048)
            {
                Console.WriteLine("get face feature error!");
                return;
            }
            Marshal.Copy(ptr1, fea1, 0, 2048);

            // 获取特征值2   共2048个字节
            byte[] fea2 = new byte[2048];
            string file_name2 = "G:\\Development\\Application\\testface\\img\\beckham\\8.jpg";
            int len2 = 0;
            IntPtr ptr2 = get_face_feature(file_name2, ref len2);
            if (len2 != 2048)
            {
                Console.WriteLine("get face feature error!");
                return;
            }
            Marshal.Copy(ptr2, fea2, 0, 2048);
            // 比对
            float score = compare_feature(fea1, len1, fea2, len2);
            Console.WriteLine("compare_feature score is:"+score);
        }
        /// <summary>
        /// 通过特征值比对（1:1）
        /// </summary>
        /// <param name="file_name1"></param>
        /// <param name="file_name2"></param>
        /// <returns></returns>
        public string FaceCompareFeature(string file_name1,string file_name2)
        {
            // 获取特征值1   共2048个字节
            byte[] fea1 = new byte[2048];
            int len1 = 0;
            IntPtr ptr1 = get_face_feature(file_name1, ref len1);
            if (len1 != 2048)
            {
                return ("get face feature error!");
            }
            Marshal.Copy(ptr1, fea1, 0, 2048);

            // 获取特征值2   共2048个字节
            byte[] fea2 = new byte[2048];
            int len2 = 0;
            IntPtr ptr2 = get_face_feature(file_name2, ref len2);
            if (len2 != 2048)
            {
                return ("get face feature error!");
            }
            Marshal.Copy(ptr2, fea2, 0, 2048);
            // 比对
            float score = compare_feature(fea1, len1, fea2, len2);
            Console.WriteLine("compare_feature score is:" + score);
            return score.ToString();
        }

        // 测试1:N比较，传入提取的人脸特征值和已加载的内存中整个库比较
        public void test_identify_by_feature_with_all()
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];
            string file_name = "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg";
            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if (len != 2048)
            {
                Console.WriteLine("get face feature error!");
                return;
            }
            Marshal.Copy(ptr, fea, 0, 2048);
            IntPtr ptr_res = identify_by_feature_with_all(fea, fea.Length);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            Console.WriteLine("identify_by_feature_with_all res is:" + buf);
        }
        /// <summary>
        /// 1:N比较，传入提取的人脸特征值和已加载的内存中整个库比较
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public string FaceIdentifyByFeatureWithAll(string file_name)
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 获取特征值2048个字节
            byte[] fea = new byte[2048];

            int len = 0;
            IntPtr ptr = get_face_feature(file_name, ref len);
            if (len != 2048)
            {
                Console.WriteLine("get face feature error!");
                return "error";
            }
            Marshal.Copy(ptr, fea, 0, 2048);
            IntPtr ptr_res = identify_by_feature_with_all(fea, fea.Length);
            string buf = Marshal.PtrToStringAnsi(ptr_res);
            Console.WriteLine("identify_by_feature_with_all res is:" + buf);
            return buf;
        }

        // 测试1:N比较，传入图片文件路径和已加载的内存中整个库比较
        public void test_identify_with_all()
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 1:N
            string file1 = "G:\\Development\\Application\\testface\\img\\beckham\\3.jpg";
            IntPtr ptr = identify_with_all(file1);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify_with_all res is:" + buf);
        }
        /// <summary>
        /// 1:N比较，传入图片文件路径和已加载的内存中整个库比较
        /// </summary>
        /// <param name="file1"></param>
        /// <returns></returns>
        public string FaceIndentifyWithAll(string file1)
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 1:N
            IntPtr ptr = identify_with_all(file1);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify_with_all res is:" + buf);
            return buf;
        }
        // 测试1:N比较，传入图片文件二进制buffer和已加载的内存中整个库比较
        public void test_identify_by_buf_with_all()
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 1:N
            System.Drawing.Image img = System.Drawing.Image.FromFile("G:\\Development\\Application\\testface\\img\\beckham\\4.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);
            
            IntPtr ptr = identify_by_buf_with_all(img_bytes, img_bytes.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify_by_buf_with_all res is:" + buf);
        }
        /// <summary>
        /// 1:N比较，传入图片文件二进制buffer和已加载的内存中整个库比较
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public string FaceIdentifyByBufferWithAll(string file_name)
        {
            // 加载整个数据库到内存中
            load_db_face();
            // 1:N
            System.Drawing.Image img = System.Drawing.Image.FromFile(file_name);
            byte[] img_bytes = ImageUtil.img2byte(img);

            IntPtr ptr = identify_by_buf_with_all(img_bytes, img_bytes.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("identify_by_buf_with_all res is:" + buf);
            return buf;
        }

    }
}
