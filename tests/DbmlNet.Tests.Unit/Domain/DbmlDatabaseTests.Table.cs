using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Table_Empty()
    {
        string randomTableName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{randomTableName}}
        {
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Null(table.Database);
        Assert.Null(table.Schema);
        Assert.Equal(randomTableName, table.Name);
        Assert.Equal(randomTableName, table.ToString());
        Assert.Empty(table.Columns);
        Assert.Empty(table.Indexes);
        Assert.Empty(table.Relationships);
        Assert.Empty(table.Notes);
        Assert.Empty(table.Note);
    }

    [Fact]
    public void Create_Returns_Table_With_Name()
    {
        string randomTableName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{randomTableName}}
        {
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Equal(randomTableName, table.Name);
        Assert.Equal(randomTableName, table.ToString());
    }

    [Fact]
    public void Create_Returns_Table_With_Name_And_Schema()
    {
        string randomSchemaName = DataGenerator.CreateRandomString();
        string randomTableName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{randomSchemaName}}.{{randomTableName}}
        {
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Null(table.Database);
        Assert.Equal(randomSchemaName, table.Schema);
        Assert.Equal(randomTableName, table.Name);
        Assert.Equal($"{randomSchemaName}.{randomTableName}", table.ToString());
    }

    [Fact]
    public void Create_Returns_Table_With_Name_And_Schema_And_Database()
    {
        string randomDatabaseName = DataGenerator.CreateRandomString();
        string randomSchemaName = DataGenerator.CreateRandomString();
        string randomTableName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{randomDatabaseName}}.{{randomSchemaName}}.{{randomTableName}}
        {
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Equal(randomDatabaseName, table.Database);
        Assert.Equal(randomSchemaName, table.Schema);
        Assert.Equal(randomTableName, table.Name);
        Assert.Equal($"{randomDatabaseName}.{randomSchemaName}.{randomTableName}", table.ToString());
    }

    [Fact]
    public void Create_Returns_Table_With_Note()
    {
        const string text = """
        Table Users
        {
            note: 'This is a note.'
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        string note = Assert.Single(table.Notes);
        Assert.Equal("This is a note.", note);
        Assert.Equal("This is a note.", table.Note);
    }
}
