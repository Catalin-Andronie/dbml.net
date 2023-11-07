using DbmlNet.CodeAnalysis.Text;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Text;

public sealed class SourceTextTests
{
    [Theory]
    [InlineData(".", 1)]
    [InlineData(".\r\n", 2)]
    [InlineData(".\r\n\r\n", 3)]
    public void SourceText_IncludesLastLine(string text, int expectedLineCount)
    {
        SourceText sourceText = SourceText.From(text);
        Assert.Equal(expectedLineCount, sourceText.Lines.Length);
    }

    [Fact]
    public void SourceText_From_Returns_SourceText_With_Empty_Text()
    {
        const string text = "";

        SourceText sourceText = SourceText.From(text);

        Assert.Equal(text, sourceText.ToString());
        Assert.True(sourceText.Length == 0, $"Expected 0 == text.Length, and got {sourceText.Length} ");
        TextLine line = Assert.Single(sourceText.Lines);
        Assert.Equal(string.Empty, line.ToString());
        Assert.True(line.Start == 0, $"Expected 0 == line.Start, and got {line.Start}");
        Assert.True(line.Length == 0, $"Expected 0 == line.Length, and got {line.Length}");
        Assert.True(line.End == 0, $"Expected 0 == line.End, and got {line.End}");
        Assert.True(line.LengthIncludingLineBreak == 0, $"Expected 0 == line.LengthIncludingLineBreak, and got {line.LengthIncludingLineBreak}");
        Assert.True(line.Span.Start == 0, $"Expected 0 == line.Span.Start, and got {line.Span.Start}");
        Assert.True(line.Span.Length == 0, $"Expected 0 == line.Span.Length, and got {line.Span.Length}");
        Assert.True(line.Span.End == 0, $"Expected 0 == line.Span.End, and got {line.Span.End}");
        Assert.True(line.SpanIncludingLineBreak.Start == 0, $"Expected 0 == line.SpanIncludingLineBreak.Start, and got {line.SpanIncludingLineBreak.Start}");
        Assert.True(line.SpanIncludingLineBreak.Length == 0, $"Expected 0 == line.SpanIncludingLineBreak.Length, and got {line.SpanIncludingLineBreak.Length}");
        Assert.True(line.SpanIncludingLineBreak.End == 0, $"Expected 0 == line.SpanIncludingLineBreak.End, and got {line.SpanIncludingLineBreak.End}");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void SourceText_From_Returns_SourceText_With_MultiLine_Text(
        int minLineCount, int maxLineCount)
    {
        Assert.True(minLineCount >= 0, "Invalid test input expected param minLineCount >= 0");
        Assert.True(minLineCount <= maxLineCount, "Invalid test input expected param minLineCount <= param maxLineCount");
        string text = DataGenerator.CreateRandomMultiLineText(minLineCount, maxLineCount);

        SourceText sourceText = SourceText.From(text);

        Assert.Equal(text, sourceText.ToString());
        Assert.True(sourceText.Lines.Length >= minLineCount, $"Expect text.Lines.Length >= minLineCount, and got {sourceText.Lines.Length} >= {minLineCount}");
        Assert.True(sourceText.Lines.Length <= maxLineCount + 1, $"Expect text.Lines.Length <= maxLineCount, and got {sourceText.Lines.Length} <= {maxLineCount + 1}");
    }
}
