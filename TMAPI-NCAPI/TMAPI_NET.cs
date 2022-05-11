// ************************************************* //
//    --- Copyright (c) 2014 iMCS Productions ---    //
// ************************************************* //
//              PS3Lib v4 By FM|T iMCSx              //
//                                                   //
// Features v4.4 :                                   //
// - Support CCAPI v2.6 C# by iMCSx                  //
// - Set Boot Console ID                             //
// - Popup better form with icon                     //
// - CCAPI Consoles List Popup French/English        //
// - CCAPI Get Console Info                          //
// - CCAPI Get Console List                          //
// - CCAPI Get Number Of Consoles                    //
// - Get Console Name TMAPI/CCAPI                    //
//                                                   //
// Credits : FM|T Enstone , Buc-ShoTz                //
//                                                   //
// Follow me :                                       //
//                                                   //
// FrenchModdingTeam.com                             //
// Youtube.com/iMCSx                                 //
// Twitter.com/iMCSx                                 //
// Facebook.com/iMCSx                                //
//                                                   //
// ************************************************* //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TMAPI_NCAPI
{
    public class PS3TMAPI
    {
        public enum SNRESULT
        {
            SN_E_BAD_ALIGN = -28,
            SN_E_BAD_MEMSPACE = -18,
            SN_E_BAD_PARAM = -21,
            SN_E_BAD_TARGET = -3,
            SN_E_BAD_UNIT = -11,
            SN_E_BUSY = -22,
            SN_E_CHECK_TARGET_CONFIGURATION = -33,
            SN_E_COMMAND_CANCELLED = -36,
            SN_E_COMMS_ERR = -5,
            SN_E_COMMS_EVENT_MISMATCHED_ERR = -39,
            SN_E_CONNECT_TO_GAMEPORT_FAILED = -35,
            SN_E_CONNECTED = -38,
            SN_E_DATA_TOO_LONG = -26,
            SN_E_DECI_ERROR = -23,
            SN_E_DEPRECATED = -27,
            SN_E_DLL_NOT_INITIALISED = -15,
            SN_E_ERROR = -2147483648,
            SN_E_EXISTING_CALLBACK = -24,
            SN_E_FILE_ERROR = -29,
            SN_E_HOST_NOT_FOUND = -8,
            SN_E_INSUFFICIENT_DATA = -25,
            SN_E_LICENSE_ERROR = -32,
            SN_E_LOAD_ELF_FAILED = -10,
            SN_E_LOAD_MODULE_FAILED = -31,
            SN_E_MODULE_NOT_FOUND = -34,
            SN_E_NO_SEL = -20,
            SN_E_NO_TARGETS = -19,
            SN_E_NOT_CONNECTED = -4,
            SN_E_NOT_IMPL = -1,
            SN_E_NOT_LISTED = -13,
            SN_E_NOT_SUPPORTED_IN_SDK_VERSION = -30,
            SN_E_OUT_OF_MEM = -12,
            SN_E_PROTOCOL_ALREADY_REGISTERED = -37,
            SN_E_TARGET_IN_USE = -9,
            SN_E_TARGET_RUNNING = -17,
            SN_E_TIMEOUT = -7,
            SN_E_TM_COMMS_ERR = -6,
            SN_E_TM_NOT_RUNNING = -2,
            SN_E_TM_VERSION = -14,
            SN_S_NO_ACTION = 6,
            SN_S_NO_MSG = 3,
            SN_S_OK = 0,
            SN_S_PENDING = 1,
            SN_S_REPLACED = 5,
            SN_S_TARGET_STILL_REGISTERED = 7,
            SN_S_TM_VERSION = 4
        }

        public enum UnitType
        {
            PPU,
            SPU,
            SPURAW
        }

        [Flags]
        public enum ResetParameter : ulong
        {
            Hard = 1L,
            Quick = 2L,
            ResetEx = 9223372036854775808L,
            Soft = 0L
        }

        private class ScopedGlobalHeapPtr
        {
            private IntPtr m_intPtr = IntPtr.Zero;

            public ScopedGlobalHeapPtr(IntPtr intPtr)
            {
                this.m_intPtr = intPtr;
            }

            ~ScopedGlobalHeapPtr()
            {
                if (this.m_intPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.m_intPtr);
                }
            }

            public IntPtr Get()
            {
                return this.m_intPtr;
            }
        }

        public enum ConnectStatus
        {
            Connected,
            Connecting,
            NotConnected,
            InUse,
            Unavailable
        }

        [StructLayout(LayoutKind.Sequential)]
        public class TCPIPConnectProperties
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0xff)]
            public string IPAddress;
            public uint Port;
        }

        [Flags]
        public enum TargetInfoFlag : uint
        {
            Boot = 0x20,
            FileServingDir = 0x10,
            HomeDir = 8,
            Info = 4,
            Name = 2,
            TargetID = 1
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TargetInfoPriv
        {
            public PS3TMAPI.TargetInfoFlag Flags;
            public int Target;
            public IntPtr Name;
            public IntPtr Type;
            public IntPtr Info;
            public IntPtr HomeDir;
            public IntPtr FSDir;
            public PS3TMAPI.BootParameter Boot;
        }

        [Flags]
        public enum BootParameter : ulong
        {
            BluRayEmuOff = 4L,
            BluRayEmuUSB = 0x20L,
            DebugMode = 0x10L,
            Default = 0L,
            DualNIC = 0x80L,
            HDDSpeedBluRayEmu = 8L,
            HostFSTarget = 0x40L,
            MemSizeConsole = 2L,
            ReleaseMode = 1L,
            SystemMode = 0x11L
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TargetInfo
        {
            public PS3TMAPI.TargetInfoFlag Flags;
            public int Target;
            public string Name;
            public string Type;
            public string Info;
            public string HomeDir;
            public string FSDir;
            public PS3TMAPI.BootParameter Boot;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PPUThreadInfo
        {
            public ulong ThreadID;
            public uint Priority;
            public PPUThreadState State;
            public ulong StackAddress;
            public ulong StackSize;
            public string ThreadName;
        }

        [Flags]
        public enum PPUThreadState : uint
        {
            Idle = 0x00000000,
            Runnable = 0x00000001,
            OnProc = 0x00000002,
            Sleep = 0x00000003,
            Suspended = 0x00000004,
            SleepSuspended = 0x00000005,
            Stop = 0x00000006,
            Zombie = 0x00000007,
            Deleted = 0x00000008
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInfo
        {
            public ulong ThreadID;
            public uint Priority;
            public PPUThreadState State;
            public ulong StackAddress;
            public ulong StackSize;
            public string ThreadName;
        }

        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3InitTargetComms", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT InitTargetCommsX64();
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3InitTargetComms", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT InitTargetCommsX86();
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3PowerOn", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT PowerOnX64(int target);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3PowerOn", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT PowerOnX86(int target);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3PowerOff", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT PowerOffX64(int target, uint force);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3PowerOff", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT PowerOffX86(int target, uint force);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3Connect", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ConnectX64(int target, string application);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3Connect", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ConnectX86(int target, string application);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetConnectionInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetConnectionInfoX64(int target, IntPtr connectProperties);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetConnectionInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetConnectionInfoX86(int target, IntPtr connectProperties);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetConnectStatus", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetConnectStatusX64(int target, out uint status, out IntPtr usage);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetConnectStatus", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetConnectStatusX86(int target, out uint status, out IntPtr usage);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int MultiByteToWideChar(int codepage, int flags, IntPtr utf8, int utf8len, StringBuilder buffer, int buflen);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessList", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetProcessListX64(int target, ref uint count, IntPtr processIdArray);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessList", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetProcessListX86(int target, ref uint count, IntPtr processIdArray);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessContinue", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ProcessContinueX64(int target, uint processId);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessContinue", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ProcessContinueX86(int target, uint processId);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessAttach", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ProcessAttachX64(int target, uint unitId, uint processId);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessAttach", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ProcessAttachX86(int target, uint unitId, uint processId);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessGetMemory", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ProcessGetMemoryX64(int target, UnitType unit, uint processId, ulong threadId, ulong address, int count, byte[] buffer);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessGetMemory", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ProcessGetMemoryX86(int target, UnitType unit, uint processId, ulong threadId, ulong address, int count, byte[] buffer);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetTargetFromName", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetTargetFromNameX64(IntPtr name, out int target);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetTargetFromName", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetTargetFromNameX86(IntPtr name, out int target);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3Reset", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ResetX64(int target, ulong resetParameter);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3Reset", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ResetX86(int target, ulong resetParameter);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ProcessSetMemory", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ProcessSetMemoryX64(int target, UnitType unit, uint processId, ulong threadId, ulong address, int count, byte[] buffer);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ProcessSetMemory", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ProcessSetMemoryX86(int target, UnitType unit, uint processId, ulong threadId, ulong address, int count, byte[] buffer);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetTargetInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetTargetInfoX64(ref TargetInfoPriv targetInfoPriv);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetTargetInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetTargetInfoX86(ref TargetInfoPriv targetInfoPriv);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3Disconnect", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT DisconnectX64(int target);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3Disconnect", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT DisconnectX86(int target);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3ThreadList", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ThreadListX64(int target, uint processID, uint[] numPPU, ulong[] ppuThreadIDs, uint[] numSPU, ulong[] spuThreadIDs);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3ThreadList", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT ThreadListX86(int target, uint processID, uint[] numPPU, ulong[] ppuThreadIDs, uint[] numSPU, ulong[] spuThreadIDs);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3PPUThreadInfoEx", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT PPUThreadInfoX64(int target, uint processID, ulong threadID, uint[] bufferSize, byte[] buffer);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3PPUThreadInfoEx", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT PPUThreadInfoX86(int target, uint processID, ulong threadID, uint[] bufferSize, byte[] buffer);
        [DllImport("PS3TMAPIX64.dll", EntryPoint = "SNPS3GetProcessInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetProcessInfoX64(int target, uint processID, out ProcessInfo pInfo);
        [DllImport("PS3TMAPI.dll", EntryPoint = "SNPS3GetProcessInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern SNRESULT GetProcessInfoX86(int target, uint processID, out ProcessInfo pInfo);

        private static bool Is32Bit()
        {
            return (IntPtr.Size == 4);
        }

        public static bool FAILED(SNRESULT res)
        {
            return !SUCCEEDED(res);
        }

        public static bool SUCCEEDED(SNRESULT res)
        {
            return (res >= SNRESULT.SN_S_OK);
        }

        private static IntPtr AllocUtf8FromString(string wcharString)
        {
            if (wcharString == null)
            {
                return IntPtr.Zero;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(wcharString);
            IntPtr destination = Marshal.AllocHGlobal((int)(bytes.Length + 1));
            Marshal.Copy(bytes, 0, destination, bytes.Length);
            Marshal.WriteByte((IntPtr)(destination.ToInt64() + bytes.Length), 0);
            return destination;
        }

        public static string Utf8ToString(IntPtr utf8, uint maxLength)
        {
            int len = MultiByteToWideChar(65001, 0, utf8, -1, null, 0);
            if (len == 0) throw new System.ComponentModel.Win32Exception();
            var buf = new StringBuilder(len);
            len = MultiByteToWideChar(65001, 0, utf8, -1, buf, len);
            return buf.ToString();
        }

        private static IntPtr ReadDataFromUnmanagedIncPtr<T>(IntPtr unmanagedBuf, ref T storage)
        {
            storage = (T)Marshal.PtrToStructure(unmanagedBuf, typeof(T));
            return new IntPtr(unmanagedBuf.ToInt64() + Marshal.SizeOf((T)storage));
        }

        public static SNRESULT InitTargetComms()
        {
            if (!Is32Bit())
            {
                return InitTargetCommsX64();
            }
            return InitTargetCommsX86();
        }

        public static SNRESULT Connect(int target, string application)
        {
            if (!Is32Bit())
            {
                return ConnectX64(target, application);
            }
            return ConnectX86(target, application);
        }

        public static SNRESULT PowerOn(int target)
        {
            if (!Is32Bit())
            {
                return PowerOnX64(target);
            }
            return PowerOnX86(target);
        }

        public static SNRESULT PowerOff(int target, bool bForce)
        {
            uint force = bForce ? (uint)1 : 0;
            if (!Is32Bit())
            {
                return PowerOffX64(target, force);
            }
            return PowerOffX86(target, force);
        }

        public static SNRESULT GetProcessList(int target, out uint[] processIDs)
        {
            processIDs = null;
            uint count = 0;
            SNRESULT res = Is32Bit() ? GetProcessListX86(target, ref count, IntPtr.Zero) : GetProcessListX64(target, ref count, IntPtr.Zero);
            if (!FAILED(res))
            {
                ScopedGlobalHeapPtr ptr = new ScopedGlobalHeapPtr(Marshal.AllocHGlobal((int)(4 * count)));
                res = Is32Bit() ? GetProcessListX86(target, ref count, ptr.Get()) : GetProcessListX64(target, ref count, ptr.Get());
                if (FAILED(res))
                {
                    return res;
                }
                IntPtr unmanagedBuf = ptr.Get();
                processIDs = new uint[count];
                for (uint i = 0; i < count; i++)
                {
                    unmanagedBuf = ReadDataFromUnmanagedIncPtr<uint>(unmanagedBuf, ref processIDs[i]);
                }
            }
            return res;
        }

        public static SNRESULT ProcessAttach(int target, UnitType unit, uint processID)
        {
            if (!Is32Bit())
            {
                return ProcessAttachX64(target, (uint)unit, processID);
            }
            return ProcessAttachX86(target, (uint)unit, processID);
        }

        public static SNRESULT ProcessContinue(int target, uint processID)
        {
            if (!Is32Bit())
            {
                return ProcessContinueX64(target, processID);
            }
            return ProcessContinueX86(target, processID);
        }

        public static SNRESULT GetTargetInfo(ref TargetInfo targetInfo)
        {
            TargetInfoPriv targetInfoPriv = new TargetInfoPriv
            {
                Flags = targetInfo.Flags,
                Target = targetInfo.Target
            };
            SNRESULT res = Is32Bit() ? GetTargetInfoX86(ref targetInfoPriv) : GetTargetInfoX64(ref targetInfoPriv);
            if (!FAILED(res))
            {
                targetInfo.Flags = targetInfoPriv.Flags;
                targetInfo.Target = targetInfoPriv.Target;
                targetInfo.Name = Utf8ToString(targetInfoPriv.Name, uint.MaxValue);
                targetInfo.Type = Utf8ToString(targetInfoPriv.Type, uint.MaxValue);
                targetInfo.Info = Utf8ToString(targetInfoPriv.Info, uint.MaxValue);
                targetInfo.HomeDir = Utf8ToString(targetInfoPriv.HomeDir, uint.MaxValue);
                targetInfo.FSDir = Utf8ToString(targetInfoPriv.FSDir, uint.MaxValue);
                targetInfo.Boot = targetInfoPriv.Boot;
            }
            return res;
        }

        public static SNRESULT GetTargetFromName(string name, out int target)
        {
            ScopedGlobalHeapPtr ptr = new ScopedGlobalHeapPtr(AllocUtf8FromString(name));
            if (!Is32Bit())
            {
                return GetTargetFromNameX64(ptr.Get(), out target);
            }
            return GetTargetFromNameX86(ptr.Get(), out target);
        }

        public static SNRESULT GetConnectionInfo(int target, out TCPIPConnectProperties connectProperties)
        {
            connectProperties = null;
            ScopedGlobalHeapPtr ptr = new ScopedGlobalHeapPtr(Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TCPIPConnectProperties))));
            SNRESULT res = Is32Bit() ? GetConnectionInfoX86(target, ptr.Get()) : GetConnectionInfoX64(target, ptr.Get());
            if (SUCCEEDED(res))
            {
                connectProperties = new TCPIPConnectProperties();
                Marshal.PtrToStructure(ptr.Get(), connectProperties);
            }
            return res;
        }

        public static SNRESULT GetConnectStatus(int target, out ConnectStatus status, out string usage)
        {
            IntPtr ptr;
            uint num;
            SNRESULT snresult = Is32Bit() ? GetConnectStatusX86(target, out num, out ptr) : GetConnectStatusX64(target, out num, out ptr);
            status = (ConnectStatus)num;
            usage = Utf8ToString(ptr, uint.MaxValue);
            return snresult;
        }

        public static SNRESULT Reset(int target, ResetParameter resetParameter)
        {
            if (!Is32Bit())
            {
                return ResetX64(target, (ulong)resetParameter);
            }
            return ResetX86(target, (ulong)resetParameter);
        }

        public static SNRESULT ProcessGetMemory(int target, UnitType unit, uint processID, ulong threadID, ulong address, ref byte[] buffer)
        {
            if (!Is32Bit())
            {
                return ProcessGetMemoryX64(target, unit, processID, threadID, address, buffer.Length, buffer);
            }
            return ProcessGetMemoryX86(target, unit, processID, threadID, address, buffer.Length, buffer);
        }

        public static SNRESULT ProcessSetMemory(int target, UnitType unit, uint processID, ulong threadID, ulong address, byte[] buffer)
        {
            if (!Is32Bit())
            {
                return ProcessSetMemoryX64(target, unit, processID, threadID, address, buffer.Length, buffer);
            }
            return ProcessSetMemoryX86(target, unit, processID, threadID, address, buffer.Length, buffer);
        }

        public static SNRESULT Disconnect(int target)
        {
            if (!Is32Bit())
            {
                return DisconnectX64(target);
            }
            return DisconnectX86(target);
        }

        public static SNRESULT GetThreadList(int target, uint processID, out ulong[] ppuThreadIDs, out ulong[] spuThreadIDs)
        {
            uint[] ppu = new uint[0x1], spu = new uint[0x1];
            if (!Is32Bit())
            {
                ThreadListX64(target, processID, ppu, null, spu, null);
                ppuThreadIDs = new ulong[ppu.Length];
                spuThreadIDs = new ulong[spu.Length];
                return ThreadListX64(target, processID, ppu, ppuThreadIDs, spu, spuThreadIDs);
            }
            ThreadListX86(target, processID, ppu, null, spu, null);

            ppuThreadIDs = new ulong[ppu[0]];
            spuThreadIDs = new ulong[spu[0]];
            return ThreadListX86(target, processID, ppu, ppuThreadIDs, spu, spuThreadIDs);
        }

        public static SNRESULT GetPPUThreadInfo(int target, uint processID, ulong threadID, out PPUThreadInfo threadInfo)
        {
            byte[] buffer = new byte[1024];
            uint[] bSize = new uint[] { 1024 };
            SNRESULT ret;

            if (!Is32Bit())
            {
                ret = PPUThreadInfoX64(target, processID, threadID, bSize, buffer);
            }
            else
            {
                ret = PPUThreadInfoX86(target, processID, threadID, bSize, buffer);
            }

            threadInfo = new PPUThreadInfo();
            threadInfo.ThreadID = BitConverter.ToUInt64(buffer, 0);
            threadInfo.Priority = BitConverter.ToUInt32(buffer, 8);
            threadInfo.State = (PPUThreadState)BitConverter.ToUInt32(buffer, 0x10);
            threadInfo.StackAddress = BitConverter.ToUInt64(buffer, 0x14);
            threadInfo.StackSize = BitConverter.ToUInt64(buffer, 0x1C);
            threadInfo.ThreadName = TMAPI.ByteArrayToString(buffer, 0x28, 0);

            return ret;
        }

    }
}
