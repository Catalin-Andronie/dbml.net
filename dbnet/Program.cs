using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using DbmlNet;

if (args.Length == 0)
{
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine("ERROR: Missing input file.");
    Console.ResetColor();
    PrintDbnetUsage();
    return 1;
}

string filePath = args[0];

if (!File.Exists(filePath))
{
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine($"ERROR: File dose not exist: '{filePath}'.");
    Console.ResetColor();
    return 1;
}

string text = File.ReadAllText(filePath);
Parser parser = new Parser(text);
CompilationUnitSyntax syntax = parser.ParseCompilation();

if (parser.Diagnostics.Length > 0)
{
    Console.ForegroundColor = ConsoleColor.DarkRed;

    foreach (var diagnostic in parser.Diagnostics)
        Console.WriteLine(diagnostic);

    Console.ResetColor();

    return 1;
}

PrintSyntax(syntax);

var sqlWriter = new SqlWriter(syntax);
sqlWriter.WriteTo(Console.Out);

return 0;

void PrintDbnetUsage()
{
    Console.WriteLine("Usage: dbnet <input-file>");
}

void PrintSyntax(SyntaxNode node, string indent = "", bool isLast = true)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write(indent);
    Console.Write(isLast ? "└── " : "├── ");
    Console.ResetColor();

    if (node is not SyntaxToken)
        Console.ForegroundColor = ConsoleColor.DarkGreen;
    else
        Console.ForegroundColor = ConsoleColor.DarkYellow;

    Console.Write(node.Kind);
    Console.ResetColor();

    if (node is SyntaxToken token)
    {
        Console.Write(' ');

        if (token.Kind.ToString().EndsWith("Keyword"))
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
        else
            Console.ForegroundColor = ConsoleColor.DarkGray;

        Console.Write(token.Text);
        Console.ResetColor();
    }

    Console.WriteLine();

    indent += isLast ? "   " : "│  ";

    var lastChild = node.GetChildren().LastOrDefault();
    foreach (var child in node.GetChildren())
        PrintSyntax(child, indent, child == lastChild);
}

namespace DbmlNet
{
    public sealed class SqlWriter
    {
        private readonly CompilationUnitSyntax _syntax;

        public SqlWriter(CompilationUnitSyntax syntax)
        {
            _syntax = syntax;
        }

        public void WriteTo(TextWriter writer)
        {
            foreach (var member in _syntax.Members)
                WriteMember(writer, member);
        }

        private void WriteMember(TextWriter writer, SyntaxNode node)
        {
            switch (node.Kind)
            {
                case SyntaxKind.TableDefinition:
                    WriteTableDefinition(writer, (TableDefinitionSyntax)node);
                    break;
                default:
                    throw new InvalidOperationException($"ERROR: Unknown syntax kind: '{node.Kind}'.");
            }
        }

        private void WriteTableDefinition(TextWriter writer, TableDefinitionSyntax table)
        {
            writer.Write("CREATE TABLE");
            writer.Write(' ');
            writer.Write(table.IdentifierToken);
            writer.Write(' ');
            writer.WriteLine();
            writer.Write('(');
            writer.WriteLine();

            ColumnDefinitionSyntax? lastColumn = table.Columns.LastOrDefault();
            foreach (var column in table.Columns)
            {
                writer.Write("  ");
                WriteColumnDefinition(writer, column);
                if (column != lastColumn)
                    writer.Write(',');

                writer.WriteLine();
            }

            writer.Write(')');
            writer.WriteLine();
        }

        private void WriteColumnDefinition(TextWriter writer, ColumnDefinitionSyntax column)
        {
            writer.Write(column.IdentifierToken);
            writer.Write(' ');
            writer.Write(column.TypeToken);
        }
    }

    sealed class Lexer
    {
        private readonly string _text;
        private readonly List<string> _diagnostics = [];
        private int _position;

        public Lexer(string text)
        {
            _text = text;
        }

