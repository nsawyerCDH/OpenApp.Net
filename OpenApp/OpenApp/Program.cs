using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace OpenApp
{
    internal class Program
    {
        /// <summary>
        /// Accept 2 arguments, the directory to run against and the delay (Ms) between opening apps.  By default, it will run against the current directory the app is executing in, with 1000 ms delay between opening apps.
        /// </summary>
        /// <param name="args">args[0] = directory to run against (Optional) (Default = Executing Directory)</param>
        /// <exception cref="Exception"></exception>
        static void Main(string[] args)
        {
            Console.WriteLine($"OpenApp.Net v{Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine("");

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

            //Check if args delay is provided
            int Delay = 1000;
            if (args != null && args.Length > 1 && args[1].Length > 0)
            {
                if (int.TryParse(args[1], out int result))
                {
                    Delay = result;
                }
                else
                {
                    throw new Exception("The Delay from Argument 1 is not a valid integer!");
                }
            }
            Console.WriteLine($"Delay between opening apps: ({Delay / 1000}) seconds");

            //Iterate over all the files in the directory that are shortcut files
            foreach (string file in Directory.GetFiles(Dir, "*.lnk"))
            {
                //Delay for the previous file to open
                Thread.Sleep(Delay);

                //Write to the Console the name of the file
                Console.WriteLine($"> Opening File: {Path.GetFileNameWithoutExtension(file)}");

                //Open the Folder in file explorer and select it
                OpenFolderAndSelectFile(file);

                //Press the enter key to open the file
                SendKeys.SendWait("{ENTER}");
            }

            //Delay to allow the last file to open
            Thread.Sleep(Delay);

            //Display Completion Message and countdown to close the console window
            Console.WriteLine("");
            Console.WriteLine("Operation Complete!");
            for (int i = 10; i > 0; i--)
            {
                Console.Write($"\rConsole will automatically close in ({i.ToString().PadLeft(2)}) seconds...");
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
