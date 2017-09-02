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
			Assert.Throws<ArgumentNullException>(() => DiffUtility.FindDifferences(null, new[] { 1, 2, 3 }));
		}

		[Test]
		public void DifferenceFirstSecond()
		{
			Assert.Throws<ArgumentNullException>(() => DiffUtility.FindDifferences(new[] { 1, 2, 3 }, null));
		}

		[Test]
		public void IntegerListDeleteDifference()
		{
			var difference = DiffUtility.FindDifferences(new[] { 1, 2, 3 }, new[] { 1, 3 });

			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 1), new IndexRange(1, 0)));
		}

		[Test]
		public void IntegerListInsertDifference()
		{
			var difference = DiffUtility.FindDifferences(new[] { 1, 3 }, new[] { 1, 2, 3 });

			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 0), new IndexRange(1, 1)));
		}

		[Test]
		public void IntegerListChangeDifference()
		{
			var difference = DiffUtility.FindDifferences(new[] { 1, 4, 3 }, new[] { 1, 2, 3 });

			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 1), new IndexRange(1, 1)));
		}

		[Test]
		public void IntegerListChangeDifferenceTwo()
		{
			var difference = DiffUtility.FindDifferences(new[] { 1, 4, 3 }, new[] { 1, 2, 2, 3 });

			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 1), new IndexRange(1, 2)));
		}

		[Test]
		public void IntegerListChangeDifferenceThree()
		{
			var difference = DiffUtility.FindDifferences(new[] { 1, 4, 3 }, new[] { 1, 2, 2, 2, 3 });

			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 1), new IndexRange(1, 3)));
		}

		[Test]
		public void IntegerListChangeDifferenceFour()
		{
			var difference = DiffUtility.FindDifferences(new[] { 1, 4, 4, 3 }, new[] { 1, 2, 2, 2, 3 });

			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 2), new IndexRange(1, 3)));
		}

		[Test]
		public void IntegerListChangeDifferenceFive()
		{
			var first = new[] { 1, 4, 4, 4, 4, 3 };
			var second = new[] { 1, 2, 2, 2, 3 };

			var difference = DiffUtility.FindDifferences(first, second);

			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 4), new IndexRange(1, 3)));
		}

		[Test]
		public void StringListChangeDifference()
		{
			var first = new[] { "1", "4", "4", "4", "4", "3" };
			var second = new[] { "1", "2", "2", "2", "3" };

			var difference = DiffUtility.FindDifferences(first, second);

			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 4), new IndexRange(1, 3)));
		}

		[Test]
		public void StringListChangeDifferenceTwo()
		{
			var first = new[] { "one", "Two", "three", "four", "five", "Six" };
			var second = new[] { "one", "two", "Three", "Four", "Six" };

			var difference = DiffUtility.FindDifferences(first, second);
			Assert.AreEqual(1, difference.Count);
			Assert.AreEqual(difference[0], (new IndexRange(1, 4), new IndexRange(1, 3)));
		}

		public static IEnumerable<StringDifference> StringDifferenceTests()
		{
			// test one
			var diff1 = new StringDifference
			{
				First = "This is a test",
				Second = "This is test",
				Changes = new List<ValueTuple<IndexRange, IndexRange>>
				{
					(new IndexRange(2, 1), new IndexRange(2, 0))
				},
			};
			yield return diff1;

			// test two
			var diff2 = new StringDifference
			{
				First = "This is test a",
				Second = "This is a test",
				Changes = new List<ValueTuple<IndexRange, IndexRange>>
				{
					(new IndexRange(2, 0), new IndexRange(2, 1)),
					(new IndexRange(3, 1), new IndexRange(4, 0))
				},
			};
			yield return diff2;

			// test three
			var diff3 = new StringDifference
			{
				First = "This is a test",
				Second = "This is test a",
				Changes = new List<ValueTuple<IndexRange, IndexRange>>
				{
					(new IndexRange(2, 0), new IndexRange(2, 1)),
					(new IndexRange(3, 1), new IndexRange(4, 0))
				},
			};
			yield return diff3;
		}

		[TestCaseSource(nameof(StringDifferenceTests))]
		public void StringListChangeDifferenceTwo(StringDifference difference)
		{
			var first = difference.First.Split(' ');
			var second = difference.Second.Split(' ');

			var result = DiffUtility.FindDifferences(first, second);
			CollectionAssert.AreEqual(difference.Changes, result);
		}

		public struct StringDifference
		{
			public string First;
			public string Second;
			public List<ValueTuple<IndexRange, IndexRange>> Changes;
		}
	}
}
