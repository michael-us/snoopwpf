﻿namespace Snoop.mscoree
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct _COR_GC_STATS
    {
        public uint Flags;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint ExplicitGCCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = (UnmanagedType)80)]
        public uint[] GenCollectionsTaken;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint CommittedKBytes;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint ReservedKBytes;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint Gen0HeapSizeKBytes;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint Gen1HeapSizeKBytes;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint Gen2HeapSizeKBytes;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint LargeObjectHeapSizeKBytes;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint KBytesPromotedFromGen0;

        [ComAliasName("mscoree.ULONG_PTR")]
        public uint KBytesPromotedFromGen1;
    }
}