namespace StartProcessAsUser.PInvoke
{
    using System;

    internal partial class NativeMethods
    {
        public class Constants
        {
            public const Int32  GENERIC_ALL_ACCESS = 0x10000000;
            public const UInt32 INFINITE = 0xFFFFFFFF;
            public const UInt32 WAIT_FAILED = 0xFFFFFFFF;
        }
    }
}
