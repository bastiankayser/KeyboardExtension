﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceProcess;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fleck;
using static KeyboardExtensionServer.WindowListener;

namespace KeyboardExtensionServer
{
    class Program
    {
        string myFolder = @"D:\temp\testwebsite";
        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(JsonObject));
        public Program()
        {

        }

        private static UpdateProgramName SocketSend;
        private static UpdateProgramName SocketSendCeption;

        static void Main(string[] args)
        {

            Program prog = new Program();

            //GetLocalIPAddress().TakeWhile((s) => !String.IsNullOrEmpty(s)).ToList().ForEach(ip =>
             // {
                  var server = new WebSocketServer("ws://0.0.0.0:9010");
                  server.Start(socket =>
                  {
                      socket.OnOpen = () => Console.WriteLine("Open!");
                      socket.OnClose = () => Console.WriteLine("Close!");
                      //socket.OnMessage = message => socket.Send(message);
                      socket.OnMessage = message => Console.WriteLine(message);
                      SocketSend += (programTitle) => socket.Send(programTitle);
                  });
    //          });

            SocketSendCeption = (txt) =>
            {
                if (SocketSend != null)
                {
                    var jsonString = ToJsonString(txt, new string[] { "test", "test2" });
                    SocketSend(jsonString);
                }
            };

            WindowListener.Init(SocketSendCeption);

            Task.Factory.StartNew(StartNewWebserver);

            Application.Run(); //<----
            Console.ReadKey();
        }

        public static IEnumerable<string> GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(ip.ToString());
                    yield return ip.ToString();
                }
            }
            yield return "localhost";
            yield break;
            throw new Exception("Local IP Address Not Found!");
        }


        private static void StartNewWebserver()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:9050/");
            listener.Start();
            listener.BeginGetContext(OnNewConnect, listener);

            //GetLocalIPAddress().TakeWhile(s => !String.IsNullOrEmpty(s)).ToList().ForEach(ip =>
            //  {
            //      Task.Factory.StartNew(ipI =>
            //      {
            //          using (Microsoft.Owin.Hosting.WebApp.Start<Startup>("http://" + ipI + ":9000"))
            //          {
            //              Console.WriteLine("Press [enter] to quit...");
            //              Console.ReadLine();
            //          }
            //      }, ip);
            //  });
        }

        private static void OnNewConnect(IAsyncResult ar)
        {
            var listener = (HttpListener)ar.AsyncState;
            listener.BeginGetContext(OnNewConnect, listener);
            var context = listener.EndGetContext(ar);
            using(var writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.WriteLine(File.ReadAllText("Index.html"));
            }
        }

        private static string ToJsonString(string title, string[] actions)
        {
            var jsonObject = new JsonObject();
            jsonObject.title = title;
            jsonObject.actions = actions;
            using (MemoryStream textStream = new MemoryStream())
            {

                jsonSerializer.WriteObject(textStream, jsonObject);
                return Encoding.Default.GetString(textStream.ToArray());
            }

        }

        [DataContract]
        private class JsonObject
        {
            [DataMember]
            public string title;
            [DataMember]
            public string[] actions;
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
