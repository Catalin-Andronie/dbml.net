using System.Collections.Generic;
using System.Globalization;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_PrimaryKeyColumnSettingClause()
    {
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ primary key ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.PrimaryKeyColumnSettingClause);
        e.AssertToken(SyntaxKind.PrimaryKeyword, "primary");
        e.AssertToken(SyntaxKind.KeyKeyword, "key");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_PkColumnSettingClause()
    {
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ pk ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.PkColumnSettingClause);
        e.AssertToken(SyntaxKind.PkKeyword, "pk");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NullColumnSettingClause()
    {
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ null ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NullColumnSettingClause);
        e.AssertToken(SyntaxKind.NullKeyword, "null");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NotNullColumnSettingClause()
    {
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ not null ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NotNullColumnSettingClause);
        e.AssertToken(SyntaxKind.NotKeyword, "not");
        e.AssertToken(SyntaxKind.NullKeyword, "null");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UniqueColumnSettingClause()
    {
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ unique ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UniqueColumnSettingClause);
        e.AssertToken(SyntaxKind.UniqueKeyword, "unique");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_IncrementColumnSettingClause()
    {
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ increment ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.IncrementColumnSettingClause);
        e.AssertToken(SyntaxKind.IncrementKeyword, "increment");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NoteColumnSettingClause_With_QuotationMarksString_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"\"{randomText}\"";
        object? settingValue = randomText;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ note: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteColumnSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NoteColumnSettingClause_With_SingleQuotationMarksString_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"\'{randomText}\'";
        object? settingValue = randomText;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ note: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteColumnSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Identifier_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string settingText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: {settingText} ]";
        string[] diagnosticMessages = new[]
        {
            "Disallowed 'default' column setting value expression 'NameExpression'.",
        };

        ColumnSettingListSyntax columnSettingListClause =
            ParseColumnSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Number_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.NumberToken;
        decimal randomNumber = DataGenerator.GetRandomDecimal(min: 0);
        string settingText = $"{randomNumber}";
        object? settingValue = randomNumber;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_DecimalPointNumber_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.NumberToken;
        string settingText = $"{DataGenerator.GetRandomNumber(min: 0)}.{DataGenerator.GetRandomNumber(min: 0)}";
        object? settingValue = decimal.Parse(settingText, CultureInfo.InvariantCulture);
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Bool_False_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.FalseKeyword;
        string settingText = "false";
        object? settingValue = false;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Bool_True_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.TrueKeyword;
        string settingText = "true";
        object? settingValue = true;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Null_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.NullKeyword;
        string settingText = "null";
        object? settingValue = null;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.NullExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_QuotationMarksString_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"\"{randomText}\"";
        object? settingValue = randomText;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_SingleQuotationMarksString_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"\'{randomText}\'";
        object? settingValue = randomText;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_NameExpression_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string identifierExpressionText = $"{DataGenerator.CreateRandomString()}";
        string settingText = $"{identifierExpressionText}";
        object? settingValue = null;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ default: `{settingText}` ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.BacktickExpression);
        e.AssertToken(SyntaxKind.BacktickToken, "`");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.BacktickToken, "`");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Simple_Setting_Identifier()
    {
        const SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ {settingNameText} ]";
        string[] diagnosticMessages = new[]
        {
            $"Unknown column setting '{settingNameText}'.",
        };

        ColumnSettingListSyntax columnSettingListClause =
            ParseColumnSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingKind, settingNameText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    public static IEnumerable<object[]?> GetUnknownColumnSettingNameAllowedKeywordsData()
    {
        foreach ((SyntaxKind itemKind, string itemText, object? itemValue) in DataGenerator.GetSyntaxKeywords())
        {
            bool skip = itemKind switch
            {
                SyntaxKind.PkKeyword => true,
                SyntaxKind.UniqueKeyword => true,
                SyntaxKind.IncrementKeyword => true,
                SyntaxKind.NullKeyword => true,
                _ => false
            };

            if (!skip)
            {
                yield return new object[] { itemKind, itemText, itemValue! };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetUnknownColumnSettingNameAllowedKeywordsData))]
    public void Parse_UnknownColumnSettingClause_With_Simple_Setting_Keyword(
        SyntaxKind settingNameKind,
        string settingNameText,
        object? settingNameValue)
    {
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ {settingNameText} ]";
        string[] diagnosticMessages = new[]
        {
            $"Unknown column setting '{settingNameText}'.",
        };

        ColumnSettingListSyntax columnSettingListClause =
            ParseColumnSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Composed_Setting_Identifier_Value()
    {
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ {settingText} ]";
        string[] diagnosticMessages = new[]
        {
            $"Unknown column setting '{settingNameText}'.",
        };

        ColumnSettingListSyntax columnSettingListClause =
            ParseColumnSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Composed_Setting_QuotationMarksString_Value()
    {
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSettingValue = DataGenerator.CreateRandomMultiWordString();
        string settingValueText = $"\"{randomSettingValue}\"";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ {settingText} ]";
        string[] diagnosticMessages = new[]
        {
            $"Unknown column setting '{settingNameText}'.",
        };

        ColumnSettingListSyntax columnSettingListClause =
            ParseColumnSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Composed_Setting_SingleQuotationMarksString_Value()
    {
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSettingValue = DataGenerator.CreateRandomMultiWordString();
        string settingValueText = $"\'{randomSettingValue}\'";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ {settingText} ]";
        string[] diagnosticMessages = new[]
        {
            $"Unknown column setting '{settingNameText}'.",
        };

        ColumnSettingListSyntax columnSettingListClause =
            ParseColumnSettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Theory]
    [InlineData(SyntaxKind.LessToken, "<")]
    [InlineData(SyntaxKind.GraterToken, ">")]
    [InlineData(SyntaxKind.LessGraterToken, "<>")]
    [InlineData(SyntaxKind.MinusToken, "-")]
    public void Parse_RelationshipColumnSettingClause_With_Left_Relation_Explicit(
        SyntaxKind relationshipTypeKind,
        string relationshipTypeText)
    {
        string fromSchemaName = DataGenerator.CreateRandomString();
        string fromTableName = DataGenerator.CreateRandomString();
        string fromColumnName = DataGenerator.CreateRandomString();
        string fromIdentifierText = $"{fromSchemaName}.{fromTableName}.{fromColumnName}";
        string toSchemaName = DataGenerator.CreateRandomString();
        string toTableName = DataGenerator.CreateRandomString();
        string toColumnName = DataGenerator.CreateRandomString();
        string toIdentifierText = $"{toSchemaName}.{toTableName}.{toColumnName}";
        string settingText = $"{fromIdentifierText} {relationshipTypeText} {toIdentifierText}";
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ ref: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.RelationshipColumnSettingClause);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.RelationshipConstraintClause);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, fromSchemaName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromTableName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromColumnName);
        e.AssertToken(relationshipTypeKind, relationshipTypeText);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, toSchemaName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toTableName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toColumnName);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Theory]
    [InlineData(SyntaxKind.LessToken, "<")]
    [InlineData(SyntaxKind.GraterToken, ">")]
    [InlineData(SyntaxKind.LessGraterToken, "<>")]
    [InlineData(SyntaxKind.MinusToken, "-")]
    public void Parse_RelationshipColumnSettingClause_With_Left_Relation_Implicit(
        SyntaxKind relationshipTypeKind,
        string relationshipTypeText)
    {
        string toSchemaName = DataGenerator.CreateRandomString();
        string toTableName = DataGenerator.CreateRandomString();
        string toColumnName = DataGenerator.CreateRandomString();
        string toIdentifierText = $"{toSchemaName}.{toTableName}.{toColumnName}";
        string settingText = $"{relationshipTypeText} {toIdentifierText}";
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ ref: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.RelationshipColumnSettingClause);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.RelationshipConstraintClause);
        e.AssertToken(relationshipTypeKind, relationshipTypeText);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, toSchemaName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toTableName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toColumnName);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
