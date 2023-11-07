using DbmlNet.CodeAnalysis.Syntax;

using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_ParenthesizedExpression_With_No_Expression()
    {
        const SyntaxKind expectedKind = SyntaxKind.IdentifierToken;
        string expectedText = string.Empty;
        const string text = "()";
        object? expectedValue = null;
        string[] diagnosticMessages = new[]
        {
            "Unexpected token <CloseParenthesisToken>, expected <IdentifierToken>.",
        };

        ExpressionSyntax expression = ParseExpression(text, diagnosticMessages);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.ParenthesizedExpression);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(expectedKind, expectedText, expectedValue, isMissing: true);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_ParenthesizedExpression_With_Expression()
    {
        const SyntaxKind expectedKind = SyntaxKind.NumberToken;
        decimal randomNumber = DataGenerator.GetRandomDecimal(min: 0);
        string expectedText = $"{randomNumber}";
        object? expectedValue = randomNumber;
        string text = "(" + expectedText + ")";

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.ParenthesizedExpression);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(expectedKind, expectedText, expectedValue);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }
}
