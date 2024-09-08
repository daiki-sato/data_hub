using System.Text;

namespace DataHubIntern.Client.Services;

public interface ICsvValidator
{
    ValidationResult ValidateHeaders(List<string> headers);
}

public class CsvHeaderValidator : ICsvValidator
{
    private readonly List<string> _requiredHeaders = new()
    {
        "Id", "OrganizationName", "CreatedAt", "UpdatedAt"
    };

    public ValidationResult ValidateHeaders(List<string> headers)
    {
        var missingHeaders = _requiredHeaders.Except(headers).ToList();

        var isValid = !missingHeaders.Any();

        if (isValid)
        {
            return new ValidationResult(isValid, null);
        }

        var errorMessage = new StringBuilder();
        errorMessage.AppendLine("ファイルのインポートに失敗しました。ファイルを修正のうえ、再度インポートしてください。\n");

        if (!missingHeaders.Any()) return new ValidationResult(isValid, errorMessage.ToString());
        errorMessage.AppendLine("不足しているヘッダー:");

        foreach (var header in missingHeaders)
        {
            errorMessage.AppendLine(header);
        }

        return new ValidationResult(isValid, errorMessage.ToString());
    }
}

public record ValidationResult(bool IsValid, string? ErrorMessage);
