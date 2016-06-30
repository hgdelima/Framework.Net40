using System;
using System.Collections.Generic;

namespace Alcoa.Framework.Common.Entity
{
    /// <summary>
    /// Class that implements IEqualityComparer to compare two objects instances and properties
    /// </summary>
	public class EqualBy<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, Boolean> _comparer;
		private readonly Func<T, int> _hashCodeEvaluator;

		/// <summary>
		/// Initializes an equality using a function to compare objects and its properties
		/// </summary>
		public EqualBy(Func<T, T, Boolean> comparer)
		{
			_comparer = comparer;
		}

        /// <summary>
        /// Initializes an equality using a function to compare objects, its properties and a hash code evaluator
        /// </summary>
        public EqualBy(Func<T, T, Boolean> comparer, Func<T, int> hashCodeEvaluator)
		{
			_comparer = comparer;
			_hashCodeEvaluator = hashCodeEvaluator;
		}

		public bool Equals(T x, T y)
		{
			return _comparer(x, y);
		}

		public int GetHashCode(T obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			
			return _hashCodeEvaluator == null ? 0 : _hashCodeEvaluator(obj);
		}
	}
}