        public ImmutableArray<string> Diagnostics => [.. _diagnostics];

        private char Current => Peek(0);

        private char Peek(int offset)
        {
            int index = _position + offset;
            return index < _text.Length
                ? _text[index]
                : '\0';
        }

        public SyntaxToken Lex()
        {
            int start = _position;

            switch (Current)
            {
                case '\0':
                    return new SyntaxToken(SyntaxKind.EndOfFileToken, start, null);
                case '{':
                    {
                        _position++;
                        var text = _text[start.._position];
                        return new SyntaxToken(SyntaxKind.OpenBraceToken, start, text);
                    }
                case '}':
                    {
                        _position++;
                        var text = _text[start.._position];
                        return new SyntaxToken(SyntaxKind.CloseBraceToken, start, text);
                    }
                case '\n':
                case '\r':
                case ' ':
                    {
                        _position++;
                        while (char.IsWhiteSpace(Current))
                            _position++;

                        var text = _text[start.._position];
                        var kind = SyntaxKind.WhiteSpaceToken;
                        return new SyntaxToken(kind, start, text);
                    }
                default:
                    {
                        while (char.IsLetterOrDigit(Current) || Current == '_')
                            _position++;

                        if (start == _position)
                        {
                            _diagnostics.Add($"Unknown character '{Current}'.");
                            _position++;
                            string text = _text[start.._position];
                            return new SyntaxToken(SyntaxKind.BadToken, start, text);
                        }
                        else
                        {
                            string text = _text[start.._position];
                            SyntaxKind kind = GetTokenKind(text);
                            return new SyntaxToken(kind, start, text);
                        }
                    }
            }
        }

        private SyntaxKind GetTokenKind(string text)
        {
            return text switch
            {
                "Table" => SyntaxKind.TableKeyword,
                _ => SyntaxKind.IdentifierToken,
            };
        }
    }

    sealed class Parser
    {
        private readonly ImmutableArray<SyntaxToken> _tokens;
        private readonly List<string> _diagnostics = [];
        private int _position;

        public Parser(string text)
        {
            List<SyntaxToken> tokens = [];

            Lexer lexer = new Lexer(text);
            SyntaxToken token;
            do
            {
                token = lexer.Lex();
                if (token.Kind != SyntaxKind.BadToken
                    && token.Kind != SyntaxKind.WhiteSpaceToken)
                {
                    tokens.Add(token);
                }

            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _tokens = [.. tokens];
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public ImmutableArray<string> Diagnostics => [.. _diagnostics];

        private SyntaxToken Current => Peek(0);

        private SyntaxToken Peek(int offset)
        {
            int index = _position + offset;
            return index < _tokens.Length
                ? _tokens[index]
                : _tokens[^1];
        }

        public SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.Add($"Expected '{kind}', but found '{Current.Kind}'.");
            return new SyntaxToken(kind, _position, null);
        }

        private SyntaxToken NextToken()
        {
            SyntaxToken token = Current;
            _position++;
            return token;
        }

        public CompilationUnitSyntax ParseCompilation()
        {
            ImmutableArray<SyntaxNode> members = ParseMembers();
            SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new CompilationUnitSyntax(members, endOfFileToken);
        }

        private ImmutableArray<SyntaxNode> ParseMembers()
        {
            List<SyntaxNode> members = [];

            while (Current.Kind != SyntaxKind.EndOfFileToken)
            {
                SyntaxToken token = Current;

                var member = ParseMember();
                members.Add(member);

                // If ParseMember() dose not consume a token,
                // we advance the position in order to avoid infinite loops.
                if (token == Current)
                    NextToken();
            }

            return [.. members];
        }

        private SyntaxNode ParseMember()
        {
            return Current.Kind switch
            {
                SyntaxKind.TableKeyword => ParseTableDefinition(),
                _ => throw new InvalidOperationException($"ERROR: Unknown token kind '{Current.Kind}'."),
            };
        }

        private TableDefinitionSyntax ParseTableDefinition()
        {
            SyntaxToken tableKeyword = MatchToken(SyntaxKind.TableKeyword);
            SyntaxToken identifierToken = MatchToken(SyntaxKind.IdentifierToken);
            SyntaxToken openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);
            List<ColumnDefinitionSyntax> columns = [];

            while (Current.Kind != SyntaxKind.CloseBraceToken
                && Current.Kind != SyntaxKind.EndOfFileToken)
            {
                SyntaxToken token = Current;

                ColumnDefinitionSyntax column = ParseColumnDefinition();
                columns.Add(column);

                // If ParseColumnDefinition() dose not consume a token,
                // we advance the position in order to avoid infinite loops.
                if (token == Current)
                    NextToken();
            }

            SyntaxToken closeBraceToken = MatchToken(SyntaxKind.CloseBraceToken);
            return new TableDefinitionSyntax(
                tableKeyword,
                identifierToken,
                openBraceToken,
                columns,
                closeBraceToken);
        }

