using FluentMigrator;

namespace MusicTagger.Database.Migrations;

[Migration(1)]
public class InitializeDatabase : Migration
{
    // Users
    private const string USERS_TABLE_NAME = "users";

    // Tags
    private const string TAGS_TABLE_NAME = "tags";
    private const string UQ_TAGS_TAG_USER = "UQ_tags_tag_user";
    private const string FK_TAGS_USER_ID = "FK_tags_user_id";

    // Track tags
    private const string TRACK_TAGS_TABLE_NAME = "track_tags";
    private const string UQ_TRACK_TAGS_TAG_USER_SPOTIFY_TRACK = "UQ_track_tags_tag_user_spotify_track";
    private const string FK_TRACK_TAGS_TAG_ID = "FK_track_tags_tag_id";
    private const string FK_TRACK_TAGS_USER_ID = "FK_track_tags_user_id";

    public override void Up()
    {
        Create.Table("users")
            .WithColumn("user_id").AsInt32().PrimaryKey().Identity()
            .WithColumn("username").AsString(16).NotNullable().Unique()
            .WithColumn("password_hash").AsString(60).NotNullable();
        Create.Table("tags")
            .WithColumn("tag_id").AsInt64().PrimaryKey().Identity()
            .WithColumn("tag_name").AsString(16).NotNullable()
            .WithColumn("user_id").AsInt32().NotNullable();
        Create.Table("track_tags")
            .WithColumn("tag_id").AsInt64().NotNullable()
            .WithColumn("user_id").AsInt32().NotNullable()
            .WithColumn("spotify_track_id").AsString().NotNullable();

        // Tags
        Create.UniqueConstraint(UQ_TAGS_TAG_USER)
            .OnTable("tags")
            .Columns("tag_name", "user_id");
        Create.ForeignKey(FK_TAGS_USER_ID)
            .FromTable("tags").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("user_id");

        // Track tags
        Create.UniqueConstraint(UQ_TRACK_TAGS_TAG_USER_SPOTIFY_TRACK)
            .OnTable("track_tags")
            .Columns("tag_id", "user_id", "spotify_track_id");
        Create.ForeignKey(FK_TRACK_TAGS_TAG_ID)
            .FromTable("track_tags").ForeignColumn("tag_id")
            .ToTable("tags").PrimaryColumn("tag_id");
        Create.ForeignKey(FK_TRACK_TAGS_USER_ID)
            .FromTable("track_tags").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("user_id");

        Insert.IntoTable("users")
            .Row(new Dictionary<string, object>
            {
                {"username", "test" },
                {"password_hash", "blah"}
            });
        Insert.IntoTable("tags")
            .Row(new Dictionary<string, object>
            {
                {"tag_name", "happy"},
                {"user_id", 1}
            })
            .Row(new Dictionary<string, object>
            {
                {"tag_name", "sad"},
                {"user_id", 1}
            })
            .Row(new Dictionary<string, object>
            {
                {"tag_name", "electronic"},
                {"user_id", 1}
            })
            .Row(new Dictionary<string, object>
            {
                {"tag_name", "upbeat"},
                {"user_id", 1}
            })
            .Row(new Dictionary<string, object>
            {
                {"tag_name", "chill"},
                {"user_id", 1}
            });
    }

    public override void Down()
    {
        Delete.ForeignKey(FK_TRACK_TAGS_USER_ID).OnTable(TRACK_TAGS_TABLE_NAME);
        Delete.ForeignKey(FK_TRACK_TAGS_TAG_ID).OnTable(TRACK_TAGS_TABLE_NAME);
        Delete.UniqueConstraint(UQ_TRACK_TAGS_TAG_USER_SPOTIFY_TRACK).FromTable(TRACK_TAGS_TABLE_NAME);
        Delete.Table(TRACK_TAGS_TABLE_NAME);

        Delete.ForeignKey(FK_TAGS_USER_ID).OnTable(TAGS_TABLE_NAME);
        Delete.UniqueConstraint(UQ_TAGS_TAG_USER).FromTable(TAGS_TABLE_NAME);
        Delete.Table(TAGS_TABLE_NAME);

        Delete.Table(USERS_TABLE_NAME);
    }
}
