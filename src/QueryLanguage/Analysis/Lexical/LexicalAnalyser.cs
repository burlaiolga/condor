using System.Collections.Generic;

namespace QueryLanguage.Analysis.Lexical
{
    public class LexicalAnalyser : ILexicalAnalyser
    {
        public List<string> GetLexemes(string input)
        {
            input = input?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<string>();
            }

            var result = new List<string>();

            var quoteOpened = false;
            var lexeme = string.Empty;

            foreach (var character in input)
            {
                switch (character)
                {
                    case ' ':
                        if (!quoteOpened)
                        {
                            result.Add(lexeme);
                            lexeme = string.Empty;
                        }
                        else
                        {
                            lexeme += ' ';
                        }

                        break;
                    case '\'':
                        if (quoteOpened)
                        {
                            result.Add(lexeme);
                            lexeme = string.Empty;
                        }

                        quoteOpened = !quoteOpened;
                        break;
                    default:
                        lexeme += character;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(lexeme))
            {
                result.Add(lexeme);
            }

            return result;
        }
    }
}
