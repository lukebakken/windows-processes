namespace StartProcessAsUser.PInvoke
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal partial class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetEnvironmentStrings();

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static  extern bool CreateProcess
        (
            String lpApplicationName,
            String lpCommandLine,
            SecurityAttributes lpProcessAttributes, 
            SecurityAttributes lpThreadAttributes,
            Boolean bInheritHandles, 
            CreateProcessFlags dwCreationFlags,
            IntPtr lpEnvironment,
            String lpCurrentDirectory,
            [In] StartupInfo lpStartupInfo, 
            out ProcessInformation lpProcessInformation
        );

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Boolean CreateProcessAsUser 
        (
            IntPtr hToken,
            String lpApplicationName,
            [In] StringBuilder lpCommandLine,
            SecurityAttributes lpProcessAttributes,
            SecurityAttributes lpThreadAttributes,
            Boolean bInheritHandles,
            CreateProcessFlags dwCreationFlags,
            IntPtr lpEnvironment,
            String lpCurrentDirectory,
            [In] StartupInfo lpStartupInfo,
            out ProcessInformation lpProcessInformation
        );

        [Flags]
        public enum LogonFlags
        {
            LOGON_WITH_PROFILE = 0x00000001,
            LOGON_NETCREDENTIALS_ONLY = 0x00000002
        }

        [StructLayout(LayoutKind.Sequential)]
        public class StartupInfo
        {
            public Int32 cb              = 0;
            public IntPtr lpReserved     = IntPtr.Zero;
            public IntPtr lpDesktop      = IntPtr.Zero; // MUST be Zero
            public IntPtr lpTitle        = IntPtr.Zero;
            public Int32 dwX             = 0;
            public Int32 dwY             = 0;
            public Int32 dwXSize         = 0;
            public Int32 dwYSize         = 0;
            public Int32 dwXCountChars   = 0;
            public Int32 dwYCountChars   = 0;
            public Int32 dwFillAttribute = 0;
            public Int32 dwFlags         = 0;
            public Int16 wShowWindow     = 0;
            public Int16 cbReserved2     = 0;
            public IntPtr lpReserved2    = IntPtr.Zero;
            public IntPtr hStdInput      = IntPtr.Zero;
            public IntPtr hStdOutput     = IntPtr.Zero;
            public IntPtr hStdError      = IntPtr.Zero;

            public StartupInfo()
            {
                this.cb = Marshal.SizeOf(this);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInformation
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessId;
            public Int32 dwThreadId;
        }

        [Flags]
        public enum CreateProcessFlags : uint
        {
            DEBUG_PROCESS                    = 0x00000001,
            DEBUG_ONLY_THIS_PROCESS          = 0x00000002,
            CREATE_SUSPENDED                 = 0x00000004,
            DETACHED_PROCESS                 = 0x00000008,
            CREATE_NEW_CONSOLE               = 0x00000010,
            NORMAL_PRIORITY_CLASS            = 0x00000020,
            IDLE_PRIORITY_CLASS              = 0x00000040,
            HIGH_PRIORITY_CLASS              = 0x00000080,
            REALTIME_PRIORITY_CLASS          = 0x00000100,
            CREATE_NEW_PROCESS_GROUP         = 0x00000200,
            CREATE_UNICODE_ENVIRONMENT       = 0x00000400,
            CREATE_SEPARATE_WOW_VDM          = 0x00000800,
            CREATE_SHARED_WOW_VDM            = 0x00001000,
            CREATE_FORCEDOS                  = 0x00002000,
            BELOW_NORMAL_PRIORITY_CLASS      = 0x00004000,
            ABOVE_NORMAL_PRIORITY_CLASS      = 0x00008000,
            INHERIT_PARENT_AFFINITY          = 0x00010000,
            INHERIT_CALLER_PRIORITY          = 0x00020000,
            CREATE_PROTECTED_PROCESS         = 0x00040000,
            EXTENDED_STARTUPINFO_PRESENT     = 0x00080000,
            PROCESS_MODE_BACKGROUND_BEGIN    = 0x00100000,
            PROCESS_MODE_BACKGROUND_END      = 0x00200000,
            CREATE_BREAKAWAY_FROM_JOB        = 0x01000000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_DEFAULT_ERROR_MODE        = 0x04000000,
            CREATE_NO_WINDOW                 = 0x08000000,
            PROFILE_USER                     = 0x10000000,
            PROFILE_KERNEL                   = 0x20000000,
            PROFILE_SERVER                   = 0x40000000,
            CREATE_IGNORE_SYSTEM_DEFAULT     = 0x80000000,
        }

        [Flags]
        public enum DuplicateOptions : uint
        {
            DUPLICATE_CLOSE_SOURCE = 0x00000001,
            DUPLICATE_SAME_ACCESS  = 0x00000002
        }
    }
}
