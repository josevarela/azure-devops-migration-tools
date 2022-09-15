using System.IO;
using System.Threading.Tasks;
using MigrationTools;
using MigrationTools.Host;

namespace VstsSyncMigrator.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (File.Exists(".\\AreaPathsAndIterations.csv"))
            {
                System.IO.File.Delete(".\\AreaPathsAndIterations.csv");
            }
            if (File.Exists(".\\MappedAreaPathsAndIterations.csv"))
            {
                System.IO.File.Delete(".\\MappedAreaPathsAndIterations.csv");
            }
            var hostBuilder = MigrationToolHost.CreateDefaultBuilder(args);
            if(hostBuilder is null)
            {
                return;
            }

            hostBuilder
                .ConfigureServices((context, services) =>
                {
                    // New v2 Architecture fpr testing
                    services.AddMigrationToolServicesForClientFileSystem();
                    services.AddMigrationToolServicesForClientAzureDevOpsObjectModel(context.Configuration);
                    services.AddMigrationToolServicesForClientAzureDevopsRest(context.Configuration);

                    // v1 Architecture (Legacy)
                    services.AddMigrationToolServicesForClientLegacyAzureDevOpsObjectModel();
                    services.AddMigrationToolServicesForClientLegacyCore();
                });

            await hostBuilder.RunMigrationTools(args);
        }
    }
}