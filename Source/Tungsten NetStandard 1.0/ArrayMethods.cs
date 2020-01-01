using System;
using System.Collections.Generic;
using System.Text;

namespace W
{
    /// <summary>
    /// Methods to peek and modify arrays
    /// </summary>
    public static class ArrayMethods
    {
        /// <summary>
        /// Retrieves the specified number of elements from the start of the array without changing the source array
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="length">The number of elements to retrieve</param>
        /// <returns>A new array containing only the specified subset of elements</returns>
        public static T[] PeekStart<T>(T[] source, int length)
        {
            T[] result = null;
            if (source.Length >= length)
            {
                result = new T[length];
                Array.Copy(source, result, length);
            }
            return result;
        }
        /// <summary>
        /// Retrieves the specified range of elements from the array 
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="startIndex">The index from which to start retrieving elements</param>
        /// <param name="length">The number of elements to retrieve</param>
        /// <returns>A new array containing only the specified subset of elements</returns>
        public static T[] Peek<T>(T[] source, int startIndex, int length)
        {
            T[] result = null;
            if (source.Length >= length)
            {
                result = new T[length];
                Array.Copy(source, startIndex, result, 0, length);
            }
            return result;
        }
        /// <summary>
        /// Retrieves the specified number of elements from the end of the array
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="length">The number of elements to retrieve</param>
        /// <returns>A new array containing only the specified subset of elements</returns>
        public static T[] PeekEnd<T>(T[] source, int length)
        {
            T[] result = null;
            if (source.Length >= length)
            {
                result = new T[length];
                Array.Copy(source, source.Length - length, result, 0, length);
            }
            return result;
        }

        /// <summary>
        /// Retrieves and removes the specified number of elements from the start of the array
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="length">The number of elements to retrieve</param>
        /// <returns>A new array containing only the specified subset of elements</returns>
        public static T[] TakeFromStart<T>(ref T[] source, int length)
        {
            var data = PeekStart(source, length);
            if (data != null)
                TrimStart(ref source, length);
            return data;
        }
        /// <summary>
        /// Retrieves and removes the specified range of elements from the array
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="startIndex">The index from which to start retrieving elements</param>
        /// <param name="length">The number of elements to retrieve</param>
        /// <returns>A new array containing only the specified subset of elements</returns>
        public static T[] Take<T>(ref T[] source, int startIndex, int length)
        {
            var data = Peek(source, startIndex, length);
            if (data != null)
                Trim(ref source, startIndex, length);
            return data;
        }
        /// <summary>
        /// Retrieves and removes the specified number of elements from the end of the array
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="length">The number of elements to retrieve</param>
        /// <returns>A new array containing only the specified subset of elements</returns>
        public static T[] TakeFromEnd<T>(ref T[] source, int length)
        {
            var data = PeekEnd(source, length);
            if (data != null)
                TrimEnd(ref source, length);
            return data;
        }

        /// <summary>
        /// Removes the specified number of elements from the start of the array, resizing the array as necessary
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="length">The number of elements to remove</param>
        /// <returns>The modified source array</returns>
        public static T[] TrimStart<T>(ref T[] source, int length)
        {
            var lengthToRetain = source.Length - length;
            T[] newValue = new T[lengthToRetain];
            if (newValue.Length > 0)
                Array.Copy(source, length, newValue, 0, lengthToRetain);
            source = newValue;
            return source;
        }
        /// <summary>
        /// Removes the specified range of elements from the array, resizing the array as necessary
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="startIndex">The index from which to start removing elements</param>
        /// <param name="length">The number of elements to remove</param>
        /// <returns>The modified source array</returns>
        public static T[] Trim<T>(ref T[] source, int startIndex, int length)
        {
            var rightIndex = startIndex + length;
            var rightLength = source.Length - (startIndex + length);
            var leftLength = startIndex;
            T[] newValue = new T[leftLength + rightLength];
            if (newValue.Length > 0)
            {
                if (leftLength > 0)
                    Array.Copy(source, 0, newValue, 0, leftLength); //copy left segment
                if (rightLength > 0)
                    Array.Copy(source, rightIndex, newValue, leftLength, rightLength); //copy right segment
            }
            source = newValue;
            return source;
        }
        /// <summary>
        /// Removes the specified number of elements from the end of the array, resizing the array as necessary
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="length">The number of elements to remove</param>
        /// <returns>The modified source array</returns>
        public static T[] TrimEnd<T>(ref T[] source, int length)
        {
            var copyLength = source.Length - length;
            T[] newValue = new T[copyLength];
            if (newValue.Length > 0)
                Array.Copy(source, 0, newValue, 0, copyLength);
            source = newValue;
            return source;
        }

        /// <summary>
        /// Appends the items to an array, resizing the array as necessary
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="itemsToAdd">The array of items to append to the source</param>
        /// <returns>The modified source array</returns>
        public static T[] Append<T>(ref T[] source, T[] itemsToAdd)
        {
            var curLen = source.Length;
            var numberOfItemsToAdd = itemsToAdd.Length;
            Array.Resize(ref source, curLen + numberOfItemsToAdd);
            Array.Copy(itemsToAdd, 0, source, curLen, numberOfItemsToAdd);
            return source;
        }
        /// <summary>
        /// Appends the items to an array, resizing the array as necessary
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="itemsToInsert">The array of items to append to the source</param>
        /// <param name="index">The index where the items should be inserted</param>
        /// <returns>The modified source array</returns>
        public static T[] Insert<T>(ref T[] source, T[] itemsToInsert, int index)
        {
            var numberOfItemsToInsert = itemsToInsert.Length;
            var curLen = source.Length;
            var newLen = curLen + numberOfItemsToInsert;
            var offset = index + numberOfItemsToInsert;
            var rightLength = curLen - index;
            Array.Resize(ref source, newLen); //make room for new items
            Array.Copy(source, index, source, offset, rightLength); //move items right
            Array.Copy(itemsToInsert, 0, source, index, numberOfItemsToInsert); //now insert the items
            return source;
        }
    }
}
