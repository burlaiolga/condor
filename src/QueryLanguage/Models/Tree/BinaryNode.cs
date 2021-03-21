namespace QueryLanguage.Models.Tree
{
    public class BinaryNode: Node
    {
        public BinaryNode(Operator @operator, Node leftNode, Node rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
            Operator = @operator;
        }

        public Operator Operator { get; set; }

        public Node LeftNode { get; set; }

        public Node RightNode { get; set; }
    }
}
