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
    using System.Drawing;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("{0} v{1}", this.ProductName, this.ProductVersion);

            toolStripComboBoxSort.Items.Add("Alphabetically");
            toolStripComboBoxSort.Items.Add("System first");
            toolStripComboBoxSort.Items.Add("Non-system first");
            toolStripComboBoxSort.SelectedIndex = 0;

            toolStripComboBoxFilter.Items.Add("All");
            toolStripComboBoxFilter.Items.Add("System only");
            toolStripComboBoxFilter.Items.Add("Non-system only");
            toolStripComboBoxFilter.SelectedIndex = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/knowncolors/");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, String.Format("{0}\n\nCopyright © 2007-2012 Vurdalakov\n\nhttp://www.vurdalakov.net/", this.Text),
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FillList(int mode)
        {
            int totalCount = 0;
            int systemCount = 0;
            int nonSystemCount = 0;

            listViewColors.Items.Clear();

            Array knownColors = Enum.GetValues(typeof(KnownColor));
            foreach (KnownColor knownColor in knownColors)
            {

                String colorName = knownColor.ToString();
                Color color = Color.FromName(colorName);

                totalCount++;
                if (color.IsSystemColor)
                {
                    systemCount++;
                }
                else
                {
                    nonSystemCount++;
                }

                if ((1 == mode) && !color.IsSystemColor)
                {
                    continue;
                }
                if ((2 == mode) && color.IsSystemColor)
                {
                    continue;
                }

                ListViewItem listViewItem = new ListViewItem(colorName);
                listViewItem.ToolTipText = colorName;

                ListViewItem.ListViewSubItem listViewSubItem = listViewItem.SubItems.Add("");
                listViewItem.UseItemStyleForSubItems = false;
                listViewSubItem.BackColor = color;

                listViewItem.Tag = color.IsSystemColor;
                listViewItem.SubItems.Add(color.IsSystemColor ? "Yes" : "");

                listViewItem.SubItems.Add(String.Format("{0:X2} {1:X2} {2:X2}", color.R, color.G, color.B));
                listViewItem.SubItems.Add(String.Format("{0:X2} {1:X2} {2:X2} {3:X2}", color.A, color.R, color.G, color.B));

                listViewColors.Items.Add(listViewItem);
            }

            toolStripStatusLabelCount.Text = String.Format("{0} known colors: {1} system and {2} non-system",
                totalCount, systemCount, nonSystemCount);
        }

        private void toolStripComboBoxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewColors.ListViewItemSorter = new ListViewItemComparer(toolStripComboBoxSort.SelectedIndex);
        }

        private void toolStripComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillList(toolStripComboBoxFilter.SelectedIndex);
        }

        private ListViewItem GetSelectedItem()
        {
            return 0 == listViewColors.SelectedItems.Count ? null : listViewColors.SelectedItems[0];
        }

        private void contextMenuStripListView_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ListViewItem listViewItem = GetSelectedItem();

            bool enabled = listViewItem != null;
            foreach (ToolStripMenuItem MenuItem in contextMenuStripListView.Items)
            {
                MenuItem.Enabled = enabled;
            }

            if (!enabled)
            {
                return;
            }

            Color color = listViewItem.SubItems[1].BackColor;

            copyNameToolStripMenuItem.Tag = listViewItem.Text;

            hexRgbToolStripMenuItem.Text = String.Format("{0:X2} {1:X2} {2:X2}", color.R, color.G, color.B);
            hexRgbToolStripMenuItem.Tag = hexRgbToolStripMenuItem.Text;

            decRgbToolStripMenuItem.Text = String.Format("{0} {1} {2}", color.R, color.G, color.B);
            decRgbToolStripMenuItem.Tag = decRgbToolStripMenuItem.Text;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            if ((toolStripMenuItem != null) && (toolStripMenuItem.Tag != null))
            {
                Clipboard.SetText(toolStripMenuItem.Tag as String);
            }
        }
    }
}