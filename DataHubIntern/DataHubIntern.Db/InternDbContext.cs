using Microsoft.EntityFrameworkCore;

namespace DataHubIntern.Db;

public class InternDbContext : DbContext
{
    public DbSet<Identity> Identity { get; set; }

    //Sample テーブルを追加する場合
    //public DbSet<SampleEntity> Sample { get; set; }

    public InternDbContext()
    {
    }

    public InternDbContext(DbContextOptions<InternDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Primary Key の指定方法
        modelBuilder.Entity<Identity>().HasKey(x => x.Id);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "../db_volume", "identifiedData.db");
            optionsBuilder.UseSqlite(@$"Data Source={path}");
        }
        base.OnConfiguring(optionsBuilder);
    }
}