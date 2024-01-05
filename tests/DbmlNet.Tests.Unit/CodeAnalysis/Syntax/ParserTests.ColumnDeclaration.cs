using System;
using System.Collections.Generic;
using System.Globalization;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_ColumnDeclaration_With_Name_Identifier()
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        const SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_ColumnDeclaration_With_Name_Keyword(
        SyntaxKind columnNameKind,
        string columnNameText,
        object? columnNameValue)
    {
        const SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText, columnNameValue);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_Name_QuotationMarksString()
    {
        const SyntaxKind columnNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomColumnName = DataGenerator.CreateRandomMultiWordString();
        string columnNameText = $"\"{randomColumnName}\"";
        object? columnNameValue = randomColumnName;
        const SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText, columnNameValue);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_Name_SingleQuotationMarksString()
    {
        const SyntaxKind columnNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomColumName = DataGenerator.CreateRandomMultiWordString();
        string columnNameText = $"\'{randomColumName}\'";
        object? columnNameValue = randomColumName;
        const SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText, columnNameValue);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_ColumnType_Identifier()
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        const SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_ColumnDeclaration_With_ColumnType_Keyword(
        SyntaxKind columnTypeKind,
        string columnTypeText,
        object? columnTypeValue)
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText, columnTypeValue);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_ColumnType_QuotationMarksString()
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        const SyntaxKind columnTypeKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string columnTypeText = $"\"{randomText}\"";
        object columnTypeValue = randomText;
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText, columnTypeValue);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_ColumnType_SingleQuotationMarksString()
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        const SyntaxKind columnTypeKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string columnTypeText = $"\'{randomText}\'";
        object? columnTypeValue = randomText;
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText, columnTypeValue);
    }

    [Theory]
    [MemberData(nameof(GetSqlServerColumnTypeIdentifiersData))]
    public void Parse_ColumnDeclaration_With_ColumnType_SqlServerIdentifier(
        SyntaxKind columnTypeKind,
        string columnTypeText,
        object? columnTypeValue)
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText, columnTypeValue);
    }

    public static IEnumerable<object?[]> GetSqlServerColumnTypeIdentifiersData()
    {
        foreach (string text in DataGenerator.SqlServerDataTypes)
        {
            // Skip parenthesized identifiers
            if (text.Contains('(', StringComparison.Ordinal))
                continue;

            yield return new object?[] { SyntaxKind.IdentifierToken, text, null };
        }
    }

    [Theory]
    [MemberData(nameof(GetSqlServerColumnTypeParenthesizedIdentifiersData))]
    public void Parse_ColumnDeclaration_With_ColumnType_SqlServerParenthesizedIdentifier(
        SyntaxKind columnTypeIdentifierKind,
        string columnTypeIdentifierText,
        object? columnTypeIdentifierValue,
        SyntaxKind variableLengthIdentifierKind,
        string variableLengthIdentifierText,
        object? variableLengthIdentifierValue)
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeIdentifierText}}({{variableLengthIdentifierText}})
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeParenthesizedIdentifierClause);
        e.AssertToken(columnTypeIdentifierKind, columnTypeIdentifierText, columnTypeIdentifierValue);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertToken(variableLengthIdentifierKind, variableLengthIdentifierText, variableLengthIdentifierValue);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    public static IEnumerable<object?[]> GetSqlServerColumnTypeParenthesizedIdentifiersData()
    {
        foreach (string text in DataGenerator.SqlServerDataTypes)
        {
            // Skip non parenthesized identifiers
            if (!text.Contains('(', StringComparison.Ordinal))
                continue;

            int indefOfOpenParenthesis = text.IndexOf('(', StringComparison.Ordinal);
            int indefOfCloseParenthesis = text.IndexOf(')', StringComparison.Ordinal);
            string columnTypeIdentifierText = text[..indefOfOpenParenthesis];
            SyntaxKind columnTypeIdentifierKind = SyntaxFacts.GetKeywordKind(columnTypeIdentifierText);
            object? columnTypeIdentifierValue = columnTypeIdentifierKind.GetKnownValue();

            string variableLengthIdentifierText = text[(indefOfOpenParenthesis + 1)..indefOfCloseParenthesis];
            SyntaxKind variableLengthIdentifierKind = SyntaxFacts.GetKeywordKind(columnTypeIdentifierText);
            object? variableLengthIdentifierValue = variableLengthIdentifierKind.GetKnownValue();
            if (decimal.TryParse(variableLengthIdentifierText, NumberFormatInfo.InvariantInfo, out decimal dVal))
            {
                variableLengthIdentifierKind = SyntaxKind.NumberToken;
                variableLengthIdentifierValue = dVal;
            }

            yield return new object?[]
            {
                columnTypeIdentifierKind, columnTypeIdentifierText, columnTypeIdentifierValue,
                variableLengthIdentifierKind, variableLengthIdentifierText, variableLengthIdentifierValue,
            };
        }
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_No_Settings()
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        const SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}}
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_Empty_Settings()
    {
        const SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        const SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $$"""
        Table {{DataGenerator.CreateRandomString()}}
        {
            {{columnNameText}} {{columnTypeText}} [ ]
        }
        """;

        StatementSyntax statement = ParseColumnDeclaration(text);

        using AssertingEnumerator e = new(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
