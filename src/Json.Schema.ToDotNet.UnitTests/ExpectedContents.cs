﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace Microsoft.Json.Schema.ToDotNet.UnitTests
{
    internal class ExpectedContents
    {
        internal string ClassContents;
        internal string EqualityComparerClassContents;
        internal string ComparerClassContents;

        internal static string ComparerExtensionsClassContents =
@"using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace N
{
    [GeneratedCode(""Microsoft.Json.Schema.ToDotNet"", """ + VersionConstants.FileVersion + @""")]
    internal static class ComparerExtensions
    {
        public static bool TryReferenceCompares(this object left, object right, out int compareResult)
        {
            compareResult = 0;
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null)
            {
                compareResult = -1;
                return true;
            }

            if (right == null)
            {
                compareResult = 1;
                return true;
            }

            return false;
        }

        public static int ListCompares<T>(this IList<T> left, IList<T> right) where T : IComparable
        {
            return ListComparesHelper(left, right, (a, b) => a.CompareTo(b));
        }

        public static int ListCompares<T>(this IList<T> left, IList<T> right, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return ListComparesHelper(left, right, comparer.Compare);
        }

        public static int ListCompares<T>(this IList<T> left, IList<T> right, Func<T, T, int> compareFunction)
        {
            return ListComparesHelper(left, right, compareFunction);
        }

        private static int ListComparesHelper<T>(IList<T> left, IList<T> right, Func<T, T, int> compareFunction)
        {
            if (compareFunction == null)
            {
                throw new ArgumentNullException(nameof(compareFunction));
            }

            int compareResult = 0;
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            compareResult = left.Count.CompareTo(right.Count);
            if (compareResult != 0)
            {
                return compareResult;
            }

            for (int i = 0; i < left.Count; ++i)
            {
                if (left[i].TryReferenceCompares(right[i], out compareResult) && compareResult != 0)
                {
                    return compareResult;
                }

                compareResult = compareFunction(left[i], right[i]);
                if (compareResult != 0)
                {
                    return compareResult;
                }
            }

            return compareResult;
        }

        public static int DictionaryCompares<T>(this IDictionary<string, T> left, IDictionary<string, T> right) where T : IComparable
        {
            return DictionaryComparesHelper(left, right, (a, b) => a.CompareTo(b), (c, d) => c.CompareTo(d));
        }

        public static int DictionaryCompares<T>(this IDictionary<string, T> left, IDictionary<string, T> right, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return DictionaryComparesHelper(left, right, (a, b) => a.CompareTo(b), comparer.Compare);
        }

        public static int DictionaryCompares<TKey, TValue>(this IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right, Func<TKey, TKey, int> keyCompareFunction, Func<TValue, TValue, int> valueCompareFunction)
        {
            return DictionaryComparesHelper(left, right, (a, b) => keyCompareFunction(a, b), (c, d) => valueCompareFunction(c, d));
        }

        private static int DictionaryComparesHelper<TKey, TValue>(IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right, Func<TKey, TKey, int> keyCompareFunction, Func<TValue, TValue, int> valueCompareFunction)
        {
            if (keyCompareFunction == null)
            {
                throw new ArgumentNullException(nameof(keyCompareFunction));
            }

            if (valueCompareFunction == null)
            {
                throw new ArgumentNullException(nameof(valueCompareFunction));
            }

            int compareResult = 0;
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            compareResult = left.Count.CompareTo(right.Count);
            if (compareResult != 0)
            {
                return compareResult;
            }

            IList<TKey> leftKeys = left.Keys.ToList();
            IList<TKey> rightKeys = right.Keys.ToList();
            for (int i = 0; i < leftKeys.Count; ++i)
            {
                compareResult = keyCompareFunction(leftKeys[i], rightKeys[i]);
                if (compareResult != 0)
                {
                    return compareResult;
                }

                compareResult = valueCompareFunction(left[leftKeys[i]], right[rightKeys[i]]);
                if (compareResult != 0)
                {
                    return compareResult;
                }
            }

            return compareResult;
        }

        public static int UriCompares(this Uri left, Uri right)
        {
            int compareResult = 0;
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            return left.OriginalString.CompareTo(right.OriginalString);
        }

        public static int ObjectCompares(this object left, object right)
        {
            int compareResult = 0;
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            return Comparer<object>.Default.Compare(left, right);
        }
    }
}";
    }
}
