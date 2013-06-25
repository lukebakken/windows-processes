namespace StartProcessAsUser.PInvoke
{
    using System.Runtime.InteropServices;

    internal partial class NativeMethods
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();
    }
}
