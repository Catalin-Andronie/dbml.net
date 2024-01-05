using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_UnknownTableSettingClause_With_Identifier_Name()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        string settingText = $"{settingNameText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_UnknownTableSettingClause_With_Keyword_Name(
        SyntaxKind settingNameKind,
        string settingNameText,
        object? settingNameValue)
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        string settingText = $"{settingNameText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_UnknownTableSettingClause_With_Identifier_Name_And_Identifier_Value()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string randomSettingValue = DataGenerator.CreateRandomString();
        string settingValueText = $"{randomSettingValue}";
        object? settingValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_UnknownTableSettingClause_With_Identifier_Name_And_QuotationMarksString_Value()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSettingValue = DataGenerator.CreateRandomString();
        string settingValueText = $"\"{randomSettingValue}\"";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_UnknownTableSettingClause_With_Identifier_Name_And_SingleQuotationMarksString_Value()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingNameValue = null;
        const SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSettingValue = DataGenerator.CreateRandomString();
        string settingValueText = $"\'{randomSettingValue}\'";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [InlineData(SyntaxKind.HexTripletToken, "#000000", null)]
    [InlineData(SyntaxKind.HexTripletToken, "#FF0000", null)]
    [InlineData(SyntaxKind.HexTripletToken, "#00FF00", null)]
    [InlineData(SyntaxKind.HexTripletToken, "#0000FF", null)]
    [InlineData(SyntaxKind.HexTripletToken, "#FF00FF", null)]
    [InlineData(SyntaxKind.HexTripletToken, "#FFFFFF", null)]
    [InlineData(SyntaxKind.HexTripletToken, "#3498db", null)]
    [InlineData(SyntaxKind.HexTripletToken, "#3498DB", null)]
    public void Parse_HeaderColorTableSettingClause_With_Value(
        SyntaxKind settingValueKind,
        string settingValueText,
        object? settingValue)
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        string settingText = $"headercolor: {settingValueText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.HeaderColorTableSettingClause);
        e.AssertToken(SyntaxKind.HeaderColorKeyword, "headercolor");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
