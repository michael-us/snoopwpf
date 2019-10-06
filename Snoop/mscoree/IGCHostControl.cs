﻿namespace Snoop.mscoree
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("5513D564-8374-4CB9-AED9-0083F4160A1D")]
    [InterfaceType(1)]
    public interface IGCHostControl
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void RequestVirtualMemLimit([In] [ComAliasName("mscoree.ULONG_PTR")] uint sztMaxVirtualMemMB, [In] [Out] [ComAliasName("mscoree.ULONG_PTR")] ref uint psztNewMaxVirtualMemMB);
    }
}