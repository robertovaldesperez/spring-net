#region License

/*
 * Copyright � 2002-2005 the original author or authors.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System;
using System.Collections;
using System.Reflection;

#endregion

namespace Spring.Util
{
	/// <summary>
	/// Miscellaneous collection utility methods.
	/// </summary>
	/// <remarks>
	/// Mainly for internal use within the framework.
	/// </remarks>
	/// <author>Mark Pollack (.NET)</author>
	/// <version>$Id: CollectionUtils.cs,v 1.9 2006/04/09 07:19:00 markpollack Exp $</version>
	public sealed class CollectionUtils
	{
		#region Methods

		/// <summary>
		/// Determine whether a given collection only contains
		/// a single unique object
		/// </summary>
		/// <param name="coll"></param>
		/// <returns></returns>
		public static bool HasUniqueObject(ICollection coll)
		{
			if (coll.Count == 0)
			{
				return false;
			}
			object candidate = null;
			foreach (object elem in coll)
			{
				if (candidate == null)
				{
					candidate = elem;
				}
				else if (candidate != elem)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Determines whether the <paramref name="collection"/> contains the specified <paramref name="element"/>.
		/// </summary>
		/// <param name="collection">The collection to check.</param>
		/// <param name="element">The object to locate in the collection.</param>
		/// <returns><see lang="true"/> if the element is in the collection, <see lang="false"/> otherwise.</returns>
		public static bool Contains(ICollection collection, Object element)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("Collection cannot be null.");
			}
			MethodInfo method;
			method = collection.GetType().GetMethod("contains", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			if (null == method)
			{
				throw new InvalidOperationException("Collection type " + collection.GetType() + " does not implement a Contains() method.");
			}
			return (bool) method.Invoke(collection, new Object[] {element});
		}

		/// <summary>
		/// Adds the specified <paramref name="element"/> to the specified <paramref name="collection"/> .
		/// </summary>
		/// <param name="collection">The collection to add the element to.</param>
		/// <param name="element">The object to add to the collection.</param>
		public static void Add(ICollection collection, object element)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("Collection cannot be null.");
			}
			MethodInfo method;
			method = collection.GetType().GetMethod("add", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			if (null == method)
			{
				throw new InvalidOperationException("Collection type " + collection.GetType() + " does not implement a Add() method.");
			}
			method.Invoke(collection, new Object[] {element});
		}

		/// <summary>
		/// Determines whether the collection contains all the elements in the specified collection.
		/// </summary>
		/// <param name="targetCollection">The collection to check.</param>
		/// <param name="sourceCollection">Collection whose elements would be checked for containment.</param>
		/// <returns>true if the target collection contains all the elements of the specified collection.</returns>
		public static bool ContainsAll(ICollection targetCollection, ICollection sourceCollection)
		{
			if (targetCollection == null || sourceCollection == null)
			{
				throw new ArgumentNullException("Collection cannot be null.");
			}
			if ( sourceCollection.Count == 0 && targetCollection.Count > 1 )
				return true;

			IEnumerator sourceCollectionEnumerator = sourceCollection.GetEnumerator();

			bool contains = false;

			MethodInfo method;
			method = targetCollection.GetType().GetMethod("containsAll", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

			if (method != null)
				contains = (bool) method.Invoke(targetCollection, new Object[] {sourceCollection});
			else
			{
				method = targetCollection.GetType().GetMethod("Contains", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					throw new InvalidOperationException("Target collection does not implment a Contains() or ContainsAll() method.");
				}
				while (sourceCollectionEnumerator.MoveNext() == true)
				{
					if ((contains = (bool) method.Invoke(targetCollection, new Object[] {sourceCollectionEnumerator.Current})) == false)
						break;
				}
			}
			return contains;
		}

		/// <summary>
		/// Removes all the elements from the target collection that are contained in the source collection.
		/// </summary>
		/// <param name="targetCollection">Collection where the elements will be removed.</param>
		/// <param name="sourceCollection">Elements to remove from the target collection.</param>
		public static void RemoveAll(ICollection targetCollection, ICollection sourceCollection)
		{
			if (targetCollection == null || sourceCollection == null)
			{
				throw new ArgumentNullException("Collection cannot be null.");
			}
			ArrayList al = ToArrayList(sourceCollection);

			MethodInfo method;
				method = targetCollection.GetType().GetMethod("removeAll", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

				if (method != null)
					method.Invoke(targetCollection, new Object[] {al});
				else
				{
					method = targetCollection.GetType().GetMethod("Remove", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new Type[1] {typeof(object)}, null );
					MethodInfo methodContains = targetCollection.GetType().GetMethod("Contains", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
					if ( method == null )
					{
						throw new InvalidOperationException("Target Collection must implement either a RemoveAll() or Remove() method.");
					}
					if ( methodContains == null )
					{
						throw new InvalidOperationException("TargetCollection must implement a Contains() method.");
					}
					IEnumerator e = al.GetEnumerator();
					while (e.MoveNext() == true)
					{
						while ((bool) methodContains.Invoke(targetCollection, new Object[] {e.Current}) == true)
							method.Invoke(targetCollection, new Object[] {e.Current});
					}
				}
		}

		/// <summary>
		/// Converts an <see cref="System.Collections.ICollection"/>instance to an <see cref="System.Collections.ArrayList"/> instance.
		/// </summary>
		/// <param name="inputCollection">The <see cref="System.Collections.ICollection"/> instance to be converted.</param>
		/// <returns>An <see cref="System.Collections.ArrayList"/> instance in which its elements are the elements of the <see cref="System.Collections.ICollection"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException">if the <paramref name="inputCollection"/> is null.</exception>
		public static ArrayList ToArrayList(ICollection inputCollection)
		{
			if ( inputCollection == null )
			{
				throw new ArgumentNullException("Collection cannot be null.");
			}
			return new ArrayList(inputCollection);
		}

		#endregion
	}
}