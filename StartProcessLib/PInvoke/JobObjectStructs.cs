// -----------------------------------------------------------------------
// <copyright company="Uhuru Software, Inc.">
// Copyright (c) 2011 Uhuru Software, Inc., All Rights Reserved
// </copyright>
// Modified by Luke Bakken from the original.
// -----------------------------------------------------------------------

namespace StartProcessLib.PInvoke
{
    using System;
    using System.Runtime.InteropServices;

    public partial class NativeMethods
    {
        /// <summary>
        /// Contains basic and extended limit information for a job object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_BASIC_LIMIT_INFORMATION
        {
            /// <summary>
            /// JOB_OBJECT_LIMIT_ACTIVE_PROCESS Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008;

            /// <summary>
            /// JOB_OBJECT_LIMIT_AFFINITY Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_AFFINITY = 0x00000010;

            /// <summary>
            /// JOB_OBJECT_LIMIT_BREAKAWAY_OK Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800;

            /// <summary>
            /// JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400;

            /// <summary>
            /// JOB_OBJECT_LIMIT_JOB_MEMORY Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200;

            /// <summary>
            /// JOB_OBJECT_LIMIT_JOB_TIME Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004;

            /// <summary>
            /// JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000;

            /// <summary>
            /// JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040;

            /// <summary>
            /// JOB_OBJECT_LIMIT_PRIORITY_CLASS Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020;

            /// <summary>
            /// JOB_OBJECT_LIMIT_PROCESS_MEMORY Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100;

            /// <summary>
            /// JOB_OBJECT_LIMIT_PROCESS_TIME Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002;

            /// <summary>
            /// JOB_OBJECT_LIMIT_SCHEDULING_CLASS Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080;

            /// <summary>
            /// JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000;

            /// <summary>
            /// JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_SUBSET_AFFINITY = 0x00004000;

            /// <summary>
            /// JOB_OBJECT_LIMIT_WORKINGSET Windows API constant.
            /// </summary>
            public const uint JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001;

            /// <summary>
            /// Per process user time limit.
            /// </summary>
            public long PerProcessUserTimeLimit;

            /// <summary>
            /// Per job user time limit.
            /// </summary>
            public long PerJobUserTimeLimit;

            /// <summary>
            /// Limit flags.
            /// </summary>
            public uint LimitFlags;

            /// <summary>
            /// Minimum working set size.
            /// </summary>
            public IntPtr MinimumWorkingSetSize;

            /// <summary>
            /// Maximum working set size.
            /// </summary>
            public IntPtr MaximumWorkingSetSize;

            /// <summary>
            /// Active process limit.
            /// </summary>
            public uint ActiveProcessLimit;

            /// <summary>
            /// Processor affinity.
            /// </summary>
            public IntPtr Affinity;

            /// <summary>
            /// Priority class.
            /// </summary>
            public uint PriorityClass;

            /// <summary>
            /// Scheduling class.
            /// </summary>
            public uint SchedulingClass;
        }

        /// <summary>
        /// JOBOBJECT_BASIC_UI_RESTRICTIONS Windows API structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_BASIC_UI_RESTRICTIONS
        {
            /// <summary>
            /// UI Restrictions class.
            /// </summary>
            public uint UIRestrictionsClass;
        }

        /// <summary>
        /// JOBOBJECT_CPU_RATE_CONTROL_INFORMATION Windows API structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_CPU_RATE_CONTROL_INFORMATION
        {
            /// <summary>
            /// Control Flags.
            /// </summary>
            public uint ControlFlags;

            /// <summary>
            /// CPU rate weight.
            /// </summary>
            public uint CpuRate_Weight;
        }

        /// <summary>
        /// JOBOBJECT_EXTENDED_LIMIT_INFORMATION Windows API structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        {
            /// <summary>
            /// BasicLimitInformation Windows API structure member.
            /// </summary>
            public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;

            /// <summary>
            /// IoInfo Windows API structure member.
            /// </summary>
            public IO_COUNTERS IoInfo;

            /// <summary>
            /// ProcessMemoryLimit Windows API structure member.
            /// </summary>
            public IntPtr ProcessMemoryLimit;

