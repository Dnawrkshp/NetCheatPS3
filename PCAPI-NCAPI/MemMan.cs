using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*
 * Source:
 * http://blackandodd.blogspot.com/2012/12/c-read-and-write-process-memory-in.html
 */

namespace PCAPI_NCAPI
{
    class MemMan
    {
        public int processHandle = 0;
        public int processId = 0;


        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, ulong lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, ulong lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);

        uint DELETE = 0x00010000;
        uint READ_CONTROL = 0x00020000;
        uint WRITE_DAC = 0x00040000;
        uint WRITE_OWNER = 0x00080000;
        uint SYNCHRONIZE = 0x00100000;
        uint END = 0xFFF; //if you have WinXP or Windows Server 2003 you must change this to 0xFFFF
        uint PROCESS_ALL_ACCESS = 0;

        public MemMan()
        {
            END = (uint)(IsWinVistaOrHigher() ? 0xFFF : 0xFFFF);

            PROCESS_ALL_ACCESS = (DELETE |
                                        READ_CONTROL |
                                        WRITE_DAC |
                                        WRITE_OWNER |
                                        SYNCHRONIZE |
                                        END
                                        );
        }


        /*
         * Source:
         * http://stackoverflow.com/questions/2732432/how-to-tell-if-the-os-is-windows-xp-or-higher
         */
        static bool IsWinXPOrHigher()
        {
            OperatingSystem OS = Environment.OSVersion;
            return (OS.Platform == PlatformID.Win32NT) && ((OS.Version.Major > 5) || ((OS.Version.Major == 5) && (OS.Version.Minor >= 1)));
        }

        static bool IsWinVistaOrHigher()
        {
            OperatingSystem OS = Environment.OSVersion;
            return (OS.Platform == PlatformID.Win32NT) && (OS.Version.Major >= 6);
        }

        public bool Attach(int pid)
        {
            processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
            processId = pid;

            return processHandle > 0;
        }

        public bool ReadMemory(ulong address, ref byte[] bytes)
        {
            if (processHandle <= 0)
                return false;

            return ReadProcessMemory(processHandle, address, bytes, bytes.Length, 0);
        }

        public void WriteMemory(ulong address, byte[] processBytes)
        {
            if (processHandle <= 0)
                return;

            WriteProcessMemory(processHandle, address, processBytes, processBytes.Length, 0);
        }

        public bool PauseProcess()
        {
            if (processId <= 0)
                return false;

            var process = Process.GetProcessById(processId);
            process.Suspend();
            return true;
        }

        public bool ContinueProcess()
        {
            if (processId <= 0)
                return false;

            var process = Process.GetProcessById(processId);
            process.Resume();

            return true;
        }

        public bool isSuspended()
        {
            if (processId <= 0)
                return false;

            return Process.GetProcessById(processId).isSuspended();
        }

        public void KillProcess()
        {
            if (processId <= 0)
                return;

            Process.GetProcessById(processId).Kill();
        }

    }

    /*
     * Source:
     * http://stackoverflow.com/questions/71257/suspend-process-in-c-sharp
     */
    public static class ProcessExtension
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);

        public static bool isSuspended(this Process process)
        {
            bool ret = false;
            foreach (ProcessThread thread in process.Threads)
            {
                ret |= thread.ThreadState == ThreadState.Running;
            }
            return !ret;
        }

        public static void Suspend(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }
                SuspendThread(pOpenThread);
            }
        }
        public static void Resume(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }
                ResumeThread(pOpenThread);
            }
        }
    }


}
