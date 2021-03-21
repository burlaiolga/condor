using System.Collections.Generic;

namespace QueryLanguage.Analysis.Lexical
{
    public interface ILexicalAnalyser
    {
        List<string> GetLexemes(string input);
    }
}