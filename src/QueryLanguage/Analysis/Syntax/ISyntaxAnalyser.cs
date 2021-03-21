using QueryLanguage.Models.Tree;
using System.Collections.Generic;

namespace QueryLanguage.Analysis.Syntax
{
    public interface ISyntaxAnalyser
    {
        Node ParseExpression(List<string> lexemes);
    }
}