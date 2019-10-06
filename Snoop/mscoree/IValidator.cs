﻿namespace Snoop.mscoree
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComImport]
    [InterfaceType(1)]
    [Guid("63DF8730-DC81-4062-84A2-1FF943F59FAC")]
    public interface IValidator
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void Validate([In] [MarshalAs(UnmanagedType.Interface)]
                      IVEHandler veh, [In] [MarshalAs(UnmanagedType.IUnknown)]
                      object pAppDomain, [In] uint ulFlags, [In] uint ulMaxError, [In] uint Token, [In] [MarshalAs(UnmanagedType.LPWStr)] string fileName, [In] ref byte pe, [In] uint ulSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void FormatEventInfo([In] [MarshalAs(UnmanagedType.Error)] int hVECode, [In] tag_VerError Context, [In] [Out] [MarshalAs(UnmanagedType.LPWStr)]
                             StringBuilder msg, [In] uint ulMaxLength, [In] [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]
                             Array psa);
    }
}