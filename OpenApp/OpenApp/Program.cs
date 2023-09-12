using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace OpenApp
{
    internal class Program
    {
        /// <summary>
        /// Accept 1 argument, which will be the directory to run against.  By default, it will run against the current directory the app is executing in.
        /// </summary>
        /// <param name="args">args[0] = directory to run against (Optional) (Default = Executing Directory)</param>
        /// <exception cref="Exception"></exception>
        static void Main(string[] args)
        {
            string Dir = Environment.CurrentDirectory;

            //Check if args directory is provided
            if (args != null && args.Length > 0 && args[0].Length > 0)
            {
                if (Directory.Exists(args[0]))
                {
                    Dir = args[0];
                }
                else
                {
                    throw new Exception("The Directory from Argument 0 cannot be found!");
                }
            }
            Console.WriteLine($"Operating on Directory: {Dir}");

            //Iterate over all the files in the directory that are shortcut files
            foreach (string file in Directory.GetFiles(Dir, "*.lnk"))
            {
                //Wait 1 seconds for the previous file to open
                Thread.Sleep(1000);

                //Write to the Console the name of the file
                Console.WriteLine($"> Opening File: {Path.GetFileNameWithoutExtension(file)}");

                //Open the Folder in file explorer and select it
                OpenFolderAndSelectFile(file);

                //Press the enter key to open the file
                SendKeys.SendWait("{ENTER}");
            }

            //Sleep for 1 second to allow the last file to open
            Thread.Sleep(1000);

            //Set the file explorer window to the desktop
            OpenFolderAndSelectFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            //Display Completion Message and countdown to close the console window
            Console.WriteLine("Operation Complete!");
            for (int i = 10; i > 0; i--)
            {
                Console.Write("\r                                            ");
                Console.Write($"\rConsole will automatically close in {i}...");
                Thread.Sleep(1000);
            }
        }

        public static void OpenFolderAndSelectFile(string filePath)
        {
            IntPtr pidl = ILCreateFromPathW(filePath);
            SHOpenFolderAndSelectItems(pidl, 0, IntPtr.Zero, 0);
            ILFree(pidl);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr ILCreateFromPathW(string pszPath);

        [DllImport("shell32.dll")]
        private static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, int cild, IntPtr apidl, int dwFlags);

        [DllImport("shell32.dll")]
        private static extern void ILFree(IntPtr pidl);
    }
}
