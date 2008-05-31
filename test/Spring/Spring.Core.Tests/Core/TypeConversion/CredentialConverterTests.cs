#region License

/*
 * Copyright � 2002-2007 the original author or authors.
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
using System.Net;

using NUnit.Framework;

#endregion

namespace Spring.Core.TypeConversion
{
    /// <summary>
    /// Unit tests for the CredentialConverter class.
    /// </summary>
    /// <author>Bruno Baia</author>
    /// <version>$Id: CredentialConverterTests.cs,v 1.1 2007/07/31 18:21:01 bbaia Exp $</version>
    [TestFixture]
    public sealed class CredentialConverterTests
    {
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertFromNullReference()
        {
            CredentialConverter cc = new CredentialConverter();
            cc.ConvertFrom(null);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ConvertFromNonSupportedOptionBails()
        {
            CredentialConverter cc = new CredentialConverter();
            cc.ConvertFrom(12);
        }

        [Test]
        public void ConvertFrom()
        {
            CredentialConverter cc = new CredentialConverter();
            object credential = cc.ConvertFrom(@"Spring\bbaia:sprnet");
            Assert.IsNotNull(credential);
            Assert.IsTrue(credential is NetworkCredential);
            
            NetworkCredential nc = (NetworkCredential)credential;
            Assert.AreEqual("Spring", nc.Domain);
            Assert.AreEqual("bbaia", nc.UserName);
            Assert.AreEqual("sprnet", nc.Password);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertFromEmptyString()
        {
            CredentialConverter cc = new CredentialConverter();
            cc.ConvertFrom(string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertFromMalformedString()
        {
            CredentialConverter cc = new CredentialConverter();
            object credential = cc.ConvertFrom(@"Spring:bbaia\sprnet");
        }

        [Test]
        public void ConvertFromStringWithoutDomain()
        {
            CredentialConverter cc = new CredentialConverter();
            object credential = cc.ConvertFrom(@"bbaia:sprnet");
            Assert.IsNotNull(credential);
            Assert.IsTrue(credential is NetworkCredential);

            NetworkCredential nc = (NetworkCredential)credential;
            Assert.AreEqual(string.Empty, nc.Domain);
            Assert.AreEqual("bbaia", nc.UserName);
            Assert.AreEqual("sprnet", nc.Password);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertFromStringWithIncorrectDomain()
        {
            CredentialConverter cc = new CredentialConverter();
            object credential = cc.ConvertFrom(@"\bbaia:sprnet");
        }

        [Test]
        public void ConvertFromStringWithoutPassword()
        {
            CredentialConverter cc = new CredentialConverter();
            object credential = cc.ConvertFrom(@"Spring\bbaia");
            Assert.IsNotNull(credential);
            Assert.IsTrue(credential is NetworkCredential);

            NetworkCredential nc = (NetworkCredential)credential;
            Assert.AreEqual("Spring", nc.Domain);
            Assert.AreEqual("bbaia", nc.UserName);
            Assert.AreEqual(string.Empty, nc.Password);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertFromStringWithIncorrectPassword()
        {
            CredentialConverter cc = new CredentialConverter();
            object credential = cc.ConvertFrom(@"\bbaia:");
        }

        [Test]
        public void ConvertFromStringWithUsernameOnly()
        {
            CredentialConverter cc = new CredentialConverter();
            object credential = cc.ConvertFrom(@"bbaia");
            Assert.IsNotNull(credential);
            Assert.IsTrue(credential is NetworkCredential);

            NetworkCredential nc = (NetworkCredential)credential;
            Assert.AreEqual(string.Empty, nc.Domain);
            Assert.AreEqual("bbaia", nc.UserName);
            Assert.AreEqual(string.Empty, nc.Password);
        }
    }
}