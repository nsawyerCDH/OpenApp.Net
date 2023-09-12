using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace OpenApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string Dir = Environment.CurrentDirectory;
            //Check is the args is empty.  If it is, then the program will run against it's current directory.
            //If not, pull and validate the directory from args[0]
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

            //Set the file explorer window to the desktop
            OpenFolderAndSelectFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            Console.WriteLine("Operation Complete!");
            for (int i = 10; i > 0; i--)
            {
                Console.Write($"\rConsole will automatically close in {i}... ");
                Thread.Sleep(1000);
            }
        }

        public static void OpenFolderAndSelectFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

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
