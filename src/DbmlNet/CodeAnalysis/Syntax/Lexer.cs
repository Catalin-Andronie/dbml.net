using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;

using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

internal sealed class Lexer
{
    private readonly SourceText _text;
    private readonly SyntaxTree _syntaxTree;
    private readonly ImmutableArray<SyntaxTrivia>.Builder _triviaBuilder =
        ImmutableArray.CreateBuilder<SyntaxTrivia>();

    private int _start;
    private int _position;
    private SyntaxKind _kind = SyntaxKind.BadToken;
    private object? _value;

    public Lexer(SyntaxTree syntaxTree)
    {
        _syntaxTree = syntaxTree;
        _text = _syntaxTree.Text;
    }

    public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

    private char Current => Peek(0);

    private char Lookahead => Peek(1);

    public SyntaxToken Lex()
    {
        ReadTrivia(leading: true);

        ImmutableArray<SyntaxTrivia> leadingTrivia = _triviaBuilder.ToImmutable();

        int tokenStart = _position;

        ReadToken();

        SyntaxKind tokenKind = _kind;
        object? tokenValue = _value ?? _kind.GetKnownValue();
        int tokenLength = _position - tokenStart;
        string tokenText = tokenKind.GetKnownText() ?? _text.ToString(tokenStart, tokenLength);

        ReadTrivia(leading: false);

        ImmutableArray<SyntaxTrivia> trailingTrivia = _triviaBuilder.ToImmutable();

        return new SyntaxToken(_syntaxTree, tokenKind, tokenStart, tokenText, tokenValue, leadingTrivia, trailingTrivia);
    }

    private char Peek(int offset = 0)
    {
        int index = _position + offset;
        return index < _text.Length
            ? _text[index]
            : '\0';
    }

    private void ReadTrivia(bool leading)
    {
        _triviaBuilder.Clear();

        bool done = false;

        while (!done)
        {
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;

            switch (Current)
            {
                case '\0':
                    done = true;
                    break;
                case '/':
                    if (Lookahead == '/')
                        ReadSingleLineComment();
                    else if (Lookahead == '*')
                        ReadMultiLineComment();
                    else
                        done = true;
                    break;
                case '\n':
                case '\r':
                    if (!leading)
                        done = true;
                    ReadLineBreak();
                    break;
                case ' ':
                case '\t':
                    ReadWhiteSpace();
                    break;
                default:
                    if (char.IsWhiteSpace(Current))
                        ReadWhiteSpace();
                    else
                        done = true;
                    break;
            }

            int length = _position - _start;
            if (length > 0)
            {
                string text = _text.ToString(_start, length);
                SyntaxTrivia trivia = new(_syntaxTree, _kind, _start, text);
                _triviaBuilder.Add(trivia);
            }
        }
    }

    private void ReadLineBreak()
    {
        if (Current == '\r' && Lookahead == '\n')
        {
            _position += 2;
        }
        else
        {
            _position++;
        }

        _kind = SyntaxKind.LineBreakTrivia;
    }

    private void ReadWhiteSpace()
    {
        bool done = false;

        while (!done)
        {
            switch (Current)
            {
                case '\0':
                case '\r':
                case '\n':
                    done = true;
                    break;
                default:
                    if (!char.IsWhiteSpace(Current))
                        done = true;
                    else
                        _position++;
                    break;
            }
        }

        _kind = SyntaxKind.WhitespaceTrivia;
    }

    private void ReadSingleLineComment()
    {
        // Skip the current '//'
        _position += 2;
        bool done = false;

        while (!done)
        {
            switch (Current)
            {
                case '\0':
                case '\r':
                case '\n':
                    done = true;
                    break;
                default:
                    _position++;
                    break;
            }
        }

        _kind = SyntaxKind.SingleLineCommentTrivia;
    }

    private void ReadMultiLineComment()
    {
        int start = _position;

        // Skip the current '/*'
        _position += 2;
        bool done = false;

        while (!done)
        {
            switch (Current)
            {
                case '\0':
                    TextSpan span = new(start, 2);
                    TextLocation location = new(_text, span);
                    Diagnostics.ReportUnterminatedMultiLineComment(location);
                    done = true;
                    break;
                case '*':
                    if (Lookahead == '/')
                    {
                        _position++;
                        done = true;
                    }

                    _position++;
                    break;
                default:
                    _position++;
                    break;
            }
        }

        _kind = SyntaxKind.MultiLineCommentTrivia;
    }

#pragma warning disable CA1502 // Avoid excessive complexity
#pragma warning disable MA0051 // Method is too long (maximum allowed: 60)

