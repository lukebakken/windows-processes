namespace StartProcessLib.PInvoke
{
    using System;
    using Microsoft.Win32.SafeHandles;

    internal sealed class SafeUserTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeUserTokenHandle() : base(true)
        {
        }

        public SafeUserTokenHandle(IntPtr existingHandle) : base(true)
        {
            base.SetHandle(existingHandle);
        }

        public static explicit operator IntPtr(SafeUserTokenHandle userTokenHandle)
        {
            return userTokenHandle.DangerousGetHandle();
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(this.handle);
        }
    }
}
