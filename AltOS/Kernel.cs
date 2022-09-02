using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem.VFS;

namespace AltOS
{
    public class Kernel : Sys.Kernel
    {
        void fread(string name)
        {
            File.ReadAllText(current_directory + name);
        }
        void fwrite(string name)
        {
            
        }
        void mkfile(string name)
        {
            try
            {
                var file_stream = File.Create(@current_directory + name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        void dir()
        {
            string[] dirs = Directory.GetDirectories(current_directory);
            foreach (var directory in dirs)
            {
                Console.WriteLine(directory);
            }
            string[] files = Directory.GetFiles(current_directory);
            foreach (var item in files)
            {
                Console.WriteLine(item);
            }
        }
        void mkdir(string path, string name)
        {
            Directory.CreateDirectory(@path + @name);
        }
        void shutdown(string todo = "s")
        {
            if (todo == "s")
            {
                Sys.Power.Shutdown();
            }
            if (todo == "r")
            {
                Sys.Power.Reboot();
            }
        }

        void clear()
        {
            Console.Clear();
        }

        void cdf(string where)
        {
            current_directory = where;
        }
        void disklist()
        {
            var dsklist = VFSManager.GetDisks();
            foreach (var disk in dsklist)
            {
                var partitions = disk.Partitions;
                Console.WriteLine("------------------");
                foreach (var partition in partitions)
                {
                    Console.WriteLine(partition.RootPath);
                }
                Console.WriteLine(disk.Size);
                Console.WriteLine("------------------");
            }
        }

        Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
        string current_directory = "0:\\";

        protected override void BeforeRun()
        {
            fs = new Sys.FileSystem.CosmosVFS();
            VFSManager.RegisterVFS(fs);
            Console.Clear();
            Console.WriteLine("System booted sucessfully! \n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Welcome to AltOS!");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected override void Run()
        {
            Console.WriteLine("You are in - " + current_directory + ".");
            Console.Write("/~$> ");
            string command = Console.ReadLine();
            if (command.StartsWith("echo "))
            {
                Console.WriteLine(command.Remove(0, 5));
            }
            else if (command == "shutdown")
            {
                shutdown();
            }
            else if (command.StartsWith("cdf "))
            {

            }
            else if (command == "reboot")
            {
                shutdown("r");
            }
            else if (command == "dir")
            {
                dir();
            }
            else if (command.StartsWith("mkdir "))
            {
                mkdir(current_directory, command.Remove(0, 6));
            }
            else if (command == "clear")
            {
                clear();
            }
            else if (command.StartsWith("cd "))
            {
                current_directory += command.Remove(0, 3);
                current_directory += "\\";
            }
            else if (command.StartsWith("mkfile "))
            {
                mkfile(command.Remove(0, 7));
            }
            else if (command == "dlist")
            {
                disklist();
            }
            else if (command == "help")
            {
                Console.WriteLine("Oh, are you new in this system or forgot command?" +
                    "\n Well, this meny will help you!\n" +
                    "help - show this menu \n" +
                    "shutdown - turn off the computer \n" +
                    "reboot - turn off and then turn on computer \n" +
                    "dir - show all files in directory \n" +
                    "mkdir [name] - create directory \n" +
                    "mkfile [name] - create file \n" +
                    "cd [name] - go to directory \n" +
                    "cdf [path] - go to path like: cdf 0:\\example\\hi\\ \n" +
                    "dlist - get list of disks \n" +
                    "clear - clear the screen \n" +
                    "echo [text] - will repeat the text you wrote \n" +
                    "fwrite [file] [text] - write text to file \n" +
                    "It\'s every command that is aviable now!");
            }
            else if (command == "")
            {

            }
            else
            {
                Console.WriteLine("Unknown command, \"\"" + command + "!");
            }
        }
    }
}
