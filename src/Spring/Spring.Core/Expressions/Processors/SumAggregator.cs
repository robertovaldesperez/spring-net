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

using System;
using System.Collections;
using Spring.Util;

namespace Spring.Expressions.Processors
{
    /// <summary>
    /// Implementation of the sum aggregator.
    /// </summary>
    /// <author>Aleksandar Seovic</author>
    /// <version>$Id: SumAggregator.cs,v 1.2 2006/12/04 08:58:33 aseovic Exp $</version>
    public class SumAggregator : ICollectionProcessor
    {
        /// <summary>
        /// Returns the sum of the numeric values in the source collection.
        /// </summary>
        /// <param name="source">
        /// The source collection to process.
        /// </param>
        /// <param name="args">
        /// Ignored.
        /// </param>
        /// <returns>
        /// The sum of the numeric values in the source collection.
        /// </returns>
        public object Process(ICollection source, object[] args)
        {
            object total = 0d;
            foreach (object item in source)
            {
                if (item != null)
                {
                    if (NumberUtils.IsNumber(item))
                    {
                        total = NumberUtils.Add(total, item);
                    }
                    else
                    {
                        throw new ArgumentException("Sum can only be calculated for a collection of numeric values.");
                    }
                }
            }
            
            return total;
        }
    }
}