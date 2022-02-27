using Ledger8.Common;
using Ledger8.DataAccess.Entities;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.Data;

namespace Ledger8.DataAccess;

public class LedgerContext : DbContext
{
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;

#nullable disable

    #region DbSets

    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<AccountNumberEntity> AccountNumbers { get; set; }
    public DbSet<AccountTypeEntity> AccountTypes { get; set; }
    public DbSet<AllotmentEntity> Allotments { get; set; }
    public DbSet<CompanyEntity> Companies { get; set; }
    public DbSet<IdentityEntity> Identities { get; set; }
    public DbSet<PoolEntity> Pools { get; set; }
    public DbSet<SettingsEntity> Settings { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }

    #endregion

    #region Constructors

    public LedgerContext(IConfiguration configuration, DbContextOptions<LedgerContext> options) : base(options)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString(Constants.ConnectionStringName);
    }

    public LedgerContext(IConfiguration configuration) : base()
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString(Constants.ConnectionStringName);
    }

    public LedgerContext(DbContextOptions<LedgerContext> options) : base(options)
    {
        _configuration = new ConfigurationFactory().Create(Constants.ConfigurationFilename, false);
        _connectionString = _configuration.GetConnectionString(Constants.ConnectionStringName);
    }

    public LedgerContext() : base()
    {
        _configuration = new ConfigurationFactory().Create(Constants.ConfigurationFilename, false);
        _connectionString = _configuration.GetConnectionString(Constants.ConnectionStringName);
    }

    #endregion

#nullable enable

    #region Public methods / properties

    public SettingsEntity? GetSettings => Settings.AsNoTracking().SingleOrDefault();

    public void Seed()
    {
        if (GetSettings is null)
        {
            Settings.Add(SettingsEntity.Default);
            SaveChanges();
        }
    }

    public DatabaseInfo DatabaseInfo()
    {
        var conn = Database.GetDbConnection();
        if (conn is not SqlConnection connection)
        {
            return new();
        }
        using var command = new SqlCommand("sp_spaceused")
        {
            CommandType = CommandType.StoredProcedure,
            Connection = connection
        };
        using var adapter = new SqlDataAdapter(command);
        var dataset = new DataSet();
        connection.Open();
        adapter.Fill(dataset);
        connection.Close();
        return new(dataset);
    }

    public bool? DatabaseExists(string databaseName)
    {
        using var connection = Database.GetDbConnection() as SqlConnection;
        if (connection is null)
        {
            return null;
        }
        using var command = new SqlCommand("select dbid from master.dbo.sysdatabases where name = @n;")
        {
            CommandType = CommandType.Text,
            Connection = connection
        };
        command.Parameters.Add(new SqlParameter("n", databaseName));
        connection.Open();
        var resultObject = command.ExecuteScalar();
        connection.Close();
        if (resultObject is not short dbid)
        {
            return null;
        }
        return dbid != 0;
    }

    public void Backup(string filename)
    {
        if (Database.GetDbConnection() is not SqlConnection connection)
        {
            return;
        }
        using var command = new SqlCommand("backup database @n to disk = @l with init;")
        {
            CommandType = CommandType.Text,
            Connection = connection
        };
        var dbname = connection.Database;
        command.Parameters.Add(new SqlParameter { ParameterName = "n", SqlDbType = SqlDbType.NVarChar, Value = dbname });
        command.Parameters.Add(new SqlParameter { ParameterName = "l", SqlDbType = SqlDbType.NVarChar, Value = filename });
        connection.Open();
        try
        {
            command.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    #endregion

    #region Overrides

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        builder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AccountEntity>().HasIndex(x => x.CompanyId).IsClustered(false);
        builder.Entity<AccountEntity>().HasIndex(x => x.AccountTypeId).IsClustered(false);
        builder.Entity<AccountEntity>().HasIndex(x => x.Tag).IsClustered(false);
        builder.Entity<AccountEntity>().Property(x => x.ClosedDate).HasColumnType(Constants.DateTime);
        builder.Entity<AccountEntity>().HasOne(x => x.AccountType);

        builder.Entity<AccountNumberEntity>().HasIndex(x => x.AccountId).IsClustered(false);
        builder.Entity<AccountNumberEntity>().HasIndex(x => x.StartDate).IsClustered(false);
        builder.Entity<AccountNumberEntity>().HasIndex(x => x.StopDate).IsClustered(false);
        builder.Entity<AccountNumberEntity>().Property(x => x.StartDate).HasColumnType(Constants.DateTime);
        builder.Entity<AccountNumberEntity>().Property(x => x.StopDate).HasColumnType(Constants.DateTime);
        builder.Entity<AccountNumberEntity>().Property(x => x.Salt).HasColumnType(Constants.VarBinary64);

        builder.Entity<AccountTypeEntity>().HasIndex(x => x.Description).IsUnique().IsClustered(false);

        builder.Entity<AllotmentEntity>().HasIndex(x => x.PoolId).IsClustered(false);
        builder.Entity<AllotmentEntity>().HasIndex(x => x.CompanyId).IsClustered(false);
        builder.Entity<AllotmentEntity>().HasOne(x => x.Company);
        builder.Entity<AllotmentEntity>().Property(x => x.Date).HasColumnType(Constants.DateTime);
        builder.Entity<AllotmentEntity>().Property(x => x.Amount).HasColumnType(Constants.MoneyFormat);

        builder.Entity<CompanyEntity>().HasIndex(x => x.Name).IsUnique().IsClustered(false);

        builder.Entity<IdentityEntity>().HasIndex(x => x.CompanyId).IsClustered(false);
        builder.Entity<IdentityEntity>().HasIndex(x => x.Tag).IsClustered(false);
        builder.Entity<IdentityEntity>().HasOne(x => x.Company);
        builder.Entity<IdentityEntity>().Property(x => x.UserSalt).HasColumnType(Constants.VarBinary64);
        builder.Entity<IdentityEntity>().Property(x => x.PasswordSalt).HasColumnType(Constants.VarBinary64);

        builder.Entity<PoolEntity>().HasIndex(x => x.Name).IsUnique().IsClustered(false);
        builder.Entity<PoolEntity>().Property(x => x.Date).HasColumnType(Constants.Date);
        builder.Entity<PoolEntity>().Property(x => x.Amount).HasColumnType(Constants.MoneyFormat);
        builder.Entity<PoolEntity>().Property(x => x.Balance).HasColumnType(Constants.MoneyFormat);

        builder.Entity<SettingsEntity>().HasKey(x => new { x.Lock, x.SystemId });
        builder.Entity<SettingsEntity>().HasIndex(x => x.Lock).IsUnique().IsClustered(false);
        builder.Entity<SettingsEntity>().HasCheckConstraint("SettingsCheck", "[Lock] = 1");
        builder.Entity<SettingsEntity>().HasIndex(x => x.SystemId).IsUnique().IsClustered(false);
        builder.Entity<SettingsEntity>().Property(x => x.Salt).HasColumnType(Constants.VarBinary64);
        builder.Entity<SettingsEntity>().Property(x => x.Hash).HasColumnType(Constants.VarBinary64);

        builder.Entity<TransactionEntity>().HasIndex(x => x.AccountId).IsClustered(false);
        builder.Entity<TransactionEntity>().Property(x => x.Date).HasColumnType(Constants.Date);
        builder.Entity<TransactionEntity>().Property(x => x.Balance).HasColumnType(Constants.MoneyFormat);
        builder.Entity<TransactionEntity>().Property(x => x.Payment).HasColumnType(Constants.MoneyFormat);
    }

    #endregion
}
