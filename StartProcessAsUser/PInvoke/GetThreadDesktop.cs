namespace StartProcessAsUser.PInvoke
{
    using System;
    using System.Runtime.InteropServices;

    internal partial class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetThreadDesktop(int dwThreadId);
    }
}
