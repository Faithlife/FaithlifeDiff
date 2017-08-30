using NUnit.Framework;

namespace Faithlife.Diff.Tests
{
	[TestFixture]
	public class IndexRangeTests
	{
		[Test]
		public void Start()
		{
			IndexRange ts = new IndexRange(3, 4);
			Assert.AreEqual(3, ts.Start);
		}

		[Test]
		public void Length()
		{
			IndexRange ts = new IndexRange(3, 4);
			Assert.AreEqual(4, ts.Length);
		}

		[Test]
		public void Equal()
		{
			IndexRange ts1 = new IndexRange(3, 4);
			IndexRange ts2 = new IndexRange(3, 4);

			Assert.IsTrue(ts1 == ts2);
			Assert.IsTrue(ts2 == ts1);
			Assert.IsFalse(ts1 != ts2);
			Assert.IsFalse(ts2 != ts1);
			Assert.IsTrue(ts1.Equals(ts2));
			Assert.IsTrue(ts2.Equals(ts1));
			Assert.IsTrue(ts1.Equals((object) ts2));
			Assert.IsTrue(ts2.Equals((object) ts1));
			Assert.IsTrue(ts1.GetHashCode() == ts2.GetHashCode());
		}

		[Test]
		public void NotEqualStart()
		{
			IndexRange ts1 = new IndexRange(3, 4);
			IndexRange ts2 = new IndexRange(2, 4);

			Assert.IsFalse(ts1 == ts2);
			Assert.IsFalse(ts2 == ts1);
			Assert.IsTrue(ts1 != ts2);
			Assert.IsTrue(ts2 != ts1);
			Assert.IsFalse(ts1.Equals(ts2));
			Assert.IsFalse(ts2.Equals(ts1));
			Assert.IsFalse(ts1.Equals((object) ts2));
			Assert.IsFalse(ts2.Equals((object) ts1));
		}

		[Test]
		public void NotEqualLength()
		{
			IndexRange ts1 = new IndexRange(3, 4);
			IndexRange ts2 = new IndexRange(3, 5);

			Assert.IsFalse(ts1 == ts2);
			Assert.IsFalse(ts2 == ts1);
			Assert.IsTrue(ts1 != ts2);
			Assert.IsTrue(ts2 != ts1);
			Assert.IsFalse(ts1.Equals(ts2));
			Assert.IsFalse(ts2.Equals(ts1));
			Assert.IsFalse(ts1.Equals((object) ts2));
			Assert.IsFalse(ts2.Equals((object) ts1));
		}
	}
}
