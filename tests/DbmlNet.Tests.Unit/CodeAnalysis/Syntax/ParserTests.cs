using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;

using Tynamix.ObjectFiller;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    private static ImmutableArray<Diagnostic> ParseDiagnostics(string text)
    {
        SyntaxTree syntaxTree = SyntaxTree.Parse(text);
        Assert.NotEmpty(syntaxTree.Diagnostics);
        return syntaxTree.Diagnostics;
    }

    private static MemberSyntax ParseMember(
        string text, string[]? diagnosticMessages = null)
    {
        MemberSyntax member = ParseMember(text, out ImmutableArray<Diagnostic> diagnostics);
        AssertDiagnostics(diagnosticMessages, diagnostics);
        return member;
    }

    private static MemberSyntax ParseMember(string text, out ImmutableArray<Diagnostic> diagnostics)
    {
        SyntaxTree syntaxTree = SyntaxTree.Parse(text);
        diagnostics = syntaxTree.Diagnostics;

        Assert.Equal(SyntaxKind.CompilationUnitMember, syntaxTree.Root.Kind);
        Assert.True(syntaxTree.Root.Members.Any(), "Expected at least one syntax member, currently zero.");
        MemberSyntax member = Assert.Single(syntaxTree.Root.Members);
        Assert.NotNull(syntaxTree.Root.EndOfFileToken);
        return member;
    }

    private static StatementSyntax ParseStatement(
        string text, string[]? diagnosticMessages = null)
    {
        StatementSyntax statement = ParseStatement(text, out ImmutableArray<Diagnostic> diagnostics);
        AssertDiagnostics(diagnosticMessages, diagnostics);
        return statement;
    }

    private static StatementSyntax ParseStatement(string text, out ImmutableArray<Diagnostic> diagnostics)
    {
        MemberSyntax member = ParseMember(text, out diagnostics);
        Assert.Equal(SyntaxKind.GlobalStatementMember, member.Kind);
        Assert.Single(member.GetChildren());
        return Assert.IsAssignableFrom<GlobalStatementSyntax>(member).Statement;
    }

    private static ExpressionSyntax ParseExpression(
        string text, string[]? diagnosticMessages = null)
    {
        ExpressionSyntax expression = ParseExpression(text, out ImmutableArray<Diagnostic> diagnostics);
        AssertDiagnostics(diagnosticMessages, diagnostics);
        return expression;
    }

    private static ExpressionSyntax ParseExpression(string text, out ImmutableArray<Diagnostic> diagnostics)
    {
        StatementSyntax statement = ParseStatement(text, out diagnostics);
        return Assert.IsAssignableFrom<ExpressionStatementSyntax>(statement).Expression;
    }

    private static void AssertDiagnostics(string[]? expectedMessages, ImmutableArray<Diagnostic> diagnostics)
    {
        Assert.True(
            (expectedMessages?.Length ?? 0) == diagnostics.Length,
            $"Expected {expectedMessages?.Length ?? 0} diagnostics, but got {diagnostics.Length}: \'{string.Join('\n', diagnostics.Select(d => $"\'{d}\'"))}\'.");

        for (int i = 0; i < diagnostics.Length; i++)
        {
            Diagnostic diagnostic = diagnostics[i];
            Assert.NotNull(expectedMessages);
            string diagnosticMessage = expectedMessages[i];
            Assert.Equal(diagnosticMessage, diagnostic.Message);
        }
    }

    private static BacktickExpressionSyntax ParseBacktickExpression(
        string text, string[]? diagnosticMessages = null)
    {
        StatementSyntax statement = ParseStatement(text, diagnosticMessages);

        ExpressionStatementSyntax expressionStatementSyntax =
            Assert.IsAssignableFrom<ExpressionStatementSyntax>(statement);

        return Assert.IsAssignableFrom<BacktickExpressionSyntax>(
                expressionStatementSyntax.Expression);
    }

    private static TableDeclarationSyntax ParseTableDeclaration(
        string text, string[]? diagnosticMessages = null)
    {
        MemberSyntax member = ParseMember(text, diagnosticMessages);

        return Assert.IsAssignableFrom<TableDeclarationSyntax>(member);
    }

    private static ColumnDeclarationSyntax ParseColumnDeclaration(
        string text, string[]? diagnosticMessages = null)
    {
        TableDeclarationSyntax tableDeclaration = ParseTableDeclaration(text, diagnosticMessages);

        BlockStatementSyntax tableBody =
            Assert.IsAssignableFrom<BlockStatementSyntax>(tableDeclaration.Body);

        StatementSyntax statement = Assert.Single(tableBody.Statements);

        return Assert.IsAssignableFrom<ColumnDeclarationSyntax>(statement);
    }

    private static SingleFieldIndexDeclarationSyntax ParseSingleFieldIndexDeclaration(
        string text, string[]? diagnosticMessages = null)
    {
        StatementSyntax statement = ParseStatement(text, diagnosticMessages);

        IndexesDeclarationSyntax indexesDeclarationSyntax =
            Assert.IsAssignableFrom<IndexesDeclarationSyntax>(statement);

        return Assert.IsAssignableFrom<SingleFieldIndexDeclarationSyntax>(
                Assert.Single(indexesDeclarationSyntax.Indexes));
    }

    private static CompositeIndexDeclarationSyntax ParseCompositeIndexDeclaration(
        string text, string[]? diagnosticMessages = null)
    {
        StatementSyntax statement = ParseStatement(text, diagnosticMessages);

        IndexesDeclarationSyntax indexesDeclarationSyntax =
            Assert.IsAssignableFrom<IndexesDeclarationSyntax>(statement);

        return Assert.IsAssignableFrom<CompositeIndexDeclarationSyntax>(
                Assert.Single(indexesDeclarationSyntax.Indexes));
    }

    private static ProjectSettingListSyntax ParseProjectSettingListClause(
        string text, string[]? diagnosticMessages = null)
    {
        MemberSyntax member = ParseMember(text, diagnosticMessages);

        ProjectDeclarationSyntax projectDeclarationMember =
            Assert.IsAssignableFrom<ProjectDeclarationSyntax>(member);

        return projectDeclarationMember.Settings;
    }

    private static ColumnSettingListSyntax ParseColumnSettingListClause(
        string text, string[]? diagnosticMessages = null)
    {
        StatementSyntax statement = ParseStatement(text, diagnosticMessages);
        ColumnDeclarationSyntax columnDeclarationStatement =
            Assert.IsAssignableFrom<ColumnDeclarationSyntax>(statement);
        Assert.NotNull(columnDeclarationStatement.SettingList);
        return columnDeclarationStatement.SettingList;
    }

    private static EnumDeclarationSyntax ParseEnumDeclaration(
           string text, string[]? diagnosticMessages = null)
    {
        MemberSyntax member = ParseMember(text, diagnosticMessages);

        return Assert.IsAssignableFrom<EnumDeclarationSyntax>(member);
    }

    private static EnumEntryDeclarationSyntax ParseEnumEntryDeclaration(
        string text, string[]? diagnosticMessages = null)
    {
        EnumDeclarationSyntax enumDeclaration = ParseEnumDeclaration(text, diagnosticMessages);

        BlockStatementSyntax enumBody =
            Assert.IsAssignableFrom<BlockStatementSyntax>(enumDeclaration.Body);

        StatementSyntax statement = Assert.Single(enumBody.Statements);

        return Assert.IsAssignableFrom<EnumEntryDeclarationSyntax>(statement);
    }

    private static EnumEntrySettingListSyntax ParseEnumEntrySettingListClause(
        string text, string[]? diagnosticMessages = null)
    {
        StatementSyntax statement = ParseStatement(text, diagnosticMessages);
        EnumEntryDeclarationSyntax enumEntryDeclarationStatement =
            Assert.IsAssignableFrom<EnumEntryDeclarationSyntax>(statement);
        Assert.NotNull(enumEntryDeclarationStatement.SettingList);
        return enumEntryDeclarationStatement.SettingList;
    }

    private static readonly string[] SqlServerDataTypes = new string[]
    {
        // Exact numerics: These are data types that store integer or decimal numbers with exact precision and scale.
        "bigint",
        "bit",
        "decimal",
        "int",
        "money",
        "numeric",
        "smallint",
        "smallmoney",
        "tinyint",

        // Approximate numerics: These are data types that store floating-point numbers with approximate precision and scale.
        "float",
        "real",

        // Date and time: These are data types that store date and time values with various levels of accuracy and range.
        "date",
        "datetime2",
        "datetime",
        "datetimeoffset",
        "smalldatetime",
        "time",

        // Character strings: These are data types that store character data of fixed or variable length.
        "char",
        "char(1)",
        "char(8000)",

        "varchar",
        "varchar(1)",
        "varchar(8000)",
        "varchar(MAX)",

        "text",

        // Unicode character strings: These are data types that store Unicode character data of fixed or variable length.
        "ncar",
        "ncar(1)",
        "ncar(8000)",

        "nvarchar",
        "nvarchar(1)",
        "nvarchar(8000)",
        "nvarchar(MAX)",

        "ntext",
        "nenum",

        // Binary strings: These are data types that store binary data of fixed or variable length.
        "binary",
        "varbinary",
        "blob",
        "image",
    };

    public static IEnumerable<object[]?> GetSyntaxKeywordTokensData()
    {
        foreach ((SyntaxKind itemKind, string itemText, object? itemValue) in DataGenerator.GetSyntaxKeywords())
        {
            yield return new object[] { itemKind, itemText, itemValue! };
        }
    }

    public static IEnumerable<object[]?> GetSyntaxKeywordsTextData()
    {
        foreach ((_, string itemText, _) in DataGenerator.GetSyntaxKeywords())
        {
            yield return new object[] { itemText };
        }
    }
}
