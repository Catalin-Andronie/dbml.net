using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Column_Empty()
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{DataGenerator.CreateRandomString()}} [ ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.NotNull(column.Table);
        Assert.Equal(table, column.Table);
        Assert.Null(column.MaxLength);
        Assert.False(column.HasMaxLength, "Column should not have max length");
        Assert.False(column.IsPrimaryKey, "Column should not be primary key");
        Assert.False(column.IsUnique, "Column should not be unique");
        Assert.False(column.IsAutoIncrement, "Column should not be auto increment");
        Assert.False(column.IsNullable, "Column should not be nullable");
        Assert.True(column.IsRequired, "Column should be required");
        Assert.False(column.HasDefaultValue, "Column should not have default value");
        Assert.Null(column.DefaultValue);
        Assert.Empty(column.UnknownSettings);
        Assert.Null(column.Note);
        Assert.Empty(column.Notes);
    }

    [Theory]
    [InlineData("unknownType", null)]
    [InlineData("bit", "false")]
    [InlineData("int", "0")]
    [InlineData("float", "0")]
    public void Create_Returns_Column_With_Name_And_Type(string columnType, string? defaultValue)
    {
        string randomColumnName = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{randomColumnName}} {{columnType}}
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.Equal(randomColumnName, column.Name);
        Assert.Equal(randomColumnName, column.ToString());
        Assert.Equal(columnType, column.Type);
        Assert.Equal(defaultValue, column.DefaultValue);
    }

    [Theory]
    [InlineData("unknownType(1)", null)]
    [InlineData("datetimeoffset(1)", 3.155378976E+18)]
    [InlineData("binary(MAX)", 1.7976931348623157E+308)]
    [InlineData("varbinary(MAX)", 1.7976931348623157E+308)]
    [InlineData("char(MAX)", 1.7976931348623157E+308)]
    [InlineData("varchar(MAX)", 1.7976931348623157E+308)]
    [InlineData("nchar(MAX)", 1.7976931348623157E+308)]
    [InlineData("nvarchar(MAX)", 1.7976931348623157E+308)]
    [InlineData("binary(123)", 123D)]
    [InlineData("varbinary(123)", 123D)]
    [InlineData("char(123)", 123D)]
    [InlineData("varchar(123)", 123D)]
    [InlineData("nchar(123)", 123D)]
    [InlineData("nvarchar(123)", 123D)]
    [InlineData("binary(123.456)", 123.456D)]
    [InlineData("varbinary(123.456)", 123.456D)]
    [InlineData("char(123.456)", 123.456D)]
    [InlineData("varchar(123.456)", 123.456D)]
    [InlineData("nchar(123.456)", 123.456D)]
    [InlineData("nvarchar(123.456)", 123.456D)]
    public void Create_Returns_Column_With_Max_Length(string columnTypeText, object? maxLength)
    {
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{DataGenerator.CreateRandomString()}} {{columnTypeText}}
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        if (double.MaxValue.Equals(maxLength))
            Assert.True(column.HasMaxLength, "Column should have max length");
        else
            Assert.False(column.HasMaxLength, "Column should not have max length");
        Assert.Equal(maxLength, column.MaxLength);
    }
}
