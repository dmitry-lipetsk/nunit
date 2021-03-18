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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Filters
{
    internal sealed class CompositeFilterNode
    {
        public CompositeFilterNode()
        {
            m_FilterValues__Direct = new HashSet<string>();

            m_FilterValues__WithRegExp = new HashSet<Regex>();
        }

        //------------------------------------------------------
        public bool Add(ValueMatchFilter filter)
        {
            if (filter.IsRegex)
                return m_FilterValues__WithRegExp.Add(new Regex(filter.ExpectedValue));

            return m_FilterValues__Direct.Add(filter.ExpectedValue);
        }//Add

        //------------------------------------------------------
        public bool Test(string value)
        {
            if (m_FilterValues__Direct.Contains(value))
            {
                return true;
            }//if

            if (!object.ReferenceEquals(value, null))
            {
                foreach(var r in m_FilterValues__WithRegExp)
                {
                    if (r.IsMatch(value))
                    {
                        return true;
                    }//if
                }
            }//if

            return false;
        }//Test

        //------------------------------------------------------
        private readonly HashSet<string> m_FilterValues__Direct;

        private readonly HashSet<Regex> m_FilterValues__WithRegExp;
    };//class CompositeFilterNode
}