        private ColumnDefinitionSyntax ParseColumnDefinition()
        {
            SyntaxToken identifierToken = MatchToken(SyntaxKind.IdentifierToken);
            SyntaxToken typeToken = MatchToken(SyntaxKind.IdentifierToken);
            return new ColumnDefinitionSyntax(identifierToken, typeToken);
        }
    }

    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }

    public sealed class CompilationUnitSyntax : SyntaxNode
    {
        public CompilationUnitSyntax(
            IEnumerable<SyntaxNode> members,
            SyntaxToken endOfFileToken)
        {
            Members = members;
            EndOfFileToken = endOfFileToken;
        }

        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
        public IEnumerable<SyntaxNode> Members { get; }
        public SyntaxToken EndOfFileToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            foreach (var member in Members)
                yield return member;

            yield return EndOfFileToken;
        }
    }

    public sealed class TableDefinitionSyntax : SyntaxNode
    {
        public TableDefinitionSyntax(
            SyntaxToken tableKeyword,
            SyntaxToken identifierToken,
            SyntaxToken openBraceToken,
            IEnumerable<ColumnDefinitionSyntax> columns,
            SyntaxToken closeBraceToken)
        {
            TableKeyword = tableKeyword;
            IdentifierToken = identifierToken;
            OpenBraceToken = openBraceToken;
            Columns = columns;
            CloseBraceToken = closeBraceToken;
        }

        public override SyntaxKind Kind => SyntaxKind.TableDefinition;
        public SyntaxToken TableKeyword { get; }
        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken OpenBraceToken { get; }
        public IEnumerable<ColumnDefinitionSyntax> Columns { get; }
        public SyntaxToken CloseBraceToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return TableKeyword;
            yield return IdentifierToken;
            yield return OpenBraceToken;
            foreach (var column in Columns)
                yield return column;
            yield return CloseBraceToken;
        }
    }

    public sealed class ColumnDefinitionSyntax : SyntaxNode
    {
        public ColumnDefinitionSyntax(
            SyntaxToken identifierToken,
            SyntaxToken typeToken)
        {
            IdentifierToken = identifierToken;
            TypeToken = typeToken;
        }

        public override SyntaxKind Kind => SyntaxKind.ColumnDefinition;
        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken TypeToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return TypeToken;
        }
    }

    public sealed class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxKind kind, int start, string? text)
        {
            Kind = kind;
            Start = start;
            Text = text ?? string.Empty;
        }

        public override SyntaxKind Kind { get; }
        public int Start { get; }
        public string Text { get; }

        public override IEnumerable<SyntaxNode> GetChildren() => [];

        public override string ToString() => Text;
    }

    public enum SyntaxKind
    {
        EndOfFileToken,
        BadToken,
        WhiteSpaceToken,
        OpenBraceToken,
        CloseBraceToken,
        IdentifierToken,
        TableKeyword,

        // Nodes
        CompilationUnit,
        TableDefinition,
        ColumnDefinition,
    }
}
