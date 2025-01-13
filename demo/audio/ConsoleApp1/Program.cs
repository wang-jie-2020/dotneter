using System.Runtime.InteropServices;
using System.Text;
using static ConsoleApp1.SdkWorker;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var result = SdkWorker.startRecord2(3, null, null, EnumCallback, 16000);
            //var result = SdkWorker.startRecord9(3, null, null, EnumCallback2, 16000);
            //var result2 = SdkWorker.startRecord9(3, null, null, EnumCallback2, 16000);


            var aaa = new AAA();

            InitializeCallBack1 call1 = null;
            InitializeCallBack2 call2 = null;
            call1 = aaa.CallBack11;
            call2 = aaa.CallBack12;
            var result1 = SdkWorker.Initialize(call1, call2);
            var result2 = SdkWorker.Initialize(call1, call2);

            InitializeCallBack3 call3 = null;
            InitializeCallBack4 call4 = null;
            call3 = aaa.CallBack13;
            call4 = aaa.CallBack14;
            var inptr3 = Marshal.GetFunctionPointerForDelegate(call3);
            var inptr4 = Marshal.GetFunctionPointerForDelegate(call4);
            var result3 = SdkWorker.Initialize(inptr3, inptr4);
            var result4 = SdkWorker.Initialize(inptr3, inptr4);

            Console.WriteLine("Hello, World!");
            Console.ReadKey();

        }

        static void EnumCallback(int nEvt, string pData, int nValue)
        {
            Console.WriteLine(123);
        }

        static void EnumCallback2(int nEvt, IntPtr pData, int nValue)
        {
            Console.WriteLine(123);
        }
    }

    public class AAA
    {
        public void CallBack11(int nEvt)
        {
            Console.WriteLine(123);
        }

        public void CallBack12(byte[] nEvt)
        {
            Console.WriteLine(123);
        }

        public void CallBack13(Int64 nEvt)
        {
            Console.WriteLine(123);
        }

        public void CallBack14(IntPtr nEvt)
        {
            Console.WriteLine(123);
        }
    }
}