using Autofac;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MusicTagger.Auth;
using MusicTagger.Database.Migrations;
using MusicTagger.Database.Tag;
using MusicTagger.Database.TrackTag;
using MusicTagger.Database.User;

namespace MusicTagger;

public class Startup
{
    private const string DATABASE_CONNECTION_STRING = "ConnectionStrings:MusicTagger";
    private const string TOKEN_EXPIRATION_HOURS = "Auth:TokenExpirationHours";
    private const string TOKEN_ISSUER = "Auth:TokenIssuer";

    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = _configuration[TOKEN_ISSUER],
                    ValidIssuer = _configuration[TOKEN_ISSUER],
                    IssuerSigningKey = JwtSigningKey.GetKey(_configuration)
                };
            });
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(_configuration[DATABASE_CONNECTION_STRING])
                .ScanIn(typeof(InitializeDatabase).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        // Auth
        builder.RegisterType<TokenGenerator>().As<ITokenGenerator>()
            .SingleInstance()
            .WithParameter("tokenExpirationHours", _configuration[TOKEN_EXPIRATION_HOURS])
            .WithParameter("tokenIssuer", _configuration[TOKEN_ISSUER]);

        builder.Register(c => new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false })
            .AsSelf()
            .SingleInstance();
        builder.Register(_ => new SigningCredentials(
            JwtSigningKey.GetKey(_configuration),
            SecurityAlgorithms.HmacSha256))
            .As<SigningCredentials>()
            .SingleInstance();

        // Database
        builder.RegisterType<DatabaseUserProvider>().As<IUserProvider>()
            .SingleInstance()
            .WithParameter("connectionString", _configuration[DATABASE_CONNECTION_STRING]);
        builder.RegisterType<DatabaseUserWriter>().As<IUserWriter>()
            .SingleInstance()
            .WithParameter("connectionString", _configuration[DATABASE_CONNECTION_STRING]);

        builder.RegisterType<DatabaseTagProvider>().As<ITagProvider>()
            .SingleInstance()
            .WithParameter("connectionString", _configuration[DATABASE_CONNECTION_STRING]);
        builder.RegisterType<DatabaseTagWriter>().As<ITagWriter>()
            .SingleInstance()
            .WithParameter("connectionString", _configuration[DATABASE_CONNECTION_STRING]);

        builder.RegisterType<DatabaseTrackTagProvider>().As<ITrackTagProvider>()
            .SingleInstance()
            .WithParameter("connectionString", _configuration[DATABASE_CONNECTION_STRING]);
        builder.RegisterType<DatabaseTrackTagWriter>().As<ITrackTagWriter>()
            .SingleInstance()
            .WithParameter("connectionString", _configuration[DATABASE_CONNECTION_STRING]);

        // Database migrations
        builder.RegisterType<MigrationRunner>().As<IMigrationRunner>()
            .SingleInstance()
            .InstancePerDependency();
        builder.RegisterType<StartupMigrationRunner>().AsSelf()
            .AutoActivate();

#pragma warning restore CS8604 // Possible null reference argument.
    }
}
