using QueryLanguage.Models;
using QueryLanguage.Models.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryLanguage.Analysis.Syntax
{
    public class SyntaxAnalyser : ISyntaxAnalyser
    {
        public Node ParseExpression(List<string> lexemes)
        {
            if (lexemes.Count < 3)
            {
                throw new ExpressionInvalidException();
            }

            if (lexemes.Count == 3)
            {
                return ParsePredicate(lexemes);
            }

            var leftNode = ParsePredicate(lexemes.Take(3).ToList());
            var @operator = GetOperator(lexemes[3]);
            var rightNode = ParseExpression(lexemes.Skip(4).ToList());

            return new BinaryNode(@operator, leftNode, rightNode);
        }

        private Node ParsePredicate(List<string> lexemes)
        {
            var @operator = GetOperator(lexemes[1]);
            return new BinaryNode(@operator, new Leaf(lexemes[0]), new Leaf(lexemes[2]));
        }

        private Operator GetOperator(string lexeme)
        {
            Operator @operator;
            if (!Enum.TryParse(lexeme, true, out @operator))
                throw new ExpressionInvalidException($"Couldn't find operator {lexeme}");

            return @operator;
        }
    }
}
