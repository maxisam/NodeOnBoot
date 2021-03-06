﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace NodeOnBoot
{
    class Dependencies
    {
        // check if Node.js is installed
        public static bool HasNode()
        {
            // check registry
            const string regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var regRgx = new Regex(@"(?i:node)");

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regKey))
            {
                foreach (string skName in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(skName))
                    {
                        if (subkey?.GetValue("DisplayName") == null) continue;
                        var name = subkey.GetValue("DisplayName").ToString();
                        if (regRgx.IsMatch(name)) return true;
                    }
                }
            }

            // check directory
            var rooDirectories = Directory.GetDirectories(@"C:\Program Files");
            var dirRgx = new Regex(@"(?i:node)");
            foreach (var directory in rooDirectories)
            {
                if (dirRgx.IsMatch(directory)) return true;
            }


            return false;
        }

        public static void InstallNpm()
        {
            // TODO: check if installing pm2 is not necessary
            
            // install pm2
            string[] npmDependencies =
            {
                "/C npm install -g pm2"            };

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
            startInfo.FileName = "cmd.exe";
            foreach (var dependency in npmDependencies)
            {
                startInfo.Arguments = dependency;
                process.StartInfo = startInfo;
                process.Start();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("NPM Dependencies Are Installing");
            Console.WriteLine("Please wait until all of the dependencies are installed");
            Console.ResetColor();
        }



    }
}
