using System.Linq;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Database_Empty()
    {
        string text = string.Empty;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.Empty(database.Providers);
        Assert.Empty(database.Notes);
        Assert.Empty(database.Note);
        Assert.Null(database.Project);
        Assert.Empty(database.Tables);
    }

    [Fact]
    public void Create_Returns_Database_With_Note()
    {
        string randomNote = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        note: '{{randomNote}}'
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.Equal(randomNote, database.Note);
    }

    private static SyntaxTree ParseNoDiagnostics(string text)
    {
        SyntaxTree syntax = SyntaxTree.Parse(text);
        Assert.True(syntax.Diagnostics.Length == 0, $"There should be no diagnostics for text '{text}', but found {string.Join(", ", syntax.Diagnostics.Select(d => d.Message))}.");
        return syntax;
    }

    private static SyntaxTree ParseNoErrorDiagnostics(string text)
    {
        SyntaxTree syntax = SyntaxTree.Parse(text);
        Assert.Empty(syntax.Diagnostics.Where(d => d.IsError));
        return syntax;
    }
}
