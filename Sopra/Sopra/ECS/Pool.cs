using System;
using System.Collections.Generic;

namespace Sopra.ECS
{
    /// <summary>
    /// Object pools can be used to reduce the amount of object allocations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Michael Fleig</author>
    public abstract class Pool<T>
    {
        private readonly int mMax;
        private readonly Stack<T> mFreeObjects;

        protected Pool(int capazity = 16, int max = int.MaxValue)
        {
            mMax = max;
            mFreeObjects = new Stack<T>(capazity);
        }

        /// <summary>
        /// Optain a object from the pool.
        /// If the pool is empty, a new object will be created.
        /// </summary>
        /// <returns>T</returns>
        public T Optain()
        {
            return mFreeObjects.Count == 0 ? NewObject() : mFreeObjects.Pop();
        }

        /// <summary>
        /// Create a new object for the pool.
        /// </summary>
        /// <returns></returns>
        protected abstract T NewObject();

        /// <summary>
        /// Return an object to the pool.
        /// </summary>
        /// <param name="obj">object to return</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Free(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            
            if (mFreeObjects.Count >= mMax)
            {
                return;
            }

            mFreeObjects.Push(obj);
            Reset(obj);
        }

        private void Reset(T obj)
        {
            var poolable = obj as IPoolable;
            poolable?.Reset();
        }
    }
}