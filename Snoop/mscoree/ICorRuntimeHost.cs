﻿namespace Snoop.mscoree
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("CB2F6722-AB3A-11D2-9C40-00C04FA30A3E")]
    [InterfaceType(1)]
    [ComConversionLoss]
    public interface ICorRuntimeHost
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateLogicalThreadState();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void DeleteLogicalThreadState();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SwitchInLogicalThreadState([In] ref uint pFiberCookie);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SwitchOutLogicalThreadState([Out] IntPtr pFiberCookie);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void LocksHeldByLogicalThread(out uint pCount);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void MapFile([In] IntPtr hFile, out IntPtr hMapAddress);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void GetConfiguration([MarshalAs(UnmanagedType.Interface)] out ICorConfiguration pConfiguration);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void Start();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void Stop();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateDomain([In] [MarshalAs(UnmanagedType.LPWStr)] string pwzFriendlyName, [In] [MarshalAs(UnmanagedType.IUnknown)]
                          object pIdentityArray, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void GetDefaultDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void EnumDomains(out IntPtr hEnum);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void NextDomain([In] IntPtr hEnum, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CloseEnum([In] IntPtr hEnum);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateDomainEx([In] [MarshalAs(UnmanagedType.LPWStr)] string pwzFriendlyName, [In] [MarshalAs(UnmanagedType.IUnknown)]
                            object pSetup, [In] [MarshalAs(UnmanagedType.IUnknown)]
                            object pEvidence, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateDomainSetup([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomainSetup);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateEvidence([MarshalAs(UnmanagedType.IUnknown)] out object pEvidence);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void UnloadDomain([In] [MarshalAs(UnmanagedType.IUnknown)]
                          object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CurrentDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);
    }
}