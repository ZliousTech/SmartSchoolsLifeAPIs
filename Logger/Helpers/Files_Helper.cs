using System.IO;
namespace Logger.Service
{
    internal static class Files_Helper
    {
        public static void CreateDirectory(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && !Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
    }
}
