using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class SdkWorker
    {
        [DllImport("ac_buf_come.dll", EntryPoint = "checkOutRecordPermission")]
        public static extern int checkOutRecordPermission1();

        [DllImport("IflyRecordDll.dll", EntryPoint = "checkOutRecordPermission")]
        public static extern int checkOutRecordPermission2();

        [DllImport("mix_record_lib.dll", EntryPoint = "checkOutRecordPermission")]
        public static extern int checkOutRecordPermission3();

        [DllImport("ac_buf_come.dll", EntryPoint = "startRecord")]
        public static extern int startRecord1(int recordType, string? outRecordDeviceName, string? innerRecordDeviceName, RecordCallback recordCallback, int sampleRate);

        [DllImport("IflyRecordDll.dll", EntryPoint = "startRecord", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.ThisCall)]
        public static extern int startRecord2(int recordType, string? outRecordDeviceName, string? innerRecordDeviceName, RecordCallback recordCallback, int sampleRate);

        [DllImport("mix_record_lib.dll", EntryPoint = "startRecord")]
        public static extern int startRecord3(int recordType, string? outRecordDeviceName, string? innerRecordDeviceName, RecordCallback recordCallback, int sampleRate);

        [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void RecordCallback(int nEvt, string pData, int nValue);

        [DllImport("IflyRecordDll.dll", EntryPoint = "startRecord", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int startRecord9(int recordType, string? outRecordDeviceName, string? innerRecordDeviceName, RecordCallback2 recordCallback, int sampleRate);

        [UnmanagedFunctionPointer(callingConvention: CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void RecordCallback2(int nEvt, IntPtr pData, int nValue);










        [DllImport("IflyRecordDll.dll", EntryPoint = "startRecord", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int startRecord4(int recordType, RecordCallback2 recordCallback, int sampleRate);


        [DllImport("mix_record_lib.dll", EntryPoint = "initialize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.ThisCall)]
        public static extern int Initialize(IntPtr callBack1, IntPtr callBack2);




        [DllImport("mix_record_lib.dll", EntryPoint = "initialize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.ThisCall)]
        public static extern int Initialize([MarshalAs(UnmanagedType.FunctionPtr)] InitializeCallBack1 callBack1, [MarshalAs(UnmanagedType.FunctionPtr)] InitializeCallBack2 callBack2);

        [UnmanagedFunctionPointer(callingConvention: CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public delegate void InitializeCallBack1(int nEvt);

        [UnmanagedFunctionPointer(callingConvention: CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public delegate void InitializeCallBack2(byte[] nEvt);

        [UnmanagedFunctionPointer(callingConvention: CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public delegate void InitializeCallBack3(Int64 nEvt);

        [UnmanagedFunctionPointer(callingConvention: CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
        public delegate void InitializeCallBack4(IntPtr nEvt);


    }
}
