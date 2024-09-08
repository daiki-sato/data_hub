using DataHubIntern.MinimumApi.ApiClient;
using DataHubIntern.MinimumApi.Repository;
using DataHubIntern.Shared.File;
using DataHubIntern.Shared.Identity;

namespace DataHubIntern.MinimumApi.Services;

public class IdentityService(IIdentityRepository identityRepository, IIdentifyApiClient identifyApiClient)
    : IIdentityService
{
    public async Task<ListResponse> ListAsync(CancellationToken cancellationToken = default)
    {
        var storedIdentifiedData = await identityRepository.ListAsync(cancellationToken);
        return new ListResponse { Status = "OK", Identities = storedIdentifiedData };
    }

    public async Task<ListBySocResponse> ListBySocAsync(ListBySocRequest request,
        CancellationToken cancellationToken = default)
    {
        var identities = await identityRepository.ListAsync(cancellationToken);

        var identitiesBySoc = identities.Where(x => x.Soc == request.UniqueKey).ToList();

        if (identitiesBySoc.Count == 0)
            return new ListBySocResponse { Status = "Not Found" };

        return new ListBySocResponse { Status = "OK", Identities = identitiesBySoc };
    }

    public async Task<Identity> IdentifyAsync(RawData record, CancellationToken cancellationToken)
    {
        var soc = await identifyApiClient.IdentifyAsync(record);

        return new Identity
        {
            Id = record.Id,
            OrganizationName = record.OrganizationName,
            ZipCode = record.ZipCode,
            Address = record.Address,
            Tel = record.Tel,
            Fax = record.Fax,
            Email = record.Email,
            FirstName = record.FirstName,
            LastName = record.LastName,
            FullName = record.FullName,
            Mobile = record.Mobile,
            Url = record.Url,
            Division = record.Division,
            Position = record.Position,
            DataCreatedAt = record.CreatedAt,
            DataUpdatedAt = record.UpdatedAt,
            Soc = soc.Soc,
        };
    }
}
