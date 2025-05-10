using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace HelloPoint.Communication
{
    public class StartWpfApp
    {

        private bool StartWpf()
        {
            try
            {
                //Process.Start(@"D:\AltranProject\HelloProject\HelloPoint\WpfHelloPoint\WpfHelloPoint\bin\Debug\WpfHelloPoint.exe");
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(@"D:\AltranProject\HelloProject\HelloPoint\WpfHelloPoint\WpfHelloPoint\bin\Debug\WpfHelloPoint.exe");
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(@"D:\AltranProject\HelloProject\HelloPoint\WpfHelloPoint\WpfHelloPoint\bin\Debug\WpfHelloPoint.exe");
                p.Start();

                return true;
            } catch (Exception e)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt");
                file.WriteLine("Error to open WPF  - "+ e.ToString());

                file.Close();
            }
            return false;
        }

        private bool KillWpf()
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("WpfHelloPoint"))
                {
                    proc.Kill();
                }
                return true;
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public bool RestartWpf()
        {
            if (KillWpf() && StartWpf()) return true;
            else return false;
        }
    }
}