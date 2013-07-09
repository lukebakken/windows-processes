namespace StartProcessLib.PInvoke
{
    using System;
    using System.Runtime.InteropServices;

    public partial class NativeMethods
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Boolean LogonUser(
            String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            LogonType dwLogonType,
            LogonProvider dwLogonProvider,
            out IntPtr phToken);

        [DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LoadUserProfile(IntPtr hToken, ProfileInfo lpProfileInfo);

        [DllImport("userenv.dll", SetLastError=true, CharSet=CharSet.Auto)]
        public static extern bool UnloadUserProfile(IntPtr hToken, IntPtr hProfile);

        [StructLayout(LayoutKind.Sequential)]
        public class ProfileInfo
        {
            public Int32 dwSize; 
            public Int32 dwFlags;

            [MarshalAs(UnmanagedType.LPTStr)] 
            public String lpUserName; 

            [MarshalAs(UnmanagedType.LPTStr)] 
            public String lpProfilePath; 

            [MarshalAs(UnmanagedType.LPTStr)] 
            public String lpDefaultPath; 

            [MarshalAs(UnmanagedType.LPTStr)] 
            public String lpServerName; 

            [MarshalAs(UnmanagedType.LPTStr)] 
            public String lpPolicyPath; 

            public IntPtr hProfile;

            public ProfileInfo()
            {
                this.dwSize = Marshal.SizeOf(this);
            }
        }
    }
}
