﻿namespace Snoop.mscoree
{
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("CB2F6722-AB3A-11D2-9C40-00C04FA30A3E")]
    [CoClass(typeof(CorRuntimeHostClass))]
    public interface CorRuntimeHost : ICorRuntimeHost
    {
    }
}