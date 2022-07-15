using System.Runtime.InteropServices;

namespace AutoCleaner
{
    internal class RecentDocsHelpers
    {
        public enum ShellAddToRecentDocsFlags
        {
            Pidl = 0x001,
            Path = 0x002,
            PathW = 0x003
        }

        [DllImport("shell32.dll")]
        private static extern void SHAddToRecentDocs(ShellAddToRecentDocsFlags flag, string path);

        public static void AddFile(string path)
        {
            SHAddToRecentDocs(ShellAddToRecentDocsFlags.PathW, path); 
        }

        public static void ClearAll()
        {
            try
            {
                SHAddToRecentDocs(ShellAddToRecentDocsFlags.Pidl, null);
            }
            catch (System.Exception)
            {

            }
        }
    }
}
