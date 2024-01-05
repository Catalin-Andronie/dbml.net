using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_LiteralExpression_With_Bool_True()
    {
        const SyntaxKind expectedKind = SyntaxKind.TrueKeyword;
        const string expectedText = "true";
        object? expectedValue = true;

        ExpressionSyntax expression = ParseExpression(expectedText);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.LiteralExpression);
        LiteralExpressionSyntax literalExpression =
            Assert.IsAssignableFrom<LiteralExpressionSyntax>(e.Node);
        Assert.Equal(expectedValue, literalExpression.Value);
        e.AssertToken(expectedKind, expectedText, expectedValue);
    }

    [Fact]
    public void Parse_LiteralExpression_With_Bool_False()
    {
        const SyntaxKind expectedKind = SyntaxKind.FalseKeyword;
        const string expectedText = "false";
        object? expectedValue = false;

        ExpressionSyntax expression = ParseExpression(expectedText);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.LiteralExpression);
        LiteralExpressionSyntax literalExpression =
            Assert.IsAssignableFrom<LiteralExpressionSyntax>(e.Node);
        Assert.Equal(expectedValue, literalExpression.Value);
        e.AssertToken(expectedKind, expectedText, expectedValue);
    }

    [Fact]
    public void Parse_LiteralExpression_With_Number()
    {
        const SyntaxKind expectedKind = SyntaxKind.NumberToken;
        decimal randomNumber = DataGenerator.GetRandomDecimal(min: 0);
        string expectedText = $"{randomNumber}";
        object? expectedValue = randomNumber;

        ExpressionSyntax expression = ParseExpression(expectedText);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.LiteralExpression);
        LiteralExpressionSyntax literalExpression =
            Assert.IsAssignableFrom<LiteralExpressionSyntax>(e.Node);
        Assert.Equal(expectedValue, literalExpression.Value);
        e.AssertToken(expectedKind, expectedText, expectedValue);
    }

    [Fact]
    public void Parse_LiteralExpression_With_QuotationMarksString()
    {
        const SyntaxKind expectedKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string expectedText = $"\"{randomText}\"";
        object? expectedValue = randomText;

        ExpressionSyntax expression = ParseExpression(expectedText);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.LiteralExpression);
        LiteralExpressionSyntax literalExpression =
            Assert.IsAssignableFrom<LiteralExpressionSyntax>(e.Node);
        Assert.Equal(expectedValue, literalExpression.Value);
        e.AssertToken(expectedKind, expectedText, expectedValue);
    }

    [Fact]
    public void Parse_LiteralExpression_With_SingleQuotationMarksString()
    {
        const SyntaxKind expectedKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string expectedText = $"\'{randomText}\'";
        object? expectedValue = randomText;

        ExpressionSyntax expression = ParseExpression(expectedText);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.LiteralExpression);
        LiteralExpressionSyntax literalExpression =
            Assert.IsAssignableFrom<LiteralExpressionSyntax>(e.Node);
        Assert.Equal(expectedValue, literalExpression.Value);
        e.AssertToken(expectedKind, expectedText, expectedValue);
    }
}
