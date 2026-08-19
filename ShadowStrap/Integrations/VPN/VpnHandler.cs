using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ShadowStrap.Utilities;
using ShadowStrap.Utilities.Security;
using ShadowStrap.Integrations.Logging;

namespace ShadowStrap.Integrations.VPN
{
    public class VpnHandler
    {
        private Process? _singBoxProcess;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string SecureDataPath = Path.Combine(Paths.Data, "core_v1.dat");

        public async Task StartVpn(string userLicenseKey)
        {
            DebugLogger.Log($"Verifying license key: {userLicenseKey}...");
            
            await DependencyManager.EnsureSingBoxInstalled();

            // 1. Запрос к API с ключом (Сервер должен проверить ключ + IP пользователя)
            string? encryptedConfig = await FetchLicensedConfig(userLicenseKey);
            
            if (string.IsNullOrEmpty(encryptedConfig))
            {
                DebugLogger.Log("Access Denied: Invalid license key or IP mismatch.", "SECURITY_ERROR");
                // Здесь можно вызвать UI уведомление об ошибке лицензии
                return;
            }

            // 2. Сохраняем полученный защищенный конфиг
            File.WriteAllText(SecureDataPath, encryptedConfig);
            
            // 3. Расшифровка в памяти и запуск
            string decrypted = CryptoService.DecryptFromFile(SecureDataPath);
            string tempPath = Path.Combine(Paths.Data, "run_task.json");
            File.WriteAllText(tempPath, decrypted);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(Paths.Data, "sing-box.exe"),
                Arguments = $"run -c {tempPath}",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _singBoxProcess = Process.Start(startInfo);
            
            // Очистка следов
            Task.Run(async () => { await Task.Delay(2000); if(File.Exists(tempPath)) File.Delete(tempPath); });
            
            DebugLogger.Log("ShadowStrap VPN Activated and Running.");
        }

        private async Task<string?> FetchLicensedConfig(string key)
        {
            // Сервер на вашей стороне должен проверять: 
            // if (Database.CheckKey(key) && Database.GetIP(key) == Request.UserIP)
            string authUrl = "https://your-api.com/v1/license/validate";
            try {
                var request = new HttpRequestMessage(HttpMethod.Get, authUrl);
                request.Headers.Add("Authorization", $"Bearer {key}");
                
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                
                return null;
            } catch {
                return null;
            }
        }

        public void StopVpn()
        {
            _singBoxProcess?.Kill();
            if(File.Exists(SecureDataPath)) File.Delete(SecureDataPath);
        }
    }
}
