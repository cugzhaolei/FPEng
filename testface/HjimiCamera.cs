using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using OpenCvSharp;

namespace testface
{
    class HjimiCamera
    {
        // 华杰艾米 获取摄像头设备对象
        [DllImport("BaiduFaceApi.dll", EntryPoint = "new_hjimi", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr new_hjimi();
        // 华杰艾米 释放摄像头设备对象
        [DllImport("BaiduFaceApi.dll", EntryPoint = "hjimi_release", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        public static extern void hjimi_release(IntPtr hjimi);
        // 华杰艾米 打开摄像头设备对象  传入opencv视频帧rgb和depth的mat
        [DllImport("BaiduFaceApi.dll", EntryPoint = "open_hjimimat", CharSet = CharSet.Ansi
           , CallingConvention = CallingConvention.Cdecl)]
        public static extern bool open_hjimimat(IntPtr hjimi, IntPtr rgb_mat, IntPtr depth_mat);
    }
}