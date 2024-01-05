using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_DatabaseProviderProjectSettingClause_With_Identifier_Value()
    {
        const SyntaxKind providerKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
        string providerText = $"{randomText}";
        object? providerValue = null;
        string settingText = $"database_type: {providerText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax settings = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new(settings);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.DatabaseProviderProjectSettingClause);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_DatabaseProviderProjectSettingClause_With_Keyword_Value(
        SyntaxKind providerKind,
        string providerText,
        object? providerValue)
    {
        string settingText = $"database_type: {providerText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax settings = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new(settings);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.DatabaseProviderProjectSettingClause);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }

    [Fact]
    public void Parse_DatabaseProviderProjectSettingClause_With_QuotationMarksString_Value()
    {
        const SyntaxKind providerKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string providerText = $"\"{randomText}\"";
        object? providerValue = randomText;
        string settingText = $"database_type: {providerText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax settings = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new(settings);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.DatabaseProviderProjectSettingClause);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }

    [Fact]
    public void Parse_DatabaseProviderProjectSettingClause_With_SingleQuotationMarksString_Value()
    {
        const SyntaxKind providerKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string providerText = $"\'{randomText}\'";
        object? providerValue = randomText;
        string settingText = $"database_type: {providerText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax settings = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new(settings);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.DatabaseProviderProjectSettingClause);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }

    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteProjectSettingClause_With_Identifier_Value(string noteKeywordText)
    {
        const SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
        string noteValueText = $"{randomText}";
        object? noteValue = null;
        string settingText = $"{noteKeywordText}: {noteValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.NoteProjectSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, noteValueText, noteValue);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_NoteProjectSettingClause_With_Keyword_Value(
        SyntaxKind settingValueKind,
        string settingValueText,
        object? settingValue)
    {
        string settingText = $"note: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.NoteProjectSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }

    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteProjectSettingClause_With_QuotationMarksString_Value(string noteKeywordText)
    {
        const SyntaxKind settingKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string noteValueText = $"\"{randomText}\"";
        object? noteValue = randomText;
        string settingText = $"{noteKeywordText}: {noteValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.NoteProjectSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, noteValueText, noteValue);
    }

    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteProjectSettingClause_With_SingleQuotationMarksString_Value(string noteKeywordText)
    {
        const SyntaxKind settingKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string noteValueText = $"\'{randomText}\'";
        object? noteValue = randomText;
        string settingText = $"{noteKeywordText}: {noteValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.NoteProjectSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, noteValueText, noteValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_SimpleSetting()
    {
        const SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingNameText + "}";
        string[] diagnosticMessages = new[]
        {
            $"Unknown project setting '{settingNameText}'.",
        };

        ProjectSettingListSyntax columnSettingListClause =
            ParseProjectSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingKind, settingNameText, settingValue);
    }

    public static IEnumerable<object[]?> GetUnknownProjectSettingNameAllowedKeywordsData()
    {
        foreach ((SyntaxKind itemKind, string itemText, object? itemValue) in DataGenerator.GetSyntaxKeywords())
        {
            bool skip = itemKind switch
            {
                SyntaxKind.NoteKeyword => true,
                SyntaxKind.DatabaseTypeKeyword => true,
                _ => false
            };

            if (!skip)
            {
                yield return new object[] { itemKind, itemText, itemValue! };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetUnknownProjectSettingNameAllowedKeywordsData))]
    public void Parse_UnknownProjectSettingClause_With_ComposedSetting_Name_Keyword(
        SyntaxKind settingNameKind,
        string settingNameText,
        object? settingNameValue)
    {
        const SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";
        string[] diagnosticMessages = new[]
        {
            $"Unknown project setting '{settingNameText}'.",
        };

        ProjectSettingListSyntax columnSettingListClause =
            ParseProjectSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_ComposedSetting_Identifier_Value()
    {
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";
        string[] diagnosticMessages = new[]
        {
            $"Unknown project setting '{settingNameText}'.",
        };

        ProjectSettingListSyntax columnSettingListClause =
            ParseProjectSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_UnknownProjectSettingClause_With_Composed_Setting_Keyword_Value(
        SyntaxKind settingValueKind,
        string settingValueText,
        object? settingValue)
    {
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";
        string[] diagnosticMessages = new[]
        {
            $"Unknown project setting '{settingNameText}'.",
        };

        ProjectSettingListSyntax columnSettingListClause =
            ParseProjectSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_Composed_Setting_QuotationMarksString_Value()
    {
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSettingValue = DataGenerator.CreateRandomMultiWordString();
        string settingValueText = $"\"{randomSettingValue}\"";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";
        string[] diagnosticMessages = new[]
        {
            $"Unknown project setting '{settingNameText}'.",
        };

        ProjectSettingListSyntax columnSettingListClause =
            ParseProjectSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_Composed_Setting_SingleQuotationMarksString_Value()
    {
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSettingValue = DataGenerator.CreateRandomMultiWordString();
        string settingValueText = $"\'{randomSettingValue}\'";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";
        string[] diagnosticMessages = new[]
        {
            $"Unknown project setting '{settingNameText}'.",
        };

        ProjectSettingListSyntax columnSettingListClause =
            ParseProjectSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }
}
