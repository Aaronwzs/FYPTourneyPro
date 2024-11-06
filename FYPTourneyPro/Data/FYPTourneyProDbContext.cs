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

namespace FYPTourneyPro.Data;

public class FYPTourneyProDbContext : AbpDbContext<FYPTourneyProDbContext>
{
    public DbSet<Book> Books { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }


    //organizer
    public DbSet<Category> Category { get; set; }
    public DbSet<Tournament> Tournament { get; set; }
    public DbSet<PlayerRegistration> PlayerRegistration { get; set; }
    public DbSet<CategoryParticipant> CategoryParticipant { get; set; }

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
        builder.Entity<PlayerRegistration>(b =>
        {
            b.ToTable("PlayerRegistration");
        });
        builder.Entity<CategoryParticipant>(b =>
        {
            b.ToTable("CategoryParticipant");
        });
    }
}

