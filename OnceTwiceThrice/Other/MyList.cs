using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
    public class MyList<T> : IEnumerable<T>
    {
        public class Node<T>
        {
            public T Value;
            public Node<T> Next;

            public Node(T value)
            {
                Value = value;
            }
        }

        public Node<T> Begin;
        public Node<T> End;

        public MyList()
        {
            ;
        }

        public int Count { get; private set; }

        public void Add(T value)
        {
            var newNode = new Node<T>(value);
            if (Begin != null)
            {
                End.Next = newNode;
                End = newNode;
            }
            else
                Begin = End = newNode;
            Count++;
        }

        public void Remove(T value)
        {
            if (Begin == null)
                return;

            if (Begin.Value.Equals(value))
            {
                Begin = Begin.Next;
                Count--;
                return;
            }

            var cur = Begin;
            while (cur.Next != null && !cur.Next.Value.Equals(value))
                cur = cur.Next;

            if (cur.Next == null)
                return;

            cur.Next = cur.Next.Next;
            Count--;
            while (cur.Next != null)
                cur = cur.Next;
            End = cur;
        }

        public IEnumerator<T> GetEnumerator()
        {
            //if (Begin == null)
            if (Count == 0)
                yield break;
            var cur = Begin;
            yield return cur.Value;
            while (cur.Next != null)
            {
                cur = cur.Next;
                yield return cur.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
