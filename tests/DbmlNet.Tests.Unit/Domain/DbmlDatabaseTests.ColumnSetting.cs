using System.Collections.Generic;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_ColumnSetting_With_QuotationMarksString_Note()
    {
        string noteValueText = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ note: "{{noteValueText}}" ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        string note = Assert.Single(column.Notes);
        Assert.Equal(noteValueText, note);
        Assert.Equal(noteValueText, column.Note);
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_SingleQuotationMarksString_Note()
    {
        string noteValueText = DataGenerator.CreateRandomMultiWordString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ note: '{{noteValueText}}' ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        string note = Assert.Single(column.Notes);
        Assert.Equal(noteValueText, note);
        Assert.Equal(noteValueText, column.Note);
    }

    [Theory]
    [InlineData("pk")]
    [InlineData("primary key")]
    public void Create_Returns_ColumnSetting_With_PrimaryKey_Flag(string primaryKeyText)
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ {{primaryKeyText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsPrimaryKey, "Column should be primary key");
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_Unique_Flag()
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ unique ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsUnique, "Column should be unique");
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_Increment_Flag()
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ increment ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsAutoIncrement, "Column should be auto increment");
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_Nullable_Flag()
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ null ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsNullable, "Column should be nullable");
        Assert.False(column.IsRequired, "Column should not be required");
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_Not_Nullable_Flag()
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ not null ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.False(column.IsNullable, "Column should not be nullable");
        Assert.True(column.IsRequired, "Column should be required");
    }

    [Theory]
    [InlineData("123", "123")]
    [InlineData("123.456", "123.456")]
    [InlineData("true", "true")]
    [InlineData("false", "false")]
    [InlineData("null", null)]
    [InlineData("'some string value'", "some string value")]
    [InlineData("\"some string value\"", "some string value")]
    [InlineData("`now()`", null)]
    public void Create_Returns_ColumnSetting_With_Default_Value(string valueText, object? value)
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ default: {{valueText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        if (value is null)
            Assert.False(column.HasDefaultValue, "Column should not have default value");
        else
            Assert.True(column.HasDefaultValue, "Column should have default value");
        Assert.Equal(value, column.DefaultValue);
    }

    [Theory]
    [InlineData("<", TableRelationshipType.OneToMany)]
    [InlineData(">", TableRelationshipType.ManyToOne)]
    [InlineData("-", TableRelationshipType.OneToOne)]
    [InlineData("<>", TableRelationshipType.ManyToMany)]
    public void Create_Returns_ColumnSetting_With_Relationship(
        string relationshipText, TableRelationshipType relationshipType)
    {
        string fromDatabaseName = DataGenerator.CreateRandomString();
        string fromSchemaName = DataGenerator.CreateRandomString();
        string fromTableName = DataGenerator.CreateRandomString();
        string fromColumnName = DataGenerator.CreateRandomString();
        string toColumnName = $"{DataGenerator.CreateRandomString()}.{DataGenerator.CreateRandomString()}.{DataGenerator.CreateRandomString()}";
        string text = $$"""
        Table {{fromDatabaseName}}.{{fromSchemaName}}.{{fromTableName}}
        {
            {{fromColumnName}} {{DataGenerator.CreateRandomString()}} [ ref: {{relationshipText}} {{toColumnName}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableRelationship relationship = Assert.Single(table.Relationships);
        Assert.Equal(relationshipType, relationship.RelationshipType);
        Assert.Equal($"{fromSchemaName}.{fromTableName}.{fromColumnName}", relationship.FromIdentifier.ToString());
        Assert.Equal(toColumnName, relationship.ToIdentifier.ToString());
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_OneToMany_Relationship()
    {
        string tableName = DataGenerator.CreateRandomString();
        string fromColumnName = DataGenerator.CreateRandomString();
        string toColumnName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{tableName}}
        {
            {{fromColumnName}} {{DataGenerator.CreateRandomString()}} [ ref: < {{toColumnName}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableRelationship relationship = Assert.Single(table.Relationships);
        Assert.Equal(TableRelationshipType.OneToMany, relationship.RelationshipType);
        Assert.Equal($"{tableName}.{fromColumnName}", relationship.FromIdentifier.ToString());
        Assert.Equal(toColumnName, relationship.ToIdentifier.ToString());
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_ManyToOne_Relationship()
    {
        string tableName = DataGenerator.CreateRandomString();
        string fromColumnName = DataGenerator.CreateRandomString();
        string toColumnName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{tableName}}
        {
            {{fromColumnName}} {{DataGenerator.CreateRandomString()}} [ ref: > {{toColumnName}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableRelationship relationship = Assert.Single(table.Relationships);
        Assert.Equal(TableRelationshipType.ManyToOne, relationship.RelationshipType);
        Assert.Equal($"{tableName}.{fromColumnName}", relationship.FromIdentifier.ToString());
        Assert.Equal(toColumnName, relationship.ToIdentifier.ToString());
    }

    public static IEnumerable<object[]?> GetAllowedKeywordsInUnknownColumnSettingClauseData()
    {
        foreach ((_, string itemText, _) in DataGenerator.GetSyntaxKeywords())
        {
            yield return new object[] { itemText };
        }
    }

    [Theory]
    [MemberData(nameof(GetAllowedKeywordsInUnknownColumnSettingClauseData))]
    [Trait("BUG", "https://github.com/Catalin-Andronie/dbml.net/issues/51")]
    public void Create_Returns_ColumnSetting_With_ManyToOne_Relationship_And_Keyword_ToColumnName(
        string toColumnName)
    {
        string tableName = DataGenerator.CreateRandomString();
        string fromColumnName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{tableName}}
        {
            {{fromColumnName}} {{DataGenerator.CreateRandomString()}} [ ref: > {{toColumnName}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableRelationship relationship = Assert.Single(table.Relationships);
        Assert.Equal(TableRelationshipType.ManyToOne, relationship.RelationshipType);
        Assert.Equal($"{tableName}.{fromColumnName}", relationship.FromIdentifier.ToString());
        Assert.Equal(toColumnName, relationship.ToIdentifier.ToString());
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_OneToOne_Relationship()
    {
        string tableName = DataGenerator.CreateRandomString();
        string fromColumnName = DataGenerator.CreateRandomString();
        string toColumnName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{tableName}}
        {
            {{fromColumnName}} {{DataGenerator.CreateRandomString()}} [ ref: - {{toColumnName}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableRelationship relationship = Assert.Single(table.Relationships);
        Assert.Equal(TableRelationshipType.OneToOne, relationship.RelationshipType);
        Assert.Equal($"{tableName}.{fromColumnName}", relationship.FromIdentifier.ToString());
        Assert.Equal(toColumnName, relationship.ToIdentifier.ToString());
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_ManyToMany_Relationship()
    {
        string tableName = DataGenerator.CreateRandomString();
        string fromColumnName = DataGenerator.CreateRandomString();
        string toColumnName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{tableName}}
        {
            {{fromColumnName}} {{DataGenerator.CreateRandomString()}} [ ref: <> {{toColumnName}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableRelationship relationship = Assert.Single(table.Relationships);
        Assert.Equal(TableRelationshipType.ManyToMany, relationship.RelationshipType);
        Assert.Equal($"{tableName}.{fromColumnName}", relationship.FromIdentifier.ToString());
        Assert.Equal(toColumnName, relationship.ToIdentifier.ToString());
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_Unknown_Setting()
    {
        string settingName = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string settingText = $"{settingName}";
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown column setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(column.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_Unknown_Setting_Identifier_Value()
    {
        string settingName = DataGenerator.CreateRandomString();
        object? settingValue = DataGenerator.CreateRandomString();
        string settingText = $"{settingName}: {settingValue}";
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown column setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(column.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_Unknown_Setting_QuotationMarksString_Value()
    {
        string settingName = DataGenerator.CreateRandomString();
        object? settingValue = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"{settingName}: \"{settingValue}\"";
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown column setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(column.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }

    [Fact]
    public void Create_Returns_ColumnSetting_With_Unknown_Setting_SingleQuotationMarksString_Value()
    {
        string settingName = DataGenerator.CreateRandomString();
        object? settingValue = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"{settingName}: '{settingValue}'";
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ {{settingText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown column setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(column.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }
}
