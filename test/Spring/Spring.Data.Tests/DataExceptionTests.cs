#region License

/*
 * Copyright 2004 the original author or authors.
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
using System.Reflection;

using NUnit.Framework;

#endregion

namespace Spring
{
    /// <summary>
    /// Unit tests for all of the exception classes in the Spring.Data library...
    /// </summary>
    /// <author>Rick Evans</author>
    /// <version>$Id: DataExceptionTests.cs,v 1.6 2006/05/18 21:37:53 markpollack Exp $</version>
    [TestFixture]
    public sealed class DataExceptionTests : ExceptionsTest
    {
        [TestFixtureSetUp]
        public void FixtureSetUp ()
        {
            AssemblyToCheck = Assembly.GetAssembly (typeof (Spring.Transaction.CannotCreateTransactionException));
        }
    }
}