// -----------------------------------------------------------------------
// <copyright file="JobObjectHandle.cs" company="Uhuru Software, Inc.">
// Copyright (c) 2011 Uhuru Software, Inc., All Rights Reserved
// </copyright>
// Modified by Luke Bakken from the original.
// -----------------------------------------------------------------------

namespace StartProcessLib.PInvoke
{
    using System.Runtime.ConstrainedExecution;
    using System.Security.Permissions;
    using Microsoft.Win32.SafeHandles;

    public partial class NativeMethods
    {
        /// <summary>
        /// A safe handle for Job Object.
        /// </summary>
        [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public class JobObjectHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            /// <summary>
            /// Prevents a default instance of the <see cref="JobObjectHandle"/> class from being created.
            /// </summary>
            private JobObjectHandle()
                : base(true)
            {
            }

            /// <summary>
            /// When overridden in a derived class, executes the code required to free the handle.
            /// </summary>
            /// <returns>
            /// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
            /// </returns>
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                return NativeMethods.CloseHandle(handle);
            }
        }
    }
}
