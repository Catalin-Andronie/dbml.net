using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_EnumEntryDeclaration_With_Name_Identifier()
    {
        const SyntaxKind enumEntryNameKind = SyntaxKind.IdentifierToken;
        string enumEntryNameText = DataGenerator.CreateRandomString();
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}}
        }
        """;

        StatementSyntax statement = ParseEnumEntryDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.EnumEntryDeclarationStatement);
        e.AssertToken(enumEntryNameKind, enumEntryNameText);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_EnumEntryDeclaration_With_Name_Keyword(
        SyntaxKind enumEntryNameKind,
        string enumEntryNameText,
        object? enumEntryNameValue)
    {
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}}
        }
        """;

        StatementSyntax statement = ParseEnumEntryDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.EnumEntryDeclarationStatement);
        e.AssertToken(enumEntryNameKind, enumEntryNameText, enumEntryNameValue);
    }

    [Fact]
    public void Parse_EnumEntryDeclaration_With_Name_QuotationMarksString()
    {
        const SyntaxKind enumEntryNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomEnumEntryName = DataGenerator.CreateRandomMultiWordString();
        string enumEntryNameText = $"\"{randomEnumEntryName}\"";
        object? enumEntryNameValue = randomEnumEntryName;
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}}
        }
        """;

        StatementSyntax statement = ParseEnumEntryDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.EnumEntryDeclarationStatement);
        e.AssertToken(enumEntryNameKind, enumEntryNameText, enumEntryNameValue);
    }

    [Fact]
    public void Parse_EnumEntryDeclaration_With_Name_SingleQuotationMarksString()
    {
        const SyntaxKind enumEntryNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomColumName = DataGenerator.CreateRandomMultiWordString();
        string enumEntryNameText = $"\'{randomColumName}\'";
        object? enumEntryNameValue = randomColumName;
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}}
        }
        """;

        StatementSyntax statement = ParseEnumEntryDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.EnumEntryDeclarationStatement);
        e.AssertToken(enumEntryNameKind, enumEntryNameText, enumEntryNameValue);
    }

    [Fact]
    public void Parse_EnumEntryDeclaration_With_No_Settings()
    {
        const SyntaxKind enumEntryNameKind = SyntaxKind.IdentifierToken;
        string enumEntryNameText = DataGenerator.CreateRandomString();
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}}
        }
        """;

        StatementSyntax statement = ParseEnumEntryDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.EnumEntryDeclarationStatement);
        e.AssertToken(enumEntryNameKind, enumEntryNameText);
    }

    [Fact]
    public void Parse_EnumEntryDeclaration_With_Empty_Settings()
    {
        const SyntaxKind enumEntryNameKind = SyntaxKind.IdentifierToken;
        string enumEntryNameText = DataGenerator.CreateRandomString();
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}} [ ]
        }
        """;

        StatementSyntax statement = ParseEnumEntryDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.EnumEntryDeclarationStatement);
        e.AssertToken(enumEntryNameKind, enumEntryNameText);
        e.AssertNode(SyntaxKind.EnumEntrySettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
