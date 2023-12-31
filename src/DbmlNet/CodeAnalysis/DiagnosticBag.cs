using System.Collections;
using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis;

internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new();

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddRange(IEnumerable<Diagnostic> diagnostics)
    {
        _diagnostics.AddRange(diagnostics);
    }

    public void ReportBadCharacter(TextLocation location, char character)
    {
        string message = $"Bad character input: '{character}'.";
        ReportError(location, message);
    }

    public void ReportUnterminatedString(TextLocation location)
    {
        const string Message = "Unterminated string literal.";
        ReportError(location, Message);
    }

    public void ReportUnterminatedMultiLineString(TextLocation location)
    {
        const string message = "Unterminated multi-line string literal.";
        ReportError(location, message);
    }

    public void ReportUnrecognizedEscapeSequence(TextLocation location)
    {
        const string message = "Unrecognized escape sequence.";
        ReportError(location, message);
    }

    public void ReportDisallowedColumnSettingDefaultValue(TextLocation location, SyntaxKind expressionKind)
    {
        string message = $"Disallowed 'default' column setting value expression '{expressionKind}'.";
        ReportError(location, message);
    }

    public void ReportUnterminatedMultiLineComment(TextLocation location)
    {
        const string message = "Unterminated multi-line comment.";
        ReportError(location, message);
    }

    public void ReportNumberToLarge(TextLocation location, string numberText)
    {
        string message = $"The number '{numberText}' is too large.";
        ReportError(location, message);
    }

    public void ReportUnexpectedToken(TextLocation location, SyntaxKind currentKind, SyntaxKind expectedKind)
    {
        string message = $"Unexpected token <{currentKind}>, expected <{expectedKind}>.";
        ReportError(location, message);
    }

    public void ReportUnknownEnumEntrySetting(TextLocation location, string settingName)
    {
        string message = $"Unknown enum entry setting '{settingName}'.";
        ReportWarning(location, message);
    }

    public void ReportUnknownColumnSetting(TextLocation location, string settingName)
    {
        string message = $"Unknown column setting '{settingName}'.";
        ReportWarning(location, message);
    }

    public void ReportUnknownProjectSetting(TextLocation location, string settingName)
    {
        string message = $"Unknown project setting '{settingName}'.";
        ReportWarning(location, message);
    }

    public void ReportDuplicateTableName(TextLocation location, string columnName)
    {
        string message = $"Table '{columnName}' already declared.";
        ReportWarning(location, message);
    }

    public void ReportDuplicateColumnName(TextLocation location, string columnName)
    {
        string message = $"Column '{columnName}' already declared.";
        ReportWarning(location, message);
    }

    public void ReportDuplicateEnumEntryName(TextLocation location, string enumEntryName)
    {
        string message = $"Enum entry '{enumEntryName}' already declared.";
        ReportWarning(location, message);
    }

    public void ReportDuplicateEnumEntrySettingName(TextLocation location, string settingName)
    {
        string message = $"Enum entry setting '{settingName}' already declared.";
        ReportWarning(location, message);
    }

    public void ReportDuplicateColumnSettingName(TextLocation location, string settingName)
    {
        string message = $"Column setting '{settingName}' already declared.";
        ReportWarning(location, message);
    }

    public void ReportDuplicateIndexSettingName(TextLocation location, string settingName)
    {
        string message = $"Index setting '{settingName}' already declared.";
        ReportWarning(location, message);
    }

    public void ReportUnknownIndexSetting(TextLocation location, string settingName)
    {
        string message = $"Unknown index setting '{settingName}'.";
        ReportWarning(location, message);
    }

    public void ReportUnknownIndexSettingType(TextLocation location, string unknownType)
    {
        string message = $"Unknown index setting type '{unknownType}'. Allowed index types [{string.Join("|", Parser.IndexSettingTypes)}].";
        ReportWarning(location, message);
    }

    private void ReportError(TextLocation location, string message)
    {
        Diagnostic diagnostic = Diagnostic.Error(location, message);
        _diagnostics.Add(diagnostic);
    }

    private void ReportWarning(TextLocation location, string message)
    {
        Diagnostic diagnostic = Diagnostic.Warning(location, message);
        _diagnostics.Add(diagnostic);
    }
}
