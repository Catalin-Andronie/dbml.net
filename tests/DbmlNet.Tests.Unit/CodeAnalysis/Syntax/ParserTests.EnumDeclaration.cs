using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_EnumDeclaration_With_Name_Identifier()
    {
        const SyntaxKind enumNameKind = SyntaxKind.IdentifierToken;
        string enumNameText = DataGenerator.CreateRandomString();
        object? enumNameValue = null;
        string text = $$"""
        enum {{enumNameText}} {
            // no values
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.EnumDeclarationMember);
        e.AssertToken(SyntaxKind.EnumKeyword, "enum");
        e.AssertNode(SyntaxKind.EnumIdentifierClause);
        e.AssertToken(enumNameKind, enumNameText, enumNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_EnumDeclaration_With_Name_Keyword(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        string text = $$"""
        enum {{keywordText}} {
            // no values
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.EnumDeclarationMember);
        e.AssertToken(SyntaxKind.EnumKeyword, "enum");
        e.AssertNode(SyntaxKind.EnumIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_EnumDeclaration_With_Name_And_Schema_Identifier()
    {
        const SyntaxKind schemaNameKind = SyntaxKind.IdentifierToken;
        string schemaNameText = DataGenerator.CreateRandomString();
        object? schemaNameValue = null;
        const SyntaxKind enumNameKind = SyntaxKind.IdentifierToken;
        string enumNameText = DataGenerator.CreateRandomString();
        object? enumNameValue = null;
        string text = $$"""
        enum {{schemaNameText}}.{{enumNameText}} {
            // no values
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.EnumDeclarationMember);
        e.AssertToken(SyntaxKind.EnumKeyword, "enum");
        e.AssertNode(SyntaxKind.EnumIdentifierClause);
        e.AssertToken(schemaNameKind, schemaNameText, schemaNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(enumNameKind, enumNameText, enumNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_EnumDeclaration_With_Name_And_Schema_Keyword(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        string text = $$"""
        enum {{keywordText}}.{{keywordText}} {
            // no values
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.EnumDeclarationMember);
        e.AssertToken(SyntaxKind.EnumKeyword, "enum");
        e.AssertNode(SyntaxKind.EnumIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_EnumDeclaration_With_Empty_Body()
    {
        const SyntaxKind enumNameKind = SyntaxKind.IdentifierToken;
        string enumNameText = DataGenerator.CreateRandomString();
        object? enumNameValue = null;
        string text = $$"""
        enum {{enumNameText}} {
            // no values
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.EnumDeclarationMember);
        e.AssertToken(SyntaxKind.EnumKeyword, "enum");
        e.AssertNode(SyntaxKind.EnumIdentifierClause);
        e.AssertToken(enumNameKind, enumNameText, enumNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_EnumDeclaration_With_Note_QuotationMarksString()
    {
        const SyntaxKind enumNameKind = SyntaxKind.IdentifierToken;
        string enumNameText = DataGenerator.CreateRandomString();
        object? enumNameValue = null;
        const SyntaxKind noteValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomNoteText = DataGenerator.CreateRandomMultiWordString();
        string noteValueText = $"\"{randomNoteText}\"";
        object? noteValue = randomNoteText;
        string text = $$"""
        enum {{enumNameText}} {
            note: {{noteValueText}}
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.EnumDeclarationMember);
        e.AssertToken(SyntaxKind.EnumKeyword, "enum");
        e.AssertNode(SyntaxKind.EnumIdentifierClause);
        e.AssertToken(enumNameKind, enumNameText, enumNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteValueKind, noteValueText, noteValue);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_EnumDeclaration_With_Note_SingleQuotationMarksString()
    {
        const SyntaxKind enumNameKind = SyntaxKind.IdentifierToken;
        string enumNameText = DataGenerator.CreateRandomString();
        object? enumNameValue = null;
        const SyntaxKind noteValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomNoteText = DataGenerator.CreateRandomMultiWordString();
        string noteValueText = $"\'{randomNoteText}\'";
        object? noteValue = randomNoteText;
        string text = $$"""
        enum {{enumNameText}} {
            note: {{noteValueText}}
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.EnumDeclarationMember);
        e.AssertToken(SyntaxKind.EnumKeyword, "enum");
        e.AssertNode(SyntaxKind.EnumIdentifierClause);
        e.AssertToken(enumNameKind, enumNameText, enumNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteValueKind, noteValueText, noteValue);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
