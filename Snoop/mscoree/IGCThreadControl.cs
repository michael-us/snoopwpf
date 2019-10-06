﻿namespace Snoop.mscoree
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [InterfaceType(1)]
    [Guid("F31D1788-C397-4725-87A5-6AF3472C2791")]
    public interface IGCThreadControl
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void ThreadIsBlockingForSuspension();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SuspensionStarting();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SuspensionEnding(uint Generation);
    }
}