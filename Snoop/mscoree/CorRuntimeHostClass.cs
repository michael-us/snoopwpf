﻿namespace Snoop.mscoree
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComImport]
    [Guid("CB2F6723-AB3A-11D2-9C40-00C04FA30A3E")]
    [TypeLibType(2)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class CorRuntimeHostClass : ICorRuntimeHost, CorRuntimeHost, IGCHost, ICorConfiguration, IValidator, IDebuggerInfo
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void CreateLogicalThreadState();

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void DeleteLogicalThreadState();

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void SwitchInLogicalThreadState([In] ref uint pFiberCookie);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void SwitchOutLogicalThreadState([Out] IntPtr pFiberCookie);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void LocksHeldByLogicalThread(out uint pCount);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void MapFile([In] IntPtr hFile, out IntPtr hMapAddress);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void GetConfiguration([MarshalAs(UnmanagedType.Interface)] out ICorConfiguration pConfiguration);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void Start();

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void Stop();

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void CreateDomain([In] [MarshalAs(UnmanagedType.LPWStr)] string pwzFriendlyName, [In] [MarshalAs(UnmanagedType.IUnknown)]
                                                object pIdentityArray, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void GetDefaultDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void EnumDomains(out IntPtr hEnum);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void NextDomain([In] IntPtr hEnum, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void CloseEnum([In] IntPtr hEnum);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void CreateDomainEx([In] [MarshalAs(UnmanagedType.LPWStr)] string pwzFriendlyName, [In] [MarshalAs(UnmanagedType.IUnknown)]
                                                  object pSetup, [In] [MarshalAs(UnmanagedType.IUnknown)]
                                                  object pEvidence, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void CreateDomainSetup([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomainSetup);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void CreateEvidence([MarshalAs(UnmanagedType.IUnknown)] out object pEvidence);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void UnloadDomain([In] [MarshalAs(UnmanagedType.IUnknown)]
                                                object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void CurrentDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void SetGCStartupLimits([In] uint SegmentSize, [In] uint MaxGen0Size);       

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void Collect([In] int Generation);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void GetStats([In] [Out] ref _COR_GC_STATS pStats);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void GetThreadStats([In] ref uint pFiberCookie, [In] [Out] ref _COR_GC_THREAD_STATS pStats);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void SetVirtualMemLimit([In] [ComAliasName("mscoree.ULONG_PTR")]
                                                      uint sztMaxVirtualMemMB);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void SetGCThreadControl([In] [MarshalAs(UnmanagedType.Interface)]
                                                      IGCThreadControl pGCThreadControl);


        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void SetGCHostControl([In] [MarshalAs(UnmanagedType.Interface)]
                                                    IGCHostControl pGCHostControl);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void SetDebuggerThreadControl([In] [MarshalAs(UnmanagedType.Interface)]
                                                            IDebuggerThreadControl pDebuggerThreadControl);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void AddDebuggerSpecialThread([In] uint dwSpecialThreadId);      

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void Validate([In] [MarshalAs(UnmanagedType.Interface)]
                                            IVEHandler veh, [In] [MarshalAs(UnmanagedType.IUnknown)]
                                            object pAppDomain, [In] uint ulFlags, [In] uint ulMaxError, [In] uint Token, [In] [MarshalAs(UnmanagedType.LPWStr)] string fileName, [In] ref byte pe, [In] uint ulSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void FormatEventInfo([In] [MarshalAs(UnmanagedType.Error)] int hVECode, [In] tag_VerError Context, [In] [Out] [MarshalAs(UnmanagedType.LPWStr)]
                                                   StringBuilder msg, [In] uint ulMaxLength, [In] [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]
                                                   Array psa);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void IsDebuggerAttached(out int pbAttached);
    }
}