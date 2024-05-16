using System;
using System.Linq;

namespace Sopra.ECS
{
    public sealed class Bits
    {
        private const int BitsPerField = 64;

        private long[] mBits = {0};

        /// <summary>
        /// Set the bit at the given index to one / true;
        /// </summary>
        /// <param name="index"></param>
        public void Set(int index)
        {
            EnsureBounds(GetArrayIndex(index));
            mBits[GetArrayIndex(index)] |= 1L << (index % BitsPerField);
        }

        /// <summary>
        /// Check wheter the bit at a given index is set
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Get(int index)
        {
            return (mBits[GetArrayIndex(index)] & 1L << (index % BitsPerField)) != 0L;
        }

        public bool GetAndClear(int index)
        {
            var word = GetArrayIndex(index);
            EnsureBounds(word);
            var old  = mBits[word];
            mBits[word] &= ~(1L << (index % BitsPerField));
            return mBits[word] != old;
        }

        /// <summary>
        /// Check wheter the current bit set is a super set of a given bit set.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool ContainsAll(Bits other)
        {
            for (var i = mBits.Length; i < other.mBits.Length; i++)
            {
                if (other.mBits[i] != 0L)
                {
                    return false;
                }
            }

            for (var i = 0; i < Math.Min(mBits.Length, other.mBits.Length); i++)
            {
                if ((mBits[i] & other.mBits[i]) != other.mBits[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <param name="other"></param>
        /// <returns>true, if the bitset contains no bits that are set to true</returns>
        public bool Intersects(Bits other)
        {
            for (var i = 0; i < Math.Min(mBits.Length, other.mBits.Length); i++)
            {
                if ((mBits[i] & other.mBits[i]) != 0L)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the bitset contains no bits that are set to true.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return mBits.All(word => word == 0L);
        }


        private void EnsureBounds(int length)
        {
            if (mBits.Length > length)
            {
                return;
            }

            var newBits = new long[length];
            Array.Copy(mBits, newBits, mBits.Length);
            mBits = newBits;
        }

        private static int GetArrayIndex(int index)
        {
            return index / BitsPerField;
        }
    }
}