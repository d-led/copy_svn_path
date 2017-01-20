using System;
using System.Linq;
using System.Text;

namespace copy_svn_path
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args.Count())
            {
                case 0:
                    RemoveFromRegistry();
                    AddToRegistry();
                    break;
                case 1:
                    if (args[0] == "unregister")
                        RemoveFromRegistry();
                    else
                        CopyPathToClipboard(args[0]);
                    break;
                default:
                    Clipboard.SetText(new StringBuilder()
                        .AppendLine("USAGE:")
                        .AppendLine(" copy_svn_path : register explorer context menu entry")
                        .AppendLine(" copy_svn_path <dir> : copy svn path into registry (svn should be in %PATH%)")
                        .AppendLine(" copy_svn_path unregister : remove explorer context menu entry")
                        .ToString());
                    break;
            }
        }

        static void CopyPathToClipboard(string dir)
        {
            try
            {
                Clipboard.SetText(SvnInfo.Url(dir));
                System.Media.SystemSounds.Beep.Play();
            }
            catch (Exception e)
            {
                Clipboard.SetText(String.Format("Error: {0} @ {1}", e.Message, dir));
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private static void RemoveFromRegistry()
        {
            try
            {
                RegistryAccessor.Unregister();
            }
            catch (Exception e)
            {
                Clipboard.SetText(String.Format("Error: {0}", e.Message));
            }
        }

        private static void AddToRegistry()
        {
            try
            {
                RegistryAccessor.Register();
            }
            catch (Exception e)
            {
                Clipboard.SetText(String.Format("Error: {0}", e.Message));
            }
        }
    }
}
