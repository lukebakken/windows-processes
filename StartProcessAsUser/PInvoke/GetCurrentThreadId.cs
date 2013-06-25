namespace StartProcessAsUser.PInvoke
{
    using System.Runtime.InteropServices;

    internal partial class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetCurrentThreadId();
    }
}
