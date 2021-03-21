using QueryLanguage.Analysis.Lexical;
using QueryLanguage.Analysis.Semantic;
using QueryLanguage.Analysis.Syntax;
using QueryLanguage.Models;
using System.Collections.Generic;
using System.Linq;

namespace QueryLanguage.Analysis
{
    public class LanguageProcessor : ILanguageProcessor
    {
        private readonly ILexicalAnalyser lexicalAnalyser;
        private readonly ISyntaxAnalyser syntaxAnalyser;
        private readonly ISemanticAnalyser semanticAnalyser;


        public LanguageProcessor()
        {
            lexicalAnalyser = new LexicalAnalyser();
            syntaxAnalyser = new SyntaxAnalyser();
            semanticAnalyser = new SemanticAnalyser();
        }

        public List<Country> Find(List<Country> countries, string expression)
        {
            var lexemes = lexicalAnalyser.GetLexemes(expression);
            var expressionTree = syntaxAnalyser.ParseExpression(lexemes);
            var predicate = semanticAnalyser.BuildLogicalPredicate(expressionTree);

            return countries.Where(predicate).ToList();
        }
    }
}
