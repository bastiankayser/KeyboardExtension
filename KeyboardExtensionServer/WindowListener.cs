using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardExtensionServer
{
    static class WindowListener
    {
        static WinEventDelegate dele = null; //STATIC

        public delegate void UpdateProgramName(string programName);

        private static UpdateProgramName _callBackUpdateProgramName;

        public static void Init(UpdateProgramName callbackFunction)
        {
            dele = new WinEventDelegate(WinEventProc);
            _callBackUpdateProgramName = callbackFunction;
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
        }
        
        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private static string GetActiveWindowTitle() //STATIC
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();
            uint procId = 0;
            GetWindowThreadProcessId(handle, out procId);
            var proc = Process.GetProcessById((int)procId);
            var mainModule = proc.MainModule;
            return mainModule.ModuleName;

            //if (GetWindowText(handle, Buff, nChars) > 0)
            //{
            //    return Buff.ToString();
            //}
            //return null;
        }

        public static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) //STATIC
        {
            var activeWindowTitle = GetActiveWindowTitle();
            _callBackUpdateProgramName(activeWindowTitle);

        }
    }
}
