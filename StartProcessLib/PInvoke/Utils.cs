namespace StartProcessLib.PInvoke
{
    using System;
    using System.ComponentModel;

    public static class Utils
    {
        public static IntPtr LogonAndGetPrimaryToken(string userName, string password)
        {
            IntPtr primaryToken = IntPtr.Zero;

            if (NativeMethods.RevertToSelf())
            {
                if (NativeMethods.LogonUser(userName, ".", password,
                    NativeMethods.LogonType.LOGON32_LOGON_INTERACTIVE,
                    NativeMethods.LogonProvider.LOGON32_PROVIDER_DEFAULT,
                    out primaryToken))
                {
                    return primaryToken;
                }
                else
                {
                    throw new Win32Exception();
                }
            }
            else
            {
                throw new Win32Exception();
            }
        }
    }
}
