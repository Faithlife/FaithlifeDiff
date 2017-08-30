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
		/// <param name="listFirst">The first list.</param>
		/// <param name="listSecond">The second list.</param>
		/// <returns>A sequence of pairs that indicate the differences between the lists.</returns>
		/// <remarks><para>Each pair identifies a range of items in each list. The items before and after each range
		/// are the same in both lists; the items in each range are the difference. One of the two ranges can be
		/// empty, but the index of the range still indicates where the missing items are.</para>
		/// <para>EqualityComparer&lt;T>.Default is used to compare items.</para></remarks>
		public static IEnumerable<ValueTuple<IndexRange, IndexRange>> FindDifferences<T>(IList<T> listFirst, IList<T> listSecond)
		{
			return FindDifferences(listFirst, listSecond, null);
		}

		/// <summary>
		/// Finds the differences between the two lists.
		/// </summary>
		/// <typeparam name="T">The type of item in the lists.</typeparam>
		/// <param name="listFirst">The first list.</param>
		/// <param name="listSecond">The second list.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>A sequence of pairs that indicate the differences between the lists.</returns>
		/// <remarks><para>Each pair identifies a range of items in each list. The items before and after each range
		/// are the same in both lists; the items in each range are the difference. One of the two ranges can be
		/// empty, but the index of the range still indicates where the missing items are.</para>
		/// <para>If comparer is null, EqualityComparer&lt;T>.Default is used.</para></remarks>
		public static IEnumerable<ValueTuple<IndexRange, IndexRange>> FindDifferences<T>(IList<T> listFirst, IList<T> listSecond,
			IEqualityComparer<T> comparer)
		{
			if (listFirst == null)
				throw new ArgumentNullException("listFirst");
			if (listSecond == null)
				throw new ArgumentNullException("listSecond");

			int[] anCodesFirst;
			int[] anCodesSecond;
			IList<int> listFirstOfInt = listFirst as IList<int>;
			if (listFirstOfInt != null && (comparer == null || comparer == EqualityComparer<T>.Default))
			{
				anCodesFirst = listFirstOfInt.ToArray();
				anCodesSecond = ((IList<int>) listSecond).ToArray();
			}
			else
			{
				Dictionary<T, int> dict = new Dictionary<T, int>(listFirst.Count + listSecond.Count, comparer);
				anCodesFirst = CreateUniqueIntegerForEachItem(listFirst, dict);
				anCodesSecond = CreateUniqueIntegerForEachItem(listSecond, dict);
			}

			return DoFindDifferences(anCodesFirst, anCodesSecond);
		}

		private static IEnumerable<ValueTuple<IndexRange, IndexRange>> DoFindDifferences(int[] anCodesFirst, int[] anCodesSecond)
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
							listChanges[listChanges.Count - 1] = new ValueTuple<IndexRange, IndexRange>(pairLastChange.Item1, rangeVariant);
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
							listChanges[listChanges.Count - 1] = new ValueTuple<IndexRange, IndexRange>(rangeBase, pairLastChange.Item2);
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
					listChanges.Add(new ValueTuple<IndexRange, IndexRange>(rangeFirst.Value, rangeSecond.Value));
					rangeFirst = null;
					rangeSecond = null;
				}
			}

			return listChanges;
		}

		private static int[] CreateUniqueIntegerForEachItem<T>(IList<T> list, Dictionary<T, int> dict)
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
