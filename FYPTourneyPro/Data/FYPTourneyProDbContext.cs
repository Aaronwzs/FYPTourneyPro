using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using FYPTourneyPro.Entities.Books;
using FYPTourneyPro.Entities.TodoList;
using FYPTourneyPro.Entities.DiscussionBoard;
using FYPTourneyPro.Entities.Organizer;
using System.Reflection.Emit;
using FYPTourneyPro.Entities.Chatroom;
using Volo.Abp.Identity;

using FYPTourneyPro.Entities.UserM;
using FYPTourneyPro.Entities.Notification;
using Volo.Abp.Domain.Entities;
using FYPTourneyPro.Entities.UserM;

namespace FYPTourneyPro.Data;

public class FYPTourneyProDbContext : AbpDbContext<FYPTourneyProDbContext>
{
    public DbSet<Book> Books { get; set; }

    public DbSet<Notification> Notification { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Post> Post { get; set; }
    public DbSet<PostVotes> postVotes { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Tournament> Tournament { get; set; }

    public DbSet<CategoryParticipant> CategoryParticipant { get; set; }

    //Match
    public DbSet<MatchParticipant> MatchParticipant { get; set; }
    public DbSet<Match> Match { get; set; }

    public DbSet<Registration> Registration { get; set; }
    public DbSet<Participant> Participant { get; set; }
    public DbSet<MatchScore> MatchScore { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatRoomParticipant> ChatRoomParticipants { get; set; }
    public DbSet<IdentityUser> Users { get; set; } // DbSet for IdentityUser if required

    //Custom Register
    public DbSet<CustomUser> User { get; set; }

    //Wallet
    public DbSet<Wallet> Wallet { get; set; }

    public DbSet<AppForm> appForms { get; set; }

    public const string DbTablePrefix = "App";
    public const string DbSchema = null;

    public FYPTourneyProDbContext(DbContextOptions<FYPTourneyProDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigurePermissionManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        
        builder.Entity<Book>(b =>
        {
            b.ToTable(DbTablePrefix + "Books",
                DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        });

        /* Configure your own entities here */

        builder.Entity<TodoItem>(b =>
        {
            b.ToTable("TodoItems");
        });

        builder.Entity<Notification>(b =>
        {
            b.ToTable("Notifications");
            b.Property(n => n.IsRead).HasDefaultValue(false);
        });

        builder.Entity<Post>(b =>
        {
            b.ToTable("Posts");
            b.ConfigureByConvention();
            b.HasMany(p => p.Comments)
             .WithOne(c => c.Post)
             .HasForeignKey(c => c.PostId);
        });

        builder.Entity<Comment>(b =>
        {
            b.ToTable("Comments");
            b.ConfigureByConvention();
            b.HasOne(c => c.Post)
             .WithMany(p => p.Comments)
             .HasForeignKey(c => c.PostId);
        });

        builder.Entity<PostVotes>(b =>
        {
            b.ToTable("PostVotes"); // Table name in the database

            b.HasKey(pv => pv.Id); // Primary key

            // Foreign key to Posts
            b.HasOne<Post>() // Relationship with Post entity
             .WithMany() // A Post can have many votes
             .HasForeignKey(pv => pv.PostId) // Foreign key property
             .IsRequired(); // PostId must not be null

            // Configure VoteType property
            b.Property(pv => pv.VoteType)
             .IsRequired() // Required field
             .HasMaxLength(10) // Max length of 10 characters
             .HasComment("Allowed values: 'Upvote' or 'Downvote'");
        });

        builder.Entity<TodoItem>(b =>
        {
            b.ToTable("TodoItems");
        });

        builder.Entity<Category>(b =>
        {
            b.ToTable("Category");
        });
        builder.Entity<Tournament>(b =>
        {
            b.ToTable("Tournament");
        });

        builder.Entity<CategoryParticipant>(b =>
        {
            b.ToTable("CategoryParticipant");
        });

        builder.Entity<MatchParticipant>(b =>
        {
            b.ToTable("MatchParticipant");
        });

        builder.Entity<Match>(b =>
        {
            b.ToTable("Match");
        });

        builder.Entity<Registration>(b =>
        {
            b.ToTable("Registration");
        });

        builder.Entity<Participant>(b =>
        {
            b.ToTable("Participant");
        });

        builder.Entity<MatchScore>(b =>
        {
            b.ToTable("MatchScore");
        });


        builder.Entity<Wallet>(b =>
        {
            b.ToTable("Wallets");

        });

        builder.Entity<ChatRoom>(b =>
        {
            b.ToTable("ChatRooms"); // Set the table name
            b.HasMany(c => c.Messages)
             .WithOne(m => m.ChatRoom)
             .HasForeignKey(m => m.ChatRoomId)
             .OnDelete(DeleteBehavior.Cascade); // Setting up relationship
        });

        builder.Entity<ChatMessage>(b =>
        {
            b.ToTable("ChatMessages"); // Set the table name
        });

        builder.Entity<ChatRoomParticipant>(b =>
        {
            b.ToTable("ChatRoomParticipants"); // Set the table name
            b.HasOne(c => c.ChatRoom)
             .WithMany() // You could add a collection in ChatRoom if needed
             .HasForeignKey(c => c.ChatRoomId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(c => c.User)
             .WithMany() // If your IdentityUser class has a collection, reference it here
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        //Custom Register
        builder.Entity<CustomUser>(b =>
        {
            b.ToTable("Users");

        });
        builder.Entity<ChatRoom>(b =>
        {
            b.ToTable("ChatRooms"); // Set the table name
            b.HasMany(c => c.Messages)
             .WithOne(m => m.ChatRoom)
             .HasForeignKey(m => m.ChatRoomId)
             .OnDelete(DeleteBehavior.Cascade); // Setting up relationship
        });

        builder.Entity<ChatMessage>(b =>
        {
            b.ToTable("ChatMessages"); // Set the table name
        });



        builder.Entity<ChatRoomParticipant>(b =>
        {
            b.ToTable("ChatRoomParticipants"); // Set the table name
            b.HasOne(c => c.ChatRoom)
             .WithMany() // You could add a collection in ChatRoom if needed
             .HasForeignKey(c => c.ChatRoomId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(c => c.User)
             .WithMany() // If your IdentityUser class has a collection, reference it here
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        //Custom Register
        builder.Entity<CustomUser>(b =>
        {
            b.ToTable("Users");

        });

        builder.Entity<Wallet>(b =>
        {
            b.ToTable("Wallets");

        });
        builder.Entity<AppForm>(b =>
        {
            b.ToTable("AppForms");

        });
    }
}

