using System;

namespace Faithlife.Diff
{
	/// <summary>
	/// Structure representing an index and its range.
	/// </summary>
	public struct IndexRange : IEquatable<IndexRange>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IndexRange"/> class.
		/// </summary>
		/// <param name="start">The start of the index range.</param>
		/// <param name="length">Length of the index range.</param>
		public IndexRange(int start, int length)
		{
			Start = start;
			Length = length;
		}

		/// <summary>
		/// Start of the index range.
		/// </summary>
		public int Start { get; }

		/// <summary>
		/// Length of the index range.
		/// </summary>
		public int Length { get; }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the other parameter; otherwise, false.
		/// </returns>
		public bool Equals(IndexRange other) => Start == other.Start && Length == other.Length;

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>
		/// true if obj and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override bool Equals(object obj) => obj is IndexRange && Equals((IndexRange) obj);

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode() => Start * 33 + Length;

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> the instances are equal; otherwise, <c>false</c>.</returns>
		public static bool operator ==(IndexRange left, IndexRange right) => left.Equals(right);

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
		public static bool operator !=(IndexRange left, IndexRange right) => !left.Equals(right);
	}
}
