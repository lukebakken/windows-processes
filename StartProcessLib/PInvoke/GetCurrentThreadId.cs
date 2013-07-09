namespace StartProcessLib.PInvoke
{
    using System.Runtime.InteropServices;

    public partial class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetCurrentThreadId();
    }
}