    private void ReadToken()
    {
        _start = _position;
        _kind = SyntaxKind.BadToken;
        _value = null;

        switch (Current)
        {
            case '\0':
                _kind = SyntaxKind.EndOfFileToken;
                break;
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                ReadNumber();
                break;
            case '.':
                _kind = SyntaxKind.DotToken;
                _position++;
                break;
            case '-':
                _kind = SyntaxKind.MinusToken;
                _position++;
                break;
            case '+':
                _kind = SyntaxKind.PlusToken;
                _position++;
                break;
            case '/':
                _kind = SyntaxKind.SlashToken;
                _position++;
                break;
            case '*':
                _kind = SyntaxKind.StarToken;
                _position++;
                break;
            case ',':
                _kind = SyntaxKind.CommaToken;
                _position++;
                break;
            case ':':
                _kind = SyntaxKind.ColonToken;
                _position++;
                break;
            case '<':
                if (Lookahead == '>')
                {
                    _kind = SyntaxKind.LessGraterToken;
                    _position += 2;
                }
                else
                {
                    _kind = SyntaxKind.LessToken;
                    _position++;
                }

                break;
            case '>':
                _kind = SyntaxKind.GraterToken;
                _position++;
                break;
            case '(':
                _kind = SyntaxKind.OpenParenthesisToken;
                _position++;
                break;
            case ')':
                _kind = SyntaxKind.CloseParenthesisToken;
                _position++;
                break;
            case '{':
                _kind = SyntaxKind.OpenBraceToken;
                _position++;
                break;
            case '}':
                _kind = SyntaxKind.CloseBraceToken;
                _position++;
                break;
            case '[':
                _kind = SyntaxKind.OpenBracketToken;
                _position++;
                break;
            case ']':
                _kind = SyntaxKind.CloseBracketToken;
                _position++;
                break;
            case '`':
                _kind = SyntaxKind.BacktickToken;
                _position++;
                break;
            case '#':
                ReadHexTriplet();
                break;
            case '"':
                ReadQuotationString();
                break;
            case '\'':
                if (Peek(1) == '\'' && Peek(2) == '\'')
                    ReadMultiLineString();
                else
                    ReadSingleQuotationString();

                break;
            default:
                if (char.IsLetterOrDigit(Current) || Current == '_')
                {
                    ReadIdentifier();
                }
                else
                {
                    TextSpan span = new(_position, 1);
                    TextLocation location = new(_text, span);
                    Diagnostics.ReportBadCharacter(location, Current);
                    _position++; // skip bad token
                }

                break;
        }
    }

    private void ReadHexTriplet()
    {
        _position++; // Skip the current pound sign

        for (int i = 0; i < 6; i++)
        {
            if (char.IsLetterOrDigit(Current))
            {
                _position++;
                continue;
            }

            TextSpan span = new(_position, 1);
            TextLocation location = new(_text, span);
            Diagnostics.ReportBadCharacter(location, Current);
            _position++; // skip bad token
            break;
        }

        _kind = SyntaxKind.HexTripletToken;
    }

#pragma warning restore CA1502 // Avoid excessive complexity
#pragma warning restore MA0051 // Method is too long (maximum allowed: 60)

    private void ReadQuotationString()
    {
        int start = _position;

        // Skip the current quote
        _position++;

        StringBuilder sb = new();
        bool done = false;
        while (!done)
        {
            switch (Current)
            {
                case '\0':
                case '\r':
                case '\n':
                    TextSpan span = new(start, 1);
                    TextLocation location = new(_text, span);
                    Diagnostics.ReportUnterminatedString(location);
                    done = true;
                    break;
                case '"':
                    if (Lookahead == '"')
                    {
                        sb.Append(Current);
                        _position += 2;
                    }
                    else
                    {
                        _position++;
                        done = true;
                    }

                    break;
                default:
                    sb.Append(Current);
                    _position++;
                    break;
            }
        }

        _kind = SyntaxKind.QuotationMarksStringToken;
        _value = sb.ToString();
    }

