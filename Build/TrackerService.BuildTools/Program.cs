using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

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

            var rootCommand = new RootCommand
            {
                Description = "The tracking service command line utility.",
                Name = ROOT_COMMAND_NAME,
                TreatUnmatchedTokensAsErrors = true
            };
            rootCommand.AddCommand(GetCombineSqlScriptsRootCommand());
            rootCommand.AddCommand(GetDatabaseUpgradeCommand());

            rootCommand.InvokeAsync(args).Wait();
        }

        private static void CombineSqlScripts(string sqlFileFolder, string outputFileName)
        {
            try
            {
                Console.WriteLine("Executing the combining of SQL scripts command.");
                Console.WriteLine($"sql file folder: {sqlFileFolder}, output file: {outputFileName}");
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

        private static void UpgradeDatabase(string sqlFileName)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            var config = builder.Build();
            var endpoint = config["KeyVaultEndpoint"];
            var clientId = config["KeyVaultClientId"];
            var secret = config["KeyVaultClientSecret"];

            Console.WriteLine($"endpoint: {endpoint}, client id: {clientId}, secret: {secret}");

            builder.AddAzureKeyVault(endpoint, clientId, secret);
            config = builder.Build();

            var dbConnectionString = config.GetConnectionString("SimpleTaxDB");
            var sql = File.ReadAllText(sqlFileName);
            var sqlStatements = sql.Split("GO", StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s));

            using (var conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(string.Empty, conn))
                {
                    foreach (var sqlStatement in sqlStatements)
                    {
                        Console.WriteLine($"Executing query: {sqlStatement}");
                        cmd.CommandText = sqlStatement;
                        cmd.ExecuteNonQuery();
                    }
                }
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
            var optOutputPath = new Option("--output-file-name", "The output file name of the combined sql script.");
            optOutputPath.AddAlias("-o");
            optOutputPath.Argument = new Argument<string>();
            command.AddOption(optSqlFileFolder);
            command.AddOption(optOutputPath);
            command.Handler = CommandHandler.Create(new Action<string, string>(CombineSqlScripts));

            return command;
        }

        private static Command GetDatabaseUpgradeCommand()
        {
            var command = new Command(DB_UPGRADE)
            {
                Description = "Upgrades the database using connection stored in Azure Key Vault",
                TreatUnmatchedTokensAsErrors = true,
                Handler = CommandHandler.Create(new Action<string>(UpgradeDatabase))
            };
            var optSqlFileName = new Option(new []{"--sql-file-name", "-sf"})
            {
                Description = "The path to the database upgrade sql file.",
                Argument = new Argument<string>()
            };
            command.AddOption(optSqlFileName);

            return command;
        }
    }
}
