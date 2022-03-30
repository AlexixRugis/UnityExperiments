using System;
using System.Collections.Generic;

namespace ARTech.GameFramework.AI
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }

    public abstract class Node
    {
        private Node _parent;
        private List<Node> _children = new List<Node>();

        protected NodeState State;

        public Node()
        {
            _parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (var child in children)
            {
                Attach(child);
            }
        }
        public BehaviorTree BehaviorTree { get; set; }

        public Node Parent => _parent;
        public IEnumerable<Node> Children => _children;

        private void Attach(Node child)
        {
            child._parent = this;
            _children.Add(child);
        }

        public void Traverse(Action<Node> visitor)
        {
            visitor.Invoke(this);

            foreach (var child in Children)
            {
                child.Traverse(visitor);
            }
        }

        public abstract NodeState Evaluate();
    }
}