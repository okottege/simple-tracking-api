using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerService.BuildTools
{
    class Program
    {
        static void  Main(string[] args)
        {
            Console.WriteLine("Tracker Service Build Tools started.");
            Console.WriteLine($"Command Line arguments: {string.Join(", ", args)}");

            if (args.Any())
            {
                switch (args[0])
                {
                    case "sql-script-combine":
                        CombineSqlScripts(args).Wait();
                        break;
                    default:
                        Console.WriteLine($"Unsupported command '{args[0]}'");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Command Line Arguments must be supplied.");
                Environment.Exit(-1);
            }

            Console.WriteLine("Build Tools finished execution.");
        }

        private static async Task CombineSqlScripts(string[] args)
        {
            try
            {
                Console.WriteLine("Executing the combining of SQL scripts command.");

                if (args.Length < 3)
                {
                    Console.WriteLine("Cannot combine sql scripts as source sql folder and destination sql file not provided.");
                    Environment.Exit(-1);
                }
                else
                {
                    var sb = new StringBuilder();
                    var sqlFiles = Directory.EnumerateFiles(args[1], "*.sql").ToList();
                    var outputFileName = args[2];

                    Console.WriteLine($"Found the following SQL files: {string.Join(", ", sqlFiles)}");

                    foreach (var sqlFile in sqlFiles.OrderBy(f => f))
                    {
                        var content = await File.ReadAllTextAsync(sqlFile);
                        sb.Append(content);
                        sb.Append(Environment.NewLine);
                    }

                    if (sb.Length > 0)
                    {
                        if (File.Exists(outputFileName))
                        {
                            File.Delete(outputFileName);
                        }

                        using (var outputFile = new StreamWriter(outputFileName))
                        {
                            await outputFile.WriteAsync(sb.ToString());
                            Console.WriteLine($"Created the combined sql file: {outputFileName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }
        }
    }
}
