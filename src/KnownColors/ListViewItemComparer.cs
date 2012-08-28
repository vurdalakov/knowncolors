// This software is part of the Known Colors application.
//
// Copyright (c) 2012 Vurdalakov
// http://www.vurdalakov.net
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

namespace Vurdalakov.KnownColors
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    internal class ListViewItemComparer : IComparer
    {
        private int mode;

        public ListViewItemComparer(int mode)
        {
            this.mode = mode;
        }

        public int Compare(Object object1, Object object2)
        {
            ListViewItem item1 = object1 as ListViewItem;
            ListViewItem item2 = object2 as ListViewItem;

            Boolean isSystem1 = (Boolean)item1.Tag;
            Boolean isSystem2 = (Boolean)item2.Tag;

            if (1 == mode) // system first
            {
                if (!isSystem1 && isSystem2)
                    return 1;
                if (isSystem1 && !isSystem2)
                    return -1;
            }
            else if (2 == mode) // non-system first
            {
                if (isSystem1 && !isSystem2)
                    return 1;
                if (!isSystem1 && isSystem2)
                    return -1;
            }

            return String.Compare(item1.Text, item2.Text);
        }
    }
}
