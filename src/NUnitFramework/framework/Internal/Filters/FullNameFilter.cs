// ***********************************************************************
// Copyright (c) 2015 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Filters
{
    /// <summary>
    /// FullName filter selects tests based on their FullName
    /// </summary>
    internal class FullNameFilter : ValueMatchFilter
    {
        /// <summary>
        /// Construct a FullNameFilter for a single name
        /// </summary>
        /// <param name="expectedValue">The name the filter will recognize.</param>
        public FullNameFilter(string expectedValue) : base(expectedValue) { }

        public override bool Pass(ITest test, bool negated)
        {
               if(negated)
                    return base.Pass(test, negated);

               //---------------------------
               if (this.IsRegex)
                    return base.Pass(test, negated);

               for(;;)
               {
                   if(test.GetType()==typeof(TestSuite))
                   {
                        var t=test as TestSuite;

                        if (t.FullName.StartsWith(this.ExpectedValue))
                        {
                            if (t.FullName.Length==this.ExpectedValue.Length)
                                return true;

                            test=t.Parent;

                            if (object.ReferenceEquals(test,null))
                                return false;

                            continue;
                        }//if

                        //Skip all parents TestSuite

                        for(;;)
                        {
                            test=test.Parent;

                            if (object.ReferenceEquals(test,null))
                                return false;

                            if (test.GetType()!=typeof(TestSuite))
                                break;
                        }//for[ever]

                        continue;
                   }//if

                   break;
               }//for[ever]

               return base.Pass(test, negated);
        }//Pass

        /// <summary>
        /// Match a test against a single value.
        /// </summary>
        public override bool Match(ITest test)
        {
            return Match(test.FullName);
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "test"; }
        }
    }
}
