using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Tests.Core;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_TableDeclaration_With_Name_Identifier()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        string text = $"Table {tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_Identifier_And_Alias()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind aliasNameKind = SyntaxKind.IdentifierToken;
        string aliasNameText = DataGenerator.CreateRandomString();
        object? aliasNameValue = null;
        string text = $"Table {tableNameText} as {aliasNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableAliasClause);
        e.AssertToken(SyntaxKind.AsKeyword, "as");
        e.AssertToken(aliasNameKind, aliasNameText, aliasNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_TableDeclaration_With_Name_Keyword(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        string text = $"Table {keywordText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_TableDeclaration_With_Name_Keyword_And_Alias(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        const SyntaxKind aliasNameKind = SyntaxKind.IdentifierToken;
        string aliasNameText = DataGenerator.CreateRandomString();
        object? aliasNameValue = null;
        string text = $"Table {keywordText} as {aliasNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.TableAliasClause);
        e.AssertToken(SyntaxKind.AsKeyword, "as");
        e.AssertToken(aliasNameKind, aliasNameText, aliasNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_And_Schema_Identifier()
    {
        const SyntaxKind schemaNameKind = SyntaxKind.IdentifierToken;
        string schemaNameText = DataGenerator.CreateRandomString();
        object? schemaNameValue = null;
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        string text = $"Table {schemaNameText}.{tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(schemaNameKind, schemaNameText, schemaNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_And_Schema_Identifier_And_Alias()
    {
        const SyntaxKind schemaNameKind = SyntaxKind.IdentifierToken;
        string schemaNameText = DataGenerator.CreateRandomString();
        object? schemaNameValue = null;
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind aliasNameKind = SyntaxKind.IdentifierToken;
        string aliasNameText = DataGenerator.CreateRandomString();
        object? aliasNameValue = null;
        string text = $"Table {schemaNameText}.{tableNameText} as {aliasNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(schemaNameKind, schemaNameText, schemaNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableAliasClause);
        e.AssertToken(SyntaxKind.AsKeyword, "as");
        e.AssertToken(aliasNameKind, aliasNameText, aliasNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_TableDeclaration_With_Name_And_Schema_Keyword(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        string text = $"Table {keywordText}.{keywordText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_TableDeclaration_With_Name_And_Schema_Keyword_And_Alias(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        const SyntaxKind aliasNameKind = SyntaxKind.IdentifierToken;
        string aliasNameText = DataGenerator.CreateRandomString();
        object? aliasNameValue = null;
        string text = $"Table {keywordText}.{keywordText} as {aliasNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.TableAliasClause);
        e.AssertToken(SyntaxKind.AsKeyword, "as");
        e.AssertToken(aliasNameKind, aliasNameText, aliasNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_And_Schema_And_Database_Identifier()
    {
        const SyntaxKind databaseNameKind = SyntaxKind.IdentifierToken;
        string databaseNameText = DataGenerator.CreateRandomString();
        object? databaseNameValue = null;
        const SyntaxKind schemaNameKind = SyntaxKind.IdentifierToken;
        string schemaNameText = DataGenerator.CreateRandomString();
        object? schemaNameValue = null;
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        string text = $"Table {databaseNameText}.{schemaNameText}.{tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(databaseNameKind, databaseNameText, databaseNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(schemaNameKind, schemaNameText, schemaNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_And_Schema_And_Database_Identifier_And_Alias()
    {
        const SyntaxKind databaseNameKind = SyntaxKind.IdentifierToken;
        string databaseNameText = DataGenerator.CreateRandomString();
        object? databaseNameValue = null;
        const SyntaxKind schemaNameKind = SyntaxKind.IdentifierToken;
        string schemaNameText = DataGenerator.CreateRandomString();
        object? schemaNameValue = null;
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind aliasNameKind = SyntaxKind.IdentifierToken;
        string aliasNameText = DataGenerator.CreateRandomString();
        object? aliasNameValue = null;
        string text = $"Table {databaseNameText}.{schemaNameText}.{tableNameText} as {aliasNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(databaseNameKind, databaseNameText, databaseNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(schemaNameKind, schemaNameText, schemaNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableAliasClause);
        e.AssertToken(SyntaxKind.AsKeyword, "as");
        e.AssertToken(aliasNameKind, aliasNameText, aliasNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_TableDeclaration_With_Name_And_Schema_And_Database_Keyword(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        string text = $"Table {keywordText}.{keywordText}.{keywordText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_TableDeclaration_With_Name_And_Schema_And_Database_Keyword_And_Alias(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        const SyntaxKind aliasNameKind = SyntaxKind.IdentifierToken;
        string aliasNameText = DataGenerator.CreateRandomString();
        object? aliasNameValue = null;
        string text = $"Table {keywordText}.{keywordText}.{keywordText} as {aliasNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.TableAliasClause);
        e.AssertToken(SyntaxKind.AsKeyword, "as");
        e.AssertToken(aliasNameKind, aliasNameText, aliasNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Empty_Body()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        string text = $"Table {tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Empty_Body_And_Alias()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind aliasNameKind = SyntaxKind.IdentifierToken;
        string aliasNameText = DataGenerator.CreateRandomString();
        object? aliasNameValue = null;
        string text = $"Table {tableNameText} as {aliasNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableAliasClause);
        e.AssertToken(SyntaxKind.AsKeyword, "as");
        e.AssertToken(aliasNameKind, aliasNameText, aliasNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Note_QuotationMarksString()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind noteValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomNoteText = DataGenerator.CreateRandomMultiWordString();
        string noteValueText = $"\"{randomNoteText}\"";
        object? noteValue = randomNoteText;
        string text = $"Table {tableNameText} {{ note: {noteValueText} }}";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteValueKind, noteValueText, noteValue);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Note_SingleQuotationMarksString()
    {
        const SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        const SyntaxKind noteValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomNoteText = DataGenerator.CreateRandomMultiWordString();
        string noteValueText = $"\'{randomNoteText}\'";
        object? noteValue = randomNoteText;
        string text = $"Table {tableNameText} {{ note: {noteValueText} }}";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteValueKind, noteValueText, noteValue);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
