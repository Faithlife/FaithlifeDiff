using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Faithlife.Diff.Tests
{
	[TestFixture]
	public class DiffUtilityTests
	{
		[Test]
		public void DifferenceFirstNull()
		{
			Assert.Throws<ArgumentNullException>(() => DiffUtility.FindDifferences(null, new int[] { 1, 2, 3 }));
		}

		[Test]
		public void DifferenceFirstSecond()
		{
			Assert.Throws<ArgumentNullException>(() => DiffUtility.FindDifferences(new int[] { 1, 2, 3 }, null));
		}

		[Test]
		public void IntegerListDeleteDifference()
		{
			List<int> listFirst = new List<int>();
			listFirst.Add(1);
			listFirst.Add(2);
			listFirst.Add(3);

			List<int> listSecond = new List<int>();
			listSecond.Add(1);
			listSecond.Add(3);

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 1), new IndexRange(1, 0)));
		}

		[Test]
		public void IntegerListInsertDifference()
		{
			List<int> listFirst = new List<int>();
			listFirst.Add(1);
			listFirst.Add(3);

			List<int> listSecond = new List<int>();
			listSecond.Add(1);
			listSecond.Add(2);
			listSecond.Add(3);

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 0), new IndexRange(1, 1)));
		}

		[Test]
		public void IntegerListChangeDifference()
		{
			List<int> listFirst = new List<int>();
			listFirst.Add(1);
			listFirst.Add(4);
			listFirst.Add(3);

			List<int> listSecond = new List<int>();
			listSecond.Add(1);
			listSecond.Add(2);
			listSecond.Add(3);

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 1), new IndexRange(1, 1)));
		}

		[Test]
		public void IntegerListChangeDifferenceTwo()
		{
			List<int> listFirst = new List<int>();
			listFirst.Add(1);
			listFirst.Add(4);
			listFirst.Add(3);

			List<int> listSecond = new List<int>();
			listSecond.Add(1);
			listSecond.Add(2);
			listSecond.Add(2);
			listSecond.Add(3);

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 1), new IndexRange(1, 2)));
		}

		[Test]
		public void IntegerListChangeDifferenceThree()
		{
			List<int> listFirst = new List<int>();
			listFirst.Add(1);
			listFirst.Add(4);
			listFirst.Add(3);

			List<int> listSecond = new List<int>();
			listSecond.Add(1);
			listSecond.Add(2);
			listSecond.Add(2);
			listSecond.Add(2);
			listSecond.Add(3);

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 1), new IndexRange(1, 3)));
		}

		[Test]
		public void IntegerListChangeDifferenceFour()
		{
			List<int> listFirst = new List<int>();
			listFirst.Add(1);
			listFirst.Add(4);
			listFirst.Add(4);
			listFirst.Add(3);

			List<int> listSecond = new List<int>();
			listSecond.Add(1);
			listSecond.Add(2);
			listSecond.Add(2);
			listSecond.Add(2);
			listSecond.Add(3);

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 2), new IndexRange(1, 3)));
		}

		[Test]
		public void IntegerListChangeDifferenceFive()
		{
			List<int> listFirst = new List<int>();
			listFirst.Add(1);
			listFirst.Add(4);
			listFirst.Add(4);
			listFirst.Add(4);
			listFirst.Add(4);
			listFirst.Add(3);

			List<int> listSecond = new List<int>();
			listSecond.Add(1);
			listSecond.Add(2);
			listSecond.Add(2);
			listSecond.Add(2);
			listSecond.Add(3);

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 4), new IndexRange(1, 3)));
		}

		[Test]
		public void StringListChangeDifference()
		{
			List<string> listFirst = new List<string>();
			listFirst.Add("1");
			listFirst.Add("4");
			listFirst.Add("4");
			listFirst.Add("4");
			listFirst.Add("4");
			listFirst.Add("3");

			List<string> listSecond = new List<string>();
			listSecond.Add("1");
			listSecond.Add("2");
			listSecond.Add("2");
			listSecond.Add("2");
			listSecond.Add("3");

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 4), new IndexRange(1, 3)));
		}

		[Test]
		public void StringListChangeDifferenceTwo()
		{
			List<string> listFirst = new List<string>();
			listFirst.Add("one");
			listFirst.Add("Two");
			listFirst.Add("three");
			listFirst.Add("four");
			listFirst.Add("five");
			listFirst.Add("Six");

			List<string> listSecond = new List<string>();
			listSecond.Add("one");
			listSecond.Add("two");
			listSecond.Add("Three");
			listSecond.Add("Four");
			listSecond.Add("Six");

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));
			Assert.AreEqual(1, listDifference.Count);
			Assert.AreEqual(listDifference[0], new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 4), new IndexRange(1, 3)));
		}

		public static IEnumerable<StringDifference> StringDifferenceTests()
		{
			// test one
			StringDifference diff1 = new StringDifference();
			diff1.First = "This is a test";
			diff1.Second = "This is test";
			diff1.Changes = new List<ValueTuple<IndexRange, IndexRange>>(new[] {
					new ValueTuple<IndexRange, IndexRange>(new IndexRange(2, 1), new IndexRange(2, 0))
				});
			yield return diff1;

			// test two
			StringDifference diff2 = new StringDifference();
			diff2.First = "This is test a";
			diff2.Second = "This is a test";
			diff2.Changes = new List<ValueTuple<IndexRange, IndexRange>>(new[] {
					new ValueTuple<IndexRange, IndexRange>(new IndexRange(2, 0), new IndexRange(2, 1)),
					new ValueTuple<IndexRange, IndexRange>(new IndexRange(3, 1), new IndexRange(4, 0))
				});
			yield return diff2;

			// test three
			StringDifference diff3 = new StringDifference();
			diff3.First = "This is a test";
			diff3.Second = "This is test a";
			diff3.Changes = new List<ValueTuple<IndexRange, IndexRange>>(new[] {
					new ValueTuple<IndexRange, IndexRange>(new IndexRange(2, 0), new IndexRange(2, 1)),
					new ValueTuple<IndexRange, IndexRange>(new IndexRange(3, 1), new IndexRange(4, 0))
				});
			yield return diff3;
		}

		[TestCaseSource("StringDifferenceTests")]
		public void StringListChangeDifferenceTwo(StringDifference difference)
		{
			List<string> listFirst = new List<string>(difference.First.Split(' '));
			List<string> listSecond = new List<string>(difference.Second.Split(' '));

			List<ValueTuple<IndexRange, IndexRange>> listDifference = new List<ValueTuple<IndexRange, IndexRange>>(DiffUtility.FindDifferences(listFirst, listSecond));

			CollectionAssert.AreEqual(difference.Changes, listDifference);
		}

		[Test]
		public void ListDifferenceEquality()
		{
			ValueTuple<IndexRange, IndexRange> ld1111 = new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 1), new IndexRange(1, 1));
			ValueTuple<IndexRange, IndexRange> ld1111b = new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 1), new IndexRange(1, 1));
			ValueTuple<IndexRange, IndexRange> ld1112 = new ValueTuple<IndexRange, IndexRange>(new IndexRange(1, 1), new IndexRange(1, 2));
			ValueTuple<IndexRange, IndexRange> ld2111 = new ValueTuple<IndexRange, IndexRange>(new IndexRange(2, 1), new IndexRange(1, 1));

			Assert.IsTrue(ld1111.Equals(ld1111b));
			Assert.IsTrue(ld1111.Equals((object) ld1111b));
			Assert.IsFalse(ld1111.Equals((object) null));
			Assert.IsFalse(ld1111.Equals(ld1112));
			Assert.IsFalse(ld1111.Equals((object) ld1112));
			Assert.IsTrue(ld1111.GetHashCode() == ld1111b.GetHashCode());

			// TODO: okay to remove these?
			//Assert.IsTrue(ld1111 == ld1111b);
			//Assert.IsTrue(ld1111 != ld1112);
			//Assert.IsTrue(ld1111 != ld2111);
			//Assert.IsFalse(ld1111 == ld1112);
			//Assert.IsFalse(ld1111 == ld2111);
			//Assert.IsFalse(ld1111 != ld1111b);
		}

		public struct StringDifference
		{
			public string First;
			public string Second;
			public List<ValueTuple<IndexRange, IndexRange>> Changes;
		}
	}
}
