﻿namespace Snoop.mscoree
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("23D86786-0BB5-4774-8FB5-E3522ADD6246")]
    [InterfaceType(1)]
    public interface IDebuggerThreadControl
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void ThreadIsBlockingForDebugger();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void ReleaseAllRuntimeThreads();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void StartBlockingForDebugger(uint dwUnused);
    }
}