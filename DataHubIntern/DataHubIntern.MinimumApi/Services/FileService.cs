using System.Globalization;
using System.Net;
using System.Text;
using CsvHelper;
using DataHubIntern.MinimumApi.Repository;
using DataHubIntern.Shared.File;
using DataHubIntern.Shared.Identity;

namespace DataHubIntern.MinimumApi.Services;

public class FileService(IIdentityRepository identityRepository, IIdentityService identityService)
    : IFileService
{
    public async Task<UploadResponse> UploadAsync(IAsyncEnumerable<UploadRequest> request, CancellationToken cancellationToken = default)
    {
        await using var ms = new MemoryStream();

        await foreach (var message in request.WithCancellation(cancellationToken))
            await ms.WriteAsync(message.ChunkData, cancellationToken);
        ms.Seek(0, SeekOrigin.Begin);

        using var fs = new StreamReader(ms, Encoding.UTF8);
        using var csv = new CsvReader(fs, CultureInfo.InvariantCulture);

        var line = 0;
        var successes = new List<string>();
        var errors = new Dictionary<int, List<string>>();

        var records = await csv.GetRecordsAsync<RawData>(cancellationToken).ToListAsync(cancellationToken);

        // Start here !

        foreach (var record in records)
        {
            line++;

            // バリデーションを実行
            if (!TryValidate(record, out var validationErrors))
            {
                errors.Add(line, validationErrors);
                continue;
            }

            try
            {
                var identity = await identityService.IdentifyAsync(record, cancellationToken);

                // Idで既存のレコードを検索
                var existingIdentity = await identityRepository.GetAsync(identity.Id, cancellationToken);

                if (existingIdentity != null)
                {
                    // レコードが存在すれば更新
                    await identityRepository.UpdateAsync(identity, cancellationToken);
                    successes.Add($"Line {line}: Successfully updated record with ID {identity.Id}");
                }
                else
                {
                    // レコードが存在しなければ新規作成
                    await identityRepository.CreateAsync(identity, cancellationToken);
                    successes.Add($"Line {line}: Successfully created record with ID {identity.Id}");
                }
            }
            catch (Exception ex)
            {
                // 例外が発生した場合、そのエラーを収集
                errors.Add(line, new List<string> { ex.Message });
            }
        }



        return new UploadResponse { StatusCode = HttpStatusCode.MultiStatus, Successes = successes, Errors = errors };
    }

    private static bool TryValidate(RawData record, out List<string> errors)
    {
        errors = new List<string>();

        // 1. OrganizationName と Email の両方が空の場合
        if (string.IsNullOrEmpty(record.OrganizationName) && string.IsNullOrEmpty(record.Email))
        {
            errors.Add("会社名とメールアドレスが両方とも空の場合、識別ができません。");
        }

        // 2. Email の形式が不正な場合
        if (!string.IsNullOrEmpty(record.Email))
        {
            var emailParts = record.Email.Split('@');
            if (emailParts.Length != 2 || !emailParts[1].Contains('.') || emailParts[1].EndsWith('.'))
            {
                errors.Add("メールアドレスの形式が不正な場合、識別ができません。");
            }
        }

        // 3. CreatedAt または UpdatedAt が空の場合
        if (record.CreatedAt == "" || record.UpdatedAt == "")
        {
            errors.Add("作成日時もしくは更新日時が空の場合、識別ができません。");
        }

        // エラーがある場合は false を返す
        return !errors.Any();
    }
}
