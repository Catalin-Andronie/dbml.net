using System.Collections.Immutable;

using DbmlNet.CodeAnalysis;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Theory]
    [InlineData("!")]
    [InlineData("?")]
    [InlineData("|")]
    public void Parse_Skip_BadToken(string text)
    {
        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        string expectedDiagnosticMessage = $"Bad character input: '{text}'.";
        Assert.Equal(expectedDiagnosticMessage, diagnostic.Message);
        Assert.Equal(expectedDiagnosticMessage, $"{diagnostic}");
        Assert.True(diagnostic.IsError, "Diagnostic should be an error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic should not be an warning.");
        Assert.Equal(0, diagnostic.Location.StartLine);
        Assert.Equal(0, diagnostic.Location.EndLine);
        Assert.Equal(0, diagnostic.Location.StartCharacter);
        Assert.Equal(1, diagnostic.Location.EndCharacter);
        Assert.Equal(0, diagnostic.Location.Span.Start);
        Assert.Equal(1, diagnostic.Location.Span.End);
    }

    [Fact]
    public void Parse_Error_Disallowed_Default_ColumnSetting_Value_NameExpression()
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}}
            [
                default: {{DataGenerator.CreateRandomString()}}
            ]
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.True(diagnostic.IsError, "Should not be error");
        Assert.False(diagnostic.IsWarning, "Should be warning");
        Assert.Equal("Disallowed 'default' column setting value expression 'NameExpression'.", diagnostic.Message);
    }

    [Fact]
    public void Parse_Warning_Unknown_ProjectSetting()
    {
        string settingNameText = DataGenerator.CreateRandomString();
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingNameText + "}";

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        string expectedDiagnosticMessage = $"Unknown project setting '{settingNameText}'.";
        Assert.Equal(expectedDiagnosticMessage, diagnostic.Message);
        Assert.Equal(expectedDiagnosticMessage, $"{diagnostic}");
        Assert.True(diagnostic.IsWarning, "Diagnostic should be warning.");
        Assert.False(diagnostic.IsError, "Diagnostic should not be error.");
    }

    [Fact]
    public void Parse_Warning_Unknown_ColumnSetting()
    {
        string settingNameText = DataGenerator.CreateRandomString();
        string text = $"{DataGenerator.CreateRandomString()} {DataGenerator.CreateRandomString()} [ {settingNameText} ]";

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        string expectedDiagnosticMessage = $"Unknown column setting '{settingNameText}'.";
        Assert.Equal(expectedDiagnosticMessage, diagnostic.Message);
        Assert.Equal(expectedDiagnosticMessage, $"{diagnostic}");
        Assert.True(diagnostic.IsWarning, "Diagnostic should be warning.");
        Assert.False(diagnostic.IsError, "Diagnostic should not be error.");
    }

    [Fact]
    public void Parse_Warning_Table_Already_Declared()
    {
        string randomText = DataGenerator.CreateRandomString();
        string firstTableName = randomText;
        string secondTableName = randomText;
        string text = $$"""
        Table {{firstTableName}} { }
        Table {{secondTableName}} { }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Table '{secondTableName}' already declared.", diagnostic.Message);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordsTextData))]
    public void Parse_Warning_Table_Already_Declared_For_Keyword_ColumnName(
        string tableNameText)
    {
        string text = $$"""
        Table {{tableNameText}} { }
        Table {{tableNameText}} { }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Table '{tableNameText}' already declared.", diagnostic.Message);
    }

    [Fact]
    public void Parse_Warning_Column_Already_Declared()
    {
        string randomText = DataGenerator.CreateRandomString();
        string firstColumnName = randomText;
        string secondColumnName = randomText;
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{firstColumnName}} {{DataGenerator.CreateRandomString()}}
            {{secondColumnName}} {{DataGenerator.CreateRandomString()}}
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Column '{secondColumnName}' already declared.", diagnostic.Message);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordsTextData))]
    public void Parse_Warning_Column_Already_Declared_For_Keyword_ColumnName(
        string columnNameText)
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{DataGenerator.CreateRandomString()}}
            {{columnNameText}} {{DataGenerator.CreateRandomString()}}
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Column '{columnNameText}' already declared.", diagnostic.Message);
    }

    [Theory]
    [InlineData("pk", "pk")]
    [InlineData("primarykey", "primary key")]
    [InlineData("null", "null")]
    [InlineData("notnull", "not null")]
    [InlineData("unique", "unique")]
    [InlineData("increment", "increment")]
    [InlineData("default", "default: \"Some value\"")]
    [InlineData("default", "default: \'Some value\'")]
    [InlineData("note", "note: \"Some value\"")]
    [InlineData("note", "note: \'Some value\'")]
    [InlineData("note", "Note: \"Some value\"")]
    [InlineData("note", "Note: \'Some value\'")]
    public void Parse_Warning_ColumnSetting_Already_Declared(
        string settingName, string settingText)
    {
        string text = $$"""
        {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ {{settingText}}, {{settingText}} ]
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Column setting '{settingName}' already declared.", diagnostic.Message);
    }

    [Theory]
    [InlineData("pk", "pk")]
    [InlineData("primarykey", "primary key")]
    [InlineData("unique", "unique")]
    [InlineData("name", "name: Some_value")]
    [InlineData("name", "name: \"Some value\"")]
    [InlineData("name", "name: \'Some value\'")]
    [InlineData("type", "type: btree")]
    [InlineData("type", "type: \"btree\"")]
    [InlineData("type", "type: \'btree\'")]
    [InlineData("type", "type: gin")]
    [InlineData("type", "type: \"gin\"")]
    [InlineData("type", "type: \'gin\'")]
    [InlineData("type", "type: gist")]
    [InlineData("type", "type: \"gist\"")]
    [InlineData("type", "type: \'gist\'")]
    [InlineData("type", "type: hash")]
    [InlineData("type", "type: \"hash\"")]
    [InlineData("type", "type: \'hash\'")]
    public void Parse_Warning_IndexSetting_Already_Declared(
        string settingName, string settingText)
    {
        string text = $$"""
        indexes
        {
            {{DataGenerator.CreateRandomString()}} [ {{settingText}}, {{settingText}} ]
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Index setting '{settingName}' already declared.", diagnostic.Message);
    }

    [Fact]
    public void Parse_Warning_EnumEntry_Already_Declared()
    {
        string randomText = DataGenerator.CreateRandomString();
        string firstEnumEntryName = randomText;
        string secondEnumEntryName = randomText;
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{firstEnumEntryName}}
            {{secondEnumEntryName}}
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Enum entry '{secondEnumEntryName}' already declared.", diagnostic.Message);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordsTextData))]
    public void Parse_Warning_EnumEntry_Already_Declared_For_Keyword_EnumEntryName(
        string enumEntryNameText)
    {
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}}
            {{enumEntryNameText}}
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Enum entry '{enumEntryNameText}' already declared.", diagnostic.Message);
    }

    [Theory]
    [InlineData("note", "note: \"Some value\"")]
    [InlineData("note", "note: \'Some value\'")]
    [InlineData("note", "Note: \"Some value\"")]
    [InlineData("note", "Note: \'Some value\'")]
    public void Parse_Warning_EnumEntrySetting_Already_Declared(
        string settingName, string settingText)
    {
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} [ {{settingText}}, {{settingText}} ]
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Enum entry setting '{settingName}' already declared.", diagnostic.Message);
    }
}
