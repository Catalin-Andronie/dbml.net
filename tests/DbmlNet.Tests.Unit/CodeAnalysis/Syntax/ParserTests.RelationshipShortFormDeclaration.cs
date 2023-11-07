using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_RelationshipShortFormDeclaration_Without_Name()
    {
        // From identifier
        string fromSchemaText = DataGenerator.CreateRandomString();
        string fromTableText = DataGenerator.CreateRandomString();
        string fromColumnText = DataGenerator.CreateRandomString();
        string fromIdentifierText = $"{fromSchemaText}.{fromTableText}.{fromColumnText}";
        // To identifier
        string toSchemaText = DataGenerator.CreateRandomString();
        string toTableText = DataGenerator.CreateRandomString();
        string toColumnText = DataGenerator.CreateRandomString();
        string toIdentifierText = $"{toSchemaText}.{toTableText}.{toColumnText}";
        // Relationship declaration
        string text = $"Ref: {fromIdentifierText} < {toIdentifierText}";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.RelationshipShortFormDeclarationMember);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.RelationshipConstraintClause);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, fromSchemaText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromTableText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromColumnText);
        e.AssertToken(SyntaxKind.LessToken, "<");
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, toSchemaText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toTableText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toColumnText);
    }

    [Fact]
    public void Parse_RelationshipShortFormDeclaration_With_Name_Identifier()
    {
        const SyntaxKind identifierKind = SyntaxKind.IdentifierToken;
        string identifierText = DataGenerator.CreateRandomString();
        object? identifierValue = null;
        // From identifier
        string fromSchemaText = DataGenerator.CreateRandomString();
        string fromTableText = DataGenerator.CreateRandomString();
        string fromColumnText = DataGenerator.CreateRandomString();
        string fromIdentifierText = $"{fromSchemaText}.{fromTableText}.{fromColumnText}";
        // To identifier
        string toSchemaText = DataGenerator.CreateRandomString();
        string toTableText = DataGenerator.CreateRandomString();
        string toColumnText = DataGenerator.CreateRandomString();
        string toIdentifierText = $"{toSchemaText}.{toTableText}.{toColumnText}";
        // Relationship declaration
        string text = $"Ref {identifierText}: {fromIdentifierText} < {toIdentifierText}";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.RelationshipShortFormDeclarationMember);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(identifierKind, identifierText, identifierValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.RelationshipConstraintClause);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, fromSchemaText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromTableText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromColumnText);
        e.AssertToken(SyntaxKind.LessToken, "<");
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, toSchemaText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toTableText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toColumnText);
    }

    [Fact]
    public void Parse_RelationshipLongFormDeclaration_Without_Name()
    {
        const string text = """
        Ref
        {
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.RelationshipLongFormDeclarationMember);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_RelationshipLongFormDeclaration_With_Name_Identifier()
    {
        const SyntaxKind identifierKind = SyntaxKind.IdentifierToken;
        string identifierText = DataGenerator.CreateRandomString();
        object? identifierValue = null;
        string text = $$"""
        Ref {{identifierText}}
        {
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.RelationshipLongFormDeclarationMember);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(identifierKind, identifierText, identifierValue);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_RelationshipLongFormDeclaration_Without_Relationships()
    {
        const string text = """
        Ref
        {
            // no relationships
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.RelationshipLongFormDeclarationMember);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_RelationshipLongFormDeclaration_With_Relationship()
    {
        // From identifier
        string fromSchemaText = DataGenerator.CreateRandomString();
        string fromTableText = DataGenerator.CreateRandomString();
        string fromColumnText = DataGenerator.CreateRandomString();
        string fromIdentifierText = $"{fromSchemaText}.{fromTableText}.{fromColumnText}";
        // To identifier
        string toSchemaText = DataGenerator.CreateRandomString();
        string toTableText = DataGenerator.CreateRandomString();
        string toColumnText = DataGenerator.CreateRandomString();
        string toIdentifierText = $"{toSchemaText}.{toTableText}.{toColumnText}";
        // Relationship declaration
        string text = $$"""
        Ref
        {
            {{fromIdentifierText}} < {{toIdentifierText}}
        }
        """;

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.RelationshipLongFormDeclarationMember);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.RelationshipConstraintClause);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, fromSchemaText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromTableText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromColumnText);
        e.AssertToken(SyntaxKind.LessToken, "<");
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, toSchemaText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toTableText);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toColumnText);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
