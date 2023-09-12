# Welcome to OpenApp.Net!

**OpenApp.Net** is a simple application that opens applications on your behalf. The tool was developed to simplify the opening of multiple different applications as part of a specific workload.

OpenApp.Net is built using .NET 4.8.1, and designed exclusively for Windows based Operating Systems.

To get started with OpenApp.Net, please follow these simple instructions:
1. Create a new directory on your computer and generate shortcut (\*.lnk) files for all your desired applications into this directory.

1. Fork the repository and build the app. It builds into a C# Console Application.

1. Copy and Past OpenApp.exe to anywhere on your computer (saving OpenApp.exe into your shortcuts directory leads to an even easier configuration!)

1. OpenApp accepts 1 command line argument: the directory to launch programs inside of. This argument is not necessary, and by default OpenApp will launch all programs inside the same directory of the OpenApp.exe file.

1. OpenApp can launch programs from any directory using the command line argument. Note: OpenApp does not parse nested folders, all shortcut files for launching must be on the root directory.

1. That's it! Consider creating a shortcut to run OpenApp, that way you can define the command line arguments inside the shortcut!
