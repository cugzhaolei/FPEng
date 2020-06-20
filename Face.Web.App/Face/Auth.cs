using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace testface
{
    public class Auth
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void FaceCallback(IntPtr bytes, int size, String res);
        // sdk初始化
        [DllImport("BaiduFaceApi.dll", EntryPoint = "sdk_init", CharSet = CharSet.Ansi
             , CallingConvention = CallingConvention.Cdecl)]
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
        [DllImport("BaiduFaceApi.dll", EntryPoint = "sdk_destroy", CharSet = CharSet.Ansi
             , CallingConvention = CallingConvention.Cdecl)]
        private static extern void sdk_destroy();

        // 测试获取设备指纹device_id
        public static void test_get_device_id()
        {
            IntPtr ptr = get_device_id();
            string buf = Marshal.PtrToStringAnsi(ptr);
            Console.WriteLine("device id is:" + buf);
        }

        public Auth()
        {
            SDK_Init();
        }

        public void SDK_Destory()
        {
            sdk_destroy();
        }

        public void SDK_Init()
        {
            Console.WriteLine("in main");
            bool id = false;

            int n = sdk_init(id);
            if (n != 0)
            {
                Console.WriteLine("auth result is {0:D}", n);
                Console.ReadLine();
            }

            // 测试是否授权
            bool authed = is_auth();
            Console.WriteLine("authed res is:" + authed);
            test_get_device_id();
            long t_begin = TimeUtil.get_time_stamp();

            long t_end = TimeUtil.get_time_stamp();
            Console.WriteLine("time cost is:" + (t_end - t_begin));

            // sdk_destroy();
            Console.WriteLine("end main");
            Console.ReadLine();
        }
    }
}
