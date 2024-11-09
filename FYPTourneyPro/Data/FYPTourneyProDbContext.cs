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
using FYPTourneyPro.Entities.Organizer;
using System.Reflection.Emit;

namespace FYPTourneyPro.Data;

public class FYPTourneyProDbContext : AbpDbContext<FYPTourneyProDbContext>
{
    public DbSet<Book> Books { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }


    //organizer
    public DbSet<Category> Category { get; set; }
    public DbSet<Tournament> Tournament { get; set; }
  
    public DbSet<CategoryParticipant> CategoryParticipant { get; set; }

    //Match
    public DbSet<MatchParticipant> MatchParticipant { get; set; }
    public DbSet<Match> Match { get; set; }

    public DbSet<Registration> Registration { get; set; }
    public DbSet<Participant> Participant { get; set; }

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


    }
}

