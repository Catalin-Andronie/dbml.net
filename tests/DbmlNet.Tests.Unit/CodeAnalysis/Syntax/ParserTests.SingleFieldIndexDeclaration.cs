using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Identifier_Name()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        string indexText = $"{indexNameText}";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_SingleFieldIndexDeclaration_With_Keyword_Name(
        SyntaxKind indexNameKind,
        string indexNameText,
        object? indexNameValue)
    {
        string indexText = $"{indexNameText}";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact(Skip = "Skip to avoid infinite loop.")]
    public void Parse_SingleFieldIndexDeclaration_With_QuotationMarksString_Name()
    {
        const SyntaxKind indexNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string indexNameText = $"\"{randomText}\"";
        object? indexNameValue = randomText;
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact(Skip = "Skip to avoid infinite loop.")]
    public void Parse_SingleFieldIndexDeclaration_With_SingleQuotationMarksString_Name()
    {
        const SyntaxKind indexNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string indexNameText = $"\'{randomText}\'";
        object? indexNameValue = randomText;
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_No_Settings()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        string indexText = $"{indexNameText}";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Empty_Settings()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Unique_Setting()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ unique ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UniqueIndexSettingClause);
        e.AssertToken(SyntaxKind.UniqueKeyword, "unique");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Pk_Setting()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ pk ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.PkIndexSettingClause);
        e.AssertToken(SyntaxKind.PkKeyword, "pk");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_PrimaryKey_Setting()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ primary key ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.PrimaryKeyIndexSettingClause);
        e.AssertToken(SyntaxKind.PrimaryKeyword, "primary");
        e.AssertToken(SyntaxKind.KeyKeyword, "key");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Name_Setting_Identifier_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string indexText = $"{indexNameText} [ name: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NameIndexSettingClause);
        e.AssertToken(SyntaxKind.NameKeyword, "name");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Name_Setting_QuotationMarksStringToken_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSetting = DataGenerator.CreateRandomString();
        string settingValueText = $"\"{randomSetting}\"";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ name: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NameIndexSettingClause);
        e.AssertToken(SyntaxKind.NameKeyword, "name");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Name_Setting_SingleQuotationMarksStringToken_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSetting = DataGenerator.CreateRandomString();
        string settingValueText = $"\'{randomSetting}\'";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ name: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NameIndexSettingClause);
        e.AssertToken(SyntaxKind.NameKeyword, "name");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Type_Setting_Identifier_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string indexText = $"{indexNameText} [ type: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";
        string[] diagnosticMessages = new[]
        {
            $"Unknown index setting type '{settingValueText}'. Allowed index types [btree|gin|gist|hash].",
        };

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text, diagnosticMessages);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.TypeIndexSettingClause);
        e.AssertToken(SyntaxKind.TypeKeyword, "type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Type_Setting_QuotationMarksStringToken_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSetting = DataGenerator.CreateRandomString();
        string settingValueText = $"\"{randomSetting}\"";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ type: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";
        string[] diagnosticMessages = new[]
        {
            $"Unknown index setting type '{settingValue}'. Allowed index types [btree|gin|gist|hash].",
        };

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text, diagnosticMessages);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.TypeIndexSettingClause);
        e.AssertToken(SyntaxKind.TypeKeyword, "type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Type_Setting_SingleQuotationMarksStringToken_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSetting = DataGenerator.CreateRandomString();
        string settingValueText = $"\'{randomSetting}\'";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ type: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";
        string[] diagnosticMessages = new[]
        {
            $"Unknown index setting type '{settingValue}'. Allowed index types [btree|gin|gist|hash].",
        };

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text, diagnosticMessages);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.TypeIndexSettingClause);
        e.AssertToken(SyntaxKind.TypeKeyword, "type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Note_Setting_Identifier_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string indexText = $"{indexNameText} [ note: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteIndexSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Note_Setting_QuotationMarksStringToken_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSetting = DataGenerator.CreateRandomString();
        string settingValueText = $"\"{randomSetting}\"";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ note: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteIndexSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Note_Setting_SingleQuotationMarksStringToken_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSetting = DataGenerator.CreateRandomString();
        string settingValueText = $"\'{randomSetting}\'";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ note: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteIndexSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Unknown_Setting_Identifier_Name()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingName = null;
        string indexText = $"{indexNameText} [ {settingNameText} ]";
        string text = "indexes { " + indexText + " }";
        string[] diagnosticMessages = new[]
        {
            $"Unknown index setting '{settingNameText}'.",
        };

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text, diagnosticMessages);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownIndexSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingName);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    public static IEnumerable<object[]?> GetUnknownSingleFieldIndexSettingNameAllowedKeywordsData()
    {
        foreach ((SyntaxKind itemKind, string itemText, object? itemValue) in DataGenerator.GetSyntaxKeywords())
        {
            bool skip = itemKind switch
            {
                SyntaxKind.PkKeyword => true,
                SyntaxKind.UniqueKeyword => true,
                _ => false
            };

            if (!skip)
            {
                yield return new object[] { itemKind, itemText, itemValue! };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetUnknownSingleFieldIndexSettingNameAllowedKeywordsData))]
    public void Parse_SingleFieldIndexDeclaration_With_Unknown_Setting_Keyword_Name(
        SyntaxKind settingNameKind,
        string settingNameText,
        object? settingNameValue)
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ {settingNameText} ]";
        string text = "indexes { " + indexText + " }";
        string[] diagnosticMessages = new[]
        {
            $"Unknown index setting '{settingNameText}'.",
        };

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text, diagnosticMessages);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownIndexSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Unknown_KeyValue_Setting_Identifier_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingName = null;
        const SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string indexText = $"{indexNameText} [ {settingNameText}: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";
        string[] diagnosticMessages = new[]
        {
            $"Unknown index setting '{settingNameText}'.",
        };

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text, diagnosticMessages);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownIndexSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingName);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Unknown_KeyValue_Setting_QuotationMarksStringToken_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingName = null;
        const SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSetting = DataGenerator.CreateRandomString();
        string settingValueText = $"\"{randomSetting}\"";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ {settingNameText}: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";
        string[] diagnosticMessages = new[]
        {
            $"Unknown index setting '{settingNameText}'.",
        };

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text, diagnosticMessages);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownIndexSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingName);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Unknown_KeyValue_Setting_SingleQuotationMarksStringToken_Value()
    {
        const SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        object? indexNameValue = null;
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingName = null;
        const SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSetting = DataGenerator.CreateRandomString();
        string settingValueText = $"\'{randomSetting}\'";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ {settingNameText}: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";
        string[] diagnosticMessages = new[]
        {
            $"Unknown index setting '{settingNameText}'.",
        };

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text, diagnosticMessages);

        using AssertingEnumerator e = new(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownIndexSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingName);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
