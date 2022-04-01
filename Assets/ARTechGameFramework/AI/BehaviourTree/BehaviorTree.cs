using System.Collections.Generic;

namespace ARTech.GameFramework.AI
{
    public sealed class BehaviorTree
    {
        private Node _root = null;
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public void SetNodes(Node root)
        {
            _root = root;
            
            if (_root != null)
            {
                _root.Traverse(n => n.BehaviorTree = this);
            }
        }

        public void Tick()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        public void SetData(string name, object o)
        {
            _dataContext[name] = o;
        }

        public void ClearData(string name)
        {
            _dataContext.Remove(name);
        }

        public T GetData<T>(string name) where T : class
        {
            object o = null;
            _dataContext.TryGetValue(name, out o);
            return o as T;
        }
    }
}