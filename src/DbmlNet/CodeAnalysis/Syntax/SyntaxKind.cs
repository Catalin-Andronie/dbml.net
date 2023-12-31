namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents all available syntax kinds.
/// </summary>
public enum SyntaxKind
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member.
#pragma warning disable SA1602 // Enumeration items should be documented.
    BadToken,

    // Trivia
    LineBreakTrivia,
    WhitespaceTrivia,
    SingleLineCommentTrivia,
    MultiLineCommentTrivia,

    // Tokens
    EndOfFileToken,
    NumberToken,
    DotToken,
    MinusToken,
    PlusToken,
    SlashToken,
    StarToken,
    CommaToken,
    ColonToken,
    LessToken,
    LessGraterToken,
    GraterToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    OpenBraceToken,
    CloseBraceToken,
    OpenBracketToken,
    CloseBracketToken,
    BacktickToken,
    HexTripletToken,
    QuotationMarksStringToken,
    SingleQuotationMarksStringToken,
    MultiLineStringToken,
    IdentifierToken,

    // Keywords
    AsKeyword,
    DatabaseTypeKeyword,
    DefaultKeyword,
    EnumKeyword,
    FalseKeyword,
    HeaderColorKeyword,
    IncrementKeyword,
    IndexesKeyword,
    KeyKeyword,
    NameKeyword,
    NoteKeyword,
    NotKeyword,
    NullKeyword,
    PkKeyword,
    PrimaryKeyword,
    ProjectKeyword,
    RefKeyword,
    TableKeyword,
    TrueKeyword,
    TypeKeyword,
    UniqueKeyword,

    // Nodes
    CompilationUnitMember,
    ProjectDeclarationMember,
    EnumDeclarationMember,
    TableDeclarationMember,
    RelationshipShortFormDeclarationMember,
    RelationshipLongFormDeclarationMember,
    GlobalStatementMember,

    // Project nodes
    ProjectSettingListClause,
    DatabaseProviderProjectSettingClause,
    NoteProjectSettingClause,
    UnknownProjectSettingClause,

    // Enum nodes
    EnumIdentifierClause,

    // Enum entry nodes
    EnumEntrySettingListClause,
    NoteEnumEntrySettingClause,
    UnknownEnumEntrySettingClause,

    // Table nodes
    TableIdentifierClause,
    TableAliasClause,
    TableSettingListClause,
    HeaderColorTableSettingClause,
    UnknownTableSettingClause,

    // Index nodes
    IndexSettingListClause,
    NameIndexSettingClause,
    NoteIndexSettingClause,
    PkIndexSettingClause,
    PrimaryKeyIndexSettingClause,
    TypeIndexSettingClause,
    UniqueIndexSettingClause,
    UnknownIndexSettingClause,

    // Column nodes
    ColumnIdentifierClause,
    ColumnTypeIdentifierClause,
    ColumnTypeParenthesizedIdentifierClause,
    ColumnSettingListClause,
    PrimaryKeyColumnSettingClause,
    PkColumnSettingClause,
    NullColumnSettingClause,
    NotNullColumnSettingClause,
    UniqueColumnSettingClause,
    IncrementColumnSettingClause,
    DefaultColumnSettingClause,
    NoteColumnSettingClause,
    RelationshipColumnSettingClause,
    RelationshipConstraintClause,
    UnknownColumnSettingClause,

    // Statements
    BlockStatement,
    NoteDeclarationStatement,
    EnumEntryDeclarationStatement,
    ColumnDeclarationStatement,
    IndexesDeclarationStatement,
    SingleFieldIndexDeclarationStatement,
    CompositeIndexDeclarationStatement,
    ExpressionStatement,

    // Expressions
    LiteralExpression,
    BacktickExpression,
    NameExpression,
    NullExpression,
    ParenthesizedExpression,
    CallExpression,

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member.
#pragma warning restore SA1602 // Enumeration items should be documented.
}