            /// <summary>
            /// JobMemoryLimit Windows API structure member.
            /// </summary>
            public IntPtr JobMemoryLimit;

            /// <summary>
            /// PeakProcessMemoryUsed Windows API structure member.
            /// </summary>
            public IntPtr PeakProcessMemoryUsed;

            /// <summary>
            /// PeakJobMemoryUsed Windows API structure member.
            /// </summary>
            public IntPtr PeakJobMemoryUsed;
        }

        /// <summary>
        /// IO_COUNTERS Windows API structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct IO_COUNTERS
        {
            /// <summary>
            /// ReadOperationCount Windows API structure member.
            /// </summary>
            public ulong ReadOperationCount;

            /// <summary>
            /// WriteOperationCount Windows API structure member.
            /// </summary>
            public ulong WriteOperationCount;

            /// <summary>
            /// OtherOperationCount Windows API structure member.
            /// </summary>
            public ulong OtherOperationCount;

            /// <summary>
            /// ReadTransferCount Windows API structure member.
            /// </summary>
            public ulong ReadTransferCount;

            /// <summary>
            /// WriteTransferCount Windows API structure member.
            /// </summary>
            public ulong WriteTransferCount;

            /// <summary>
            /// OtherTransferCount Windows API structure member.
            /// </summary>
            public ulong OtherTransferCount;
        }

        /// <summary>
        /// JOBOBJECT_BASIC_ACCOUNTING_INFORMATION Windows API structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_BASIC_ACCOUNTING_INFORMATION
        {
            /// <summary>
            /// TotalUserTime Windows API structure member.
            /// </summary>
            public ulong TotalUserTime;

            /// <summary>
            /// TotalKernelTime Windows API structure member.
            /// </summary>
            public ulong TotalKernelTime;

            /// <summary>
            /// ThisPeriodTotalUserTime Windows API structure member.
            /// </summary>
            public ulong ThisPeriodTotalUserTime;

            /// <summary>
            /// ThisPeriodTotalKernelTime Windows API structure member.
            /// </summary>
            public ulong ThisPeriodTotalKernelTime;

            /// <summary>
            /// TotalPageFaultCount Windows API structure member.
            /// </summary>
            public uint TotalPageFaultCount;

            /// <summary>
            /// TotalProcesses Windows API structure member.
            /// </summary>
            public uint TotalProcesses;

            /// <summary>
            /// ActiveProcesses Windows API structure member.
            /// </summary>
            public uint ActiveProcesses;

            /// <summary>
            /// TotalTerminatedProcesses Windows API structure member.
            /// </summary>
            public uint TotalTerminatedProcesses;
        }

        /// <summary>
        /// JOBOBJECT_BASIC_AND_IO_ACCOUNTING_INFORMATION Windows API structure.
        /// </summary>
        public struct JOBOBJECT_BASIC_AND_IO_ACCOUNTING_INFORMATION
        {
            /// <summary>
            /// BasicInfo Windows API structure member.
            /// </summary>
            public JOBOBJECT_BASIC_ACCOUNTING_INFORMATION BasicInfo;

            /// <summary>
            /// IoInfo Windows API structure member.
            /// </summary>
            public IO_COUNTERS IoInfo;
        }

        /// <summary>
        /// JOBOBJECT_BASIC_PROCESS_ID_LIST Windows API structure.
        /// </summary>
        public struct JOBOBJECT_BASIC_PROCESS_ID_LIST
        {
            /// <summary>
            /// The maximum number of processes that are allocated when querying Windows API. 
            /// </summary>
            public const uint MaxProcessListLength = 200;

            /// <summary>
            /// NumberOfAssignedProcesses Windows API structure member.
            /// </summary>
            public uint NumberOfAssignedProcesses;

            /// <summary>
            /// NumberOfProcessIdsInList Windows API structure member.
            /// </summary>
            public uint NumberOfProcessIdsInList;

            /// <summary>
            /// ProcessIdList Windows API structure member.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)MaxProcessListLength)]
            public IntPtr[] ProcessIdList;
        }
    }
}
