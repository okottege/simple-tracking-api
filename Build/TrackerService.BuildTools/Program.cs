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
            }

            Console.WriteLine("Build Tools finished execution.");
        }

        private static async Task CombineSqlScripts(string[] args)
        {
            try
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("Cannot combine sql scripts as source sql folder and destination sql file not provided.");
                }
                else
                {
                    var sb = new StringBuilder();

                    foreach (var sqlFile in Directory.EnumerateFiles(args[1], "*.sql").OrderBy(f => f))
                    {
                        var content = await File.ReadAllTextAsync(sqlFile);
                        sb.Append(content);
                        sb.Append(Environment.NewLine);
                    }

                    if (sb.Length > 0)
                    {
                        if (File.Exists(args[2]))
                        {
                            File.Delete(args[2]);
                        }

                        using (var outputFile = new StreamWriter(args[2]))
                        {
                            await outputFile.WriteAsync(sb.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
