using System;

namespace Zadanie_3
{
    public abstract class Tree
    {

    }

    public class TreeNode : Tree
    {
        public Tree Left { get; set; }
        public Tree Right { get; set; }
    }

    public class TreeLeaf : Tree
    {
        public int Value { get; set; }
    }

    public abstract class TreeVisitor
    {
        // ta metoda nie jest potrzebna ale ułatwia korzystanie z Visitora
        public int Visit(Tree tree)
        {
            if (tree is TreeNode)
                return this.VisitNode((TreeNode)tree);
            if (tree is TreeLeaf)
                return this.VisitLeaf((TreeLeaf)tree);
            throw new ArgumentException();
        }
        public abstract int VisitNode(TreeNode node);
        public abstract int VisitLeaf(TreeLeaf leaf);
    }

    public class HeightTreeVisitor : TreeVisitor
    {
        public override int VisitNode(TreeNode node)
        {
            return 1 + Math.Max(Visit(node.Left), Visit(node.Right));
        }

        public override int VisitLeaf(TreeLeaf leaf)
        {
            return 0;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Tree root = new TreeNode()
            {
                Left = new TreeNode()
                {
                    Left = new TreeLeaf() { Value = 1 },
                    Right = new TreeLeaf() { Value = 2 }
                },
                Right = new TreeLeaf() { Value = 3 }
            };

            HeightTreeVisitor visitor = new HeightTreeVisitor();

            int treeHeight = visitor.Visit( root );

            Console.WriteLine("Tree height: {0}", treeHeight);            
        }
    }
}
