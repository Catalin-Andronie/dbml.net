namespace DbmlNet.Web.Application.Common;

/// <summary>
/// Represents a validation error.
/// </summary>
public sealed class ValidationError
{
    /// <summary>
    /// Gets the property name.
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public required string ErrorMessage { get; init; }
}
