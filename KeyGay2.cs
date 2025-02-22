using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Essence
{
    public class KeyGay2
    {
        internal static event Action OFF;
        internal static event Action MODIFY;
        internal static event Action NET;
        internal static event Action E429;

        internal static event Action Cancel_Error;

        internal static string current_user = "";
        internal static string current_pass = "";
        internal static string pctypeshit = "";
        internal static string invitecode = "";
        internal static int invites = 0;

        internal static List<string> robloxversions;

        internal static string last_reason = "Reason: Tryed to chnage server response";

        internal static readonly string url = "https://essenceapi.discloud.app/v7/";

        internal static string Discord_ID = "";
        internal static string Roblox_IDS = "";

        internal static bool firstlogin = true;

        internal static async Task Ban(string id)
        {
            if (!Directory.Exists("C:\\ProgramData\\ServiceConnect"))
                Directory.CreateDirectory("C:\\ProgramData\\ServiceConnect");

            if (id.Length > 0)
            {
                try
                {
                    string[] result = Encriptar(id);

                    StreamWriter sw = new StreamWriter("C:\\ProgramData\\ServiceConnect\\SInfo.lofh");
                    sw.WriteLine(result[0]);
                    sw.WriteLine(result[1]);
                    sw.Close();
                }
                catch
                {

                }
            }


            try
            {
                await Get("ban", $"Pc Name: {Environment.MachineName}\nDiscordID: {Discord_ID}\nRobloxIDS: {Roblox_IDS}");
            }
            catch
            {

            }

            await Task.Delay(500);
        }


        internal static string[] Gen()
        {
            string[] data = new string[2];
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder(30);

            for (int i = 0; i < 30; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            string chave = $"M4A1-KEY-{result}";

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(chave));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                data[0] = builder.ToString();
                data[1] = result.ToString();
                return data;
            }
        }

        internal static string[] Encriptar(string text)
        {
            string[] data = new string[2];

            try
            {
                const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                var result = new StringBuilder(23);

                for (int i = 0; i < 23; i++)
                {
                    result.Append(chars[random.Next(chars.Length)]);
                }
                data[1] = result.ToString();
                var saltString = "M4A1-KEY-" + result.ToString();

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltString));

                    using (var rijndael = new RijndaelManaged())
                    {
                        rijndael.Key = hashBytes;
                        rijndael.Mode = CipherMode.ECB;
                        rijndael.Padding = PaddingMode.PKCS7;

                        using (var encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV))
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text + "\nM4A1-AUTH");
                            }
                            data[0] = Convert.ToBase64String(msEncrypt.ToArray());
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Encryyy");
            }
            return data;
        }

        internal static TimeSpan TimeLeft(string file)
        {
            try
            {
                if (!File.Exists(file))
                    return TimeSpan.FromMilliseconds(1);

                StreamReader sr = new StreamReader(file);
                string[] dados = sr.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                sr.Close();

                if (dados[0] == null || dados[1] == null)
                {
                    return TimeSpan.FromMilliseconds(1);
                }


                //string[] resp_key = resposta.Split(new string[] { "Key:" }, StringSplitOptions.RemoveEmptyEntries);

                //string chave = $"M4A1-KEY-{values[1]}-{resp_key[1]}";

                string result = Desencriptar(dados[0], "M4A1-KEY-" + dados[1]);
                if (DateTime.TryParse(result, out DateTime savedDate))
                {
                    TimeSpan difference = savedDate - DateTime.Now;

                    if (difference.TotalMinutes < 12 * 60 && difference.TotalMinutes > 0)
                    {
                        return difference;
                    }
                }

                Console.WriteLine("Erro ao ler o arquivo..");
                return TimeSpan.FromMilliseconds(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao decriptar: " + ex.ToString());
                return TimeSpan.FromMilliseconds(1);
            }
        }

        internal static string GetB(string file)
        {
            try
            {
                if (!File.Exists(file))
                    return "Erro";

                StreamReader sr = new StreamReader(file);
                string[] dados = sr.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                sr.Close();

                if (dados[0] == null || dados[1] == null)
                {
                    Console.WriteLine("Um dos valores no arquivo da key era null.");
                    return "Erro";
                }

                // Obtém a string "salting"
                var saltString = "M4A1-KEY-" + dados[1];
                var keyBase = Encoding.UTF8.GetBytes(saltString);

                // Preenche ou trunca para garantir que a chave tenha 32 bytes
                var keyBytes = new byte[32];
                for (int i = 0; i < keyBase.Length && i < keyBytes.Length; i++)
                {
                    keyBytes[i] = keyBase[i];
                }

                using (var rijndael = new RijndaelManaged())
                {
                    rijndael.Key = keyBytes;
                    rijndael.Mode = CipherMode.ECB;
                    rijndael.Padding = PaddingMode.PKCS7;

                    using (var msDecrypt = new MemoryStream(Convert.FromBase64String(dados[0])))
                    using (var csDecrypt = new CryptoStream(msDecrypt, rijndael.CreateDecryptor(rijndael.Key, rijndael.IV), CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        string re = srDecrypt.ReadToEnd();

                        if (re.Contains("\nM4A1-AUTH"))
                            re = re.Replace("\nM4A1-AUTH", "");
                        else
                            return "Erro";

                        return re;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao decriptar: " + ex.ToString());
                return "Erro";
            }
        }


        //internal static async Task<int> Check(string Key, string User)
        //{
        //    if (Key.Length != 34)
        //    {
        //        return 0;
        //    }

        //    try
        //    {
        //        string logins = await Get("check", Key + "\n" + User);


        //        if (logins.Split('\n').Length > 4)
        //        {
        //            string[] data = logins.Split('\n');
        //            return 2;
        //        }
        //        else if (logins == "outro")
        //        {
        //            return 1;
        //        }           
        //    }
        //    catch { }

        //    return 0;
        //}

        internal static string Desencriptar(string textoEncriptado, string chave)
        {
            Scanner.ScanAndKill();
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(chave);
                    byte[] hashBytes = sha256.ComputeHash(bytes);

                    using (var rijndael = new RijndaelManaged())
                    {
                        DebugProtect3.HideOSThreads();
                        rijndael.Key = hashBytes;
                        rijndael.Mode = CipherMode.ECB;
                        rijndael.Padding = PaddingMode.PKCS7;

                        using (var msDecrypt = new MemoryStream(Convert.FromBase64String(textoEncriptado)))
                        using (var csDecrypt = new CryptoStream(msDecrypt, rijndael.CreateDecryptor(rijndael.Key, rijndael.IV), CryptoStreamMode.Read))
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            string re = srDecrypt.ReadToEnd();

                            if (re.Contains("\nM4A1-AUTH") && DebugProtect1.PerformChecks() == 0 && DebugProtect2.PerformChecks() == 0)
                                return re.Replace("\nM4A1-AUTH", "");
                            else
                                return "Auth Failed";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + textoEncriptado + "\n\n" + chave, "Decryy");
                return "Erro";
            }
        }


        internal static async Task<string> RetryUntilSuccess(string final, string data, bool encrypt)
        {
            int hidh = 0;
            while (true)
            {
                await Task.Delay(5000 * hidh);
                string result = await Get(final, data, encrypt, true);

                if (result != "Erro" && result != "777" && result != "429")
                {
                    Cancel_Error?.Invoke();

                    await Task.Delay(3000);
                    return result;
                }

                if (hidh < 4)
                    hidh++;
            }
        }

        static async Task<bool> CheckInternet2()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                using var response = await client.GetAsync("http://www.example.com");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }


        [DllImport("kernel32.dll")]
        public static extern int GetTickCount();

        public static bool IsBeingDebugged()
        {
            int ticksBefore = GetTickCount();
            System.Threading.Thread.Sleep(100); // Atraso artificial
            int ticksAfter = GetTickCount();

            return ticksAfter < ticksBefore; // Se os ticks aumentarem mais rápido do que o esperado, um debugger pode estar presente
        }

        private static bool repeat;
        internal static async Task<string> Get(string final, string data = "Essence", bool encrypted = true, bool force_return = false)
        {
            if (App.dnu3ndf3ndn23nd && (IsBeingDebugged() || System.Diagnostics.Debugger.IsAttached))
            {
                if (force_return)
                    return "Erro";

                if (repeat)
                {
                    repeat = false;

                    last_reason = "The server response was not protected";
                    MODIFY?.Invoke();
                }
                else
                    repeat = true;

                return await RetryUntilSuccess(final, data, encrypted);
            }


            //MessageBox.Show(final);
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    if (!await CheckInternet2())
                    {
                        if (force_return)
                            return "Erro";

                        NET?.Invoke();
                        return await RetryUntilSuccess(final, data, encrypted);
                    }
                }

                using HttpClient client = new HttpClient();
                string[] values = Gen();
                string[] hh = GetHWID();

                client.DefaultRequestHeaders.Add("M4A1-KEY", values[0]);
                client.DefaultRequestHeaders.Add("M4A1-N", values[1]);

                client.DefaultRequestHeaders.Add("HWID-KEY", hh[0]);
                client.DefaultRequestHeaders.Add("HWID-N", hh[1]);

                client.DefaultRequestHeaders.Add("DeviceKind", pctypeshit);

                using (SHA256 sha256 = SHA256.Create())
                {
                    client.DefaultRequestHeaders.Add("Request-HASH", BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes("Nigger" + data))).Replace("-", "").ToLower());
                }

                client.Timeout = TimeSpan.FromSeconds(9.5);
                var content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");


                HttpResponseMessage response = await client.PostAsync(url + final, content);

                if ((int)response.StatusCode == 429 && force_return)
                {
                    return "429";
                }
                if (response.IsSuccessStatusCode)
                {
                    string resposta = await response.Content.ReadAsStringAsync();
                    //MessageBox.Show(resposta);

                    if (encrypted)
                    {
                        if (resposta.Length < 40)
                        {
                            if (force_return)
                                return "777";

                            if (repeat)
                            {
                                repeat = false;
                                OFF?.Invoke();
                            }
                            else
                                repeat = true;

                            return await RetryUntilSuccess(final, data, encrypted);
                        }

                        if (!resposta.Contains("Key:"))
                        {
                            if (force_return)
                                return "Erro";

                            if (repeat)
                            {
                                repeat = false;

                                last_reason = "The server response was not protected";
                                MODIFY?.Invoke();
                            }
                            else
                                repeat = true;

                            return await RetryUntilSuccess(final, data, encrypted);
                        }

                        else
                        {
                            string[] resp_key = resposta.Split(new string[] { "Key:" }, StringSplitOptions.RemoveEmptyEntries);

                            string chave = $"M4A1-KEY-{values[1]}-{resp_key[1]}";
                            //MessageBox.Show(chave);

                            string re = Desencriptar(resp_key[0], chave);

                            if (re == "Errorr24542")
                            {
                                if (force_return)
                                    return "Erro";

                                last_reason = "User tried to change server response";
                                MODIFY?.Invoke();
                                return await RetryUntilSuccess(final, data, encrypted);
                            }

                            Cancel_Error?.Invoke();
                            return re;
                        }
                    }
                    else
                        return resposta;
                }
                else
                {
                    if (force_return)
                        return "777";

                    OFF?.Invoke();
                    return await RetryUntilSuccess(final, data, encrypted);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.HResult.ToString() == "-2146233088")
                {
                    if (force_return)
                        return "Erro";

                    NET?.Invoke();
                    return await RetryUntilSuccess(final, data, encrypted);
                }
                else
                {
                    if (force_return)
                        return "777";

                    OFF?.Invoke();
                    return await RetryUntilSuccess(final, data, encrypted);
                }
            }
            catch (TaskCanceledException)
            {
                if (await CheckInternet2())
                {
                    if (force_return)
                        return "777";

                    OFF?.Invoke();
                }
                else
                {
                    if (force_return)
                        return "Erro";

                    NET?.Invoke();
                }

                return await RetryUntilSuccess(final, data, encrypted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fatal Error when communicating with server");
                return "Erro";
            }
        }

        internal static string[] GetHWID()
        {
            string machineGuid = null;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
            {
                try
                {
                    if (key != null)
                    {
                        machineGuid = key.GetValue("MachineGuid")?.ToString();
                    }
                }
                catch
                {
                    machineGuid = null;
                }
            }

            try
            {
                machineGuid = Regex.Replace(machineGuid, @"[^a-zA-Z0-9]", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                machineGuid = null;
            }


            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Evo3Software"))
            {
                if (key != null && key.GetValue("Identity") != null && key.GetValue("Secure") != null)
                {
                    //não foi alterado externamente, mas pode ter sido clonado de outro pc
                    if (Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure").ToString()) != "Erro" && Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure")).ToString().Length == 50)
                    {
                        //ggsgsgsg
                        if (!string.IsNullOrEmpty(machineGuid))
                        {
                            if (Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure").ToString()).Contains(machineGuid))
                            {
                                return new string[] { key.GetValue("Identity").ToString(), key.GetValue("Secure").ToString() };
                            }
                        }

                        //verificando o nome do pc
                        else
                        {
                            if (Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure").ToString()).Contains(Environment.MachineName))
                            {
                                return new string[] { key.GetValue("Identity").ToString(), key.GetValue("Secure").ToString() };
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(machineGuid))
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string machineName = Environment.MachineName;
                string randomKey = new string(Enumerable.Range(0, 40 - machineName.Length).Select(_ => chars[random.Next(chars.Length)]).ToArray());

                machineGuid = machineName + randomKey + "RAMDOM-KEY";
            }
            else
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string randomKey = new string(Enumerable.Range(0, 40 - machineGuid.Length).Select(_ => chars[random.Next(chars.Length)]).ToArray());

                machineGuid = machineGuid + randomKey + "USING-REGS";
            }

            //MessageBox.Show(machineGuid, "new guild");

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Evo3Software"))
            {
                string[] lol = Encriptar(machineGuid);
                key.SetValue("Identity", lol[0]);
                key.SetValue("Secure", lol[1]);
                return new string[] { lol[0], lol[1] };
            }

        }

        internal static string GetHWID2()
        {
            string machineGuid = null;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
            {
                try
                {
                    if (key != null)
                    {
                        machineGuid = key.GetValue("MachineGuid")?.ToString();
                    }
                }
                catch
                {
                    machineGuid = null;
                }
            }

            try
            {
                machineGuid = Regex.Replace(machineGuid, @"[^a-zA-Z0-9]", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                machineGuid = null;
            }


            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Evo3Software"))
            {
                if (key != null && key.GetValue("Identity") != null && key.GetValue("Secure") != null)
                {
                    //não foi alterado externamente, mas pode ter sido clonado de outro pc
                    if (Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure").ToString()) != "Erro" && Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure")).ToString().Length == 50)
                    {
                        //ggsgsgsg
                        if (!string.IsNullOrEmpty(machineGuid))
                        {
                            if (Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure").ToString()).Contains(machineGuid))
                            {
                                return Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure").ToString());
                            }
                        }

                        //verificando o nome do pc
                        else
                        {
                            if (Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure").ToString()).Contains(Environment.MachineName))
                            {
                                return Desencriptar(key.GetValue("Identity").ToString(), "M4A1-KEY-" + key.GetValue("Secure").ToString());
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(machineGuid))
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string machineName = Environment.MachineName;
                string randomKey = new string(Enumerable.Range(0, 40 - machineName.Length).Select(_ => chars[random.Next(chars.Length)]).ToArray());

                machineGuid = machineName + randomKey + "RAMDOM-KEY";
            }
            else
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string randomKey = new string(Enumerable.Range(0, 40 - machineGuid.Length).Select(_ => chars[random.Next(chars.Length)]).ToArray());

                machineGuid = machineGuid + randomKey + "USING-REGS";
            }

            //MessageBox.Show(machineGuid, "new guild");

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Evo3Software"))
            {
                string[] lol = Encriptar(machineGuid);
                key.SetValue("Identity", lol[0]);
                key.SetValue("Secure", lol[1]);
                return machineGuid;
            }

        }
    }
}