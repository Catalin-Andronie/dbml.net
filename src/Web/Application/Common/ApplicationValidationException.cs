using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DbmlNet.Web.Application.Common;

#pragma warning disable CA1032 // Implement standard exception constructors.
#pragma warning disable RCS1194 // Implement exception constructors.

public class ApplicationValidationException : Exception
{
    public ApplicationValidationException()
        : base("One or more validation failures have occurred.")
    {
    }

    public ApplicationValidationException(IEnumerable<ValidationError> failures)
        : this()
    {
        IEnumerable<IGrouping<string, string>> failureGroups =
            failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage, StringComparer.InvariantCulture);

        ImmutableDictionary<string, string[]>.Builder? errors =
            ImmutableDictionary.CreateBuilder<string, string[]>();

        foreach (IGrouping<string, string> failureGroup in failureGroups)
        {
            string propertyName = failureGroup.Key;
            string[] propertyFailures = failureGroup.ToArray();

            errors.Add(propertyName, propertyFailures);
        }

        Errors = errors.ToImmutable();
    }

    public ImmutableDictionary<string, string[]> Errors { get; } = ImmutableDictionary<string, string[]>.Empty;
}
