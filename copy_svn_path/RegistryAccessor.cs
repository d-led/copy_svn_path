using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace copy_svn_path
{
    public static class RegistryAccessor
    {
        static string[] RegistryPaths = new[] { 
            @"SOFTWARE\Classes\Folder\shell\SvnCopyPath",
            @"SOFTWARE\Classes\Directory\Background\shell\SvnCopyPath"
        };

        /// <summary>
        /// throws
        /// </summary>
        public static void Register()
        {
            string CommandString = "\"" + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Process.GetCurrentProcess().ProcessName.Replace(".vshost", "")) + "\"" + " \"%v\"";
            foreach (var RegistryPath in RegistryPaths)
                registerKey(RegistryPath, RegistryPath + @"\command", "SVN Path > Clipboard", CommandString);
        }

        private static void registerKey(string FolderPath, string FolderCommand, string FolderName, string CommandString)
        {
            RegistryKey regmenu = null;
            RegistryKey regcmd = null;
            try
            {
                regmenu = Registry.CurrentUser.CreateSubKey(FolderPath);
                if (regmenu != null)
                    regmenu.SetValue("", FolderName);
                regcmd = Registry.CurrentUser.CreateSubKey(FolderCommand);
                if (regcmd != null)
                    regcmd.SetValue("", CommandString);
            }
            finally
            {
                if (regmenu != null)
                    regmenu.Close();
                if (regcmd != null)
                    regcmd.Close();
            }
        }

        /// <summary>
        /// throws
        /// </summary>
        public static void Unregister()
        {
            foreach (var RegistryPath in RegistryPaths)
                unregisterKey(RegistryPath, RegistryPath + @"\command");
        }

        private static void unregisterKey(string fp, string FolderCommand)
        {
            try
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey(FolderCommand);
                if (reg != null)
                {
                    reg.Close();
                    Registry.CurrentUser.DeleteSubKey(FolderCommand);
                }
                reg = Registry.CurrentUser.OpenSubKey(fp);
                if (reg != null)
                {
                    reg.Close();
                    Registry.CurrentUser.DeleteSubKey(fp);
                }
            }
            finally
            {
            }
        }
    }
}
