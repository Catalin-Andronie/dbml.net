using DbmlNet.CodeAnalysis.Text;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Text;

public sealed class TextSpanTests
{
    [Fact]
    public void TextSpan_ctor_Create_TextSpan_With_Start_And_Length()
    {
        int start = DataGenerator.GetRandomNumber();
        int length = DataGenerator.GetRandomNumber();

        TextSpan span = new(start, length);

        Assert.True(start == span.Start, $"Expect span.Start == start, but got {span.Start} == {start}");
        Assert.True(length == span.Length, $"Expect span.Length == length, but got {span.Length} == {length}");
        Assert.True(span.End == start + length, $"Expect span.End == start + length, but got {span.End} == {start} + {length}");
        Assert.Equal($"{start}..{start + length}", span.ToString());
    }

    [Fact]
    public void TextSpan_FromBounds_Create_TextSpan_With_Start_And_Length()
    {
        const int start = 0;
        int length = DataGenerator.GetRandomNumber();

        TextSpan span = TextSpan.FromBounds(start, length);

        Assert.True(span.Start == start, $"Expect span.Start == start, but got {span.Start} == {start}");
        Assert.True(span.Length == length, $"Expect span.Length == length, but got {span.Length} == {length}");
        Assert.True(span.End == start + length, $"Expect span.End == start + length, but got {span.End} == {start} + {length}");
        Assert.Equal($"{start}..{start + length}", span.ToString());
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 10)]
    public void TextSpan_Equals_TextSpan(int start, int end)
    {
        TextSpan firstSpan = TextSpan.FromBounds(start, end);
        TextSpan secondSpan = TextSpan.FromBounds(start, end);

        Assert.True(firstSpan.Equals(secondSpan), $"Expect span == span, but got {firstSpan} != {secondSpan}");
        Assert.True(firstSpan == secondSpan, $"Expect span == span, but got {firstSpan} != {secondSpan}");
        Assert.False(firstSpan != secondSpan, $"Expect span == span, but got {firstSpan} != {secondSpan}");
    }

    [Theory]
    [InlineData(0, 1, 1, 0)]
    [InlineData(1, 245, 5, 21)]
    public void TextSpan_NotEquals_TextSpan(int start1, int end1, int start2, int end2)
    {
        TextSpan firstSpan = TextSpan.FromBounds(start1, end1);
        TextSpan secondSpan = TextSpan.FromBounds(start2, end2);

        Assert.False(firstSpan.Equals(secondSpan), $"Expect span != span, but got {firstSpan} == {secondSpan}");
        Assert.False(firstSpan == secondSpan, $"Expect span != span, but got {firstSpan} == {secondSpan}");
        Assert.True(firstSpan != secondSpan, $"Expect span != span, but got {firstSpan} == {secondSpan}");
    }
}
