﻿using System;
using System.Runtime.InteropServices;

namespace copy_svn_path
{
    /// <summary>
    /// http://stackoverflow.com/a/13571530/847349
    /// </summary>
    public static class Clipboard
    {
        [DllImport("user32.dll")]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        internal static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

        public static void SetText(string text)
        {
            OpenClipboard(IntPtr.Zero);
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.StringToHGlobalUni(text);
                SetClipboardData(13, ptr);
                CloseClipboard();
            }
            finally
            {
                try
                {
                    if (ptr != IntPtr.Zero)
                        Marshal.FreeHGlobal(ptr);
                }
                catch { /*swallow*/ }
            }
        }
    }
}
