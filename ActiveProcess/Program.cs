using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ActiveProcess
{
    internal class Program
    {

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public static (IntPtr handle, uint pid, string windowTitle, string windowClass, string processName, string filename) GetActiveProcessInfo()
        {
            IntPtr handle = default;
            uint pid = 0;
            string windowTitle = null, windowClass = null, processName = null, filename = null;
            try
            {
                handle = GetForegroundWindow();
                if (handle != IntPtr.Zero)
                {
                    try
                    {
                        const int nChars = 256;
                        StringBuilder buff = new StringBuilder(nChars);
                        if (GetWindowText(handle, buff, nChars) > 0)
                        {
                            windowTitle = buff.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    try
                    {
                        const int nChars = 256;
                        StringBuilder buff = new StringBuilder(nChars);
                        if (GetClassName(handle, buff, nChars) > 0)
                            windowClass = buff.ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    try
                    {
                        GetWindowThreadProcessId(handle, out pid);
                        Process p = Process.GetProcessById((int)pid);
                        processName = p.ProcessName;
                        filename = p.GetMainModuleFileName();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (handle, pid, windowTitle, windowClass, processName, filename);
        }

        static void Main(string[] args)
        {
            int loop = 0;
            string lastText = null;
            while (true)
            {
                Thread.Sleep(1000);
                loop++;
                (var handle, var pid, var windowTitle, var windowClass, var processName, var filename) = GetActiveProcessInfo();
                var printText = $"Handle = {handle}\nPID = {pid}\nWindowTitle = \"{windowTitle ?? "null"}\"/{windowTitle?.Length}\nWindowClass = \"{windowClass ?? "null"}\"/{windowClass?.Length}\nProcessName = {processName ?? "null"}\"/{processName?.Length}\nFilename = \"{filename ?? "null"}\"/{filename?.Length}\n";
                if (lastText != printText)
                {
                    lastText = printText;
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n{printText}");
                }
                if (loop % 5 == 0)
                {
                    Directory.CreateDirectory("Screenshots");
                    PrintWindowHelper.PrintWindow(handle, $"Screenshots\\{DateTime.Now:yyyy-MM-dd HH.mm.ss}.png");
                    PrintWindowHelper.PrintWindowClassic(handle, $"Screenshots\\{DateTime.Now:yyyy-MM-dd HH.mm.ss}-2.png");
                }
            }
        }
    }
}
