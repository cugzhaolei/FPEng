using System;
using System.Runtime.InteropServices;
using OpenCvSharp;

namespace Face.Web.Core.FaceAI
{
    // 获取人脸质量
    public class FaceQuality
    {
        [DllImport("BaiduFaceApi.dll", EntryPoint = "face_quality", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr face_quality(string file_name);
        [DllImport("BaiduFaceApi.dll", EntryPoint = "face_quality_by_buf", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr face_quality_by_buf(byte[] buf, int size);
        [DllImport("BaiduFaceApi.dll", EntryPoint = "face_quality_by_mat", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr face_quality_by_mat(IntPtr mat);
        [DllImport("BaiduFaceApi.dll", EntryPoint = "face_quality_by_face", CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr face_quality_by_face(IntPtr mat, ref TrackFaceInfo info);

        // 测试获取人脸质量
        public void test_get_face_quality()
        {
            // 传入图片文件绝对路径
            IntPtr ptr = face_quality("d:\\2.jpg");
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("quality res is:" + buf);
        }
        /// <summary>
        /// 获取人脸质量 
        /// </summary>
        /// <param name="file">需要检测的人脸图片，小于10M, 传入图片文件的地址</param>
        /// <returns></returns>
        public string GetFaceQuality(string file)
        {
            // 传入图片文件绝对路径
            IntPtr ptr = face_quality(file);
            string buf = Marshal.PtrToStringAnsi(ptr);
            return buf;
        }
        // 测试获取人脸质量(传入图片文件的二进制buffer）
        public void test_get_face_quality_by_buf()
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile("d:\\2.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);

            IntPtr ptr = face_quality_by_buf(img_bytes, img_bytes.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("quality res is:" + buf);
        }
        /// <summary>
        /// 测试获取人脸质量(传入图片文件的二进制buffer）
        /// </summary>
        /// <param name="file">需要检测的图片二进制buffer</param>
        /// <returns></returns>
        public string GetFaceQualityByBuffer(byte[] file)
        {
            // System.Drawing.Image img = System.Drawing.Image.FromFile(file);
            byte[] img_bytes = file;

            IntPtr ptr = face_quality_by_buf(img_bytes, img_bytes.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            return buf;
        }

        // 人脸质量（传入opencv视频帧）
        public void test_get_face_quality_by_mat()
        {
            Mat img = Cv2.ImRead("d:\\1112.jpg");
            IntPtr ptr = face_quality_by_mat(img.CvPtr);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("attr res is:" + buf);
        }
        /// <summary>
        /// 人脸质量（传入opencv视频帧）
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public string GetFaceQualityByMat(string frame)
        {
            Mat img = Cv2.ImRead(frame);
            IntPtr ptr = face_quality_by_mat(img.CvPtr);
            string buf = Marshal.PtrToStringAnsi(ptr);
            return buf;
        }
        // 人脸质量(传入opencv视频帧及检测到人脸信息，适应于多人脸)
        public void test_get_face_quality_by_face()
        {
            Mat img = Cv2.ImRead("d:\\1112.jpg");

            int ilen = 1;//传入的人脸数
            TrackFaceInfo track_info = new TrackFaceInfo();
            track_info.landmarks = new int[144];
            track_info.headPose = new float[3];
            track_info.face_id = 0;
            track_info.score = 0;

            int sizeTrack = Marshal.SizeOf(typeof(TrackFaceInfo));
            IntPtr ptT = Marshal.AllocHGlobal(sizeTrack * ilen);

            RotatedRect box;
            int faceSize = ilen;//返回人脸数  分配人脸数和检测到人脸数的最小值
            int curSize = ilen;//当前人脸数 输入分配的人脸数，输出实际检测到的人脸数
            faceSize = FaceTrack.track_mat(ptT, img.CvPtr, ref curSize);
            if (faceSize > 0)
            {
                IntPtr ptr = (IntPtr)(ptT.ToInt64());
                track_info = (TrackFaceInfo)Marshal.PtrToStructure(ptr, typeof(TrackFaceInfo));
                // 画人脸框
                FaceTrack track = new FaceTrack();
                box = track.bounding_box(track_info.landmarks, track_info.landmarks.Length);
                track.draw_rotated_box(ref img, ref box, new Scalar(0, 255, 0));
                //Cv2.ImShow("img", img);
                //Cv2.WaitKey(0);
            }
            Marshal.FreeHGlobal(ptT);
            Console.WriteLine("face_quality_by_face"); 
            if (faceSize > 0)
            { 
                IntPtr ptrMsg = face_quality_by_face(img.CvPtr, ref track_info);             
                string buf = Marshal.PtrToStringAnsi(ptrMsg);
                Console.WriteLine("attr res is:" + buf);
            }
        }
        /// <summary>
        /// 人脸质量(传入opencv视频帧及检测到人脸信息，适应于多人脸)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetFaceQualityByFace(string file)
        {
            Mat img = Cv2.ImRead(file);
            string result = "";
            int ilen = 1;//传入的人脸数
            TrackFaceInfo track_info = new TrackFaceInfo();
            track_info.landmarks = new int[144];
            track_info.headPose = new float[3];
            track_info.face_id = 0;
            track_info.score = 0;

            int sizeTrack = Marshal.SizeOf(typeof(TrackFaceInfo));
            IntPtr ptT = Marshal.AllocHGlobal(sizeTrack * ilen);

            RotatedRect box;
            int faceSize = ilen;//返回人脸数  分配人脸数和检测到人脸数的最小值
            int curSize = ilen;//当前人脸数 输入分配的人脸数，输出实际检测到的人脸数
            faceSize = FaceTrack.track_mat(ptT, img.CvPtr, ref curSize);
            if (faceSize > 0)
            {
                IntPtr ptr = (IntPtr)(ptT.ToInt64());
                track_info = (TrackFaceInfo)Marshal.PtrToStructure(ptr, typeof(TrackFaceInfo));
                // 画人脸框
                FaceTrack track = new FaceTrack();
                box = track.bounding_box(track_info.landmarks, track_info.landmarks.Length);
                track.draw_rotated_box(ref img, ref box, new Scalar(0, 255, 0));
                //Cv2.ImShow("img", img);
                //Cv2.WaitKey(0);
            }
            Marshal.FreeHGlobal(ptT);
            Console.WriteLine("face_quality_by_face");
            if (faceSize > 0)
            {
                IntPtr ptrMsg = face_quality_by_face(img.CvPtr, ref track_info);
                string buf = Marshal.PtrToStringAnsi(ptrMsg);
                result = buf;
                Console.WriteLine("attr res is:" + buf);
            }

            return result;
        }
    }
}
