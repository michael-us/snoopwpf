﻿namespace Snoop.mscoree
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("856CA1B2-7DAB-11D3-ACEC-00C04F86C309")]
    [InterfaceType(1)]
    public interface IVEHandler
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void VEHandler([In] [MarshalAs(UnmanagedType.Error)] int VECode, [In] tag_VerError Context, [In] [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]
                       Array psa);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetReporterFtn([In] long lFnPtr);
    }
}