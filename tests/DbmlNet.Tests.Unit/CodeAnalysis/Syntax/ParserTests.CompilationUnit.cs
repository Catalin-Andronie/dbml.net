using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_CompilationUnit_With_No_Members()
    {
        string text = string.Empty;

        SyntaxTree syntaxTree = SyntaxTree.Parse(text);

        using AssertingEnumerator e = new(syntaxTree.Root);
        e.AssertNode(SyntaxKind.CompilationUnitMember);
        e.AssertToken(SyntaxKind.EndOfFileToken, string.Empty);
    }

    [Fact]
    public void Parse_CompilationUnit_With_ProjectDeclaration()
    {
        const SyntaxKind projectNameKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
        string projectNameText = $"{randomText}";
        object? projectNameValue = null;
        string text = $"Project {projectNameText} " + "{ }";

        SyntaxTree syntaxTree = SyntaxTree.Parse(text);

        using AssertingEnumerator e = new(syntaxTree.Root);
        e.AssertNode(SyntaxKind.CompilationUnitMember);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
        e.AssertToken(SyntaxKind.EndOfFileToken, string.Empty);
    }

    [Fact]
    public void Parse_CompilationUnit_With_TableDeclaration()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        string text = $"Table {tableNameText} " + "{ }";

        SyntaxTree syntaxTree = SyntaxTree.Parse(text);

        using AssertingEnumerator e = new(syntaxTree.Root);
        e.AssertNode(SyntaxKind.CompilationUnitMember);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
        e.AssertToken(SyntaxKind.EndOfFileToken, string.Empty);
    }
}
