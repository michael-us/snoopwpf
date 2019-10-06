﻿namespace Snoop.mscoree
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("BF24142D-A47D-4D24-A66D-8C2141944E44")]
    [InterfaceType(1)]
    public interface IDebuggerInfo
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void IsDebuggerAttached(out int pbAttached);
    }
}