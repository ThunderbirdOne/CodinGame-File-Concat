using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace ConcatCSFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFolder = ConfigurationManager.AppSettings["input"];
            string output = ConfigurationManager.AppSettings["output"];
            string[] excludeList = ConfigurationManager.AppSettings["exclude"].Split(',');

            String[] allfiles = Directory.GetFiles(inputFolder, "*.cs", SearchOption.AllDirectories);

            File.WriteAllText(output, string.Empty); //clear file
            foreach (var file in allfiles)
            {
                FileInfo info = new FileInfo(file);
                if (excludeList.Contains(info.Name) || info.Name.StartsWith("TemporaryGeneratedFile")) continue;

                string[] readText = File.ReadAllLines(info.FullName);
                if(info.Name != "Player.cs") //if not the main class, remove all the usings
                {
                    readText = readText.Where(r => !r.StartsWith("using")).ToArray();
                }

                File.AppendAllLines(output, readText);
            }
        }
    }
}