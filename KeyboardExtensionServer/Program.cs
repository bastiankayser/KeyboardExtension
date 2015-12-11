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
using Fleck;


namespace KeyboardExtensionServer
{
    class Program
    {
        private WindowListener windowListener;
        string myFolder = @"D:\temp\testwebsite";
        

        public Program()
        {
            windowListener = new WindowListener();
        }

        static void Main(string[] args)
        {

            Program prog = new Program();
            
            var server = new WebSocketServer("ws://localhost:9010");
            server.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message => socket.Send(message);
            });

            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>("http://localhost:9000"))
            {
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }

            Application.Run(); //<----
            Console.ReadKey();
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
