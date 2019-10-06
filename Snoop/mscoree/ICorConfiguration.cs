﻿namespace Snoop.mscoree
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [InterfaceType(1)]
    [Guid("5C2B07A5-1E98-11D3-872F-00C04F79ED0D")]
    public interface ICorConfiguration
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetGCThreadControl([In] [MarshalAs(UnmanagedType.Interface)] IGCThreadControl pGCThreadControl);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetGCHostControl([In] [MarshalAs(UnmanagedType.Interface)] IGCHostControl pGCHostControl);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetDebuggerThreadControl([In] [MarshalAs(UnmanagedType.Interface)] IDebuggerThreadControl pDebuggerThreadControl);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void AddDebuggerSpecialThread([In] uint dwSpecialThreadId);
    }
}