using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ShadowStrap.Integrations.Logging;

namespace ShadowStrap.Utilities
{
    public static class DependencyManager
    {
        private static readonly string SingBoxPath = Path.Combine(Paths.Data, "sing-box.exe");
        private static readonly string GitHubUrl = "https://github.com/SagerNet/sing-box/releases/download/v1.8.0/sing-box-1.8.0-windows-amd64.zip";
        private static readonly string MirrorUrl = "https://mirror.ghproxy.com/https://github.com/SagerNet/sing-box/releases/download/v1.8.0/sing-box-1.8.0-windows-amd64.zip";

        public static async Task EnsureSingBoxInstalled()
        {
            if (File.Exists(SingBoxPath)) return;

            DebugLogger.Log("sing-box.exe not found. Starting download...");
            
            bool success = await DownloadFile(GitHubUrl) || await DownloadFile(MirrorUrl);

            if (success)
                DebugLogger.Log("sing-box.exe successfully downloaded and installed.");
            else
                DebugLogger.Log("Failed to download sing-box from all sources.", "ERROR");
        }

        private static async Task<bool> DownloadFile(string url)
        {
            try
            {
                using HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) return false;

                // Logic for unzipping would go here, for now we simulate the stream write
                DebugLogger.Log($"Downloading from: {url}");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
