﻿namespace Snoop.mscoree
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("FAC34F6E-0DCD-47B5-8021-531BC5ECCA63")]
    [InterfaceType(1)]
    public interface IGCHost
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetGCStartupLimits([In] uint SegmentSize, [In] uint MaxGen0Size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void Collect([In] int Generation);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void GetStats([In] [Out] ref _COR_GC_STATS pStats);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void GetThreadStats([In] ref uint pFiberCookie, [In] [Out] ref _COR_GC_THREAD_STATS pStats);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetVirtualMemLimit([In] [ComAliasName("mscoree.ULONG_PTR")] uint sztMaxVirtualMemMB);
    }
}