using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class LexerTests
{
    [Fact]
    public void Lexer_Lex_EndOfFile()
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(string.Empty, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.EndOfFileToken, token.Kind);
        Assert.False(token.IsMissing, "Token should not be missing.");
    }

    [Theory]
    [InlineData("!")]
    [InlineData("?")]
    [InlineData("|")]
    public void Lexer_Lex_BadToken(string text)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Equal($"Bad character input: '{text}'.", diagnostic.Message);
        Assert.True(diagnostic.IsError, "Diagnostic should be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic should not be warning.");
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.BadToken, token.Kind);
        Assert.False(token.IsMissing);
    }

    [Fact]
    public void Lexer_Covers_AllTokens()
    {
        IEnumerable<SyntaxKind> tokenKinds =
            Enum.GetValues(typeof(SyntaxKind))
                .Cast<SyntaxKind>()
                .Where(k => k.IsToken());

        IEnumerable<SyntaxKind> testedTokenKinds =
            GetTokens().Select(t => t.Kind);

        SortedSet<SyntaxKind> untestedTokenKinds = new(tokenKinds);

        untestedTokenKinds.Remove(SyntaxKind.BadToken);
        untestedTokenKinds.Remove(SyntaxKind.EndOfFileToken);
        untestedTokenKinds.ExceptWith(testedTokenKinds);

        Assert.Empty(untestedTokenKinds);
    }

    [Theory]
    [MemberData(nameof(GetTokensData), DisableDiscoveryEnumeration = true)]
    public void Lexer_Lex_Token(SyntaxKind kind, string text)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(kind, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.False(token.IsMissing);
    }

    public static IEnumerable<object[]> GetTokensData()
    {
        foreach ((SyntaxKind kind, string text) in GetTokens())
            yield return new object[] { kind, text };
    }

    private static IEnumerable<(SyntaxKind Kind, string Text)> GetTokens()
    {
        IEnumerable<(SyntaxKind Kind, string Text)> fixedTokens =
            Enum.GetValues<SyntaxKind>()
                .Select(kind => (Kind: kind, Text: kind.GetKnownText() ?? null!))
                .Where(t => !string.IsNullOrEmpty(t.Text));

        (SyntaxKind, string)[] dynamicTokens = new[]
        {
            (SyntaxKind.NumberToken, "1"),
            (SyntaxKind.NumberToken, "123"),
            (SyntaxKind.NumberToken, "1_123"),
            (SyntaxKind.NumberToken, "1.1"),
            (SyntaxKind.NumberToken, "123.123"),
            (SyntaxKind.NumberToken, "1_123.1_123"),
            (SyntaxKind.NumberToken, $"{DataGenerator.GetRandomNumber(min: 0)}"),
            (SyntaxKind.NumberToken, $"{DataGenerator.GetRandomDecimal(min: 0)}.{DataGenerator.GetRandomNumber(min: 0)}"),

            (SyntaxKind.HexTripletToken, "#000000"),
            (SyntaxKind.HexTripletToken, "#FF0000"),
            (SyntaxKind.HexTripletToken, "#00FF00"),
            (SyntaxKind.HexTripletToken, "#0000FF"),
            (SyntaxKind.HexTripletToken, "#FF00FF"),
            (SyntaxKind.HexTripletToken, "#FFFFFF"),
            (SyntaxKind.HexTripletToken, "#3498db"),
            (SyntaxKind.HexTripletToken, "#3498DB"),

            (SyntaxKind.QuotationMarksStringToken, "\"Test\""),
            (SyntaxKind.QuotationMarksStringToken, "\"...message: \"\"message here\"\" or \"\"message here\"\"\""),
            (SyntaxKind.QuotationMarksStringToken, $"\"{DataGenerator.CreateRandomMultiWordString()}\""),
            (SyntaxKind.SingleQuotationMarksStringToken, "\'Test\'"),
            (SyntaxKind.SingleQuotationMarksStringToken, "\'...message: \'\'message here\'\' or \'\'message here\'\'\'"),
            (SyntaxKind.SingleQuotationMarksStringToken, $"\'{DataGenerator.CreateRandomMultiWordString()}\'"),
            (SyntaxKind.MultiLineStringToken, "'''Test'''"),
            (SyntaxKind.MultiLineStringToken, """'''...message: \\ \'''message here\''' or \'''message here\''''''"""),
            (SyntaxKind.MultiLineStringToken, $"'''{DataGenerator.CreateRandomMultiWordString()}'''"),

            (SyntaxKind.IdentifierToken, "a"),
            (SyntaxKind.IdentifierToken, "abc"),
            (SyntaxKind.IdentifierToken, "_a_b_c"),
        };

        return fixedTokens.Concat(dynamicTokens);
    }
}
