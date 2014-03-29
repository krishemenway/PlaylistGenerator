using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace PlaylistGeneratorTests
{
	public static class ShouldExtensions
	{
		public static void ShouldEqual(this IEnumerable<object> actual, IEnumerable<object> expected)
		{
			Assert.IsTrue(actual.SequenceEqual(expected), "Collections are not identical {0}" , RenderComparisonOfLists(actual, expected));
		}

		private static string RenderComparisonOfLists(IEnumerable<object> actual, IEnumerable<object> expected)
		{
			return string.Format("\r\n Actual: \r\n{0} \r\n\r\n Expected: \r\n{1}\r\n", RenderList(actual), RenderList(expected));
		}

		private static string RenderList(IEnumerable<object> listToRender)
		{
			return string.Join("\r\n", listToRender);
		}

		public static void ShouldEqual(this object actual, object expected)
		{
			Assert.That(actual, new EqualConstraint(expected));
		}

		public static void ShouldNotEqual(this object actual, object expected)
		{
			Assert.That(actual, new NotConstraint(new EqualConstraint(expected)));
		}

		public static void ShouldBeTrue(this bool value, string message = null)
		{
			Assert.IsTrue(value, message);
		}

		public static void ShouldBeFalse(this bool value, string message = null)
		{
			Assert.IsFalse(value, message);
		}

		public static void ShouldHaveCount<T>(this IEnumerable<T> list, int expectedCount)
		{
			list.Count().ShouldEqual(expectedCount);
		}

		public static void ShouldNotHaveCount<T>(this IEnumerable<T> list, int notCount)
		{
			list.Count().ShouldNotEqual(notCount);
		}

		public static void AllObjectsShould<T>(this IEnumerable<T> list, Func<T, bool> predicate, string message = null)
		{
			foreach(var item in list)
			{
				if(!predicate(item) && item != null)
				{
					Trace.WriteLine(item + " " + message);
				}
			}

			list.All(predicate).ShouldBeTrue(message);
		}
	}
}
