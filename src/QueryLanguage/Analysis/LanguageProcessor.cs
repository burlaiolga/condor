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
        private readonly ILexicalAnalyser _lexicalAnalyser;
        private readonly ISyntaxAnalyser _syntaxAnalyser;
        private readonly ISemanticAnalyser _semanticAnalyser;

        public LanguageProcessor() : this(new LexicalAnalyser(), new SyntaxAnalyser(), new SemanticAnalyser()) { }

        public LanguageProcessor(ILexicalAnalyser lexicalAnalyser, ISyntaxAnalyser syntaxAnalyser, ISemanticAnalyser semanticAnalyser)
        {
            _lexicalAnalyser = lexicalAnalyser;
            _syntaxAnalyser = syntaxAnalyser;
            _semanticAnalyser = semanticAnalyser;
        }

        public List<Country> Find(List<Country> countries, string expression)
        {
            var lexemes = _lexicalAnalyser.GetLexemes(expression);
            var expressionTree = _syntaxAnalyser.ParseExpression(lexemes);
            var predicate = _semanticAnalyser.BuildLogicalPredicate(expressionTree);

            return countries.Where(predicate).ToList();
        }
    }
}
