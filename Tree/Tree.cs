namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Threading;

    public class Tree<T> : IAbstractTree<T>
    {
        private List<Tree<T>> children;
        private Tree<T> parent;
        private T value;
        public Tree(T value)
        {
            this.value = value;
            this.children = new List<Tree<T>>();
        }

        public Tree(T value, params Tree<T>[] children)
            : this(value)
        {
            foreach (var child in children)
            {
                child.parent = this;
                this.children.Add(child);
            }
        }


        public void AddChild(T parentKey, Tree<T> child)
        {
            var parent = FindParentWithBfs(parentKey);
            if (parent == null)
            {
                throw new ArgumentNullException();
            }
            parent.children.Add(child);
            child.parent = this;

        }

        private Tree<T> FindParentWithBfs(T parentKey)
        {
            Queue<Tree<T>> queue = new Queue<Tree<T>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                Tree<T> current = queue.Dequeue();
                if (current.value.Equals(parentKey))
                {
                    return current;

                }
                foreach (var child in current.children)
                {
                    queue.Enqueue(child);
                }

            }
            return null;
        }

        public IEnumerable<T> OrderBfs()
        {
            Queue<Tree<T>> queue = new Queue<Tree<T>>();
            List<T> result = new List<T>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                Tree<T> current = queue.Dequeue();
                result.Add(current.value);
                foreach (var child in current.children)
                {
                    queue.Enqueue(child);
                }

            }
            return result;
        }

        public IEnumerable<T> OrderDfs()
        {
            //List<T> values = new List<T>();
            //foreach (var child in this.children)
            //{
            //    values.AddRange(child.OrderDfs());
            //}
            //values.Add(this.value);
            //return values;
            return DfsWithStack();

        }
        private ICollection<T> DfsWithStack()
        {
            Stack<Tree<T>> stack = new Stack<Tree<T>>();
            var result = new Stack<T>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                result.Push(current.value);
                foreach (var child in current.children)
                {
                    stack.Push(child);
                }

            }
            return result.ToArray();
        }

        public void RemoveNode(T nodeKey)
        {
            var searchedNode = FindParentWithBfs(nodeKey);
            if(searchedNode is null)
            {
                throw new ArgumentNullException();
            }
            var parent = searchedNode.parent;
            if(parent is null)
            {
                throw new ArgumentException();
            }
            parent.children = parent.children.Where(x => !x.value.Equals(nodeKey)).ToList();
            searchedNode.parent = null;


        }

      

        public void Swap(T firstKey, T secondKey)
        {
            var firstNode = FindParentWithBfs(firstKey);
            var secondNode = FindParentWithBfs(secondKey);
            if (secondNode is null || firstNode is null)
            {
                throw new ArgumentNullException();
            }
            var firstParent = firstNode.parent;
            var secondParent = secondNode.parent;
            if (firstParent is null || secondParent is null)
            {
                throw new ArgumentException();

            }
            //firstNode.parent = secondParent;
            //secondNode.parent = firstParent;
          ;

            var indexFirstNode = firstParent.children.IndexOf(firstNode);
            var indexSecondNode = secondParent.children.IndexOf(secondNode);

            firstNode.parent = secondParent;
            firstParent.children[indexFirstNode] = secondNode;

            secondNode.parent = firstParent;
            secondParent.children[indexSecondNode] = firstNode;

        }
    }
}
