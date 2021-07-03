using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtility.Collections
{
    /// <summary>
    /// <see cref="IQueue{T}"/>を実装した<see cref="Queue{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T> : IQueue<T>
    {
        protected readonly System.Collections.Generic.Queue<T> queue;

        public Queue() => queue = new System.Collections.Generic.Queue<T>();
        public Queue(int capacity) => queue = new System.Collections.Generic.Queue<T>(capacity);
        public Queue(IEnumerable<T> source) => queue = new System.Collections.Generic.Queue<T>(source);

        public virtual void Enqueue(T value) => queue.Enqueue(value);
        public virtual T Dequeue() => queue.Dequeue();
        public virtual T Peek() => queue.Peek();

        public int Count => queue.Count;

        public IEnumerator<T> GetEnumerator() => queue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
