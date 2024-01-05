using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public class SyntaxTokenTests
{
    [Fact]
    public void SyntaxToken_Constructor_Should_Set_Properties_With_Given_Input()
    {
        SyntaxKind expectedKind = DataGenerator.GetRandomSyntaxKind();
        int expectedStart = DataGenerator.GetRandomNumber();
        string expectedText = DataGenerator.CreateRandomString();
        object? expectedValue = DataGenerator.CreateRandomString();

        SyntaxToken token = new(syntaxTree: null!, expectedKind, expectedStart, expectedText, expectedValue);

        Assert.Equal(expectedKind, token.Kind);
        Assert.Equal(expectedStart, token.Start);
        Assert.Equal(expectedText, token.Text);
        Assert.Equal(expectedValue, token.Value);
    }

    [Fact]
    public void SyntaxToken_Length_Should_Return_Expected_LengthValue()
    {
        SyntaxKind expectedKind = DataGenerator.GetRandomSyntaxKind();
        int expectedStart = DataGenerator.GetRandomNumber();
        string expectedText = DataGenerator.CreateRandomString();
        int expectedEnd = expectedText.Length;

        SyntaxToken token = new(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.Equal(expectedEnd, token.Length);
    }

    [Fact]
    public void SyntaxToken_End_Should_Return_Expected_EndValue()
    {
        SyntaxKind expectedKind = DataGenerator.GetRandomSyntaxKind();
        int expectedStart = DataGenerator.GetRandomNumber();
        string expectedText = DataGenerator.CreateRandomString();
        int expectedEnd = expectedStart + expectedText.Length;

        SyntaxToken token = new(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.Equal(expectedEnd, token.End);
    }

    [Fact]
    public void SyntaxToken_IsMissing_Should_Return_True_For_Null_TokenText()
    {
        SyntaxKind expectedKind = DataGenerator.GetRandomSyntaxKind();
        int expectedStart = DataGenerator.GetRandomNumber();
        const string? expectedText = null;

        SyntaxToken token = new(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.True(token.IsMissing, "Token should be missing.");
    }

    [Fact]
    public void SyntaxToken_IsMissing_Should_Return_False_For_Valid_TokenText()
    {
        SyntaxKind expectedKind = DataGenerator.GetRandomSyntaxKind();
        int expectedStart = DataGenerator.GetRandomNumber();
        string expectedText = DataGenerator.CreateRandomString();

        SyntaxToken token = new(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.False(token.IsMissing, "Token should not be missing.");
    }

    [Fact]
    public void SyntaxToken_ToString_Should_Return_TokenText()
    {
        SyntaxKind expectedKind = DataGenerator.GetRandomSyntaxKind();
        int expectedStart = DataGenerator.GetRandomNumber();
        string expectedText = DataGenerator.CreateRandomString();

        SyntaxToken token = new(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.Equal(expectedText, token.ToString());
    }

    [Fact]
    public void SyntaxToken_GetChildren_Should_Always_Return_Empty_List()
    {
        SyntaxKind expectedKind = DataGenerator.GetRandomSyntaxKind();
        int expectedStart = DataGenerator.GetRandomNumber();
        string expectedText = DataGenerator.CreateRandomString();
        object? expectedValue = DataGenerator.CreateRandomString();

        SyntaxToken token =
            new(syntaxTree: null!, expectedKind, expectedStart, expectedText, expectedValue);

        Assert.Empty(token.GetChildren());
    }
}
