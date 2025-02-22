using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Essence
{
    public class api
    {


        //private static StreamReader reader;
        //private static StreamWriter writer;
        //private static NamedPipeClientStream client;
        //private static string pipeName = "arka";

        //public static async Task<InjectionResult> Inject()
        //{
        //    writer.WriteLine("inject");
        //    MessageBox.Show(reader.ReadLine());

        //    return InjectionResult.Failed;
        //}


        //public async static void Execute(string script)
        //{
        //    File.WriteAllText("temp.lua", script);
        //    writer.WriteLine("exec:temp.lua");
        //}

        //private static void StartInjector()
        //{
        //    if (!File.Exists("ArkaInjectorV3.exe"))
        //    {
        //        MessageBox.Show("injector is missing");
        //    }
        //    else
        //    {
        //        Process.Start("ArkaInjectorV3.exe");
        //    }
        //}











        public static event EventHandler Warn;
        public static string last_message = "";
        public static int timeout = 0;

        public static bool usingcelery = false;
        public static bool celery_patched = false;

        public static string injector_name = "main";
        public static string workspace = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "workspace");

        public static bool slow_cpu = false;

        private static void Notificar(string message, int time)
        {
            timeout = time;
            last_message = message;
            Warn?.Invoke(null, EventArgs.Empty);
        }

        public enum InjectionResult
        {
            Failed,
            Success,
            HomeAt
        }

        private static long GetDirectorySize(string directoryPath)
        {
            long size = 0;

            // Calcular o tamanho dos arquivos na pasta
            foreach (string file in Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                FileInfo fileInfo = new FileInfo(file);
                size += fileInfo.Length;
            }

            return size;
        }



        public static void CloseInjectors()
        {
            foreach (Process process in Process.GetProcessesByName("main"))
            {
                try
                {
                    process.Kill();
                    Console.WriteLine("Fechando injetores main...");
                }
                catch { }
            }

            foreach (Process process in Process.GetProcessesByName("CeleryInject"))
            {
                try
                {
                    process.Kill();
                    Console.WriteLine("Fechando injetores celery...");
                }
                catch { }
            }
        }

        static bool client_initialized = false;
        public static void InitializeAPI(bool slow_pc)
        {
            slow_cpu = slow_pc;

            if (!client_initialized)
            {
                client_initialized = true;

                //client = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut);
                //reader = new StreamReader(client);
                //writer = new StreamWriter(client);
                //StartInjector();

                //client.Connect();
                //writer.AutoFlush = true;

                //// Autenticação inicial
                //writer.WriteLine("auth:" + Process.GetCurrentProcess().Id);
            }


            try
            {
                if (!Directory.Exists(@"C:\Windows\System32\bin"))
                {
                    Directory.CreateDirectory(@"C:\Windows\System32\bin");
                }

                File.Copy(@"C:\Essence\bin\API.dll", @"C:\Windows\System32\bin\API.dll", overwrite: true);
                File.Copy(@"C:\Essence\bin\API.dll", @"C:\Windows\System32\API.dll", overwrite: true);
                Console.WriteLine("Arquivo copiado com sucesso!");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Erro: Permissões insuficientes. Execute o programa como administrador.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }





            if (!Directory.Exists(System.IO.Path.GetTempPath() + "celery"))
            {
                try { Directory.CreateDirectory(System.IO.Path.GetTempPath() + "celery"); }
                catch
                {
                    //MessageBox.Show("An exception occurred. Please restart Celery. Error message: " + ex.Message);
                }
            }

            FileHelp.checkCreateFile(System.IO.Path.GetTempPath() + "celery\\callback.txt", "");
            FileHelp.checkCreateFile(System.IO.Path.GetTempPath() + "celery\\celeryhome.txt");
            FileHelp.checkCreateFile(System.IO.Path.GetTempPath() + "celery\\autolaunch.txt", "false");
            FileHelp.checkCreateFile(System.IO.Path.GetTempPath() + "celery\\launchargs.txt", "");
            FileHelp.checkCreateFile(System.IO.Path.GetTempPath() + "celery\\robloxexe.txt", "");


            try
            {
                File.WriteAllText(System.IO.Path.GetTempPath() + "celery\\celeryhome.txt", AppDomain.CurrentDomain.BaseDirectory);//, Encoding.Unicode);
            }
            catch
            {
                //MessageBox.Show("An exception occurred. Please restart Celery. Error message: " + ex.Message);
            }

            if (usingcelery)
            {
                workspace = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\celery";
                injector_name = "CeleryInject";
            }
        }

        public async static void Execute(string script)
        {
            if (celery_patched)
                usingcelery = false;

            if (usingcelery)
            {
                try
                {
                    string celeryfolder = string.Concat(Path.GetTempPath() + "celery");

                    if (!Directory.Exists(celeryfolder))
                    {
                        Directory.CreateDirectory(celeryfolder);
                        string filePath = Path.Combine(celeryfolder, "myfile.txt");
                        File.Create(filePath).Dispose();
                    }

                    File.WriteAllText(string.Concat(Path.GetTempPath() + "celery", "\\myfile.txt"), script);
                }
                catch (Exception ex)
                {
                    string error = "Faild: ";

                    if (!File.Exists("C:\\Essence\\CeleryInjector.exe"))
                    {
                        error += "Injector does not exist";
                    }

                    else if (Process.GetProcessesByName("CeleryInjector").Length <= 0)
                    {
                        error += "Injector not open";
                    }

                    else
                    {
                        MessageBox.Show(ex.Message, "Could not execute script.");
                        return;
                    }

                    Notificar(error, 3);
                }
            }
            else
            {
                ClientWebSocket ws = new ClientWebSocket();
                Uri uri;
                ArraySegment<byte> buffer;



                try
                {
                    using (var cts = new CancellationTokenSource())
                    {
                        cts.CancelAfter(TimeSpan.FromSeconds(5));
                        uri = new Uri("ws://localhost:8050/ws");
                        var connectTask = ws.ConnectAsync(uri, cts.Token);

                        if (await Task.WhenAny(connectTask, Task.Delay(Timeout.Infinite, cts.Token)) == connectTask)
                        {
                            await connectTask;
                        }
                        else
                        {
                            throw new TimeoutException("A conexão demorou mais de 5 segundos.");
                        }

                        buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(script));
                        var sendTask = ws.SendAsync(buffer, WebSocketMessageType.Text, true, cts.Token);

                        if (await Task.WhenAny(sendTask, Task.Delay(Timeout.Infinite, cts.Token)) == sendTask)
                        {
                            await sendTask;
                        }
                        else
                        {
                            throw new TimeoutException("O envio demorou mais de 5 segundos.");
                        }
                    }


                }
                catch (Exception ex)
                {
                    string error = "Faild: ";

                    if (ex.Message == "O envio demorou mais de 5 segundos.")
                    {
                        error += "|TIMEOUT| ";
                    }

                    if (!File.Exists("C:\\Essence\\main.exe"))
                    {
                        error += "Injector does not exist";
                    }

                    else if (Process.GetProcessesByName("main").Length <= 0)
                    {
                        error += "Injector not open";
                    }

                    else
                    {
                        MessageBox.Show(ex.Message, "Could not execute script.");
                        return;
                    }

                    Notificar(error, 3);
                }
                finally
                {
                    ws?.Dispose();
                    ws = null;
                    uri = null;
                    buffer = new ArraySegment<byte>();
                }
            }
        }

        public static Process LastInjectorProcess;
        public static async Task<InjectionResult> Inject()
        {
            if (celery_patched)
                usingcelery = false;

            if (usingcelery)
            {
                workspace = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\celery";
                injector_name = "CeleryInject";
            }

            Console.WriteLine($"USING {(usingcelery ? "Celery" : "Main")} TO INJECTION.");
            CloseInjectors();

            if (!usingcelery)
            {
                string tempPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");
                TimeSpan timeLimit = TimeSpan.FromMinutes(10);

                var directories = Directory.GetDirectories(tempPath)
                                           .Where(dir => Path.GetFileName(dir).StartsWith("onefile"))
                                           .Where(dir => DateTime.Now - Directory.GetCreationTime(dir) > timeLimit);

                long totalSizeBytes = 0;

                foreach (var dir in directories)
                {
                    try
                    {
                        long dirSize = GetDirectorySize(dir);
                        totalSizeBytes += dirSize;

                        Directory.Delete(dir, true);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"Removendo lixo do injetor Main: {dir}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao remover o lixo do Main. Pasta: {dir}. Erro: {ex.Message}");
                    }
                }

                double totalSizeMB = totalSizeBytes / (1024.0 * 1024.0);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Total de {totalSizeMB:F2} MB limpos.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            TaskCompletionSource<InjectionResult> Completed = new TaskCompletionSource<InjectionResult>();

            Process Injector = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = usingcelery ? "C:\\Essence\\CeleryInject.exe" : Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "main.exe"),
                    WorkingDirectory = "C:\\Essence",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            LastInjectorProcess = Injector;

            Injector.Exited += (_, _) => Completed.TrySetResult(InjectionResult.Failed);
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            cts.Token.Register(() => Completed.TrySetResult(InjectionResult.Failed));

            DataReceivedEventHandler errorHandler = (DataReceivedEventHandler)((_, e) =>
            {
                if (e.Data == null)
                    return;

                string resp = e.Data.Trim();
                Console.WriteLine(resp);

                MessageBox.Show(e.Data, "Fatal Error in Injector");
            });

            DataReceivedEventHandler handler = (DataReceivedEventHandler)((_, e) =>
            {
                if (e.Data == null)
                    return;

                string resp = e.Data.Trim();
                Console.WriteLine(resp);

                cts.CancelAfter(TimeSpan.FromSeconds(slow_cpu ? 30 : 16));

                if (usingcelery)
                {
                    if (resp == "READY")
                        Completed.TrySetResult(InjectionResult.Success);

                    else if (resp.Contains("Failed"))
                        Completed.TrySetResult(InjectionResult.Failed);

                    else if (resp.Contains("Wrong roblox version detected"))
                    {
                        celery_patched = true;
                        Completed.TrySetResult(InjectionResult.Failed);
                    }
                }
                else
                {
                    if (resp.Contains("Homescreen Attached"))
                        Completed.TrySetResult(InjectionResult.HomeAt);

                    if (resp.Contains("Ready to use"))
                        Completed.TrySetResult(InjectionResult.Success);

                    else if (resp.Contains("Dead Bridge"))
                        Completed.TrySetResult(InjectionResult.Failed);

                    else if (resp.Contains("Exception in thread"))
                        Completed.TrySetResult(InjectionResult.Failed);

                    else if (resp.Contains("[ERROR] Process not found."))
                        Completed.TrySetResult(InjectionResult.Failed);

                    else if (resp.Contains("[ERROR] Error executing code: 'NoneType' object has no attribute 'SetModuleBypass'"))
                    {
                        MessageBox.Show("Você acabou de usar o serviço de teleport?\nNosso injetor não está funcionando após o teleport. Feche o roblox e Injete depois de usar seus teleports.");
                        MessageBox.Show("Did you just use the teleport service?\nOur injector is not working after teleport. Close roblox and Inject after using your teleports.");

                        Completed.TrySetResult(InjectionResult.Failed);
                    }
                }
            });

            Injector.OutputDataReceived += handler;
            Injector.ErrorDataReceived += errorHandler;

            Notificar("Starting Injector", 1);

            Injector.Start();
            Injector.BeginOutputReadLine();
            Injector.BeginErrorReadLine();

            try
            {
                InjectionResult result = await Completed.Task;
                Injector.OutputDataReceived -= handler;
                Injector.ErrorDataReceived -= errorHandler;
                return result;
            }
            catch (TaskCanceledException)
            {
                Injector.OutputDataReceived -= handler;
                Injector.ErrorDataReceived -= errorHandler;

                return InjectionResult.Failed;
            }
        }

        public class FileHelp
        {
            public static bool checkCreateFile(string path)
            {
                if (File.Exists(path))
                    return true;

                try
                {
                    using (FileStream s = File.Create(path))
                    {
                        s.Close();
                        return true;
                    }
                }
                catch
                {
                    MessageBox.Show("There was an issue while trying to create file `" + path + "`...Please close Essence and run as an administrator", "", MessageBoxButton.OK);
                }
                return false;

            }

            public static bool checkCreateFile(string path, string defaultValue)
            {
                checkCreateFile(path);

                try
                {
                    File.WriteAllText(path, defaultValue);
                    return true;
                }
                catch
                {
                    MessageBox.Show("There was an issue while trying to write to a file...Please close Essence and run as an administrator", "", MessageBoxButton.OK);
                    return false;
                }
            }
        }

        //public class Imports
        //{
        //    public const uint PAGE_NOACCESS = 0x1;
        //    public const uint PAGE_READONLY = 0x2;
        //    public const uint PAGE_READWRITE = 0x4;
        //    public const uint PAGE_WRITECOPY = 0x8;
        //    public const uint PAGE_EXECUTE = 0x10;
        //    public const uint PAGE_EXECUTE_READ = 0x20;
        //    public const uint PAGE_EXECUTE_READWRITE = 0x40;
        //    public const uint PAGE_EXECUTE_WRITECOPY = 0x80;
        //    public const uint PAGE_GUARD = 0x100;
        //    public const uint PAGE_NOCACHE = 0x200;
        //    public const uint PAGE_WRITECOMBINE = 0x400;

        //    public const uint MEM_COMMIT = 0x1000;
        //    public const uint MEM_RESERVE = 0x2000;
        //    public const uint MEM_DECOMMIT = 0x4000;
        //    public const uint MEM_RELEASE = 0x8000;

        //    public const uint PROCESS_WM_READ = 0x0010;
        //    public const uint PROCESS_ALL_ACCESS = 0x1F0FFF;


        //    private const uint GENERIC_WRITE = 0x40000000;
        //    private const uint GENERIC_READ = 0x80000000;
        //    private const uint FILE_SHARE_READ = 0x00000001;
        //    private const uint FILE_SHARE_WRITE = 0x00000002;
        //    private const uint OPEN_EXISTING = 0x00000003;
        //    private const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        //    private const uint ERROR_ACCESS_DENIED = 5;

        //    private const uint ATTACH_PARENT = 0xFFFFFFFF;

        //    public const int EXCEPTION_CONTINUE_EXECUTION = -1;
        //    public const int EXCEPTION_CONTINUE_SEARCH = 0;

        //    public const uint STD_OUTPUT_HANDLE = 0xFFFFFFF5;//-11;
        //    public const int MY_CODE_PAGE = 437;

        //    public const int SW_HIDE = 0;
        //    public const int SW_SHOW = 5;

        //    public const Int64 WAIT_TIMEOUT = 258L;


        //    [StructLayout(LayoutKind.Sequential)]
        //    public struct MEMORY_BASIC_INFORMATION
        //    {
        //        public int BaseAddress;
        //        public int AllocationBase;
        //        public uint AllocationProtect;
        //        public int RegionSize;
        //        public uint State;
        //        public uint Protect;
        //        public uint Type;
        //    }

        //    [StructLayout(LayoutKind.Sequential)]
        //    public struct PROCESS_INSTRUMENTATION_CALLBACK
        //    {
        //        public UInt32 Version;
        //        public UInt32 Reserved;
        //        public IntPtr Callback;
        //    }

        //    [DllImport("kernel32.dll", SetLastError = true)]
        //    public static extern UInt32 WaitForSingleObject(UInt64 hProcess, UInt32 dwMilliseconds);

        //    [DllImport("ntdll.dll", SetLastError = true)]
        //    public static extern int NtSetInformationProcess(UInt64 hProcess, int processInformationClass, ref PROCESS_INSTRUMENTATION_CALLBACK processInformation, int processInformationLength);

        //    [DllImport("ntdll.dll", SetLastError = true)]
        //    public static extern int NtQueryInformationProcess(UInt64 processHandle, int processInformationClass, ref PROCESS_INSTRUMENTATION_CALLBACK processInformation, uint processInformationLength, ref int returnLength);

        //    [DllImport("ntdll.dll", SetLastError = true)]
        //    public static extern bool NtDuplicateHandle(UInt64 hSourceProcess, UInt64 hSourceHandle, UInt64 hTargetProcess, UInt64 lpTargetHandle, UInt32 dwDesiredAccess, bool bInheritHandle, UInt32 dwOptions);

        //    [DllImport("user32.dll", EntryPoint = "FindWindow")]
        //    public static extern int FindWindow(string sClass, string sWindow);

        //    [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        //    public static extern bool ShowWindow(int hWnd, int nCmdShow);

        //    [DllImport("user32.dll", EntryPoint = "MessageBoxA", SetLastError = true, CharSet = CharSet.Ansi)]
        //    public static extern int MessageBoxA(int hWnd, string sMessage, string sCaption, uint mbType);

        //    [DllImport("user32.dll", EntryPoint = "MessageBoxW", SetLastError = true, CharSet = CharSet.Unicode)]
        //    public static extern int MessageBoxW(int hWnd, string sMessage, string sCaption, uint mbType);

        //    [DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow")]
        //    public static extern int GetConsoleWindow();

        //    [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        //    public static extern UInt64 OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        //    [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        //    public static extern bool ReadProcessMemory(UInt64 hProcess, UInt64 lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        //    [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        //    public static extern bool WriteProcessMemory(UInt64 hProcess, UInt64 lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        //    [DllImport("kernel32.dll", EntryPoint = "VirtualProtectEx")]
        //    public static extern bool VirtualProtectEx(UInt64 hProcess, UInt64 lpBaseAddress, int dwSize, uint new_protect, ref uint lpOldProtect);

        //    [DllImport("kernel32.dll", EntryPoint = "VirtualQueryEx")]
        //    public static extern UInt64 VirtualQueryEx(UInt64 hProcess, UInt64 lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        //    [DllImport("kernel32.dll", EntryPoint = "VirtualAllocEx")]
        //    public static extern UInt64 VirtualAllocEx(UInt64 hProcess, UInt64 lpAddress, int size, uint allocation_type, uint protect);

        //    [DllImport("kernel32.dll", EntryPoint = "VirtualFreeEx")]
        //    public static extern UInt64 VirtualFreeEx(UInt64 hProcess, UInt64 lpAddress, int size, uint allocation_type);

        //    [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle", CharSet = CharSet.Auto)]
        //    public static extern UInt64 GetModuleHandle(string lpModuleName);

        //    [DllImport("kernel32", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        //    public static extern UInt64 GetProcAddress(UInt64 hModule, string procName);

        //    [DllImport("kernel32.dll", EntryPoint = "GetLastError")]
        //    public static extern uint GetLastError();

        //    [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        //    public static extern bool CloseHandle(UInt64 hObject);

        //    [DllImport("kernel32.dll", EntryPoint = "GetExitCodeProcess", SetLastError = true)]
        //    [return: MarshalAs(UnmanagedType.Bool)]
        //    public static extern bool GetExitCodeProcess(UInt64 hProcess, out uint lpExitCode);

        //    [DllImport("kernel32.dll", EntryPoint = "CreateRemoteThread")]
        //    public static extern int CreateRemoteThread(UInt64 hProcess, int lpThreadAttributes, uint dwStackSize, int lpStartAddress, int lpParameter, uint dwCreationFlags, out int lpThreadId);

        //    [DllImport("kernel32.dll",
        //            EntryPoint = "GetStdHandle",
        //            SetLastError = true,
        //            CharSet = CharSet.Auto,
        //            CallingConvention = CallingConvention.StdCall)]
        //    public static extern uint GetStdHandle(uint nStdHandle);

        //    [DllImport("kernel32.dll", EntryPoint = "SetStdHandle")]
        //    public static extern void SetStdHandle(uint nStdHandle, uint handle);

        //    [DllImport("kernel32.dll",
        //        EntryPoint = "AllocConsole",
        //        SetLastError = true,
        //        CharSet = CharSet.Auto,
        //        CallingConvention = CallingConvention.StdCall)]
        //    public static extern int AllocConsole();

        //    [DllImport("kernel32.dll", EntryPoint = "SetConsoleTitle", CharSet = CharSet.Auto)]
        //    public static extern bool SetConsoleTitle(string lpConsoleTitle);

        //    [DllImport("kernel32.dll",
        //        EntryPoint = "AttachConsole",
        //        SetLastError = true,
        //        CharSet = CharSet.Auto,
        //        CallingConvention = CallingConvention.StdCall)]
        //    public static extern uint AttachConsole(uint dwProcessId);

        //    [DllImport("kernel32.dll",
        //        EntryPoint = "CreateFileW",
        //        SetLastError = true,
        //        CharSet = CharSet.Auto,
        //        CallingConvention = CallingConvention.StdCall)]
        //    public static extern uint CreateFileW(
        //          string lpFileName,
        //          uint dwDesiredAccess,
        //          uint dwShareMode,
        //          uint lpSecurityAttributes,
        //          uint dwCreationDisposition,
        //          uint dwFlagsAndAttributes,
        //          uint hTemplateFile
        //        );

        //    [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcessId")]
        //    public static extern uint GetCurrentProcessId();

        //    [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "FreeConsole")]
        //    [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        //    public static extern bool FreeConsole();

        //    [DllImport("kernel32.dll", EntryPoint = "CreateFile", SetLastError = true)]
        //    public static extern uint CreateFile(string lpFileName, uint
        //    dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint
        //    dwCreationDisposition, uint dwFlagsAndAttributes, uint hTemplateFile);

        //    /*public static class ConsoleHelper
        //    {
        //        public static void CreateConsole()
        //        {
        //            AllocConsole();

        //            // stdout's handle seems to always be equal to 7
        //            uint defaultStdout = 7;
        //            uint currentStdout = GetStdHandle(STD_OUTPUT_HANDLE);

        //            if (currentStdout != defaultStdout)
        //                // reset stdout
        //                SetStdHandle(STD_OUTPUT_HANDLE, defaultStdout);

        //            // reopen stdout
        //            TextWriter writer = new StreamWriter(System.Console.OpenStandardOutput())
        //            { AutoFlush = true };

        //            System.Console.SetOut(writer);
        //        }
        //    }*/

        //    public static class ConsoleHelper
        //    {
        //        public static StreamWriter writer;
        //        public static FileStream fwriter;

        //        static public void Initialize(bool alwaysCreateNewConsole = true)
        //        {
        //            bool consoleAttached = true;
        //            if (alwaysCreateNewConsole
        //                || (AttachConsole(ATTACH_PARENT) == 0
        //                && Marshal.GetLastWin32Error() != ERROR_ACCESS_DENIED))
        //            {
        //                consoleAttached = AllocConsole() != 0;
        //            }

        //            if (consoleAttached)
        //            {
        //                InitializeOutStream();
        //                InitializeInStream();
        //            }

        //            Console.OutputEncoding = System.Text.Encoding.UTF8;
        //        }

        //        static public void Clear()
        //        {
        //            System.Console.Write("\n\n");
        //        }

        //        private static void InitializeOutStream()
        //        {
        //            fwriter = CreateFileStream("CONOUT$", GENERIC_WRITE, FILE_SHARE_WRITE, FileAccess.Write);
        //            if (fwriter != null)
        //            {
        //                writer = new StreamWriter(fwriter) { AutoFlush = true };
        //                System.Console.SetOut(writer);
        //                System.Console.SetError(writer);
        //            }
        //        }

        //        private static void InitializeInStream()
        //        {
        //            var fs = CreateFileStream("CONIN$", GENERIC_READ, FILE_SHARE_READ, FileAccess.Read);
        //            if (fs != null)
        //            {
        //                System.Console.SetIn(new StreamReader(fs));
        //            }
        //        }

        //        private static FileStream CreateFileStream(string name, uint win32DesiredAccess, uint win32ShareMode,
        //                                FileAccess dotNetFileAccess)
        //        {
        //            var file = new SafeFileHandle((System.IntPtr)CreateFileW(name, win32DesiredAccess, win32ShareMode, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0), true);
        //            if (!file.IsInvalid)
        //            {
        //                var fs = new FileStream(file, dotNetFileAccess);
        //                return fs;
        //            }
        //            return null;
        //        }
        //    }
        //}

        //public class Util
        //{
        //    private static List<UInt64> openedHandles = new List<UInt64>();

        //    public static List<ProcInfo> openProcessesByName(string processName)
        //    {
        //        var procList = new List<ProcInfo>();

        //        foreach (Process process in Process.GetProcessesByName(processName.Replace(".exe", "")))
        //        {
        //            try
        //            {
        //                if (process.Id != 0 && !process.HasExited)
        //                {
        //                    var procInfo = new ProcInfo();
        //                    procInfo.processRef = process;
        //                    procInfo.baseModule = 0;
        //                    procInfo.handle = 0;
        //                    procInfo.processId = (UInt64)process.Id;
        //                    procInfo.processName = processName;
        //                    procInfo.windowName = "";
        //                    procList.Add(procInfo);
        //                }
        //            }
        //            catch (System.NullReferenceException ex)
        //            {
        //                continue;
        //            }
        //            catch (System.Exception ex)
        //            {
        //                continue;
        //            }
        //        }

        //        return procList;
        //    }

        //    public void flush()
        //    {
        //        foreach (UInt64 handle in openedHandles)
        //        {
        //            Imports.CloseHandle(handle);
        //        }
        //    }

        //    public class ProcInfo
        //    {
        //        public ProcInfo()
        //        {
        //            processRef = null;
        //            processId = 0;
        //            handle = 0;
        //        }

        //        public Process processRef;
        //        public UInt64 processId;
        //        public string processName;
        //        public string windowName;
        //        public UInt64 handle;
        //        public UInt64 baseModule;
        //        private Int32 nothing;

        //        public bool isOpen()
        //        {
        //            try
        //            {
        //                if (processRef == null) return false;
        //                if (processRef.HasExited) return false;
        //                if (processRef.Id == 0) return false;
        //                if (processRef.Handle == IntPtr.Zero) return false;
        //            }
        //            catch (System.InvalidOperationException ex)
        //            {
        //                return false;
        //            }
        //            catch (Exception ex)
        //            {
        //                return false;
        //            }
        //            return (processId != 0 && handle != 0);
        //        }

        //        public Imports.MEMORY_BASIC_INFORMATION getPage(UInt64 address)
        //        {
        //            var mbi = new Imports.MEMORY_BASIC_INFORMATION();
        //            Imports.VirtualQueryEx(handle, address, out mbi, 0x1C);
        //            return mbi;
        //        }

        //        public bool isAccessible(UInt64 address)
        //        {
        //            var page = getPage(address);
        //            var pr = page.Protect;
        //            return page.State == Imports.MEM_COMMIT && (pr == Imports.PAGE_READWRITE || pr == Imports.PAGE_READONLY || pr == Imports.PAGE_EXECUTE_READWRITE || pr == Imports.PAGE_EXECUTE_READ);
        //        }

        //        public uint setPageProtect(UInt64 address, Int32 size, UInt32 protect)
        //        {
        //            UInt32 oldProtect = 0;
        //            Imports.VirtualProtectEx(handle, address, size, protect, ref oldProtect);
        //            return oldProtect;
        //        }

        //        public bool writeByte(UInt64 address, byte value)
        //        {
        //            byte[] bytes = new byte[sizeof(byte)];
        //            bytes[0] = value;
        //            return Imports.WriteProcessMemory(handle, address, bytes, bytes.Length, ref nothing);
        //        }

        //        public bool writeBytes(UInt64 address, byte[] bytes, Int32 count = -1)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, bytes, (count == -1) ? bytes.Length : count, ref nothing);
        //        }

        //        public bool writeString(UInt64 address, string str, Int32 count = -1)
        //        {
        //            char[] chars = str.ToCharArray(0, str.Length);
        //            List<byte> bytes = new List<byte>();

        //            foreach (byte b in chars)
        //                bytes.Add(b);

        //            return Imports.WriteProcessMemory(handle, address, bytes.ToArray(), (count == -1) ? bytes.Count : count, ref nothing);
        //        }

        //        public bool writeWString(UInt64 address, string str, Int32 count = -1)
        //        {
        //            var at = address;
        //            char[] chars = str.ToCharArray(0, str.Length);
        //            foreach (char c in chars)
        //            {
        //                writeUInt16(at, Convert.ToUInt16(c));
        //                at += 2;
        //            }
        //            return true;
        //        }

        //        public bool writeInt16(UInt64 address, Int16 value)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), sizeof(Int16), ref nothing);
        //        }

        //        public bool writeUInt16(UInt64 address, UInt16 value)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), sizeof(UInt16), ref nothing);
        //        }

        //        public bool writeInt32(UInt64 address, Int32 value)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), sizeof(Int32), ref nothing);
        //        }

        //        public bool writeUInt32(UInt64 address, UInt32 value)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), sizeof(UInt32), ref nothing);
        //        }

        //        public bool writeFloat(UInt64 address, float value)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), sizeof(float), ref nothing);
        //        }

        //        public bool writeDouble(UInt64 address, Double value)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), sizeof(Double), ref nothing);
        //        }

        //        public bool writeInt64(UInt64 address, Int64 value)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), sizeof(Int64), ref nothing);
        //        }

        //        public bool writeUInt64(UInt64 address, UInt64 value)
        //        {
        //            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), sizeof(UInt64), ref nothing);
        //        }

        //        public byte readByte(UInt64 address)
        //        {
        //            byte[] bytes = new byte[1];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(byte), ref nothing);
        //            return bytes[0];
        //        }

        //        public byte[] readBytes(UInt64 address, Int32 count)
        //        {
        //            byte[] bytes = new byte[count];
        //            Imports.ReadProcessMemory(handle, address, bytes, count, ref nothing);
        //            return bytes;
        //        }

        //        public string readString(UInt64 address, Int32 count = -1)
        //        {
        //            var result = "";
        //            var at = address;

        //            if (count == -1)
        //            {
        //                while (at != 512)
        //                {
        //                    foreach (var b in readBytes(at, 512))
        //                    {
        //                        if (!(b == '\n' || b == '\r' || b == '\t' || (b >= 0x20 && b <= 0x7F)))
        //                        {
        //                            at = 0;
        //                            break;
        //                        }
        //                        result += (char)b;
        //                    }

        //                    at += 512;
        //                }
        //            }
        //            else
        //            {
        //                foreach (byte c in readBytes(at, count))
        //                {
        //                    result += (char)c;
        //                }
        //            }

        //            return result;
        //        }

        //        public string readWString(UInt64 address, Int32 count = -1)
        //        {
        //            string result = "";
        //            var at = address;

        //            if (count == -1)
        //            {
        //                while (at != 512)
        //                {
        //                    var bytes = readBytes(at, 512);
        //                    for (int i = 0; i < bytes.Length; i += 2)
        //                    {
        //                        if (bytes[i] == 0 && bytes[i + 1] == 0) { at = 0; break; }
        //                        result += Encoding.Unicode.GetString(new byte[2] { bytes[i], bytes[i + 1] }, 0, 2); //BitConverter.ToChar(new byte[2] { bytes[i], bytes[i + 1] }, 0);
        //                    }
        //                    at += 512;
        //                }
        //            }
        //            else
        //            {
        //                var bytes = readBytes(at, count * 2);

        //                for (int i = 0; i < bytes.Length; i += 2)
        //                    result += Encoding.Unicode.GetString(new byte[2] { bytes[i], bytes[i + 1] }, 0, 2); //BitConverter.ToChar(new byte[2] { bytes[i], bytes[i + 1] }, 0);
        //            }

        //            return result;
        //        }

        //        public Int16 readInt16(UInt64 address)
        //        {
        //            byte[] bytes = new byte[sizeof(Int16)];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(Int16), ref nothing);
        //            return BitConverter.ToInt16(bytes, 0);
        //        }

        //        public UInt16 readUInt16(UInt64 address)
        //        {
        //            byte[] bytes = new byte[sizeof(UInt16)];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(UInt16), ref nothing);
        //            return BitConverter.ToUInt16(bytes, 0);
        //        }

        //        public Int32 readInt32(UInt64 address)
        //        {
        //            byte[] bytes = new byte[sizeof(Int32)];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(Int32), ref nothing);
        //            return BitConverter.ToInt32(bytes, 0);
        //        }

        //        public UInt32 readUInt32(UInt64 address)
        //        {
        //            byte[] bytes = new byte[sizeof(UInt32)];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(UInt32), ref nothing);
        //            return BitConverter.ToUInt32(bytes, 0);
        //        }

        //        public float readFloat(UInt64 address)
        //        {
        //            byte[] bytes = new byte[sizeof(float)];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(float), ref nothing);
        //            return BitConverter.ToSingle(bytes, 0);
        //        }

        //        public Double readDouble(UInt64 address)
        //        {
        //            byte[] bytes = new byte[sizeof(Double)];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(Double), ref nothing);
        //            return BitConverter.ToDouble(bytes, 0);
        //        }

        //        public Int64 readInt64(UInt64 address)
        //        {
        //            byte[] bytes = new byte[sizeof(Int64)];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(Int64), ref nothing);
        //            return BitConverter.ToInt64(bytes, 0);
        //        }

        //        public UInt64 readUInt64(UInt64 address)
        //        {
        //            byte[] bytes = new byte[sizeof(UInt64)];
        //            Imports.ReadProcessMemory(handle, address, bytes, sizeof(UInt64), ref nothing);
        //            return BitConverter.ToUInt64(bytes, 0);
        //        }

        //        public bool isPrologue(UInt64 address)
        //        {
        //            var bytes = readBytes(address, 3);

        //            // copy of a dll function?
        //            if (bytes[0] == 0x8B && bytes[1] == 0xFF && bytes[2] == 0x55)
        //                return true;

        //            // now ignore misaligned functions
        //            if (address % 0x10 != 0)
        //                return false;

        //            if (
        //                // Check for different prologues, with different registers
        //                ((bytes[0] == 0x52 && bytes[1] == 0x8B && bytes[2] == 0xD4) // push edx + mov edx, esp
        //              || (bytes[0] == 0x53 && bytes[1] == 0x8B && bytes[2] == 0xDC) // push ebx + mov ebx, esp
        //              || (bytes[0] == 0x55 && bytes[1] == 0x8B && bytes[2] == 0xEC) // push ebp + mov ebp, esp
        //              || (bytes[0] == 0x56 && bytes[1] == 0x8B && bytes[2] == 0xF4) // push esi + mov esi, esp
        //              || (bytes[0] == 0x57 && bytes[1] == 0x8B && bytes[2] == 0xFF)) // push edi + mov edi, esp
        //            )
        //                return true;

        //            // is there a switch table behind this address?
        //            //if ((readInt32(address - 4) > address - 0x8000 && readInt32(address - 4) < address)
        //            // && (readInt32(address - 8) > address - 0x8000 && readInt32(address - 8) < address)
        //            //)
        //            //    return true;

        //            return false;
        //        }

        //        public bool isEpilogue(UInt64 address)
        //        {
        //            byte first = readByte(address);

        //            switch (first)
        //            {
        //                case 0xC9: // leave
        //                    return true;

        //                case 0xC3: // retn
        //                case 0xC2: // ret
        //                case 0xCC: // align / int 3
        //                    {
        //                        switch (readByte(address - 1))
        //                        {
        //                            case 0x5A: // pop edx
        //                            case 0x5B: // pop ebx
        //                            case 0x5D: // pop ebp
        //                            case 0x5E: // pop esi
        //                            case 0x5F: // pop edi
        //                                {
        //                                    if (first == 0xC2)
        //                                    {
        //                                        var r = readUInt16(address + 1);

        //                                        // double check for return value
        //                                        if (r % 4 == 0 && r > 0 && r < 1024)
        //                                        {
        //                                            return true;
        //                                        }
        //                                    }

        //                                    return true;
        //                                }
        //                        }

        //                        break;
        //                    }
        //            }

        //            return false;
        //        }

        //        private bool isValidCode(UInt64 address)
        //        {
        //            return !(readUInt64(address) == 0 && readUInt64(address + 8) == 0);
        //        }

        //        public UInt64 gotoPrologue(UInt64 address)
        //        {
        //            UInt64 at = address;

        //            if (isPrologue(at)) return at;
        //            while (!isPrologue(at) && isValidCode(address))
        //            {
        //                if ((at % 16) != 0)
        //                    at -= (at % 16);
        //                else
        //                    at -= 16;
        //            }

        //            return at;
        //        }

        //        public UInt64 gotoNextPrologue(UInt64 address)
        //        {
        //            UInt64 at = address;

        //            if (isPrologue(at)) at += 0x10;
        //            while (!isPrologue(at) && isValidCode(at))
        //            {
        //                if ((at % 0x10) == 0)
        //                    at += 0x10;
        //                else
        //                    at += (at % 0x10);
        //            }

        //            return at;
        //        }



        //    }
        //}
    }

    //public class InjectionService
    //{
    //    private Process _injectorProc;
    //    private bool _isInjectingMainPlayer;
    //    private bool _isInjected;
    //    private Action<bool> _statusCallback;

    //    [DllImport("user32.dll", SetLastError = true)]
    //    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    //    public enum InjectionResult
    //    {
    //        Failed,
    //        Canceled,
    //        AlreadyInjecting,
    //        RobloxNotOpened,
    //        Success,
    //    }

    //    public InjectionService()
    //    {
    //        Process[] processesByName = Process.GetProcessesByName("CeleryInject");
    //        if (processesByName.Length == 0)
    //            return;
    //        if (processesByName.Length > 1)
    //        {
    //            foreach (Process process in ((IEnumerable<Process>)processesByName).Skip<Process>(1))
    //            {
    //                try
    //                {
    //                    process.Kill();
    //                }
    //                catch (Exception ex)
    //                {
    //                    //this.LoggerService.Error("Couldn't kill injector: " + ex.Message);
    //                }
    //            }
    //        }

    //        //this.LoggerService.Info("Found existing injector");
    //        this._injectorProc = processesByName[0];
    //        this._isInjected = true;
    //        this._isInjectingMainPlayer = false;
    //    }

    //    public async Task<InjectionResult> Inject()
    //    {
    //        InjectionService injectionService = this;
    //        //if (!File.Exists(Config.InjectorPath))
    //        //{
    //        //    injectionService.LoggerService.Error("Couldn't find 'CeleryInject.exe'");
    //        //    return InjectionResult.Failed;
    //        //}
    //        try
    //        {
    //            if (injectionService._injectorProc != null)
    //            {
    //                if (!injectionService._injectorProc.HasExited)
    //                    injectionService._injectorProc.Kill();
    //            }
    //        }
    //        catch
    //        {
    //        }
    //        foreach (Process process in Process.GetProcessesByName("CeleryInject"))
    //        {
    //            try
    //            {
    //                process.Kill();
    //            }
    //            catch (Exception ex)
    //            {
    //                //injectionService.LoggerService.Error("Couldn't kill injector: " + ex.Message);
    //            }
    //        }
    //        Action<bool> statusCallback1 = injectionService._statusCallback;
    //        if (statusCallback1 != null)
    //            statusCallback1(false);
    //        if (!InjectionService.IsRobloxOpen())
    //            return InjectionResult.RobloxNotOpened;
    //        int tries = 1;
    //        while (InjectionService.FindWindow((string)null, "Roblox") == IntPtr.Zero)
    //        {
    //            //injectionService.LoggerService.Info(string.Format("[{0}/30] Waiting for Roblox to start...", (object)tries));
    //            await Task.Delay(1000);
    //            ++tries;
    //            if (tries > 30)
    //            {
    //                //injectionService.LoggerService.Error("Took too long for Roblox to start, aborting...");
    //                return InjectionResult.Failed;
    //            }
    //        }
    //        TaskCompletionSource<InjectionResult> tcs = new TaskCompletionSource<InjectionResult>();
    //        injectionService._isInjectingMainPlayer = true;
    //        injectionService._injectorProc = new Process()
    //        {
    //            StartInfo = new ProcessStartInfo()
    //            {
    //                FileName = "C:\\Essence\\CeleryInject.exe",
    //                UseShellExecute = false,
    //                RedirectStandardOutput = true,
    //                CreateNoWindow = true
    //            }
    //        };
    //        injectionService._injectorProc.Exited += (EventHandler)((_1, _2) =>
    //        {
    //            tcs.TrySetResult(InjectionResult.Canceled);
    //            this._isInjected = false;
    //            this._isInjectingMainPlayer = false;
    //            Action<bool> statusCallback2 = this._statusCallback;
    //            if (statusCallback2 == null)
    //                return;
    //            statusCallback2(false);
    //        });
    //        int scanningCount = 0;
    //        injectionService._injectorProc.OutputDataReceived += (DataReceivedEventHandler)((_, e) =>
    //        {
    //            if (e.Data == null)
    //                return;
    //            string message = e.Data.Trim();
    //            Console.WriteLine(message);
    //            //this.LoggerService.Info(message);
    //            switch (message)
    //            {
    //                case "READY":
    //                    tcs.TrySetResult(InjectionResult.Success);
    //                    this._isInjected = true;
    //                    this._isInjectingMainPlayer = false;
    //                    Action<bool> statusCallback3 = this._statusCallback;
    //                    if (statusCallback3 == null)
    //                        break;
    //                    statusCallback3(true);
    //                    break;
    //                case "Scanning...":
    //                    //if (!this.SettingsService.GetSetting<bool>("autofixerrors"))
    //                    //    break;
    //                    ++scanningCount;
    //                    if (scanningCount < 10)
    //                        break;
    //                    //this.LoggerService.Error("Detected an error, force closing Roblox and Celery injector...");
    //                    foreach (Process process in Process.GetProcessesByName("RobloxPlayerBeta"))
    //                    {
    //                        try
    //                        {
    //                            process.Kill();
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            //this.LoggerService.Error("Couldn't kill Roblox: " + ex.Message);
    //                        }
    //                    }
    //                    foreach (Process process in Process.GetProcessesByName("CeleryInject"))
    //                    {
    //                        try
    //                        {
    //                            process.Kill();
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            //this.LoggerService.Error("Couldn't kill injector: " + ex.Message);
    //                        }
    //                    }
    //                    tcs.TrySetResult(InjectionResult.Failed);
    //                    this._isInjected = false;
    //                    this._isInjectingMainPlayer = false;
    //                    Action<bool> statusCallback4 = this._statusCallback;
    //                    if (statusCallback4 != null)
    //                        statusCallback4(false);
    //                    this._injectorProc?.Dispose();
    //                    break;
    //                case "No window":
    //                    tcs.TrySetResult(InjectionResult.Failed);
    //                    this._isInjected = false;
    //                    this._isInjectingMainPlayer = false;
    //                    Action<bool> statusCallback5 = this._statusCallback;
    //                    if (statusCallback5 == null)
    //                        break;
    //                    statusCallback5(false);
    //                    break;
    //            }
    //        });
    //        injectionService._injectorProc.Start();
    //        injectionService._injectorProc.BeginOutputReadLine();
    //        return await tcs.Task;
    //    }

    //    public void Execute(string script)
    //    {
    //        string celeryfolder = string.Concat(Path.GetTempPath() + "celery");

    //        if (!Directory.Exists(celeryfolder))
    //        {
    //            Directory.CreateDirectory(celeryfolder);

    //            string filePath = Path.Combine(celeryfolder, "myfile.txt");
    //            File.Create(filePath).Dispose();
    //        }
    //        File.WriteAllText(string.Concat(Path.GetTempPath() + "celery", "\\myfile.txt"), script);
    //    }

    //    public void SetStatusCallback(Action<bool> callback)
    //    {
    //        this._statusCallback = (Action<bool>)(injected =>
    //        {
    //            if (!injected)
    //            {
    //                this._isInjectingMainPlayer = false;
    //                this._isInjected = false;
    //            }
    //            callback(injected);
    //        });
    //        Action<bool> statusCallback = this._statusCallback;
    //        if (statusCallback == null)
    //            return;
    //        statusCallback(this.IsInjected());
    //    }

    //    public Action<bool> GetStatusCallback() => this._statusCallback;

    //    public bool IsInjected()
    //    {
    //        return this._injectorProc != null && InjectionService.IsRobloxOpen() && !this._injectorProc.HasExited && this._isInjected;
    //    }

    //    private static bool IsRobloxOpen()
    //    {
    //        return Process.GetProcessesByName("RobloxPlayerBeta").Length != 0;
    //    }
    //}

    //public class oldapi
    //{
    //    static string ComputeSha1Hash(string input)
    //    {
    //        byte[] byteArray = Encoding.UTF8.GetBytes(input);
    //        using (SHA1 sha1 = SHA1.Create())
    //        {
    //            byte[] hashBytes = sha1.ComputeHash(byteArray);
    //            StringBuilder sb = new StringBuilder();
    //            foreach (byte b in hashBytes)
    //            {
    //                sb.Append(b.ToString("x2"));
    //            }
    //            return sb.ToString();
    //        }
    //    }

    //    public static async Task Execute(string source)
    //    {
    //        //if (ComputeSha1Hash(key + "TTRrZXk") != "2128f345e7fb4f0a01eca22c06f53dd3f1245f2e")
    //        //{
    //        //    return;
    //        //}

    //        await BaseFunctions.Execute(source);
    //    }

    //    public static async Task Inject()
    //    {
    //        //if (ComputeSha1Hash(key + "TTRrZXk") != "2128f345e7fb4f0a01eca22c06f53dd3f1245f2e")
    //        //{
    //        //    return;
    //        //}
    //        await BaseFunctions.Inject();
    //    }

    //    internal static bool injectorStatus;
    //    public static bool isInjected()
    //    {
    //        return injectorStatus;
    //    }

    //    internal class BaseFunctions
    //    {
    //        internal static bool test = false;

    //        internal static async Task Execute(string source)
    //        {
    //            string celeryfolder = string.Concat(Path.GetTempPath() + "celery");

    //            if (!Directory.Exists(celeryfolder))
    //            {
    //                Directory.CreateDirectory(celeryfolder);

    //                string filePath = Path.Combine(celeryfolder, "myfile.txt");
    //                File.Create(filePath).Dispose();
    //            }
    //            File.WriteAllText(string.Concat(Path.GetTempPath() + "celery", "\\myfile.txt"), source);
    //        }

    //        internal static async Task Inject()
    //        {
    //            string celeryfolder = string.Concat(Path.GetTempPath() + "celery");

    //            if (!Directory.Exists(celeryfolder))
    //            {
    //                Directory.CreateDirectory(celeryfolder);

    //                string filePath = Path.Combine(celeryfolder, "myfile.txt");
    //                File.Create(filePath).Dispose();
    //            }

    //            foreach (Process p in Process.GetProcessesByName("CeleryInject"))
    //            {
    //                p.Kill();
    //            }

    //            try
    //            {
    //                foreach (Util.ProcInfo item in Util.openProcessesByName("RobloxPlayerBeta.exe"))
    //                {
    //                    if (!isInjected())
    //                    {
    //                        await WindowsPlayer.injectPlayer(item);
    //                    }
    //                }
    //            }
    //            catch /*(Exception ex)*/
    //            {
    //                //MessageBox.Show("Fatal error: " + ex.ToString());
    //            }
    //        }

    //        //internal static async Task AutoExecute(string folderPath, int timeout)
    //        //{

    //        //    Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");

    //        //    foreach (Process process in processes)
    //        //    {
    //        //        try
    //        //        {
    //        //            if (!Directory.Exists(folderPath))
    //        //            {
    //        //                Console.WriteLine("The specified folder does not exist.");
    //        //                return;
    //        //            }
    //        //            Console.WriteLine("Folder exists");
    //        //            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
    //        //            string[] luaFiles = Directory.GetFiles(folderPath, "*.lua", SearchOption.AllDirectories);
    //        //            StringBuilder Conclude = new StringBuilder();
    //        //            foreach (string file in txtFiles)
    //        //            {
    //        //                string compress = File.ReadAllText(file);
    //        //                Conclude.AppendLine(compress);
    //        //            }

    //        //            Console.WriteLine("Lua Files (.lua):");
    //        //            foreach (string file in luaFiles)
    //        //            {
    //        //                string compress = File.ReadAllText(file);
    //        //                Conclude.AppendLine(compress);
    //        //            }

    //        //            string final = Conclude.ToString();
    //        //            System.Threading.Thread.Sleep(timeout);
    //        //            while (true)
    //        //            {
    //        //                if (test == true)
    //        //                {
    //        //                    //if (autoExecuteWithCustomUnc == true)
    //        //                    //{
    //        //                    //    Thread.Sleep(timeout);
    //        //                    //    await Execute(final);
    //        //                    //    break;
    //        //                    //}
    //        //                    //else
    //        //                    //{
    //        //                        Thread.Sleep(timeout);
    //        //                        await Execute(final);
    //        //                        break;
    //        //                    //}
    //        //                }
    //        //                else
    //        //                {
    //        //                    //waiting
    //        //                }
    //        //            }
    //        //        }
    //        //        catch (Exception ex)
    //        //        {
    //        //            Console.WriteLine($"An error occurred while trying to auto-execute: {ex.Message}");
    //        //        }
    //        //    }
    //        //}
    //    }

    //    internal class Util
    //    {
    //        public class ProcInfo
    //        {
    //            public Process processRef;
    //            public ulong processId;
    //            public string processName;
    //            public string windowName;
    //            public ulong handle;
    //            public ulong baseModule;
    //            private int nothing;

    //            public ProcInfo()
    //            {
    //                processRef = null;
    //                processId = 0uL;
    //                handle = 0uL;
    //            }
    //        }

    //        public static List<ProcInfo> openProcessesByName(string processName)
    //        {
    //            List<ProcInfo> list = new List<ProcInfo>();
    //            Process[] processesByName = Process.GetProcessesByName(processName.Replace(".exe", ""));
    //            foreach (Process process in processesByName)
    //            {
    //                try
    //                {
    //                    if (process.Id != 0 && !process.HasExited)
    //                    {
    //                        ProcInfo procInfo = new ProcInfo();
    //                        procInfo.processRef = process;
    //                        procInfo.baseModule = 0uL;
    //                        procInfo.handle = 0uL;
    //                        procInfo.processId = (ulong)process.Id;
    //                        procInfo.processName = processName;
    //                        procInfo.windowName = "";
    //                        list.Add(procInfo);
    //                    }
    //                }
    //                catch (NullReferenceException)
    //                {
    //                }
    //                catch (Exception)
    //                {
    //                }
    //            }
    //            return list;
    //        }
    //    }

    //    internal enum InjectionStatus
    //    {
    //        FAILED,
    //        FAILED_ADMINISTRATOR_ACCESS,
    //        ALREADY_INJECTING,
    //        ALREADY_INJECTED,
    //        SUCCESS
    //    }

    //    internal class WindowsPlayer : Util
    //    {
    //        private static List<ProcInfo> postInjectedMainPlayer = new List<ProcInfo>();

    //        private static bool isInjectingMainPlayer = false;

    //        [DllImport("user32.dll")]
    //        private static extern IntPtr FindWindow(string sClass, string sWindow);

    //        public static async Task ExecuteAsAdmin(string local, string name)
    //        {
    //            foreach (Process p in Process.GetProcessesByName(name))
    //            {
    //                p.Kill();
    //            }

    //            Process process = new Process();
    //            process.StartInfo.FileName = local + name;
    //            process.StartInfo.UseShellExecute = false; // Must be false to redirect output
    //            //process.StartInfo.RedirectStandardOutput = true; // Redirect standard output
    //            //process.StartInfo.RedirectStandardError = true; // Redirect standard error
    //            //process.StartInfo.Verb = "runas";
    //            //process.StartInfo.Arguments = AppDomain.CurrentDomain.BaseDirectory + "abc123";
    //            //process.StartInfo.CreateNoWindow = true;

    //            process.Start();

    //            // Start reading output and error streams asynchronously
    //            Task outputTask = ReadOutputUntilMatchAsync(process.StandardOutput);
    //            Task processWaitForExitTask = Task.Run(() => process.WaitForExit());

    //            await Task.WhenAny(outputTask, processWaitForExitTask);
    //        }

    //        private static async Task ReadOutputUntilMatchAsync(StreamReader reader)
    //        {
    //            string line;
    //            while ((line = await reader.ReadLineAsync()) != null)
    //            {
    //                if (line.Contains("Injected successfully"))
    //                {
    //                    injectorStatus = true;
    //                    break;
    //                }
    //                else if(line.Contains("to update"))
    //                {
    //                    injectorStatus = false;
    //                    break;
    //                }
    //                else if (line.Contains("Error"))
    //                {
    //                    injectorStatus = false;
    //                    break;
    //                }
    //            }
    //        }

    //        public async static Task<InjectionStatus> injectPlayer(ProcInfo pinfo)
    //        {
    //            if (WindowsPlayer.isInjectingMainPlayer)
    //                return InjectionStatus.ALREADY_INJECTING;
    //            if (isInjected())
    //                return InjectionStatus.ALREADY_INJECTED;
    //            WindowsPlayer.isInjectingMainPlayer = true;
    //            WindowsPlayer.FindWindow((string)null, "Roblox");
    //            await WindowsPlayer.ExecuteAsAdmin(AppDomain.CurrentDomain.BaseDirectory, "CeleryInject.exe");
    //            WindowsPlayer.isInjectingMainPlayer = false;
    //            return InjectionStatus.SUCCESS;
    //        }
    //    }














    //    //[DllImport("Injector.dll", CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern void Execute(string source, string key = "WTFsigmaKEY");

    //    //[DllImport("Injector.dll", CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern void Inject(string key = "WTFsigmaKEY");

    //    //[DllImport("Injector.dll", CallingConvention = CallingConvention.Cdecl)]
    //    //[return: MarshalAs(UnmanagedType.I1)]
    //    //public static extern bool IsInjected();
    //}
}