using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a primary key column setting clause in the syntax tree.
/// </summary>
public sealed class PrimaryKeyColumnSettingClause : ColumnSettingClause
{
    internal PrimaryKeyColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken primaryKeyword,
        SyntaxToken keyKeyword)
        : base(syntaxTree, primaryKeyword.Text + keyKeyword.Text)
    {
        PrimaryKeyword = primaryKeyword;
        KeyKeyword = keyKeyword;
    }

    /// <summary>
    /// Gets the syntax kind of the primary key column setting clause <see cref="SyntaxKind.PrimaryKeyColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.PrimaryKeyColumnSettingClause;

    /// <summary>
    /// Gets the primary keyword.
    /// </summary>
    public SyntaxToken PrimaryKeyword { get; }

    /// <summary>
    /// Gets the key keyword.
    /// </summary>
    public SyntaxToken KeyKeyword { get; }

    /// <summary>
    /// Gets the children of the primary key column setting.
    /// </summary>
    /// <returns>The children of the primary key column setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return PrimaryKeyword;
        yield return KeyKeyword;
    }
}
