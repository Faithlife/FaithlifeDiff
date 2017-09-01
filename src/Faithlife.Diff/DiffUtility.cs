using System;
using System.Collections.Generic;
using System.Linq;
using Menees.Diffs;

namespace Faithlife.Diff
{
	/// <summary>
	/// Methods for manipulating lists.
	/// </summary>
	public static class DiffUtility
	{
		/// <summary>
		/// Finds the differences between the two lists.
		/// </summary>
		/// <typeparam name="T">The type of item in the lists.</typeparam>
		/// <param name="first">The first list.</param>
		/// <param name="second">The second list.</param>
		/// <returns>A sequence of pairs that indicate the differences between the lists.</returns>
		/// <remarks><para>Each pair identifies a range of items in each list. The items before and after each range
		/// are the same in both lists; the items in each range are the difference. One of the two ranges can be
		/// empty, but the index of the range still indicates where the missing items are.</para>
		/// <para>EqualityComparer&lt;T>.Default is used to compare items.</para></remarks>
		public static IReadOnlyList<(IndexRange FirstRange, IndexRange SecondRange)> FindDifferences<T>(IReadOnlyList<T> first, IReadOnlyList<T> second)
		{
			return FindDifferences(first, second, null);
		}

		/// <summary>
		/// Finds the differences between the two lists.
		/// </summary>
		/// <typeparam name="T">The type of item in the lists.</typeparam>
		/// <param name="first">The first list.</param>
		/// <param name="second">The second list.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>A sequence of pairs that indicate the differences between the lists.</returns>
		/// <remarks><para>Each pair identifies a range of items in each list. The items before and after each range
		/// are the same in both lists; the items in each range are the difference. One of the two ranges can be
		/// empty, but the index of the range still indicates where the missing items are.</para>
		/// <para>If comparer is null, EqualityComparer&lt;T>.Default is used.</para></remarks>
		public static IReadOnlyList<(IndexRange FirstRange, IndexRange SecondRange)> FindDifferences<T>(IReadOnlyList<T> first, IReadOnlyList<T> second,
			IEqualityComparer<T> comparer)
		{
			if (first == null)
				throw new ArgumentNullException(nameof(first));
			if (second == null)
				throw new ArgumentNullException(nameof(second));

			int[] anCodesFirst;
			int[] anCodesSecond;
			var listFirstOfInt = first as IEnumerable<int>;
			if (listFirstOfInt != null && (comparer == null || comparer == EqualityComparer<T>.Default))
			{
				anCodesFirst = listFirstOfInt.ToArray();
				anCodesSecond = ((IEnumerable<int>) second).ToArray();
			}
			else
			{
				Dictionary<T, int> dict = new Dictionary<T, int>(first.Count + second.Count, comparer);
				anCodesFirst = CreateUniqueIntegerForEachItem(first, dict);
				anCodesSecond = CreateUniqueIntegerForEachItem(second, dict);
			}

			return DoFindDifferences(anCodesFirst, anCodesSecond);
		}

		private static IReadOnlyList<(IndexRange FirstRange, IndexRange SecondRange)> DoFindDifferences(int[] anCodesFirst, int[] anCodesSecond)
		{
			MyersDiff<int> diff = new MyersDiff<int>(anCodesFirst, anCodesSecond, supportChangeEditType: true);
			EditScript script = diff.Execute();

			List<ValueTuple<IndexRange, IndexRange>> listChanges = new List<ValueTuple<IndexRange, IndexRange>>();

			IndexRange? rangeFirst = null;
			IndexRange? rangeSecond = null;

			foreach (Edit edit in script)
			{
				switch (edit.EditType)
				{
				case EditType.Insert:
					bool bExtendedSecond = false;

					if (listChanges.Count > 0)
					{
						ValueTuple<IndexRange, IndexRange> pairLastChange = listChanges[listChanges.Count - 1];

						if (pairLastChange.Item1.Start + pairLastChange.Item1.Length == edit.StartA &&
							pairLastChange.Item2.Start + pairLastChange.Item2.Length == edit.StartB)
						{
							IndexRange rangeVariant = pairLastChange.Item2;
							rangeVariant = new IndexRange(rangeVariant.Start, rangeVariant.Length + edit.Length);
							listChanges[listChanges.Count - 1] = (pairLastChange.Item1, rangeVariant);
							bExtendedSecond = true;
						}
					}

					if (!bExtendedSecond)
					{
						rangeFirst = new IndexRange(edit.StartA, 0);
						rangeSecond = new IndexRange(edit.StartB, edit.Length);
					}
					break;

				case EditType.Delete:
					bool bExtendedFirst = false;

					if (listChanges.Count > 0)
					{
						ValueTuple<IndexRange, IndexRange> pairLastChange = listChanges[listChanges.Count - 1];

						if (pairLastChange.Item1.Start + pairLastChange.Item1.Length == edit.StartA &&
							pairLastChange.Item2.Start + pairLastChange.Item2.Length == edit.StartB)
						{
							IndexRange rangeBase = pairLastChange.Item1;
							rangeBase = new IndexRange(rangeBase.Start, rangeBase.Length + edit.Length);
							listChanges[listChanges.Count - 1] = (rangeBase, pairLastChange.Item2);
							bExtendedFirst = true;
						}
					}

					if (!bExtendedFirst)
					{
						rangeFirst = new IndexRange(edit.StartA, edit.Length);
						rangeSecond = new IndexRange(edit.StartB, 0);
					}
					break;

				case EditType.Change:
					rangeFirst = new IndexRange(edit.StartA, edit.Length);
					rangeSecond = new IndexRange(edit.StartB, edit.Length);
					break;
				}

				if (rangeFirst != null)
				{
					listChanges.Add((rangeFirst.Value, rangeSecond.Value));
					rangeFirst = null;
					rangeSecond = null;
				}
			}

			return listChanges;
		}

		private static int[] CreateUniqueIntegerForEachItem<T>(IReadOnlyList<T> list, Dictionary<T, int> dict)
		{
			List<int> listCodes = new List<int>(list.Count);
			int nLastCode = dict.Count;

			foreach (T item in list)
			{
				int nCode;
				if (!dict.TryGetValue(item, out nCode))
				{
					nCode = ++nLastCode;
					dict[item] = nCode;
				}

				listCodes.Add(nCode);
			}

			return listCodes.ToArray();
		}
	}
}
