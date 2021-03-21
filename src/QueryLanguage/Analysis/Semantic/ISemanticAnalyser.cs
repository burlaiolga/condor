using QueryLanguage.Models;
using QueryLanguage.Models.Tree;
using System;

namespace QueryLanguage.Analysis.Semantic
{
    public interface ISemanticAnalyser
    {
        Func<Country, bool> BuildLogicalPredicate(Node tree);
    }
}