    private void ReadSingleQuotationString()
    {
        int start = _position;

        // Skip the current quote
        _position++;

        StringBuilder sb = new();
        bool done = false;
        while (!done)
        {
            switch (Current)
            {
                case '\0':
                case '\r':
                case '\n':
                    TextSpan span = new(start, 1);
                    TextLocation location = new(_text, span);
                    Diagnostics.ReportUnterminatedString(location);
                    done = true;
                    break;
                case '\'':
                    if (Lookahead == '\'')
                    {
                        sb.Append(Current);
                        _position += 2;
                    }
                    else
                    {
                        _position++;
                        done = true;
                    }

                    break;
                default:
                    sb.Append(Current);
                    _position++;
                    break;
            }
        }

        _kind = SyntaxKind.SingleQuotationMarksStringToken;
        _value = sb.ToString();
    }

#pragma warning disable MA0051 // Method is too long (maximum allowed: 60)

    private void ReadMultiLineString()
    {
        int start = _position;

        // Skip the current quotes '''
        _position += 3;

        StringBuilder sb = new();
        bool done = false;
        while (!done)
        {
            switch (Current)
            {
                case '\0':
                {
                    TextSpan span = new(start, 3);
                    TextLocation location = new(_text, span);
                    Diagnostics.ReportUnterminatedMultiLineString(location);
                    done = true;
                    break;
                }

                case '\\':
                {
                    if (Peek(1) == '\\')
                    {
                        sb.Append(Current);
                        _position += 2;
                    }
                    else if (Peek(1) == '\'' && Peek(2) == '\'' && Peek(3) == '\'')
                    {
                        _position++; // Skip backslash
                        sb.Append(Current); // Append single quote
                        _position++;
                        sb.Append(Current); // Append single quote
                        _position++;
                        sb.Append(Current); // Append single quote
                        _position++;
                    }
                    else
                    {
                        TextSpan span = new(_position, 1);
                        TextLocation location = new(_text, span);
                        Diagnostics.ReportUnrecognizedEscapeSequence(location);
                        _position++;
                    }

                    break;
                }

                case '\'':
                {
                    if (Peek(1) == '\'' && Peek(2) == '\'')
                    {
                        _position += 3;
                        done = true;
                    }
                    else
                    {
                        sb.Append(Current);
                        _position++;
                    }

                    break;
                }

                default:
                {
                    sb.Append(Current);
                    _position++;
                    break;
                }
            }
        }

        _kind = SyntaxKind.MultiLineStringToken;
        _value = TextUnindenter.Unindent(sb.ToString());
    }

#pragma warning restore MA0051 // Method is too long (maximum allowed: 60)

    private void ReadNumber()
    {
        /*
          Number literals are defined via next syntaxes:
            - numbers: `0`, `1`, `1000000`
            - ...with decimals: `2.7`, `3.14`, `2121296.32201`
            - ...with separators: `1_000_000`, `2_121_296.322_01`
            - ...or be crazy: `1__0__0_0___.__21_22_1____`
        */

        _kind = SyntaxKind.NumberToken;

        _position++; // Skip current number

        while (char.IsNumber(Current) || Current == '_')
            _position++;

        if (Current == '.')
        {
            _position++; // Skip dot

#pragma warning disable CA1508 // Avoid dead conditional code
            while (char.IsNumber(Current) || Current == '_')
                _position++;
#pragma warning restore CA1508 // Avoid dead conditional code
        }

        int length = _position - _start;
        string text = _text.ToString(_start, length);
        string numberText = text.Replace("_", string.Empty, StringComparison.InvariantCulture);
        try
        {
            _value = decimal.Parse(numberText, NumberFormatInfo.InvariantInfo);
        }
        catch (OverflowException)
        {
            TextSpan span = new(_position, length);
            TextLocation location = new(_text, span);
            Diagnostics.ReportNumberToLarge(location, numberText);
        }
    }

    private void ReadIdentifier()
    {
        while (char.IsLetterOrDigit(Current) || Current == '_')
            _position++;

        int length = _position - _start;
        string text = _text.ToString(_start, length);
        _kind = SyntaxFacts.GetKeywordKind(text);
    }
}
