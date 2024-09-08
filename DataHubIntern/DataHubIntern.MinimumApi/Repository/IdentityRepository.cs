using Dapper;
using DataHubIntern.Db;
using Microsoft.EntityFrameworkCore;
using Identity = DataHubIntern.Shared.Identity.Identity;

namespace DataHubIntern.MinimumApi.Repository;

public interface IIdentityRepository
{
    Task<List<Identity>> ListAsync(CancellationToken cancellationToken = default);

    Task<Identity?> GetAsync(string recordId, CancellationToken cancellationToken = default);

    Task CreateAsync(Identity record, CancellationToken cancellationToken = default);

    Task UpdateAsync(Identity record, CancellationToken cancellationToken = default);
}

public class IdentityRepository : IIdentityRepository
{
    private readonly InternDbContext _dbContext;

    public IdentityRepository(InternDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Identity>> ListAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        var result = await connection.QueryAsync<Identity>(@"
SELECT *
FROM Identity
");

        return result.ToList();
    }

    public async Task<Identity?> GetAsync(string recordId, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        IEnumerable<Identity?> result = await connection.QueryAsync<Identity>(@"
SELECT *
FROM Identity
WHERE
    Id = @Id
", new
        {
            Id = recordId
        });

        return result.FirstOrDefault();
    }

    public async Task CreateAsync(Identity record, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        const string insertSql = @"INSERT INTO Identity
    (Id, OrganizationName, ZipCode, Address, Tel, Fax, Email, FirstName, LastName, FullName, Mobile, Url, Division, Position, DataCreatedAt, DataUpdatedAt, Soc) 
VALUES 
    (@Id, @OrganizationName, @ZipCode, @Address, @Tel, @Fax, @Email, @FirstName, @LastName, @FullName, @Mobile, @Url, @Division, @Position, @DataCreatedAt, @DataUpdatedAt, @Soc)";

        var parameters = new
        {
            record.Id,
            record.OrganizationName,
            record.ZipCode,
            record.Address,
            record.Tel,
            record.Fax,
            record.Email,
            record.FirstName,
            record.LastName,
            record.FullName,
            record.Mobile,
            record.Url,
            record.Division,
            record.Position,
            record.DataCreatedAt,
            record.DataUpdatedAt,
            record.Soc
        };

        await connection.ExecuteAsync(insertSql, parameters);
    }

    public async Task UpdateAsync(Identity record, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        const string updateSql = @"UPDATE Identity 
        SET OrganizationName = @OrganizationName, ZipCode = @ZipCode, Address = @Address, Tel = @Tel, Fax = @Fax, Email = @Email, 
            FirstName = @FirstName, LastName = @LastName, FullName = @FullName, Mobile = @Mobile, Url = @Url, Division = @Division, 
            Position = @Position, DataCreatedAt = @DataCreatedAt, DataUpdatedAt = @DataUpdatedAt, Soc = @Soc
        WHERE Id = @Id";

        var parameters = new
        {
            record.Id,
            record.OrganizationName,
            record.ZipCode,
            record.Address,
            record.Tel,
            record.Fax,
            record.Email,
            record.FirstName,
            record.LastName,
            record.FullName,
            record.Mobile,
            record.Url,
            record.Division,
            record.Position,
            record.DataCreatedAt,
            record.DataUpdatedAt,
            record.Soc
        };

        await connection.ExecuteAsync(updateSql, parameters);
    }
}