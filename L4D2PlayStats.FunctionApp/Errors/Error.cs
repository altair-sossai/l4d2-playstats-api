using System.Linq;
using FluentValidation.Results;
using L4D2PlayStats.Core.Infrastructure.Extensions;

namespace L4D2PlayStats.FunctionApp.Errors;

public class Error
{
    public Error(ValidationFailure failure)
    {
        Property = string.Join('.', failure.PropertyName.Split('.').Select(p => p.FirstLetterToLower()));
        PropertyName = failure.PropertyName;
        ErrorMessage = failure.ErrorMessage;

        var values = failure.FormattedMessagePlaceholderValues;

        if (values != null && values.ContainsKey("PropertyName") && values["PropertyName"] != null)
            PropertyName = values["PropertyName"].ToString() ?? PropertyName;
    }

    public string Property { get; }
    public string PropertyName { get; }
    public string ErrorMessage { get; }
}