using System.Collections.Immutable;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.CodeAnalysis.Text;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class LexerTests
{
    [Theory]
    [InlineData("\t")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void Lexer_Lex_Trivia_Whitespace(string text)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        SyntaxTrivia trivia = Assert.Single(token.LeadingTrivia);
        Assert.Equal(SyntaxKind.WhitespaceTrivia, trivia.Kind);
        Assert.Equal(text, trivia.Text);
    }

    [Theory]
    [InlineData("\r")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Lexer_Lex_Trivia_LineBreak(string text)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        SyntaxTrivia trivia = Assert.Single(token.LeadingTrivia);
        Assert.Equal(SyntaxKind.LineBreakTrivia, trivia.Kind);
        Assert.Equal(text, trivia.Text);
    }

    [Fact]
    public void Lexer_Lex_Trivia_SingleLineComment()
    {
        string text = $"// {DataGenerator.CreateRandomMultiWordString()}";

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        SyntaxTrivia trivia = Assert.Single(token.LeadingTrivia);
        Assert.Equal(SyntaxKind.SingleLineCommentTrivia, trivia.Kind);
        Assert.Equal(text, trivia.Text);
    }

    [Fact]
    public void Lexer_Lex_Trivia_MultiLineComment()
    {
        string text = $"""
        /*
            {DataGenerator.CreateRandomMultiWordString()}
            {DataGenerator.CreateRandomMultiWordString()}
            {DataGenerator.CreateRandomMultiWordString()}
            {DataGenerator.CreateRandomMultiWordString()}
        */
        """;

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        SyntaxTrivia trivia = Assert.Single(token.LeadingTrivia);
        Assert.Equal(SyntaxKind.MultiLineCommentTrivia, trivia.Kind);
        Assert.Equal(text, trivia.Text);
    }

    [Fact]
    public void Lexer_Lex_Trivia_UnterminatedMultiLineComment()
    {
        string text = $"""
        /*
            {DataGenerator.CreateRandomMultiWordString()}
            {DataGenerator.CreateRandomMultiWordString()}
            {DataGenerator.CreateRandomMultiWordString()}
            {DataGenerator.CreateRandomMultiWordString()}
        /
        """;

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.True(diagnostic.IsError, "Diagnostic show be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic show not be warning.");
        Assert.Equal(new TextSpan(0, 2), diagnostic.Location.Span);
        Assert.Equal("Unterminated multi-line comment.", diagnostic.Message);
        SyntaxToken token = Assert.Single(tokens);
        SyntaxTrivia trivia = Assert.Single(token.LeadingTrivia);
        Assert.Equal(SyntaxKind.MultiLineCommentTrivia, trivia.Kind);
        Assert.Equal(text, trivia.Text);
    }
}
