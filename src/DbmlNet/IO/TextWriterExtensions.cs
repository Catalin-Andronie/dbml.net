using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.IO
{
#pragma warning disable SA1202 // disable 'public' members should come before 'private' members

    /// <summary>
    /// Provides extension methods for the <see cref="TextWriter"/>.
    /// </summary>
    public static class TextWriterExtensions
    {
        private static bool IsConsole(this TextWriter writer)
        {
            if (writer == Console.Out)
                return !Console.IsOutputRedirected;
            else if (writer == Console.Error)
                // Color codes are always output to Console.Out
                return !Console.IsErrorRedirected && !Console.IsOutputRedirected;
            else if (writer is IndentedTextWriter iw && iw.InnerWriter.IsConsole())
                return true;

            return false;
        }

        private static void SetForeground(this TextWriter writer, ConsoleColor color)
        {
            if (writer.IsConsole())
                Console.ForegroundColor = color;
        }

        private static void ResetColor(this TextWriter writer)
        {
            if (writer.IsConsole())
                Console.ResetColor();
        }

        /// <summary>
        /// Writes the specified message to the writer with a dark cyan color.
        /// </summary>
        /// <param name="writer">The text writer to write to.</param>
        /// <param name="message">The message to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteInformation(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkCyan);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes the specified success message to the writer with a dark green color.
        /// </summary>
        /// <param name="writer">The the text writer to write the message to.</param>
        /// <param name="message">The success message to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteSuccess(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkGreen);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes a warning message to the specified text writer with a dark yellow color.
        /// </summary>
        /// <param name="writer">The text writer to write the message to.</param>
        /// <param name="message">The warning message to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteWarning(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkYellow);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes an error message to the specified text writer with a dark red color.
        /// </summary>
        /// <param name="writer">The text writer to write the message to.</param>
        /// <param name="message">The error message to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteError(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkRed);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes a debug message to the specified text writer with a dark gray color.
        /// </summary>
        /// <param name="writer">The text writer to write the message to.</param>
        /// <param name="message">The debug message to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteDebug(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkGray);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes a keyword to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the keyword to.</param>
        /// <param name="kind">The kind of keyword to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteKeyword(this TextWriter writer, SyntaxKind kind)
        {
            ArgumentNullException.ThrowIfNull(writer);

            Debug.Assert(kind.IsKeyword(), "kind is not a keyword");
            string? text = kind.GetKnownText();
            Debug.Assert(text != null, "kind is not a known keyword");

            writer.WriteKeyword(text);
        }

        /// <summary>
        /// Writes a keyword to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the keyword to.</param>
        /// <param name="text">The text of the keyword to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteKeyword(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.Blue);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes an identifier to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the identifier to.</param>
        /// <param name="text">The text of the identifier to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteIdentifier(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkYellow);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes a number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the number to.</param>
        /// <param name="text">The text of the number to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteNumber(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkCyan);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes a string to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the string to.</param>
        /// <param name="text">The text of the string to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteString(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkMagenta);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes a space to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the space to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteSpace(this TextWriter writer)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.WritePunctuation(" ");
        }

        /// <summary>
        /// Writes a punctuation to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the punctuation to.</param>
        /// <param name="kind">The kind of punctuation to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WritePunctuation(this TextWriter writer, SyntaxKind kind)
        {
            ArgumentNullException.ThrowIfNull(writer);

            string? text = SyntaxFacts.GetKnownText(kind);
            Debug.Assert(text != null, "kind is not a known punctuation");

            writer.WritePunctuation(text);
        }

        /// <summary>
        /// Writes a punctuation to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the punctuation to.</param>
        /// <param name="text">The text of the punctuation to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WritePunctuation(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkGray);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// Writes a comment to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the comment to.</param>
        /// <param name="text">The text of the comment to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteComment(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkGreen);
            writer.Write(text);
            writer.ResetColor();
        }

#pragma warning disable MA0051 // Method is too long (maximum allowed: 60)

        /// <summary>
        /// Writes diagnostics to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to write the diagnostics to.</param>
        /// <param name="diagnostics">The diagnostics to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="diagnostics"/> is <see langword="null"/>.</exception>
        public static void WriteDiagnostics(this TextWriter writer, ICollection<Diagnostic> diagnostics)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(diagnostics);

            foreach (Diagnostic diagnostic in diagnostics
                                                .Where(d => d.Location.Text == null))
            {
                if (diagnostic.IsWarning)
                    writer.WriteWarning(diagnostic.Message);
                else
                    writer.WriteError(diagnostic.Message);

                writer.WriteLine();
            }

            foreach (Diagnostic diagnostic in diagnostics
                                                .Where(d => d.Location.Text != null)
                                                .OrderBy(d => d.Location.FileName, StringComparer.Ordinal)
                                                .ThenBy(d => d.Location.Span.Start)
                                                .ThenBy(d => d.Location.Span.Length))
            {
                SourceText text = diagnostic.Location.Text;
                string fileName = diagnostic.Location.FileName;
                int startLine = diagnostic.Location.StartLine + 1;
                int startCharacter = diagnostic.Location.StartCharacter + 1;
                int endLine = diagnostic.Location.EndLine + 1;
                int endCharacter = diagnostic.Location.EndCharacter + 1;

                TextSpan span = diagnostic.Location.Span;
                int lineIndex = text.GetLineIndex(span.Start);
                TextLine line = text.Lines[lineIndex];

                writer.WriteLine();

                string reportedDiagnostic = $"{fileName}({startLine},{startCharacter},{endLine},{endCharacter}): {diagnostic}";
                if (diagnostic.IsWarning)
                    writer.WriteWarning(reportedDiagnostic);
                else
                    writer.WriteError(reportedDiagnostic);

                writer.WriteLine();
                writer.ResetColor();

                TextSpan prefixSpan = TextSpan.FromBounds(line.Start, span.Start);
                TextSpan suffixSpan = TextSpan.FromBounds(span.End, line.End);

                string prefix = text.ToString(prefixSpan);
                string error = text.ToString(span);
                string suffix = text.ToString(suffixSpan);

                writer.Write("    ");
                writer.Write(prefix);

                if (diagnostic.IsWarning)
                    writer.WriteWarning(error);
                else
                    writer.WriteError(error);

                writer.Write(suffix);

                writer.WriteLine();
            }

            writer.WriteLine();
        }

#pragma warning restore MA0051 // Method is too long (maximum allowed: 60)
    }
}
