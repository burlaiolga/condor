using System;

namespace QueryLanguage
{
    public class ExpressionInvalidException : ArgumentException
    {
        public string Details { get; set; }

        public ExpressionInvalidException() : base("Expression is invalid") { }

        public ExpressionInvalidException(string details) : this()
        {
            Details = details;
        }
    }
}
