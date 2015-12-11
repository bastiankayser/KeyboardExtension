using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KeyboardExtensionServer
{
    class Program
    {
        private WindowListener windowListener;
        private SimpleHTTPServer simpleHttpServer;
        string myFolder = @"D:\temp\testwebsite";

        public Program()
        {
            windowListener = new WindowListener();

            simpleHttpServer = new SimpleHTTPServer(myFolder,8008);
        }

        static void Main(string[] args)
        {

            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>("http://localhost:9000"))
            {
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }

            Program prog = new Program();

            Application.Run(); //<----
            Console.ReadKey();
            //prog.simpleHttpServer.Stop();
        }


    }
}


//    class Program : ServiceBase
//    {

//        static void Main(string[] args)
//        {
//            Thread.Sleep(5000); // Test it with 5 Seconds, set a window to foreground, and you see it works!
//            IntPtr hWnd = GetForegroundWindow();
//            uint procId = 0;
//            GetWindowThreadProcessId(hWnd, out procId);
//            var proc = Process.GetProcessById((int)procId);
//            Console.WriteLine(proc.MainModule);
//            SendKeys.SendWait("^S");
//            Console.ReadKey();
//        }
//        [DllImport("user32.dll")]
//        public static extern IntPtr GetForegroundWindow();

//        [DllImport("user32.dll", SetLastError = true)]
//        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

//    }

//}
