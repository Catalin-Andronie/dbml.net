using System.Linq;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Project_Empty()
    {
        string randomProjectName = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Project "{{randomProjectName}}" {
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.NotNull(database.Project);
        Assert.Equal(randomProjectName, database.Project.Name);
        Assert.Equal(randomProjectName, database.Project.ToString());
        Assert.Empty(database.Project.Note);
        Assert.Empty(database.Project.Notes);
    }

    [Fact]
    public void Create_Returns_Project_With_Name()
    {
        string randomProjectName = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Project "{{randomProjectName}}" {
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.NotNull(database.Project);
        Assert.Equal(randomProjectName, database.Project.Name);
        Assert.Equal(randomProjectName, database.Project.ToString());
    }

    [Fact]
    public void Create_Returns_Project_With_Note()
    {
        string randomNoteText = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Project "{{DataGenerator.CreateRandomMultiWordString()}}" {
            note: '{{randomNoteText}}'
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.NotNull(database.Project);
        string note = Assert.Single(database.Project.Notes);
        Assert.Equal(randomNoteText, note);
        Assert.Equal(randomNoteText, database.Project.Note);
    }

    [Fact]
    public void Create_Returns_Project_With_Database_Provider()
    {
        string randomDatabaseTypeText = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Project "{{DataGenerator.CreateRandomMultiWordString()}}" {
            database_type: '{{randomDatabaseTypeText}}'
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        string provider = Assert.Single(database.Providers);
        Assert.Equal(randomDatabaseTypeText, provider);
    }

    [Fact]
    public void Create_Returns_Project_With_All_Settings()
    {
        string randomProjectText = DataGenerator.CreateRandomMultiWordString();
        string randomDatabaseTypeText = DataGenerator.CreateRandomMultiWordString();
        string randomNoteText = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Project "{{randomProjectText}}" {
            database_type: '{{randomDatabaseTypeText}}'
            note: '{{randomNoteText}}'
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.Single(database.Providers);
        Assert.Equal(randomDatabaseTypeText, database.Providers.ElementAt(0));
        Assert.NotNull(database.Project);
        Assert.Equal(randomProjectText, database.Project.Name);
        Assert.Equal(randomProjectText, database.Project.ToString());
        string note = Assert.Single(database.Project.Notes);
        Assert.Equal(randomNoteText, note);
        Assert.Equal(randomNoteText, database.Project.Note);
    }
}
