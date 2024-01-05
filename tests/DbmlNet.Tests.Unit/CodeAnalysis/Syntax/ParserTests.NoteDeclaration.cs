using DbmlNet.CodeAnalysis.Syntax;

using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteDeclaration_With_QuotationMarksString(string noteKeywordText)
    {
        const SyntaxKind noteKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string noteText = $"\"{randomText}\"";
        string text = $"{noteKeywordText}: {noteText}";
        object noteValue = randomText;

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteKind, noteText, noteValue);
    }

    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteDeclaration_With_SingleQuotationMarksString(string noteKeywordText)
    {
        const SyntaxKind noteKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string noteText = $"\'{randomText}\'";
        object noteValue = randomText;
        string text = $"{noteKeywordText}: {noteText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteKind, noteText, noteValue);
    }

    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteDeclaration_With_MultiLineString(string noteKeywordText)
    {
        const SyntaxKind noteKind = SyntaxKind.MultiLineStringToken;
        string randomText = DataGenerator.CreateRandomMultiLineText();
        string noteText = $"'''{randomText}'''";
        object noteValue = randomText.TrimEnd();
        string text = $"{noteKeywordText}: {noteText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteKind, noteText, noteValue);
    }

    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteDeclaration_With_MultiLineString_Sample(string noteKeywordText)
    {
        const SyntaxKind noteKind = SyntaxKind.MultiLineStringToken;
        const string noteText = """
        '''
        💸 1 = processing
        ✔️ 2 = shipped
        ❌ 3 = cancelled
        😔 4 = refunded
        '''
        """;
        object noteValue = """
        💸 1 = processing
        ✔️ 2 = shipped
        ❌ 3 = cancelled
        😔 4 = refunded
        """;
        string text = $"{noteKeywordText}: {noteText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteKind, noteText, noteValue);
    }
}
