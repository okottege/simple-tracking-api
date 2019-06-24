using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerService.BuildTools
{
    class Program
    {
        private const string SQL_SCRIPT_COMBINE = "sql-script-combine";
        private const string DB_UPGRADE = "database-upgrade";
        private const string ROOT_COMMAND_NAME = "tracksvc-util";

        static void  Main(string[] args)
        {
            Console.WriteLine("Tracker Service Build Tools started.");
            //Console.WriteLine($"Command Line arguments: {string.Join(", ", args)}");

            //if (args.Any())
            //{
            //    switch (args[0])
            //    {
            //        case SQL_SCRIPT_COMBINE:
            //            await CombineSqlScripts(args);
            //            break;
            //        case DB_UPGRADE:
            //            await UpgradeDatabase();
            //            break;
            //        default:
            //            Console.WriteLine($"Unsupported command '{args[0]}'");
            //            break;
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Command Line Arguments must be supplied.");
            //    Environment.Exit(-1);
            //}
            var rootCommand = new RootCommand
            {
                Description = "The tracking service command line utility.",
                Name = ROOT_COMMAND_NAME,
                TreatUnmatchedTokensAsErrors = true
            };
            rootCommand.AddCommand(GetCombineSqlScriptsRootCommand());

            rootCommand.InvokeAsync(args).Wait();
        }

        private static void CombineSqlScripts(string sqlFileFolder, string outputFileName)
        {
            try
            {
                Console.WriteLine("Executing the combining of SQL scripts command.");
                var sb = new StringBuilder();
                var sqlFiles = Directory.EnumerateFiles(sqlFileFolder, "*.sql").ToList();

                Console.WriteLine($"Found the following SQL files: {string.Join(", ", sqlFiles)}");

                foreach (var sqlFile in sqlFiles.OrderBy(f => f))
                {
                    var content = File.ReadAllTextAsync(sqlFile).Result;
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
                        outputFile.WriteAsync(sb.ToString()).Wait();
                        Console.WriteLine($"Created the combined sql file: {outputFileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }
        }

        private static Command GetCombineSqlScriptsRootCommand()
        {
            var command = new Command(SQL_SCRIPT_COMBINE)
            {
                Description = "Combines sql files in a folder into a single file.",
                TreatUnmatchedTokensAsErrors = true
            };
            var optSqlFileFolder = new Option("--sql-file-folder", "The path to the folder containing all the sql files.");
            optSqlFileFolder.AddAlias("-sff");
            optSqlFileFolder.Argument = new Argument<string>();
            var optOutputPath = new Option("--output", "The output file name of the combined sql script.");
            optOutputPath.AddAlias("-o");
            optOutputPath.Argument = new Argument<string>();
            command.AddOption(optSqlFileFolder);
            command.AddOption(optOutputPath);
            command.Handler = CommandHandler.Create(new Action<string, string>(CombineSqlScripts));

            return command;
        }
    }
}
