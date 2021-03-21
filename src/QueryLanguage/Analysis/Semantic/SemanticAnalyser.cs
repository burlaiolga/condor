using QueryLanguage.Models;
using QueryLanguage.Models.Tree;
using System;

namespace QueryLanguage.Analysis.Semantic
{
    public class SemanticAnalyser : ISemanticAnalyser
    {
        public Func<Country, bool> BuildLogicalPredicate(Node tree)
        {
            var node = tree as BinaryNode;

            if (node == null)
            {
                throw new ExpressionInvalidException();
            }

            switch (node.Operator)
            {
                case Operator.and:
                    {
                        var leftPredicate = BuildLogicalPredicate(node.LeftNode);
                        var rightPredicate = BuildLogicalPredicate(node.RightNode);

                        return country => leftPredicate(country) && rightPredicate(country);
                    }
                case Operator.or:
                    {
                        var leftPredicate = BuildLogicalPredicate(node.LeftNode);
                        var rightPredicate = BuildLogicalPredicate(node.RightNode);

                        return country => leftPredicate(country) || rightPredicate(country);
                    }
                default:
                    {
                        return BuildPredicate(node);
                    }
            }
        }

        private Func<Country, bool> BuildPredicate(BinaryNode node)
        {
            var propertyName = (node.LeftNode as Leaf)?.Lexeme;
            var property = typeof(Country).GetProperty(propertyName);
            if (property == null)
            {
                throw new ExpressionInvalidException($"Property {propertyName} is not valid");
            }

            var propertyType = property.PropertyType; //int or string
            var stringValue = (node.RightNode as Leaf)?.Lexeme;

            if (propertyType == typeof(int))
            {
                if (!int.TryParse(stringValue, out int intValue))
                {
                    throw new ExpressionInvalidException($"Couldn't compare a value {stringValue} with a property {propertyName} of type int");
                }

                return BuildNumericPredicate(node, propertyName, intValue);
            }
            else
            {
                if (!stringValue.StartsWith('\'') || !stringValue.EndsWith('\''))
                {
                    throw new ExpressionInvalidException($"{stringValue} must be a string (please add quotes)");
                }
                return BuildStringPredicate(node, propertyName, stringValue.Trim('\''));
            }

        }

        private static Func<Country, bool> BuildNumericPredicate(BinaryNode node, string propertyName, int intValue)
        {
            switch (node.Operator)
            {
                case Operator.eq:
                    {
                        return country => GetPropertyValue<int>(propertyName, country) == intValue;
                    }
                case Operator.ne:
                    {
                        return country => GetPropertyValue<int>(propertyName, country) != intValue;
                    }
                case Operator.ge:
                    {
                        return country => GetPropertyValue<int>(propertyName, country) >= intValue;
                    }
                case Operator.gt:
                    {
                        return country => GetPropertyValue<int>(propertyName, country) > intValue;
                    }
                case Operator.le:
                    {
                        return country => GetPropertyValue<int>(propertyName, country) <= intValue;
                    }
                case Operator.lt:
                    {
                        return country => GetPropertyValue<int>(propertyName, country) < intValue;
                    }

                default:
                    throw new ExpressionInvalidException($"Operator {node.Operator} can't be executed for integers!");
            }
        }

        private static Func<Country, bool> BuildStringPredicate(BinaryNode node, string propertyName, string stringValue)
        {
            switch (node.Operator)
            {
                case Operator.eq:
                    {
                        return country => GetPropertyValue<string>(propertyName, country) == stringValue;
                    }
                case Operator.ne:
                    {
                        return country => GetPropertyValue<string>(propertyName, country) != stringValue;
                    }
                case Operator.like:
                    {
                        return country => GetPropertyValue<string>(propertyName, country).Contains(stringValue);
                    }

                default:
                    throw new ExpressionInvalidException($"Operator {node.Operator} can't be executed for strings!");
            }
        }
        private static TValue GetPropertyValue<TValue>(string propertyName, object obj)
        {
            return (TValue)obj.GetType().GetProperty(propertyName).GetValue(obj);
        }


    }
}
