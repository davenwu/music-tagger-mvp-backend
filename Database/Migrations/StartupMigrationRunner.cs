using Autofac;
using FluentMigrator.Runner;

namespace MusicTagger.Database.Migrations;

public class StartupMigrationRunner
{
    public StartupMigrationRunner(ILifetimeScope container)
    {
        using var scope = container.BeginLifetimeScope();

        var runner = scope.Resolve<IMigrationRunner>();

        runner.MigrateUp();
    }
}
