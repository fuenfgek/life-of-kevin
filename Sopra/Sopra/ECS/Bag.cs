using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopra.ECS
{
    public sealed class Bag<T> where T : class
    {
        private const int IntitialCapazity = 32;
        
        private T[] mData;

        public int Capazity => mData.Length;

        public Bag(int capacity = IntitialCapazity)
        {
            mData = new T[capacity];
        }

        public T Get(int index)
        {
            return mData[index];
        }

        public void Set(int index, T obj)
        {
            if (Capazity <= index)
            {
                Grow(index * 2);
            }

            mData[index] = obj;
        }

        public List<T> ToList()
        {
            return mData.Where(obj => obj != null).ToList();
        }

        private void Grow(int newCapazity)
        {
            var old = mData;
            mData = new T[newCapazity];
            Array.Copy(old, mData, old.Length);
        }
    }
}