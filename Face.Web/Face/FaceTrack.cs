using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using OpenCvSharp;


namespace Face.Web.Face
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    //[StructLayout(LayoutKind.Explicit)]
    public struct FaceInfo
    {
        public FaceInfo(float iWidth, float iAngle, float iCenter_x, float iCenter_y, float iConf)
        {
            mWidth = iWidth;
            mAngle = iAngle;
            mCenter_x = iCenter_x;
            mCenter_y = iCenter_y;
            mConf = iConf;
        }
        public float mWidth;     // rectangle width
        public float mAngle;//; = 0.0F;     // rectangle tilt angle [-45 45] in degrees
        public float mCenter_y;// = 0.0F;  // rectangle center y
        public float mCenter_x;// = 0.0F;  // rectangle center x
        public float mConf;// = 0.0F;
    };
    //   [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 596)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TrackFaceInfo
    {
        [MarshalAs(UnmanagedType.Struct)]
        public FaceInfo box;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144)]
        public int[] landmarks;// = new int[144];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] headPose;// = new float[3];
        public float score;// = 0.0F;
        public UInt32 face_id;// = 0;
    }
    // 人脸检测
    public class FaceTrack
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FaceCallback(IntPtr bytes, int size, String res);

        [DllImport("BaiduFaceApi.dll", EntryPoint = "track", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr track(string file_name, int max_track_num);
        /*  trackMat
         *  传入参数: maxTrackObjNum:检测到的最大人脸数，传入外部分配人脸数，需要分配对应的内存大小。
         *            传出检测到的最大人脸数
         *    返回值: 传入的人脸数 和 检测到的人脸数 中的最小值。
         ****/
        [DllImport("BaiduFaceApi.dll", EntryPoint = "track_mat", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        public static extern int track_mat(IntPtr oface, IntPtr mat, ref int max_track_num);

        [DllImport("BaiduFaceApi.dll", EntryPoint = "track_max_face", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr track_max_face(string file_name);

        [DllImport("BaiduFaceApi.dll", EntryPoint = "track_by_buf", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr track_by_buf(byte[] image, int size, int max_track_num);

        [DllImport("BaiduFaceApi.dll", EntryPoint = "track_max_face_by_buf", CharSet = CharSet.Ansi
          , CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr track_max_face_by_buf(byte[] image, int size);

        [DllImport("BaiduFaceApi.dll", EntryPoint = "clear_tracked_faces", CharSet = CharSet.Ansi
         , CallingConvention = CallingConvention.Cdecl)]
        private static extern void clear_tracked_faces();

        // 测试人脸检测
        public void test_track()
        {
            // 传入图片文件绝对路径
            string file_name = "d:\\kehu2.jpg";
            int max_track_num = 1; // 设置最多检测人数（多人脸检测），默认为1，最多可设为10
            IntPtr ptr = track(file_name, max_track_num);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("track res is:" + buf);
        }

        // 测试人脸检测
        public void test_track_max_face()
        {
            // 传入图片文件绝对路径
            string file_name = "d:\\2.jpg";
            IntPtr ptr = track_max_face(file_name);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("track_max_face res is:" + buf);
        }

        // 测试人脸检测
        public void test_track_by_buf()
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile("d:\\3.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);
            int max_track_num = 2; // 设置最多检测人数（多人脸检测），默认为1，最多可设为10
            IntPtr ptr = track_by_buf(img_bytes, img_bytes.Length, max_track_num);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("track_by_buf res is:" + buf);
        }
        // 测试人脸检测
        public void test_track_max_face_by_buf()
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile("d:\\2.jpg");
            byte[] img_bytes = ImageUtil.img2byte(img);
            IntPtr ptr = track_max_face_by_buf(img_bytes, img_bytes.Length);
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("track_max_face_by_buf res is:" + buf);
        }
        public RotatedRect bounding_box(int[] landmarks, int size)
        {
            int min_x = 1000000;
            int min_y = 1000000;
            int max_x = -1000000;
            int max_y = -1000000;
            for (int i = 0; i<size / 2; ++i)
            {
                min_x = ( min_x < landmarks[2 * i]? min_x : landmarks[2 * i]);
                min_y = ( min_y < landmarks[2 * i + 1]?min_y: landmarks[2 * i + 1]);
                max_x = (max_x > landmarks[2 * i]? max_x: landmarks[2 * i]);
                max_y = (max_y > landmarks[2 * i + 1]? max_y: landmarks[2 * i + 1]);
            }
            int width = ((max_x - min_x) + (max_y - min_y)) / 2;
            float angle = 0;
            Point2f center = new Point2f((min_x + max_x) / 2, (min_y + max_y) / 2);
            return new RotatedRect(center, new Size2f(width, width), angle);
        }
        public void draw_rotated_box(ref Mat img, ref RotatedRect box, Scalar color)
        {
            Point2f[] vertices = new Point2f[4];
            vertices = box.Points();
            for (int j = 0; j< 4; j++)
            {
                Cv2.Line(img, vertices[j], vertices[(j + 1) % 4], color);
            }
        }
        //C#测试usb摄像头实时人脸检测
        public void usb_csharp_track_face(int dev)
        {
            using (var window = new Window("face"))
            using (VideoCapture cap = VideoCapture.FromCamera(dev))
            {
                if (!cap.IsOpened())
                {
                    Console.WriteLine("open camera error");
                    return;
                }
                // Frame image buffer
                Mat image = new Mat();
                // When the movie playback reaches end, Mat.data becomes NULL.
                while (true)
                {
                    RotatedRect box;                                       
                    cap.Read(image); // same as cvQueryFrame
                    if (!image.Empty())
                    {
                        int ilen = 2;//传入的人脸数
                        TrackFaceInfo[] track_info = new TrackFaceInfo[ilen];
                        for (int i = 0; i < ilen; i++)
                        {
                            track_info[i] = new TrackFaceInfo();
                            track_info[i].landmarks = new int[144];
                            track_info[i].headPose = new float[3];
                            track_info[i].face_id = 0;
                            track_info[i].score = 0;
                        }
                        int sizeTrack = Marshal.SizeOf(typeof(TrackFaceInfo));
                        IntPtr ptT = Marshal.AllocHGlobal(sizeTrack* ilen);
                        //Marshal.Copy(ptTrack, 0, ptT, ilen);
                        //                        FaceInfo[] face_info = new FaceInfo[ilen];
                        //                        face_info = new FaceInfo(0.0F,0.0F,0.0F,0.0F,0.0F);

                        //Cv2.ImWrite("usb_track_Cv2.jpg", image);
                        /*  trackMat
                         *  传入参数: maxTrackObjNum:检测到的最大人脸数，传入外部分配人脸数，需要分配对应的内存大小。
                         *            传出检测到的最大人脸数
                         *    返回值: 传入的人脸数 和 检测到的人脸数 中的最小值,实际返回的人脸。
                         ****/
                        int faceSize = ilen;//返回人脸数  分配人脸数和检测到人脸数的最小值
                        int curSize = ilen;//当前人脸数 输入分配的人脸数，输出实际检测到的人脸数
                        faceSize = track_mat(ptT, image.CvPtr, ref curSize);
                        for (int index = 0; index < faceSize; index++)
                        {
                            IntPtr ptr = new IntPtr();
                            if( 8 == IntPtr.Size)
                            {
                                ptr = (IntPtr)(ptT.ToInt64() + sizeTrack * index);
                            }
                            else if(4 == IntPtr.Size)
                            {
                                ptr = (IntPtr)(ptT.ToInt32() + sizeTrack * index);
                            }
                            
                            track_info[index] = (TrackFaceInfo)Marshal.PtrToStructure(ptr, typeof(TrackFaceInfo));
                            //face_info[index] = (FaceInfo)Marshal.PtrToStructure(info_ptr, typeof(FaceInfo));
                            Console.WriteLine("in Liveness::usb_track face_id is {0}:",track_info[index].face_id);
                            Console.WriteLine("in Liveness::usb_track landmarks is:");
                            for (int k = 0; k < 1; k ++)
                            {
                                Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},", 
                                    track_info[index].landmarks[k], track_info[index].landmarks[k+1],
                                    track_info[index].landmarks[k+2], track_info[index].landmarks[k + 3],
                                    track_info[index].landmarks[k+4], track_info[index].landmarks[k + 5],
                                    track_info[index].landmarks[k+6], track_info[index].landmarks[k + 7],
                                    track_info[index].landmarks[k+8], track_info[index].landmarks[k + 9]
                                    );
                            }
                        
                            for (int k = 0; k < track_info[index].headPose.Length; k++)
                            {
                                Console.WriteLine("in Liveness::usb_track angle is:{0:f}", track_info[index].headPose[k]);
                            }
                            Console.WriteLine("in Liveness::usb_track score is:{0:f}", track_info[index].score);
                            // 角度
                            Console.WriteLine("in Liveness::usb_track mAngle is:{0:f}", track_info[index].box.mAngle);
                            // 人脸宽度
                            Console.WriteLine("in Liveness::usb_track mWidth is:{0:f}", track_info[index].box.mWidth);
                            // 中心点X,Y坐标
                            Console.WriteLine("in Liveness::usb_track mCenter_x is:{0:f}", track_info[index].box.mCenter_x);
                            Console.WriteLine("in Liveness::usb_track mCenter_y is:{0:f}", track_info[index].box.mCenter_y);
                            //// 画人脸框
                            box = bounding_box(track_info[index].landmarks, track_info[index].landmarks.Length);
                            draw_rotated_box(ref image, ref box, new Scalar(0, 255, 0));
                            // 实时检测人脸属性和质量可能会视频卡顿，若一定必要可考虑跳帧检测
                            // 获取人脸属性（通过传入人脸信息）
                            //IntPtr ptrAttr = FaceAttr.face_attr_by_face(image.CvPtr, ref track_info[index]);
                            //string buf = Marshal.PtrToStringAnsi(ptrAttr);
                            //Console.WriteLine("attr res is:" + buf);
                            //// 获取人脸质量（通过传入人脸信息）
                            //IntPtr ptrQua = FaceQuality.face_quality_by_face(image.CvPtr, ref track_info[index]);
                            //buf = Marshal.PtrToStringAnsi(ptrQua);
                            //Console.WriteLine("quality res is:" + buf);
                            //// 实时检测人脸特征值可能会视频卡顿，若一定必要可考虑跳帧检测
                            //float[] feature = new float[512];
                            //IntPtr ptrfea = new IntPtr();
                            //int count = FaceCompare.get_face_feature_by_face(image.CvPtr, ref track_info[index], ref ptrfea);
                            //// 返回值为512表示取到了特征值
                            //if(ptrfea == IntPtr.Zero)
                            //{
                            //    Console.WriteLine("Get feature failed!");
                            //    continue;
                            //}
                            //if (count == 512)
                            //{
                            //    for (int i = 0; i < count; i++)
                            //    {
                            //        IntPtr floptr = new IntPtr();
                            //        if ( 8 == IntPtr.Size)
                            //        {
                            //            floptr = (IntPtr)(ptrfea.ToInt64() + i * count * Marshal.SizeOf(typeof(float)));
                            //        }
                            //        else if( 4 == IntPtr.Size)
                            //        {
                            //            floptr = (IntPtr)(ptrfea.ToInt32() + i * count * Marshal.SizeOf(typeof(float)));
                            //        }
                                    
                            //        feature[i] = (float)Marshal.PtrToStructure(floptr, typeof(float));
                            //        Console.WriteLine("feature {0} is: {1:e8}", i, feature[i]);
                            //    }
                            //    Console.WriteLine("feature count is:{0}", count);
                            //}
                        }
                        Marshal.FreeHGlobal(ptT);
                        window.ShowImage(image);
                        Cv2.WaitKey(1);
                        Console.WriteLine("mat not empty");
                    }
                    else
                    {
                        Console.WriteLine("mat is empty");
                    }
                }
                image.Release();
            }
        }
        // 测试usb摄像头实时人脸检测
        public void test_usb_track_face_info()
        {
              //FaceCallback callback =
              //(bytes, buf_len, res_out) =>
              // {
              //     if(buf_len>0)
              //     {
              //         byte[] b = new byte[buf_len];
              //         Marshal.Copy(bytes, b, 0, buf_len);
              //        // ImageUtil.byte2img(b, buf_len,"d:\\test_byte.jpg");
              //      //   Console.WriteLine("callback result is {0} and {1} and {2}", bytes, buf_len, res_out);
              //     }
              //  };
            // 默认电脑自带摄像头，device可能为0，若外接usb摄像头，则device可能为1.
            int device = 0;
            usb_csharp_track_face(device);
         }

         // 清除跟踪的人脸信息
         public void test_clear_tracked_faces()
         {
            clear_tracked_faces();
            Console.WriteLine("after clear tracked faces");
        }

     }
 }
