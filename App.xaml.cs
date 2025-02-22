using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Essence
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static StreamWriter _logFile;
        private TextWriter _originalConsoleOut;
        internal MultiTextWriter MultiTextWriterInstance { get; set; }
        internal static StartupAnimation StartupWindow;

        private bool error = false;
        internal static MainWindow window;
        internal static bool dnu3ndf3ndn23nd = false;

        protected override async void OnStartup(StartupEventArgs e)
        {
            if (Process.GetProcessesByName("Essence").Length > 1)
            {
                System.Windows.MessageBox.Show("You already have one Essence opened", "Ops!");
                Process.GetCurrentProcess().Kill();
            }

            Scanner.ScanAndKill();

            //DateTime start = DateTime.Now;
            //while (DateTime.Now - start < TimeSpan.FromMilliseconds(200))
            //{
            //    MessageBox.Show("dddd");
            //}

            StartupWindow = new StartupAnimation();
            StartupWindow.Show();

            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;
            try
            {
                if (!Directory.Exists("C:\\Essence"))
                    Directory.CreateDirectory("C:\\Essence");

                if (!Directory.Exists("C:\\Essence\\bin"))
                    Directory.CreateDirectory("C:\\Essence\\bin");

                if (!Directory.Exists("C:\\Essence\\userdata"))
                    Directory.CreateDirectory("C:\\Essence\\userdata");
                
                Environment.CurrentDirectory = "C:\\Essence";
                Directory.SetCurrentDirectory("C:\\Essence");
                //Console.Title = "Essence 2024 © Made with ♥ By m4a1_dev2";

                _originalConsoleOut = Console.Out;

                string logFilePath = "C:\\Essence\\bin\\EssenceLog.txt";
                _logFile = new StreamWriter(logFilePath, append: true, Encoding.UTF8)
                {
                    AutoFlush = true
                };

                MultiTextWriterInstance = new MultiTextWriter(_originalConsoleOut, _logFile);

                IntPtr consoleWindow = GetConsoleWindow();

                if (consoleWindow != IntPtr.Zero)
                {
                    int width = 714; // Defina a largura desejada
                    int height = 150; // Altura da janela
                    int screenX = (int)SystemParameters.PrimaryScreenWidth; // Largura total da tela
                    int screenY = (int)SystemParameters.PrimaryScreenHeight; // Altura total da tela
                    int posX = (screenX - width) / 2; // Centraliza no eixo X
                    int posY = screenY - height - 50; // Mantém a posição Y como antes

                    MoveWindow(consoleWindow, posX, posY, width, height, true);
                }

                // Captura exceções não tratadas na thread UI
                this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);

                // Captura exceções não tratadas em threads de background
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                // Captura exceções não observadas em tarefas assíncronas
                TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(TaskScheduler_UnobservedTaskException);

                Console.WriteLine("error log service started.");

                window = new MainWindow();
                window.Show();

                //JustAnChillWindow justAnChillWindow = new JustAnChillWindow();
                //justAnChillWindow.Show();

                try
                {
                    MultiTextWriterInstance = new MultiTextWriter(_originalConsoleOut, _logFile, new RichTextBoxWriter(window.console_RichTextBox));
                    Console.SetOut(MultiTextWriterInstance);
                    Console.SetError(MultiTextWriterInstance);
                }
                catch { }

                //Console.WriteLine($"\n\nAplicativo iniciado em {DateTime.Now}");

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                if (ex is not IOException)
                    MessageBox.Show($"Erro ao iniciar o aplicativo: {ex.ToString()}");

                //RestartApplication();
            }
        }

        private void RestartApplication()
        {
            // Aguarde um pouco antes de reiniciar
            Task.Delay(2000).ContinueWith(t =>
            {
                // Reinicie o aplicativo
                System.Windows.Forms.Application.Restart();
                Environment.Exit(0); // Encerre o aplicativo atual
            });
        }


        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                if (!error)
                {
                    Console.SetOut(_originalConsoleOut);
                    Console.SetError(_originalConsoleOut);

                    _logFile?.Close();
                    _logFile?.Dispose();
                }
            }
            catch { }
            base.OnExit(e);
        }


        string last_error = "";
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Console.WriteLine("App_DispatcherUnhandledException called\n\n" + e.Exception.ToString());
            //MessageBox.Show("App_DispatcherUnhandledException called\n\n" + e.Exception.ToString());

            last_error = "App_DispatcherUnhandledException";
            try { CheckUIResponsiveness(e.Exception); } catch { }

            // Previne o encerramento do aplicativo
            e.Handled = true;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("CurrentDomain_UnhandledException called\n\n" + ((Exception)e.ExceptionObject).ToString());
            //MessageBox.Show("CurrentDomain_UnhandledException called\n\n" + ((Exception)e.ExceptionObject).ToString());

            last_error = "CurrentDomain_UnhandledException";
            try { CheckUIResponsiveness((Exception)e.ExceptionObject); } catch { }
        }

        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Console.WriteLine("TaskScheduler_UnobservedTaskException called\n\n" + e.Exception.ToString());
            //MessageBox.Show("TaskScheduler_UnobservedTaskException called\n\n" + e.Exception.ToString());

            last_error = "TaskScheduler_UnobservedTaskException";
            try { CheckUIResponsiveness(e.Exception); } catch { }

            e.SetObserved();
        }

        int zeros = 0;
        bool checking = false;
        Exception lol;
        private async void CheckUIResponsiveness(Exception ex)
        {
            if (log || checking)
                return;

            int responsive = 0;
            checking = true;
            double cpuUsage;
            //int zeros = 0;

            lol = ex;

            try
            {
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    try
                    {
                        string test = "Ops... UI crashed?";
                        window.Status_Label2.Text = test;
                        Console.WriteLine("UI Responsive? [CHECK 2]");
                        if (window.Status_Label2.Text == test)
                        {
                            window.Status_Label2.Text = "";
                            Console.WriteLine("UI is Responsive!");
                            responsive += 1;
                        }
                        else
                            Console.WriteLine(window.Status_Label2.Text);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("UIResponsiveness check Stoped.");
                        LogError(ex);
                    }
                });
            }
            catch (Exception exf)
            {
                Console.WriteLine("UIResponsiveness check Stoped.");
                LogError(ex);
                return;
            }

            if (!log)
            {
                Console.WriteLine("NO CRITICAL ERRORS");
                checking = false;
            }
        }



        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        private const uint CP_UTF8 = 65001;

        bool log = false;
        private async void LogError(Exception ex)
        {
            if (log)
                return;

            this.DispatcherUnhandledException -= App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;

            log = true;
            error = true;

            try
            {
                Console.WriteLine("GETTING ERROR INFORMATION [Entire error]...");
                string exToString = "None";
                try { exToString = ex.ToString(); } catch (Exception e) { MessageBox.Show(e.Message, "COMPLETE ERROR"); }

                Console.WriteLine("GETTING ERROR INFORMATION [Message]...");
                string exMessage = "None";
                try { exMessage = ex.Message; } catch (Exception e) { MessageBox.Show(e.Message, "EX MESSAGE"); }

                Console.WriteLine("GETTING LOGS INFORMATION...");
                string complete_log = "None";
                try { complete_log = File.ReadAllText("C:\\Essence\\bin\\EssenceLog.txt"); } catch (Exception e) { }

                Console.WriteLine("SENDING ERROR TO OUR SERVERS...");
                try { await KeyGay2.Get("erro", KeyGay2.current_user + "user_error:" + "TYPE: " + last_error + "\n\n" + exMessage + "\nfull_error:" + exToString + "\nuser_log:" + complete_log, false, true); Console.WriteLine("ERROR SENDED"); } catch (Exception e) { Console.WriteLine(e.Message); }


                await Task.Delay(3000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        Application.Current.MainWindow.Hide();
                    }
                    catch (Exception e) { }


                    CriticalWindow criticalWindow = new CriticalWindow(exToString);
                    criticalWindow.Show();
                });
            }
            catch
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            Application.Current.MainWindow.Hide();
                        }
                        catch (Exception e) { }
                    });
                }
                catch { }

                IntPtr consoleWindow = GetConsoleWindow();
                if (consoleWindow == IntPtr.Zero)
                {
                    AllocConsole();
                    SetConsoleOutputCP(CP_UTF8);
                    StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput(), System.Text.Encoding.UTF8)
                    {
                        AutoFlush = true
                    };
                    Console.SetOut(standardOutput);
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(last_error);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("-------------------- FATAL ERROR IN Essence --------------------");
                Console.ForegroundColor = ConsoleColor.Red;

                try { Console.WriteLine(ex.Message); } catch { Console.WriteLine("NÃO CONSEGUIMOS OBTER O ERRO"); }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------------------------------------------------------------------\n");

                Console.WriteLine("------------------- DETAILED ERROR INFORMATION -------------------");
                Console.ForegroundColor = ConsoleColor.Red;

                try { Console.WriteLine(ex.ToString()); } catch { Console.WriteLine("NÃO CONSEGUIMOS OBTER O ERRO"); }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------------------------------------------------------------------\n");


                try
                {
                    string logFilePath = "EssenceErrorLog.txt";
                    string errorMessage = $"{DateTime.Now}\nException: {ex.ToString()}\n\n\n";
                    File.AppendAllText(logFilePath, errorMessage);
                    Console.WriteLine("PLEASE REPORT 'EssenceErrorLog.txt' TO M4A1");
                }
                catch
                {
                    Console.WriteLine("PLEASE REPORT THIS ERROR TO M4A1");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPress anything to restart.");
                Console.ReadLine();

                string fileName = Process.GetCurrentProcess().MainModule.FileName;
                Process.Start(fileName);
                Process.GetCurrentProcess().Kill();
            }
        }
    }
    public class RichTextBoxWriter : TextWriter
    {
        private readonly RichTextBox _richTextBox;

        public RichTextBoxWriter(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            _richTextBox.Dispatcher.Invoke(() =>
            {
                MainWindow.InternalConsolePrint(value.ToString(), _richTextBox);
                //_richTextBox.AppendText(value.ToString());
                _richTextBox.ScrollToEnd();
            });
        }

        public override void WriteLine(string value)
        {
            _richTextBox.Dispatcher.Invoke(() =>
            {
                MainWindow.InternalConsolePrint(value + Environment.NewLine, _richTextBox);
                _richTextBox.ScrollToEnd();
            });
        }
    }

    public class MultiTextWriter : TextWriter
    {
        internal readonly TextWriter[] _writers;

        internal MultiTextWriter(params TextWriter[] writers)
        {
            _writers = writers;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            try
            {
                foreach (var writer in _writers)
                {
                    writer.Write(value);
                }
            }
            catch { }
        }

        public override void WriteLine(string value)
        {
            try
            {
                foreach (var writer in _writers)
                {
                    writer.WriteLine(value);
                }
            }
            catch { }
        }

        public override void Flush()
        {
            try
            {
                foreach (var writer in _writers)
                {
                    writer.Flush();
                }
            }
            catch { }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var writer in _writers)
                {
                    writer.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }




















    public enum NtStatus : uint
    {
        // Success
        Success = 0x00000000,
        Wait0 = 0x00000000,
        Wait1 = 0x00000001,
        Wait2 = 0x00000002,
        Wait3 = 0x00000003,
        Wait63 = 0x0000003f,
        Abandoned = 0x00000080,
        AbandonedWait0 = 0x00000080,
        AbandonedWait1 = 0x00000081,
        AbandonedWait2 = 0x00000082,
        AbandonedWait3 = 0x00000083,
        AbandonedWait63 = 0x000000bf,
        UserApc = 0x000000c0,
        KernelApc = 0x00000100,
        Alerted = 0x00000101,
        Timeout = 0x00000102,
        Pending = 0x00000103,
        Reparse = 0x00000104,
        MoreEntries = 0x00000105,
        NotAllAssigned = 0x00000106,
        SomeNotMapped = 0x00000107,
        OpLockBreakInProgress = 0x00000108,
        VolumeMounted = 0x00000109,
        RxActCommitted = 0x0000010a,
        NotifyCleanup = 0x0000010b,
        NotifyEnumDir = 0x0000010c,
        NoQuotasForAccount = 0x0000010d,
        PrimaryTransportConnectFailed = 0x0000010e,
        PageFaultTransition = 0x00000110,
        PageFaultDemandZero = 0x00000111,
        PageFaultCopyOnWrite = 0x00000112,
        PageFaultGuardPage = 0x00000113,
        PageFaultPagingFile = 0x00000114,
        CrashDump = 0x00000116,
        ReparseObject = 0x00000118,
        NothingToTerminate = 0x00000122,
        ProcessNotInJob = 0x00000123,
        ProcessInJob = 0x00000124,
        ProcessCloned = 0x00000129,
        FileLockedWithOnlyReaders = 0x0000012a,
        FileLockedWithWriters = 0x0000012b,

        // Informational
        Informational = 0x40000000,
        ObjectNameExists = 0x40000000,
        ThreadWasSuspended = 0x40000001,
        WorkingSetLimitRange = 0x40000002,
        ImageNotAtBase = 0x40000003,
        RegistryRecovered = 0x40000009,

        // Warning
        Warning = 0x80000000,
        GuardPageViolation = 0x80000001,
        DatatypeMisalignment = 0x80000002,
        Breakpoint = 0x80000003,
        SingleStep = 0x80000004,
        BufferOverflow = 0x80000005,
        NoMoreFiles = 0x80000006,
        HandlesClosed = 0x8000000a,
        PartialCopy = 0x8000000d,
        DeviceBusy = 0x80000011,
        InvalidEaName = 0x80000013,
        EaListInconsistent = 0x80000014,
        NoMoreEntries = 0x8000001a,
        LongJump = 0x80000026,
        DllMightBeInsecure = 0x8000002b,

        // Error
        Error = 0xc0000000,
        Unsuccessful = 0xc0000001,
        NotImplemented = 0xc0000002,
        InvalidInfoClass = 0xc0000003,
        InfoLengthMismatch = 0xc0000004,
        AccessViolation = 0xc0000005,
        InPageError = 0xc0000006,
        PagefileQuota = 0xc0000007,
        InvalidHandle = 0xc0000008,
        BadInitialStack = 0xc0000009,
        BadInitialPc = 0xc000000a,
        InvalidCid = 0xc000000b,
        TimerNotCanceled = 0xc000000c,
        InvalidParameter = 0xc000000d,
        NoSuchDevice = 0xc000000e,
        NoSuchFile = 0xc000000f,
        InvalidDeviceRequest = 0xc0000010,
        EndOfFile = 0xc0000011,
        WrongVolume = 0xc0000012,
        NoMediaInDevice = 0xc0000013,
        NoMemory = 0xc0000017,
        NotMappedView = 0xc0000019,
        UnableToFreeVm = 0xc000001a,
        UnableToDeleteSection = 0xc000001b,
        IllegalInstruction = 0xc000001d,
        AlreadyCommitted = 0xc0000021,
        AccessDenied = 0xc0000022,
        BufferTooSmall = 0xc0000023,
        ObjectTypeMismatch = 0xc0000024,
        NonContinuableException = 0xc0000025,
        BadStack = 0xc0000028,
        NotLocked = 0xc000002a,
        NotCommitted = 0xc000002d,
        InvalidParameterMix = 0xc0000030,
        ObjectNameInvalid = 0xc0000033,
        ObjectNameNotFound = 0xc0000034,
        ObjectNameCollision = 0xc0000035,
        ObjectPathInvalid = 0xc0000039,
        ObjectPathNotFound = 0xc000003a,
        ObjectPathSyntaxBad = 0xc000003b,
        DataOverrun = 0xc000003c,
        DataLate = 0xc000003d,
        DataError = 0xc000003e,
        CrcError = 0xc000003f,
        SectionTooBig = 0xc0000040,
        PortConnectionRefused = 0xc0000041,
        InvalidPortHandle = 0xc0000042,
        SharingViolation = 0xc0000043,
        QuotaExceeded = 0xc0000044,
        InvalidPageProtection = 0xc0000045,
        MutantNotOwned = 0xc0000046,
        SemaphoreLimitExceeded = 0xc0000047,
        PortAlreadySet = 0xc0000048,
        SectionNotImage = 0xc0000049,
        SuspendCountExceeded = 0xc000004a,
        ThreadIsTerminating = 0xc000004b,
        BadWorkingSetLimit = 0xc000004c,
        IncompatibleFileMap = 0xc000004d,
        SectionProtection = 0xc000004e,
        EasNotSupported = 0xc000004f,
        EaTooLarge = 0xc0000050,
        NonExistentEaEntry = 0xc0000051,
        NoEasOnFile = 0xc0000052,
        EaCorruptError = 0xc0000053,
        FileLockConflict = 0xc0000054,
        LockNotGranted = 0xc0000055,
        DeletePending = 0xc0000056,
        CtlFileNotSupported = 0xc0000057,
        UnknownRevision = 0xc0000058,
        RevisionMismatch = 0xc0000059,
        InvalidOwner = 0xc000005a,
        InvalidPrimaryGroup = 0xc000005b,
        NoImpersonationToken = 0xc000005c,
        CantDisableMandatory = 0xc000005d,
        NoLogonServers = 0xc000005e,
        NoSuchLogonSession = 0xc000005f,
        NoSuchPrivilege = 0xc0000060,
        PrivilegeNotHeld = 0xc0000061,
        InvalidAccountName = 0xc0000062,
        UserExists = 0xc0000063,
        NoSuchUser = 0xc0000064,
        GroupExists = 0xc0000065,
        NoSuchGroup = 0xc0000066,
        MemberInGroup = 0xc0000067,
        MemberNotInGroup = 0xc0000068,
        LastAdmin = 0xc0000069,
        WrongPassword = 0xc000006a,
        IllFormedPassword = 0xc000006b,
        PasswordRestriction = 0xc000006c,
        LogonFailure = 0xc000006d,
        AccountRestriction = 0xc000006e,
        InvalidLogonHours = 0xc000006f,
        InvalidWorkstation = 0xc0000070,
        PasswordExpired = 0xc0000071,
        AccountDisabled = 0xc0000072,
        NoneMapped = 0xc0000073,
        TooManyLuidsRequested = 0xc0000074,
        LuidsExhausted = 0xc0000075,
        InvalidSubAuthority = 0xc0000076,
        InvalidAcl = 0xc0000077,
        InvalidSid = 0xc0000078,
        InvalidSecurityDescr = 0xc0000079,
        ProcedureNotFound = 0xc000007a,
        InvalidImageFormat = 0xc000007b,
        NoToken = 0xc000007c,
        BadInheritanceAcl = 0xc000007d,
        RangeNotLocked = 0xc000007e,
        DiskFull = 0xc000007f,
        ServerDisabled = 0xc0000080,
        ServerNotDisabled = 0xc0000081,
        TooManyGuidsRequested = 0xc0000082,
        GuidsExhausted = 0xc0000083,
        InvalidIdAuthority = 0xc0000084,
        AgentsExhausted = 0xc0000085,
        InvalidVolumeLabel = 0xc0000086,
        SectionNotExtended = 0xc0000087,
        NotMappedData = 0xc0000088,
        ResourceDataNotFound = 0xc0000089,
        ResourceTypeNotFound = 0xc000008a,
        ResourceNameNotFound = 0xc000008b,
        ArrayBoundsExceeded = 0xc000008c,
        FloatDenormalOperand = 0xc000008d,
        FloatDivideByZero = 0xc000008e,
        FloatInexactResult = 0xc000008f,
        FloatInvalidOperation = 0xc0000090,
        FloatOverflow = 0xc0000091,
        FloatStackCheck = 0xc0000092,
        FloatUnderflow = 0xc0000093,
        IntegerDivideByZero = 0xc0000094,
        IntegerOverflow = 0xc0000095,
        PrivilegedInstruction = 0xc0000096,
        TooManyPagingFiles = 0xc0000097,
        FileInvalid = 0xc0000098,
        InstanceNotAvailable = 0xc00000ab,
        PipeNotAvailable = 0xc00000ac,
        InvalidPipeState = 0xc00000ad,
        PipeBusy = 0xc00000ae,
        IllegalFunction = 0xc00000af,
        PipeDisconnected = 0xc00000b0,
        PipeClosing = 0xc00000b1,
        PipeConnected = 0xc00000b2,
        PipeListening = 0xc00000b3,
        InvalidReadMode = 0xc00000b4,
        IoTimeout = 0xc00000b5,
        FileForcedClosed = 0xc00000b6,
        ProfilingNotStarted = 0xc00000b7,
        ProfilingNotStopped = 0xc00000b8,
        NotSameDevice = 0xc00000d4,
        FileRenamed = 0xc00000d5,
        CantWait = 0xc00000d8,
        PipeEmpty = 0xc00000d9,
        CantTerminateSelf = 0xc00000db,
        InternalError = 0xc00000e5,
        InvalidParameter1 = 0xc00000ef,
        InvalidParameter2 = 0xc00000f0,
        InvalidParameter3 = 0xc00000f1,
        InvalidParameter4 = 0xc00000f2,
        InvalidParameter5 = 0xc00000f3,
        InvalidParameter6 = 0xc00000f4,
        InvalidParameter7 = 0xc00000f5,
        InvalidParameter8 = 0xc00000f6,
        InvalidParameter9 = 0xc00000f7,
        InvalidParameter10 = 0xc00000f8,
        InvalidParameter11 = 0xc00000f9,
        InvalidParameter12 = 0xc00000fa,
        MappedFileSizeZero = 0xc000011e,
        TooManyOpenedFiles = 0xc000011f,
        Cancelled = 0xc0000120,
        CannotDelete = 0xc0000121,
        InvalidComputerName = 0xc0000122,
        FileDeleted = 0xc0000123,
        SpecialAccount = 0xc0000124,
        SpecialGroup = 0xc0000125,
        SpecialUser = 0xc0000126,
        MembersPrimaryGroup = 0xc0000127,
        FileClosed = 0xc0000128,
        TooManyThreads = 0xc0000129,
        ThreadNotInProcess = 0xc000012a,
        TokenAlreadyInUse = 0xc000012b,
        PagefileQuotaExceeded = 0xc000012c,
        CommitmentLimit = 0xc000012d,
        InvalidImageLeFormat = 0xc000012e,
        InvalidImageNotMz = 0xc000012f,
        InvalidImageProtect = 0xc0000130,
        InvalidImageWin16 = 0xc0000131,
        LogonServer = 0xc0000132,
        DifferenceAtDc = 0xc0000133,
        SynchronizationRequired = 0xc0000134,
        DllNotFound = 0xc0000135,
        IoPrivilegeFailed = 0xc0000137,
        OrdinalNotFound = 0xc0000138,
        EntryPointNotFound = 0xc0000139,
        ControlCExit = 0xc000013a,
        PortNotSet = 0xc0000353,
        DebuggerInactive = 0xc0000354,
        CallbackBypass = 0xc0000503,
        PortClosed = 0xc0000700,
        MessageLost = 0xc0000701,
        InvalidMessage = 0xc0000702,
        RequestCanceled = 0xc0000703,
        RecursiveDispatch = 0xc0000704,
        LpcReceiveBufferExpected = 0xc0000705,
        LpcInvalidConnectionUsage = 0xc0000706,
        LpcRequestsNotAllowed = 0xc0000707,
        ResourceInUse = 0xc0000708,
        ProcessIsProtected = 0xc0000712,
        VolumeDirty = 0xc0000806,
        FileCheckedOut = 0xc0000901,
        CheckOutRequired = 0xc0000902,
        BadFileType = 0xc0000903,
        FileTooLarge = 0xc0000904,
        FormsAuthRequired = 0xc0000905,
        VirusInfected = 0xc0000906,
        VirusDeleted = 0xc0000907,
        TransactionalConflict = 0xc0190001,
        InvalidTransaction = 0xc0190002,
        TransactionNotActive = 0xc0190003,
        TmInitializationFailed = 0xc0190004,
        RmNotActive = 0xc0190005,
        RmMetadataCorrupt = 0xc0190006,
        TransactionNotJoined = 0xc0190007,
        DirectoryNotRm = 0xc0190008,
        CouldNotResizeLog = 0xc0190009,
        TransactionsUnsupportedRemote = 0xc019000a,
        LogResizeInvalidSize = 0xc019000b,
        RemoteFileVersionMismatch = 0xc019000c,
        CrmProtocolAlreadyExists = 0xc019000f,
        TransactionPropagationFailed = 0xc0190010,
        CrmProtocolNotFound = 0xc0190011,
        TransactionSuperiorExists = 0xc0190012,
        TransactionRequestNotValid = 0xc0190013,
        TransactionNotRequested = 0xc0190014,
        TransactionAlreadyAborted = 0xc0190015,
        TransactionAlreadyCommitted = 0xc0190016,
        TransactionInvalidMarshallBuffer = 0xc0190017,
        CurrentTransactionNotValid = 0xc0190018,
        LogGrowthFailed = 0xc0190019,
        ObjectNoLongerExists = 0xc0190021,
        StreamMiniversionNotFound = 0xc0190022,
        StreamMiniversionNotValid = 0xc0190023,
        MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
        CantOpenMiniversionWithModifyIntent = 0xc0190025,
        CantCreateMoreStreamMiniversions = 0xc0190026,
        HandleNoLongerValid = 0xc0190028,
        NoTxfMetadata = 0xc0190029,
        LogCorruptionDetected = 0xc0190030,
        CantRecoverWithHandleOpen = 0xc0190031,
        RmDisconnected = 0xc0190032,
        EnlistmentNotSuperior = 0xc0190033,
        RecoveryNotNeeded = 0xc0190034,
        RmAlreadyStarted = 0xc0190035,
        FileIdentityNotPersistent = 0xc0190036,
        CantBreakTransactionalDependency = 0xc0190037,
        CantCrossRmBoundary = 0xc0190038,
        TxfDirNotEmpty = 0xc0190039,
        IndoubtTransactionsExist = 0xc019003a,
        TmVolatile = 0xc019003b,
        RollbackTimerExpired = 0xc019003c,
        TxfAttributeCorrupt = 0xc019003d,
        EfsNotAllowedInTransaction = 0xc019003e,
        TransactionalOpenNotAllowed = 0xc019003f,
        TransactedMappingUnsupportedRemote = 0xc0190040,
        TxfMetadataAlreadyPresent = 0xc0190041,
        TransactionScopeCallbacksNotSet = 0xc0190042,
        TransactionRequiredPromotion = 0xc0190043,
        CannotExecuteFileInTransaction = 0xc0190044,
        TransactionsNotFrozen = 0xc0190045,

        MaximumNtStatus = 0xffffffff
    }



    public enum PROCESSINFOCLASS : int
    {
        ProcessBasicInformation, // 0, q: PROCESS_BASIC_INFORMATION, PROCESS_EXTENDED_BASIC_INFORMATION
        ProcessQuotaLimits, // qs: QUOTA_LIMITS, QUOTA_LIMITS_EX
        ProcessIoCounters, // q: IO_COUNTERS
        ProcessVmCounters, // q: VM_COUNTERS, VM_COUNTERS_EX
        ProcessTimes, // q: KERNEL_USER_TIMES
        ProcessBasePriority, // s: KPRIORITY
        ProcessRaisePriority, // s: ULONG
        ProcessDebugPort, // q: HANDLE
        ProcessExceptionPort, // s: HANDLE
        ProcessAccessToken, // s: PROCESS_ACCESS_TOKEN
        ProcessLdtInformation, // 10
        ProcessLdtSize,
        ProcessDefaultHardErrorMode, // qs: ULONG
        ProcessIoPortHandlers, // (kernel-mode only)
        ProcessPooledUsageAndLimits, // q: POOLED_USAGE_AND_LIMITS
        ProcessWorkingSetWatch, // q: PROCESS_WS_WATCH_INFORMATION[]; s: void
        ProcessUserModeIOPL,
        ProcessEnableAlignmentFaultFixup, // s: BOOLEAN
        ProcessPriorityClass, // qs: PROCESS_PRIORITY_CLASS
        ProcessWx86Information,
        ProcessHandleCount, // 20, q: ULONG, PROCESS_HANDLE_INFORMATION
        ProcessAffinityMask, // s: KAFFINITY
        ProcessPriorityBoost, // qs: ULONG
        ProcessDeviceMap, // qs: PROCESS_DEVICEMAP_INFORMATION, PROCESS_DEVICEMAP_INFORMATION_EX
        ProcessSessionInformation, // q: PROCESS_SESSION_INFORMATION
        ProcessForegroundInformation, // s: PROCESS_FOREGROUND_BACKGROUND
        ProcessWow64Information, // q: ULONG_PTR
        ProcessImageFileName, // q: UNICODE_STRING
        ProcessLUIDDeviceMapsEnabled, // q: ULONG
        ProcessBreakOnTermination, // qs: ULONG
        ProcessDebugObjectHandle, // 30, q: HANDLE
        ProcessDebugFlags, // qs: ULONG
        ProcessHandleTracing, // q: PROCESS_HANDLE_TRACING_QUERY; s: size 0 disables, otherwise enables
        ProcessIoPriority, // qs: ULONG
        ProcessExecuteFlags, // qs: ULONG
        ProcessResourceManagement,
        ProcessCookie, // q: ULONG
        ProcessImageInformation, // q: SECTION_IMAGE_INFORMATION
        ProcessCycleTime, // q: PROCESS_CYCLE_TIME_INFORMATION // since VISTA
        ProcessPagePriority, // q: ULONG
        ProcessInstrumentationCallback, // 40
        ProcessThreadStackAllocation, // s: PROCESS_STACK_ALLOCATION_INFORMATION, PROCESS_STACK_ALLOCATION_INFORMATION_EX
        ProcessWorkingSetWatchEx, // q: PROCESS_WS_WATCH_INFORMATION_EX[]
        ProcessImageFileNameWin32, // q: UNICODE_STRING
        ProcessImageFileMapping, // q: HANDLE (input)
        ProcessAffinityUpdateMode, // qs: PROCESS_AFFINITY_UPDATE_MODE
        ProcessMemoryAllocationMode, // qs: PROCESS_MEMORY_ALLOCATION_MODE
        ProcessGroupInformation, // q: USHORT[]
        ProcessTokenVirtualizationEnabled, // s: ULONG
        ProcessConsoleHostProcess, // q: ULONG_PTR
        ProcessWindowInformation, // 50, q: PROCESS_WINDOW_INFORMATION
        ProcessHandleInformation, // q: PROCESS_HANDLE_SNAPSHOT_INFORMATION // since WIN8
        ProcessMitigationPolicy, // s: PROCESS_MITIGATION_POLICY_INFORMATION
        ProcessDynamicFunctionTableInformation,
        ProcessHandleCheckingMode,
        ProcessKeepAliveCount, // q: PROCESS_KEEPALIVE_COUNT_INFORMATION
        ProcessRevokeFileHandles, // s: PROCESS_REVOKE_FILE_HANDLES_INFORMATION
        ProcessWorkingSetControl, // s: PROCESS_WORKING_SET_CONTROL
        ProcessHandleTable, // since WINBLUE
        ProcessCheckStackExtentsMode,
        ProcessCommandLineInformation, // 60, q: UNICODE_STRING
        ProcessProtectionInformation, // q: PS_PROTECTION
        MaxProcessInfoClass
    }

    [Flags]
    public enum DebugObjectInformationClass : int
    {
        DebugObjectFlags = 1,
        MaxDebugObjectInfoClass
    }

    public enum SYSTEM_INFORMATION_CLASS
    {
        SystemBasicInformation, // q: SYSTEM_BASIC_INFORMATION
        SystemProcessorInformation, // q: SYSTEM_PROCESSOR_INFORMATION
        SystemPerformanceInformation, // q: SYSTEM_PERFORMANCE_INFORMATION
        SystemTimeOfDayInformation, // q: SYSTEM_TIMEOFDAY_INFORMATION
        SystemPathInformation, // not implemented
        SystemProcessInformation, // q: SYSTEM_PROCESS_INFORMATION
        SystemCallCountInformation, // q: SYSTEM_CALL_COUNT_INFORMATION
        SystemDeviceInformation, // q: SYSTEM_DEVICE_INFORMATION
        SystemProcessorPerformanceInformation, // q: SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION
        SystemFlagsInformation, // q: SYSTEM_FLAGS_INFORMATION
        SystemCallTimeInformation, // 10, not implemented
        SystemModuleInformation, // q: RTL_PROCESS_MODULES
        SystemLocksInformation,
        SystemStackTraceInformation,
        SystemPagedPoolInformation, // not implemented
        SystemNonPagedPoolInformation, // not implemented
        SystemHandleInformation, // q: SYSTEM_HANDLE_INFORMATION
        SystemObjectInformation, // q: SYSTEM_OBJECTTYPE_INFORMATION mixed with SYSTEM_OBJECT_INFORMATION
        SystemPageFileInformation, // q: SYSTEM_PAGEFILE_INFORMATION
        SystemVdmInstemulInformation, // q
        SystemVdmBopInformation, // 20, not implemented
        SystemFileCacheInformation, // q: SYSTEM_FILECACHE_INFORMATION; s (requires SeIncreaseQuotaPrivilege) (info for WorkingSetTypeSystemCache)
        SystemPoolTagInformation, // q: SYSTEM_POOLTAG_INFORMATION
        SystemInterruptInformation, // q: SYSTEM_INTERRUPT_INFORMATION
        SystemDpcBehaviorInformation, // q: SYSTEM_DPC_BEHAVIOR_INFORMATION; s: SYSTEM_DPC_BEHAVIOR_INFORMATION (requires SeLoadDriverPrivilege)
        SystemFullMemoryInformation, // not implemented
        SystemLoadGdiDriverInformation, // s (kernel-mode only)
        SystemUnloadGdiDriverInformation, // s (kernel-mode only)
        SystemTimeAdjustmentInformation, // q: SYSTEM_QUERY_TIME_ADJUST_INFORMATION; s: SYSTEM_SET_TIME_ADJUST_INFORMATION (requires SeSystemtimePrivilege)
        SystemSummaryMemoryInformation, // not implemented
        SystemMirrorMemoryInformation, // 30, s (requires license value "Kernel-MemoryMirroringSupported") (requires SeShutdownPrivilege)
        SystemPerformanceTraceInformation, // s
        SystemObsolete0, // not implemented
        SystemExceptionInformation, // q: SYSTEM_EXCEPTION_INFORMATION
        SystemCrashDumpStateInformation, // s (requires SeDebugPrivilege)
        SystemKernelDebuggerInformation, // q: SYSTEM_KERNEL_DEBUGGER_INFORMATION
        SystemContextSwitchInformation, // q: SYSTEM_CONTEXT_SWITCH_INFORMATION
        SystemRegistryQuotaInformation, // q: SYSTEM_REGISTRY_QUOTA_INFORMATION; s (requires SeIncreaseQuotaPrivilege)
        SystemExtendServiceTableInformation, // s (requires SeLoadDriverPrivilege) // loads win32k only
        SystemPrioritySeperation, // s (requires SeTcbPrivilege)
        SystemVerifierAddDriverInformation, // 40, s (requires SeDebugPrivilege)
        SystemVerifierRemoveDriverInformation, // s (requires SeDebugPrivilege)
        SystemProcessorIdleInformation, // q: SYSTEM_PROCESSOR_IDLE_INFORMATION
        SystemLegacyDriverInformation, // q: SYSTEM_LEGACY_DRIVER_INFORMATION
        SystemCurrentTimeZoneInformation, // q
        SystemLookasideInformation, // q: SYSTEM_LOOKASIDE_INFORMATION
        SystemTimeSlipNotification, // s (requires SeSystemtimePrivilege)
        SystemSessionCreate, // not implemented
        SystemSessionDetach, // not implemented
        SystemSessionInformation, // not implemented
        SystemRangeStartInformation, // 50, q
        SystemVerifierInformation, // q: SYSTEM_VERIFIER_INFORMATION; s (requires SeDebugPrivilege)
        SystemVerifierThunkExtend, // s (kernel-mode only)
        SystemSessionProcessInformation, // q: SYSTEM_SESSION_PROCESS_INFORMATION
        SystemLoadGdiDriverInSystemSpace, // s (kernel-mode only) (same as SystemLoadGdiDriverInformation)
        SystemNumaProcessorMap, // q
        SystemPrefetcherInformation, // q: PREFETCHER_INFORMATION; s: PREFETCHER_INFORMATION // PfSnQueryPrefetcherInformation
        SystemExtendedProcessInformation, // q: SYSTEM_PROCESS_INFORMATION
        SystemRecommendedSharedDataAlignment, // q
        SystemComPlusPackage, // q; s
        SystemNumaAvailableMemory, // 60
        SystemProcessorPowerInformation, // q: SYSTEM_PROCESSOR_POWER_INFORMATION
        SystemEmulationBasicInformation, // q
        SystemEmulationProcessorInformation,
        SystemExtendedHandleInformation, // q: SYSTEM_HANDLE_INFORMATION_EX
        SystemLostDelayedWriteInformation, // q: ULONG
        SystemBigPoolInformation, // q: SYSTEM_BIGPOOL_INFORMATION
        SystemSessionPoolTagInformation, // q: SYSTEM_SESSION_POOLTAG_INFORMATION
        SystemSessionMappedViewInformation, // q: SYSTEM_SESSION_MAPPED_VIEW_INFORMATION
        SystemHotpatchInformation, // q; s
        SystemObjectSecurityMode, // 70, q
        SystemWatchdogTimerHandler, // s (kernel-mode only)
        SystemWatchdogTimerInformation, // q (kernel-mode only); s (kernel-mode only)
        SystemLogicalProcessorInformation, // q: SYSTEM_LOGICAL_PROCESSOR_INFORMATION
        SystemWow64SharedInformationObsolete, // not implemented
        SystemRegisterFirmwareTableInformationHandler, // s (kernel-mode only)
        SystemFirmwareTableInformation, // not implemented
        SystemModuleInformationEx, // q: RTL_PROCESS_MODULE_INFORMATION_EX
        SystemVerifierTriageInformation, // not implemented
        SystemSuperfetchInformation, // q: SUPERFETCH_INFORMATION; s: SUPERFETCH_INFORMATION // PfQuerySuperfetchInformation
        SystemMemoryListInformation, // 80, q: SYSTEM_MEMORY_LIST_INFORMATION; s: SYSTEM_MEMORY_LIST_COMMAND (requires SeProfileSingleProcessPrivilege)
        SystemFileCacheInformationEx, // q: SYSTEM_FILECACHE_INFORMATION; s (requires SeIncreaseQuotaPrivilege) (same as SystemFileCacheInformation)
        SystemThreadPriorityClientIdInformation, // s: SYSTEM_THREAD_CID_PRIORITY_INFORMATION (requires SeIncreaseBasePriorityPrivilege)
        SystemProcessorIdleCycleTimeInformation, // q: SYSTEM_PROCESSOR_IDLE_CYCLE_TIME_INFORMATION[]
        SystemVerifierCancellationInformation, // not implemented // name:wow64:whNT32QuerySystemVerifierCancellationInformation
        SystemProcessorPowerInformationEx, // not implemented
        SystemRefTraceInformation, // q; s // ObQueryRefTraceInformation
        SystemSpecialPoolInformation, // q; s (requires SeDebugPrivilege) // MmSpecialPoolTag, then MmSpecialPoolCatchOverruns != 0
        SystemProcessIdInformation, // q: SYSTEM_PROCESS_ID_INFORMATION
        SystemErrorPortInformation, // s (requires SeTcbPrivilege)
        SystemBootEnvironmentInformation, // 90, q: SYSTEM_BOOT_ENVIRONMENT_INFORMATION
        SystemHypervisorInformation, // q; s (kernel-mode only)
        SystemVerifierInformationEx, // q; s
        SystemTimeZoneInformation, // s (requires SeTimeZonePrivilege)
        SystemImageFileExecutionOptionsInformation, // s: SYSTEM_IMAGE_FILE_EXECUTION_OPTIONS_INFORMATION (requires SeTcbPrivilege)
        SystemCoverageInformation, // q; s // name:wow64:whNT32QuerySystemCoverageInformation; ExpCovQueryInformation
        SystemPrefetchPatchInformation, // not implemented
        SystemVerifierFaultsInformation, // s (requires SeDebugPrivilege)
        SystemSystemPartitionInformation, // q: SYSTEM_SYSTEM_PARTITION_INFORMATION
        SystemSystemDiskInformation, // q: SYSTEM_SYSTEM_DISK_INFORMATION
        SystemProcessorPerformanceDistribution, // 100, q: SYSTEM_PROCESSOR_PERFORMANCE_DISTRIBUTION
        SystemNumaProximityNodeInformation, // q
        SystemDynamicTimeZoneInformation, // q; s (requires SeTimeZonePrivilege)
        SystemCodeIntegrityInformation, // q // SeCodeIntegrityQueryInformation
        SystemProcessorMicrocodeUpdateInformation, // s
        SystemProcessorBrandString, // q // HaliQuerySystemInformation -> HalpGetProcessorBrandString, info class 23
        SystemVirtualAddressInformation, // q: SYSTEM_VA_LIST_INFORMATION[]; s: SYSTEM_VA_LIST_INFORMATION[] (requires SeIncreaseQuotaPrivilege) // MmQuerySystemVaInformation
        SystemLogicalProcessorAndGroupInformation, // q: SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX // since WIN7 // KeQueryLogicalProcessorRelationship
        SystemProcessorCycleTimeInformation, // q: SYSTEM_PROCESSOR_CYCLE_TIME_INFORMATION[]
        SystemStoreInformation, // q; s // SmQueryStoreInformation
        SystemRegistryAppendString, // 110, s: SYSTEM_REGISTRY_APPEND_STRING_PARAMETERS
        SystemAitSamplingValue, // s: ULONG (requires SeProfileSingleProcessPrivilege)
        SystemVhdBootInformation, // q: SYSTEM_VHD_BOOT_INFORMATION
        SystemCpuQuotaInformation, // q; s // PsQueryCpuQuotaInformation
        SystemNativeBasicInformation, // not implemented
        SystemSpare1, // not implemented
        SystemLowPriorityIoInformation, // q: SYSTEM_LOW_PRIORITY_IO_INFORMATION
        SystemTpmBootEntropyInformation, // q: TPM_BOOT_ENTROPY_NT_RESULT // ExQueryTpmBootEntropyInformation
        SystemVerifierCountersInformation, // q: SYSTEM_VERIFIER_COUNTERS_INFORMATION
        SystemPagedPoolInformationEx, // q: SYSTEM_FILECACHE_INFORMATION; s (requires SeIncreaseQuotaPrivilege) (info for WorkingSetTypePagedPool)
        SystemSystemPtesInformationEx, // 120, q: SYSTEM_FILECACHE_INFORMATION; s (requires SeIncreaseQuotaPrivilege) (info for WorkingSetTypeSystemPtes)
        SystemNodeDistanceInformation, // q
        SystemAcpiAuditInformation, // q: SYSTEM_ACPI_AUDIT_INFORMATION // HaliQuerySystemInformation -> HalpAuditQueryResults, info class 26
        SystemBasicPerformanceInformation, // q: SYSTEM_BASIC_PERFORMANCE_INFORMATION // name:wow64:whNtQuerySystemInformation_SystemBasicPerformanceInformation
        SystemQueryPerformanceCounterInformation, // q: SYSTEM_QUERY_PERFORMANCE_COUNTER_INFORMATION // since WIN7 SP1
        SystemSessionBigPoolInformation, // since WIN8
        SystemBootGraphicsInformation,
        SystemScrubPhysicalMemoryInformation,
        SystemBadPageInformation,
        SystemProcessorProfileControlArea,
        SystemCombinePhysicalMemoryInformation, // 130
        SystemEntropyInterruptTimingCallback,
        SystemConsoleInformation,
        SystemPlatformBinaryInformation,
        SystemThrottleNotificationInformation,
        SystemHypervisorProcessorCountInformation,
        SystemDeviceDataInformation,
        SystemDeviceDataEnumerationInformation,
        SystemMemoryTopologyInformation,
        SystemMemoryChannelInformation,
        SystemBootLogoInformation, // 140
        SystemProcessorPerformanceInformationEx, // since WINBLUE
        SystemSpare0,
        SystemSecureBootPolicyInformation,
        SystemPageFileInformationEx,
        SystemSecureBootInformation,
        SystemEntropyInterruptTimingRawInformation,
        SystemPortableWorkspaceEfiLauncherInformation,
        SystemFullProcessInformation, // q: SYSTEM_PROCESS_INFORMATION with SYSTEM_PROCESS_INFORMATION_EXTENSION (requires admin)
        SystemKernelDebuggerInformationEx,
        SystemBootMetadataInformation, // 150
        SystemSoftRebootInformation,
        SystemElamCertificateInformation,
        SystemOfflineDumpConfigInformation,
        SystemProcessorFeaturesInformation,
        SystemRegistryReconciliationInformation,
        SystemEdidInformation,
        MaxSystemInfoClass
    }

    public enum ThreadInformationClass
    {
        ThreadBasicInformation = 0,
        ThreadTimes = 1,
        ThreadPriority = 2,
        ThreadBasePriority = 3,
        ThreadAffinityMask = 4,
        ThreadImpersonationToken = 5,
        ThreadDescriptorTableEntry = 6,
        ThreadEnableAlignmentFaultFixup = 7,
        ThreadEventPair_Reusable = 8,
        ThreadQuerySetWin32StartAddress = 9,
        ThreadZeroTlsCell = 10,
        ThreadPerformanceCount = 11,
        ThreadAmILastThread = 12,
        ThreadIdealProcessor = 13,
        ThreadPriorityBoost = 14,
        ThreadSetTlsArrayAddress = 15,   // Obsolete
        ThreadIsIoPending = 16,
        ThreadHideFromDebugger = 17,
        ThreadBreakOnTermination = 18,
        ThreadSwitchLegacyState = 19,
        ThreadIsTerminated = 20,
        ThreadLastSystemCall = 21,
        ThreadIoPriority = 22,
        ThreadCycleTime = 23,
        ThreadPagePriority = 24,
        ThreadActualBasePriority = 25,
        ThreadTebInformation = 26,
        ThreadCSwitchMon = 27,   // Obsolete
        ThreadCSwitchPmu = 28,
        ThreadWow64Context = 29,
        ThreadGroupInformation = 30,
        ThreadUmsInformation = 31,   // UMS
        ThreadCounterProfiling = 32,
        ThreadIdealProcessorEx = 33,
        ThreadCpuAccountingInformation = 34,
        ThreadSuspendCount = 35,
        ThreadDescription = 38,
        ThreadActualGroupAffinity = 41,
        ThreadDynamicCodePolicy = 42,
    }

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

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct SYSTEM_KERNEL_DEBUGGER_INFORMATION
    {
        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U1)]
        public bool KernelDebuggerEnabled;

        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U1)]
        public bool KernelDebuggerNotPresent;
    }




    class DebugProtect1
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsDebuggerPresent();

        /// <summary>
        /// Peform basic checks, method 1
        /// Checks are very fast, there is no CPU overhead.
        /// </summary>
        public static int PerformChecks()
        {
            if (!App.dnu3ndf3ndn23nd)
                return 0;
            if (CheckDebuggerManagedPresent() == 1)
            {
                return 1;
            }

            if (CheckDebuggerUnmanagedPresent() == 1)
            {
                return 1;
            }


            if (CheckRemoteDebugger() == 1)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Asks the CLR for the presence of an attached managed debugger, and never even bothers to check for the presence of a native debugger.
        /// </summary>
        private static int CheckDebuggerManagedPresent()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Asks the kernel for the presence of an attached native debugger, and has no knowledge of managed debuggers.
        /// </summary>
        private static int CheckDebuggerUnmanagedPresent()
        {
            if (IsDebuggerPresent())
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Checks whether a process is being debugged.
        /// </summary>
        /// <remarks>
        /// The "remote" in CheckRemoteDebuggerPresent does not imply that the debugger
        /// necessarily resides on a different computer; instead, it indicates that the 
        /// debugger resides in a separate and parallel process.
        /// </remarks>
        private static int CheckRemoteDebugger()
        {
            var isDebuggerPresent = (bool)false;

            var bApiRet = CheckRemoteDebuggerPresent(System.Diagnostics.Process.GetCurrentProcess().Handle, ref isDebuggerPresent);

            if (bApiRet && isDebuggerPresent)
            {
                return 1;
            }

            return 0;
        }
    }

    class DebugProtect2
    {
        [DllImport("ntdll.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern NtStatus NtQueryInformationProcess([In] IntPtr ProcessHandle, [In] PROCESSINFOCLASS ProcessInformationClass, out IntPtr ProcessInformation, [In] int ProcessInformationLength, [Optional] out int ReturnLength);

        [DllImport("ntdll.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern NtStatus NtClose([In] IntPtr Handle);

        [DllImport("ntdll.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern NtStatus NtRemoveProcessDebug(IntPtr ProcessHandle, IntPtr DebugObjectHandle);

        [DllImport("ntdll.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern NtStatus NtSetInformationDebugObject([In] IntPtr DebugObjectHandle, [In] DebugObjectInformationClass DebugObjectInformationClass, [In] IntPtr DebugObjectInformation, [In] int DebugObjectInformationLength, [Out][Optional] out int ReturnLength);

        [DllImport("ntdll.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern NtStatus NtQuerySystemInformation([In] SYSTEM_INFORMATION_CLASS SystemInformationClass, IntPtr SystemInformation, [In] int SystemInformationLength, [Out][Optional] out int ReturnLength);

        static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        /// <summary>
        /// Peform basic checks, method 2
        /// Checks are very fast, there is a little CPU overhead.
        /// </summary>
        public static int PerformChecks()
        {
            if (!App.dnu3ndf3ndn23nd)
                return 0;

            if (CheckDebugPort() == 1)
            {
                return 1;
            }

            if (CheckKernelDebugInformation())
            {
                return 1;
            }

            if (DetachFromDebuggerProcess())
            {
                return 1;
            }

            return 0;
        }

        private static int CheckDebugPort()
        {
            NtStatus status;
            IntPtr DebugPort = new IntPtr(0);
            int ReturnLength;

            unsafe
            {
                status = NtQueryInformationProcess(System.Diagnostics.Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugPort, out DebugPort, Marshal.SizeOf(DebugPort), out ReturnLength);

                if (status == NtStatus.Success)
                {
                    if (DebugPort == new IntPtr(-1))
                    {
                        Console.WriteLine("DebugPort : {0:X}", DebugPort);
                        return 1;
                    }
                }
            }

            return 0;
        }

        private static bool DetachFromDebuggerProcess()
        {
            IntPtr hDebugObject = INVALID_HANDLE_VALUE;
            var dwFlags = 0U;
            NtStatus ntStatus;
            int retLength_1;
            int retLength_2;

            unsafe
            {
                ntStatus = NtQueryInformationProcess(System.Diagnostics.Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugObjectHandle, out hDebugObject, IntPtr.Size, out retLength_1);

                if (ntStatus != NtStatus.Success)
                {
                    return false;
                }

                ntStatus = NtSetInformationDebugObject(hDebugObject, DebugObjectInformationClass.DebugObjectFlags, new IntPtr(&dwFlags), Marshal.SizeOf(dwFlags), out retLength_2);

                if (ntStatus != NtStatus.Success)
                {
                    return false;
                }

                ntStatus = NtRemoveProcessDebug(System.Diagnostics.Process.GetCurrentProcess().Handle, hDebugObject);

                if (ntStatus != NtStatus.Success)
                {
                    return false;
                }

                ntStatus = NtClose(hDebugObject);

                if (ntStatus != NtStatus.Success)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CheckKernelDebugInformation()
        {
            SYSTEM_KERNEL_DEBUGGER_INFORMATION pSKDI;

            int retLength;
            NtStatus ntStatus;

            unsafe
            {
                ntStatus = NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemKernelDebuggerInformation, new IntPtr(&pSKDI), Marshal.SizeOf(pSKDI), out retLength);

                if (ntStatus == NtStatus.Success)
                {
                    if (pSKDI.KernelDebuggerEnabled && !pSKDI.KernelDebuggerNotPresent)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }

    class DebugProtect3
    {
        [DllImport("ntdll.dll")]
        internal static extern NtStatus NtSetInformationThread(IntPtr ThreadHandle, ThreadInformationClass ThreadInformationClass, IntPtr ThreadInformation, int ThreadInformationLength);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        public static void HideOSThreads()
        {
            if (!App.dnu3ndf3ndn23nd)
                return;

            ProcessThreadCollection currentThreads = Process.GetCurrentProcess().Threads;

            foreach (ProcessThread thread in currentThreads)
            {
                //Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine("[GetOSThreads]: thread.Id {0:X}", thread.Id);

                IntPtr pOpenThread = OpenThread(ThreadAccess.SET_INFORMATION, false, (uint)thread.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    //Console.ForegroundColor = ConsoleColor.Yellow;
                    //Console.WriteLine("[GetOSThreads]: skipped thread.Id {0:X}", thread.Id);
                    continue;
                }

                if (HideFromDebugger(pOpenThread))
                {
                    //Console.ForegroundColor = ConsoleColor.Green;
                    //Console.WriteLine("[GetOSThreads]: thread.Id {0:X} hidden from debbuger.", thread.Id);
                }

                CloseHandle(pOpenThread);
            }
        }

        /// <summary>
        /// Hide the thread from debug events.
        /// </summary>
        public static bool HideFromDebugger(IntPtr Handle)
        {
            NtStatus nStatus = NtSetInformationThread(Handle, ThreadInformationClass.ThreadHideFromDebugger, IntPtr.Zero, 0);

            if (nStatus == NtStatus.Success)
            {
                return true;
            }

            return false;
        }
    }

    class Scanner
    {
        private static HashSet<string> BadProcessnameList = new HashSet<string>();
        private static HashSet<string> BadWindowTextList = new HashSet<string>();

        public static async void ScanAndKill()
        {
            if (!App.dnu3ndf3ndn23nd)
                return;

            if (Scanner.Scan(true) != 0)
            {
                await Task.Delay(2500);
                Process.GetCurrentProcess().Kill();
            }
        }

        /// <summary>
        /// Simple scanner for "bad" processes (debuggers) using .NET code only. (for now)
        /// </summary>
        private static int Scan(bool KillProcess)
        {
            int isBadProcess = 0;

            if (BadProcessnameList.Count == 0 && BadWindowTextList.Count == 0)
            {
                Init();
            }

            Process[] processList = Process.GetProcesses();

            foreach (Process process in processList)
            {
                if (BadProcessnameList.Contains(process.ProcessName) || BadWindowTextList.Contains(process.MainWindowTitle))
                {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    //Console.Write("BAD PROCESS FOUND: " + process.ProcessName);

                    isBadProcess = 1;

                    if (KillProcess)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch (System.ComponentModel.Win32Exception w32ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Win32Exception: " + w32ex.Message);

                            break;
                        }
                        catch (System.NotSupportedException nex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("NotSupportedException: " + nex.Message);

                            break;
                        }
                        catch (System.InvalidOperationException ioex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("InvalidOperationException: " + ioex.Message);

                            break;
                        }
                    }

                    break;
                }
            }

            return isBadProcess;
        }

        /// <summary>
        /// Populate "database" with process names/window names.
        /// Using HashSet for maximum performance
        /// </summary>
        private static int Init()
        {
            if (BadProcessnameList.Count > 0 && BadWindowTextList.Count > 0)
            {
                return 1;
            }

            BadProcessnameList.Add("ollydbg");
            BadProcessnameList.Add("ida");
            BadProcessnameList.Add("ida64");
            BadProcessnameList.Add("idag");
            BadProcessnameList.Add("idag64");
            BadProcessnameList.Add("idaw");
            BadProcessnameList.Add("idaw64");
            BadProcessnameList.Add("idaq");
            BadProcessnameList.Add("idaq64");
            BadProcessnameList.Add("idau");
            BadProcessnameList.Add("idau64");
            BadProcessnameList.Add("scylla");
            BadProcessnameList.Add("scylla_x64");
            BadProcessnameList.Add("scylla_x86");
            BadProcessnameList.Add("protection_id");
            BadProcessnameList.Add("x64dbg");
            BadProcessnameList.Add("x32dbg");
            BadProcessnameList.Add("windbg");
            BadProcessnameList.Add("reshacker");
            BadProcessnameList.Add("ImportREC");
            BadProcessnameList.Add("IMMUNITYDEBUGGER");
            BadProcessnameList.Add("MegaDumper");
            BadWindowTextList.Add("HTTPDebuggerUI");
            BadWindowTextList.Add("HTTPDebuggerSvc");
            BadWindowTextList.Add("HTTP Debugger");
            BadWindowTextList.Add("HTTP Debugger (32 bit)");
            BadWindowTextList.Add("HTTP Debugger (64 bit)");
            BadWindowTextList.Add("OLLYDBG");
            BadWindowTextList.Add("ida");
            BadWindowTextList.Add("disassembly");
            BadWindowTextList.Add("scylla");
            BadWindowTextList.Add("Debug");
            BadWindowTextList.Add("[CPU");
            BadWindowTextList.Add("Immunity");
            BadWindowTextList.Add("WinDbg");
            BadWindowTextList.Add("x32dbg");
            BadWindowTextList.Add("x64dbg");
            BadWindowTextList.Add("Import reconstructor");
            BadWindowTextList.Add("MegaDumper");
            BadWindowTextList.Add("MegaDumper 1.0 by CodeCracker / SnD");

            return 0;
        }

    }
}