using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_NameExpression_With_Identifier()
    {
        const SyntaxKind expectedKind = SyntaxKind.IdentifierToken;
        string expectedText = DataGenerator.CreateRandomString();
        object? expectedValue = null;

        ExpressionSyntax expression = ParseExpression(expectedText);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(expectedKind, expectedText, expectedValue);
    }
}
