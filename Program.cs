using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Fixer
{
    class Program
    {
        static byte NewVersion = 4;
        static byte OldVersion = 5;
        static string[] files;
        static void Main(string[] args)
        {
            Console.Title = "Portal 2 File Fixer";
            Console.WriteLine("Portal 2 File Fixer (By Jsm)");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("DISCLAIMER:\nThis may break your texture files. This may not be a real issue (Just re extract the files) however I am not responsible for any damage that this program may cause");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Enter the FULL path to where you have extracted your Portal 2 materials");
            string folderPath = Console.ReadLine();
            if (!buildFileArray(folderPath))
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to close");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            makeAllChanges();
            Console.WriteLine("The textures should now work in gmod (and possibly other source engine games that do not support the new 7.5 VTF format");
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }
        static bool buildFileArray(string folder)
        {
            if (Directory.Exists(folder))
            {
                Stopwatch fileArraySw = new Stopwatch();
                fileArraySw.Start();
                files = Directory.GetFiles(folder, "*.vtf", SearchOption.AllDirectories);
                fileArraySw.Stop();
                Console.WriteLine("Done! - Found "+files.Length+" VTF files in "+fileArraySw.ElapsedMilliseconds+"ms");
                Console.WriteLine("Ready to make changes");
                return true;
            }
            else
            {
                Console.WriteLine("Error: Not a real folder");
                return false;
            }
        }
        static void makeAllChanges()
        {
            int i = 0;
            Stopwatch changesSW = new Stopwatch();
            changesSW.Start();
            while (i < files.Length)
            {
                changeFile(files[i]);
                i++;
            }
            changesSW.Stop();
            Console.WriteLine("Completely done! - Modified " + i + "("+files.Length+") files in " + changesSW.ElapsedMilliseconds + "ms");
        }
        static void changeFile(string file)
        {
            BinaryReader br = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.ReadWrite));
            br.BaseStream.Seek(8, 0); // Seek to the 8th byte because the sub version is here
            byte version = br.ReadByte();
            if (version == OldVersion)
            {
                br.BaseStream.Seek(8, 0);
                br.BaseStream.WriteByte(NewVersion);
                br.BaseStream.Seek(8, 0);
                if (br.ReadByte() == NewVersion)
                {
                    //Console.WriteLine("Fixed: " + file);
                }
                else
                {
                    Console.WriteLine("Oops I appear to have broken " + file);
                }
                br.BaseStream.Close();
            }
            else if (version == NewVersion)
            {
                //Console.WriteLine("Error: No need to fix " + file);
            }
            else
            {
                Console.WriteLine("Error: Cannot fix " + file);
            }
        }
    }
}
