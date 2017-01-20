using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace copy_svn_path
{
    public class SvnInfo
    {
        static readonly string command = "svn";

        public static string Url(string dir)
        {
            var config = new ProcessStartInfo
            {
                FileName = command,
                UseShellExecute = false,
                Arguments = String.Format("info"),
                WorkingDirectory = dir,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle=ProcessWindowStyle.Hidden,
                CreateNoWindow=true
            };

            var process = Process.Start(config);
            var output = WriteOutputAndWaitForExit(process);

            var url = ExtractUrl(output);

            return url;
        }

        static string ExtractUrl(string svn_output)
        {
            var url_extractor = new Regex("^URL:\\s*(.*)$", RegexOptions.Compiled | RegexOptions.Multiline);
            var match = url_extractor.Match(svn_output);
            if (match.Success)
            {
                var group = match.Groups[1].Value;
                return group.ToString().Trim();
            }
            else
            {
                throw new Exception("No URL found");
            }
        }

        private static string WriteOutputAndWaitForExit(Process process)
        {
            StringBuilder B = new StringBuilder();
            process.WaitForExit();
            StreamReader s = process.StandardOutput;
            string output = s.ReadToEnd();
            if (output.Length > 0)
                B.AppendLine(output);
            s = process.StandardError;
            output = s.ReadToEnd();
            if (output.Length > 0)
                B.AppendLine(output);
            return B.ToString();
        }
    }
}
