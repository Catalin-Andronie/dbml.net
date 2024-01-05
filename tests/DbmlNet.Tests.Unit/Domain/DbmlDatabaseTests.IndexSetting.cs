using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_IndexSetting_With_QuotationMarksString_Note()
    {
        string noteValue = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ note: "{{noteValue}}" ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(noteValue, index.Note);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_SingleQuotationMarksString_Note()
    {
        string noteValue = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ note: '{{noteValue}}' ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(noteValue, index.Note);
    }

    [Theory]
    [InlineData("pk")]
    [InlineData("primary key")]
    public void Create_Returns_IndexSetting_With_PrimaryKey_Flag(string settingText)
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.True(index.IsPrimaryKey, "Column should be primary key");
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_Unique_Flag()
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ unique ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.True(index.IsUnique, "Column should be unique");
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_QuotationMarksString_Name()
    {
        string columnName = DataGenerator.CreateRandomString();
        string indexName = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{columnName}} [ name: "{{indexName}}" ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(indexName, index.Name);
        Assert.Equal(columnName, index.ColumnName);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_SingleQuotationMarksString_Name()
    {
        string columnName = DataGenerator.CreateRandomString();
        string indexName = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{columnName}} [ name: '{{indexName}}' ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(indexName, index.Name);
        Assert.Equal(columnName, index.ColumnName);
    }

    [Theory]
    [InlineData("btree")]
    [InlineData("gin")]
    [InlineData("gist")]
    [InlineData("hash")]
    public void Create_Returns_IndexSetting_With_Known_Type(string typeName)
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ type: {{typeName}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(typeName, index.Type);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_Unknown_Identifier_Type()
    {
        string typeName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ type: {{typeName}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown index setting type '{typeName}'. Allowed index types [btree|gin|gist|hash].", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(typeName, index.Type);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_Unknown_QuotationMarksString_Type()
    {
        string typeName = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ type: "{{typeName}}" ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown index setting type '{typeName}'. Allowed index types [btree|gin|gist|hash].", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(typeName, index.Type);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_Unknown_SingleQuotationMarksString_Type()
    {
        string typeName = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ type: '{{typeName}}' ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown index setting type '{typeName}'. Allowed index types [btree|gin|gist|hash].", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(typeName, index.Type);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_Unknown_Setting()
    {
        string settingName = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string settingText = $"{settingName}";
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown index setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(index.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_Unknown_Setting_Identifier_Value()
    {
        string settingName = DataGenerator.CreateRandomString();
        object? settingValue = DataGenerator.CreateRandomString();
        string settingText = $"{settingName}: {settingValue}";
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown index setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(index.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_Unknown_Setting_QuotationMarksString_Value()
    {
        string settingName = DataGenerator.CreateRandomString();
        object? settingValue = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"{settingName}: \"{settingValue}\"";
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown index setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(index.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }

    [Fact]
    public void Create_Returns_IndexSetting_With_Unknown_Setting_SingleQuotationMarksString_Value()
    {
        string settingName = DataGenerator.CreateRandomString();
        object? settingValue = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"{settingName}: '{settingValue}'";
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            Indexes {
                {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown index setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(index.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }
}
