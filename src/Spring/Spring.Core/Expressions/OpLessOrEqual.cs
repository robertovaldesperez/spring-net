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
using System.Runtime.Serialization;
using Spring.Util;

namespace Spring.Expressions
{
    /// <summary>
    /// Represents logical "less than or equal" operator.
    /// </summary>
    /// <author>Aleksandar Seovic</author>
    /// <version>$Id: OpLessOrEqual.cs,v 1.10 2007/09/07 03:01:26 markpollack Exp $</version>
    [Serializable]
    public class OpLessOrEqual : BinaryOperator
    {
        /// <summary>
        /// Create a new instance
        /// </summary>
        public OpLessOrEqual():base()
        {
        }

        /// <summary>
        /// Create a new instance from SerializationInfo
        /// </summary>
        protected OpLessOrEqual(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        
        /// <summary>
        /// Returns a value for the logical "less than or equal" operator node.
        /// </summary>
        /// <param name="context">Context to evaluate expressions against.</param>
        /// <param name="evalContext">Current expression evaluation context.</param>
        /// <returns>Node's value.</returns>
        protected override object Get(object context, EvaluationContext evalContext)
        {
            object left = Left.GetValueInternal( context, evalContext );
            object right = Right.GetValueInternal( context, evalContext );

            return CompareUtils.Compare(left, right) <= 0;
        }
    }
}