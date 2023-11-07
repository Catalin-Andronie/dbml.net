using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Theory]
    [MemberData(nameof(LiteralTokensData))]
    public void Parse_BacktickExpression_With_LiteralExpression(
        SyntaxKind literalKind, string literalText, object? literalValue)
    {
        string text = $"`{literalText}`";

        BacktickExpressionSyntax expression = ParseBacktickExpression(text);

        using AssertingEnumerator e = new(expression.Expression);
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(literalKind, literalText, literalValue);
    }

    public static IEnumerable<object?[]> LiteralTokensData
    {
        get
        {
            yield return new object?[] { SyntaxKind.TrueKeyword, "true", true };
            yield return new object?[] { SyntaxKind.FalseKeyword, "false", false };
            yield return new object?[] { SyntaxKind.NumberToken, "1", 1m };
            yield return new object?[] { SyntaxKind.NumberToken, "0", 0m };
            yield return new object?[] { SyntaxKind.NumberToken, "1234567890.21", 1234567890.21m };
            yield return new object?[] { SyntaxKind.QuotationMarksStringToken, "\"ms1\"", "ms1" };
            yield return new object?[] { SyntaxKind.QuotationMarksStringToken, "\"ms1 ms2\"", "ms1 ms2" };
            yield return new object?[] { SyntaxKind.SingleQuotationMarksStringToken, "\'ms1\'", "ms1" };
            yield return new object?[] { SyntaxKind.SingleQuotationMarksStringToken, "\'ms1 ms2\'", "ms1 ms2" };
        }
    }

    [Fact]
    public void Parse_BacktickExpression_With_NameExpression()
    {
        const SyntaxKind identifierKind = SyntaxKind.IdentifierToken;
        string randomIdentifierName = DataGenerator.CreateRandomString();
        string identifierText = $"{randomIdentifierName}";
        object? identifierValue = null;
        string text = $"`{identifierText}`";

        BacktickExpressionSyntax expression = ParseBacktickExpression(text);

        using AssertingEnumerator e = new(expression.Expression);
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(identifierKind, identifierText, identifierValue);
    }

    [Fact]
    public void Parse_BacktickExpression_With_CallExpression()
    {
        const SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string randomFunctionName = DataGenerator.CreateRandomString();
        string functionNameText = $"{randomFunctionName}";
        object? functionNameValue = null;
        string expressionText = $"{randomFunctionName}()";
        string text = $"`{expressionText}`";

        BacktickExpressionSyntax expression = ParseBacktickExpression(text);

        using AssertingEnumerator e = new(expression.Expression);
        e.AssertNode(SyntaxKind.CallExpression);
        e.AssertToken(functionNameKind, functionNameText, functionNameValue);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }
}
