﻿namespace Snoop.mscoree
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct _COR_GC_THREAD_STATS
    {
        public ulong PerThreadAllocation;

        public uint Flags;
    }
}