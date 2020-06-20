using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;


// sdk使用注意事项，使用sdk前，请参考文档进行授权激活，否则
// 在输出console页面可能 authorize(): -8 failed ,表示没通过授权激活，sdk使用不了

// 人脸c#入口类
namespace Face.Web.Core.FaceAI
{
    public class Face
    {
       
       [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
       delegate void FaceCallback(IntPtr bytes, int size, String res);
       // sdk初始化
       [DllImport("BaiduFaceApi.dll", EntryPoint = "sdk_init", CharSet =CharSet.Ansi
            , CallingConvention =CallingConvention.Cdecl)]
       private static extern int sdk_init(bool id_card);
       // 是否授权
       [DllImport("BaiduFaceApi.dll", EntryPoint = "is_auth", CharSet = CharSet.Ansi
               , CallingConvention = CallingConvention.Cdecl)]
       private static extern bool is_auth();
       // 获取设备指纹
       [DllImport("BaiduFaceApi.dll", EntryPoint = "get_device_id", CharSet = CharSet.Ansi
                , CallingConvention = CallingConvention.Cdecl)]
       private static extern IntPtr get_device_id();
       // sdk销毁
       [DllImport("BaiduFaceApi.dll", EntryPoint = "sdk_destroy" , CharSet = CharSet.Ansi
            , CallingConvention = CallingConvention.Cdecl)]
       private static extern void sdk_destroy();
        // 测试获取设备指纹device_id
        public static void test_get_device_id()
        {
            IntPtr ptr = get_device_id();
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("device id is:" + buf);
        }

        // 测试人脸设置相关
        public static void test_face_setting()
        {
            FaceSetting setting = new FaceSetting();
            // 是否执行质量检测
            //setting.test_set_is_check_quality();
            // 设置光照阈值
         //   setting.test_set_illum_thr();
            // 设置模糊阈值
         //   setting.test_set_blur_thr();
            // 设置遮挡阈值
        //    setting.test_set_occlu_thr();
            // 设置pitch、yaw、roll等角度的阈值
         //   setting.test_set_eulur_angle_thr();
            // 设置非人脸的置信度阈值
         //   setting.test_set_not_face_thr();
            // 设置检测的最小人脸大小
            setting.test_set_min_face_size();
            // 设置跟踪到目标后执行检测的时间间隔
          //  setting.test_set_track_by_detection_interval();
            // 设置未跟踪到目标时的检测间隔
          //  setting.test_set_detect_in_video_interval();

        }

        // 测试人脸管理
        public static void test_face_manager()
        {
            // 测试人脸管理
            FaceManager manager = new FaceManager();
            //  人脸注册
            // manager.test_user_add();
           manager.test_user_add_by_buf();
           //manager.test_user_add_by_feature();
           // 人脸更新
           //manager.test_user_update();
             manager.test_user_update_by_buf();

           // 人脸删除
           // manager.test_user_face_delete();
           // 用户删除
           // manager.test_user_delete();
           // 组添加
           // manager.test_group_add();
           // 组删除
           // manager.test_group_delete();
           // 查询用户信息
           // manager.test_get_user_info();
           // 用户组列表查询
           // manager.test_get_user_list();
           // 组列表查询
           // manager.test_get_group_list();
        }
        // 测试人脸检测
        public static void test_face_track()
        {
            FaceTrack ft = new FaceTrack();
            // 人脸检测（传入图片文件路径)，返回json
            //  ft.test_track();
            // 最大人脸检测（传入图片文件路径)，返回json
            //ft.test_track_max_face();
            // 人脸检测（传入图片二进制文件buffer)，返回json
            // ft.test_track_by_buf();
            // 最大人脸检测（传入图片二进制文件buffer)，返回json
           // ft.test_track_max_face_by_buf();
            //usb 摄像头实时人脸检测
             ft.test_usb_track_face_info();
            // 清除跟踪的人脸信息
            //ft.test_clear_tracked_faces();
        }

        // 测试人脸比较&识别
        public static void test_face_compare()
        {
            FaceCompare comp = new FaceCompare();
            // 按图片文件路径取特征值
            // comp.test_get_face_feature();
            // 按图片二进制buffer取特征值
            // comp.test_get_face_feature_by_buf();
            // 1:1按图片文件路径比较
            comp.test_match();
            // 图片二进制buffer1:1比较
            //  comp.test_match_by_buf();
            // opencv视频帧1:1比较
            //  comp.test_match_by_mat();
            // 特征值和图片二进制buffer比较
            // comp.test_match_by_feature();
            // 通过特征值比对
            // comp.test_compare_feature();
            // 1:N识别（通过传入图片文件路径和库里的比对)
            comp.test_identify("G:\\Development\\Application\\testface\\img\\beckham\\1.jpg", "test_group", "test_user");
            // 1:N识别（通过传入图片文件二进制buffer和库里的比对)
            //  comp.test_identify_by_buf();
            // 1:N识别（通过传入提取的人脸特征值feature和库里的比对)
            //  comp.test_identify_by_feature();
            // 1：N识别，（通过传入图片文件路径和已提前加载的整个库比对)
            comp.test_identify_with_all();
            // 1：N识别，（通过传入图片文件二进制buffer和已提前加载的整个库比对)
            // comp.test_identify_by_buf_with_all();
            // 1：N识别，（通过传入提取的人脸特征值feature和已提前加载的整个库比对)
            // comp.test_identify_by_feature_with_all();
        }
        // 测试人脸活体检测
        public static void test_face_liveness()
        {
            FaceLiveness live = new FaceLiveness();
            // 测试单目RGB静默活体检测（传入图片文件路径)
            //live.test_rgb_liveness_check();
            // 测试单目RGB静默活体检测（传入图片文件二进制buffer)
            //  live.test_rgb_liveness_check_by_buf();
            // 双目RGB和IR静默活体检测（传入opencv视频帧)
            // live.test_rgb_ir_liveness_check_by_opencv();
            // 双目RGB和DEPTH静默活体检测（传入opencv视频帧，适配奥比中光mini双目摄像头
            //live.test_rgb_depth_liveness_check_by_orbe();
            // 双目摄像头进行rgb,depth活体检测(此处适配了华杰艾米的双目摄像头)
            live.test_rgb_depth_liveness_check_by_hjimi();
        }

        // 人脸c#入口方法
        static void In(string[] args)
        {
            Console.WriteLine("in main");
            bool id = false;
            Console.WriteLine("sdk_init");
            int n = sdk_init(id);
            if (n != 0)
            {
                Console.WriteLine("auth result is {0:D}", n);
                Console.ReadLine();
            }
            Console.WriteLine("sdk_isauth");
            // 测试是否授权
            bool authed = is_auth();
            Console.WriteLine("authed res is:" + authed);
            test_get_device_id();
            long t_begin = TimeUtil.get_time_stamp();
            // 测试获取人脸属性
            // FaceAttr attr = new FaceAttr();
            // attr.test_get_face_attr();
            // attr.test_get_face_attr_by_buf();
            //attr.test_get_face_attr_by_mat();
            //attr.test_get_face_attr_by_face();
            // test_face_setting();
            // 测试获取人脸质量
            // FaceQuality quality = new FaceQuality();
            //  quality.test_get_face_quality();
            //  quality.test_get_face_quality_by_buf();
            //  quality.test_get_face_quality_by_mat();
            //quality.test_get_face_quality_by_face();

            // 测试人脸设置相关
            // test_face_setting();

            // 测试人脸管理
            test_face_manager();

            // 测试人脸检测
            // test_face_track();            

            // 测试人脸比较&识别
            test_face_compare();
        // 测试人脸活体检测
        //test_face_liveness();
        long t_end = TimeUtil.get_time_stamp();
            Console.WriteLine("time cost is:"+(t_end-t_begin));
          
            sdk_destroy();
            Console.WriteLine("end main");
            Console.ReadLine();
        }
    }
}
