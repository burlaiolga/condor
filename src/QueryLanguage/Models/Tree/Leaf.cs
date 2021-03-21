namespace QueryLanguage.Models.Tree
{
    public class Leaf : Node
    {
        public string Lexeme { get; set; }

        public Leaf(string value)
        {
            Lexeme = value;
        }
    }
}
