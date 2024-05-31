using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace Baracuda.DOTS.Collections
{
    [BurstCompile]
    public static class NativeLinq
    {
        #region UnsafeHashSet

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExceptWith<T>(ref UnsafeHashSet<T> hashSet, UnsafeList<T> array)
            where T : unmanaged, IEquatable<T>
        {
            for (var index = 0; index < array.Length; index++)
            {
                var element = array[index];
                hashSet.Remove(element);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnionWith<T>(ref UnsafeHashSet<T> hashSet, UnsafeList<T> array)
            where T : unmanaged, IEquatable<T>
        {
            for (var index = 0; index < array.Length; index++)
            {
                var arrayElement = array[index];
                if (hashSet.Contains(arrayElement))
                {
                    continue;
                }
                hashSet.Add(arrayElement);
            }
        }

        #endregion


        #region UnsafeList

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExceptWith<T>(ref UnsafeList<T> list, UnsafeList<T> array)
            where T : unmanaged, IEquatable<T>
        {
            for (var indexList = 0; indexList < list.Length; indexList++)
            {
                for (var indexArray = 0; indexArray < array.Length; indexArray++)
                {
                    if (list[indexList].Equals(array[indexArray]))
                    {
                        list.RemoveAt(indexList);
                        indexList--;
                        break;
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnionWith<T>(ref UnsafeList<T> list, UnsafeList<T> array)
            where T : unmanaged, IEquatable<T>
        {
            for (var index = 0; index < array.Length; index++)
            {
                var arrayElement = array[index];
                if (list.Contains(arrayElement))
                {
                    continue;
                }
                list.Add(arrayElement);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveWhere<T, TPredicate>(ref UnsafeList<T> list, in TPredicate predicate)
            where T : unmanaged
            where TPredicate : unmanaged, INativePredicate<T>
        {
            for (var index = list.Length - 1; index >= 0; index--)
            {
                var removeElement = predicate.Evaluate(ref list.GetRef(index));
                if (removeElement)
                {
                    list.RemoveAt(index);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RemoveWhere<T, TPredicate>(UnsafeList<T>* list, TPredicate* predicate)
            where T : unmanaged
            where TPredicate : unmanaged, INativePredicate<T>
        {
            for (var index = list->Length - 1; index >= 0; index--)
            {
                var removeElement = predicate->Evaluate(ref list->GetRef(index));
                if (removeElement)
                {
                    list->RemoveAt(index);
                }
            }
        }

        #endregion
    }
}