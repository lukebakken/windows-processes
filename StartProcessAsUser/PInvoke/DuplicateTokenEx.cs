namespace StartProcessAsUser.PInvoke
{
    using System;
    using System.Runtime.InteropServices;

    internal partial class NativeMethods
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            uint dwDesiredAccess,
            SecurityAttributes lpTokenAttributes,
            SecurityImpersonationLevel impersonationLevel,
            TokenType tokenType,
            out IntPtr hNewToken);
    }
}
