using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
/*
 * Author: Yanzhi Wang
 * 
 * Purpose: This class represents the main form of MyEditor application. It handles user interface events, such as opening, saving, and editing text files, as well as font and color selection. 
 * 
 * Restrictions: None
 */

namespace MyEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Register event handlers for menu items and toolbar buttons
            this.newToolStripMenuItem.Click += new EventHandler(NewToolStripMenuItem_Click);
            this.openToolStripMenuItem.Click += new EventHandler(OpenToolStripMenuItem_Click);
            this.saveToolStripMenuItem.Click += new EventHandler(SaveToolStripMenuItem_Click);
            this.exitToolStripMenuItem.Click += new EventHandler(ExitToolStripMenuItem_Click);
            this.copyToolStripMenuItem.Click += new EventHandler(CopyToolStripMenuItem_Click);
            this.cutToolStripMenuItem.Click += new EventHandler(CutToolStripMenuItem_Click);
            this.pasteToolStripMenuItem.Click += new EventHandler(PasteToolStripMenuItem_Click);

            this.boldToolStripMenuItem.Click += new EventHandler(BoldToolStripMenuItem_Click);
            this.italicsToolStripMenuItem.Click += new EventHandler(ItalicsToolStripMenuItem_Click);
            this.underlineToolStripMenuItem.Click += new EventHandler(UnderlineToolStripMenuItem_Click);

            this.testToolStripButton.Click += new EventHandler(TestToolStripButton_Click);

            this.mSSansSerifToolStripMenuItem.Click += new EventHandler(MSSansSerifToolStripMenuItem_Click);
            this.timesNewRomanToolStripMenuItem.Click += new EventHandler(TimesNewRomanToolStripMenuItem_Click);
            this.richTextBox.SelectionChanged += new EventHandler(RichTextBox_SelectionChanged);

            this.countdownLabel.Visible = false;
            this.timer.Tick += new EventHandler(Timer_Tick);

            this.toolStrip.ItemClicked += new ToolStripItemClickedEventHandler(ToolStrip_ItemClicked);

            // Set initial window title
            this.Text = "MyEditor";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // disable the RichTextBox initially
            richTextBox.Enabled = false;
        }

        private void testToolStripButton_Click(object sender, EventArgs e)
        {
            // enable the RichTextBox after the button is clicked
            richTextBox.Enabled = true;
        }




        private void Timer_Tick(object sender, EventArgs e)
        {
            --this.toolStripProgressBar1.Value;
            if (this.toolStripProgressBar1.Value == 0)
            {
                this.timer.Stop();
                string performance = "Congratulations. You typed " + Math.Round(this.richTextBox.TextLength / 30.0, 2) + " letters per second";
                MessageBox.Show(performance);
            }
        }

        private void BoldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontStyle fontStyle = FontStyle.Bold;
            Font selectionFont = null;
            selectionFont = richTextBox.SelectionFont;

            if (selectionFont == null)
            {
                selectionFont = richTextBox.Font;
            }
            SetSelectionFont(fontStyle, !selectionFont.Bold);
        }

        private void TestToolStripButton_Click(object sender, EventArgs e)
        {
          
            this.timer.Interval = 500;
            this.toolStripProgressBar1.Value = 60;
            this.countdownLabel.Text = "3";
            this.countdownLabel.Visible = true;
            this.richTextBox.Visible = false;
            for (int i = 3; i > 0; --i)
            {
                this.countdownLabel.Text = i.ToString();
                this.Refresh();
                Thread.Sleep(1000);
            }
            this.countdownLabel.Visible = false;
            this.richTextBox.Visible = true;
            this.timer.Start();
        }

        private void ItalicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontStyle fontStyle = FontStyle.Italic;
            Font selectionFont = null;
            selectionFont = richTextBox.SelectionFont;

            if (selectionFont == null)
            {
                selectionFont = richTextBox.Font;
            }
            SetSelectionFont(fontStyle, !selectionFont.Italic);
        }

        private void UnderlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontStyle fontStyle = FontStyle.Underline;
            Font selectionFont = null;
            selectionFont = richTextBox.SelectionFont;

            if (selectionFont == null)
            {
                selectionFont = richTextBox.Font;
            }
            SetSelectionFont(fontStyle, !selectionFont.Underline);
        }

        private void MSSansSerifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Font newFont = new Font("MS Sans Serif", richTextBox.SelectionFont.Size, richTextBox.SelectionFont.Style);
            richTextBox.SelectionFont = newFont;
        }
        private void TimesNewRomanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Font newFont = new Font("Times New Roman", richTextBox.SelectionFont.Size, richTextBox.SelectionFont.Style);
            richTextBox.SelectionFont = newFont;
        }

        private void RichTextBox_SelectionChanged(object sender, EventArgs e)
        {

            if (this.richTextBox.SelectionFont != null)
            {
                this.boldToolStripButton.Checked = richTextBox.SelectionFont.Bold;
                this.italicsToolStripButton.Checked = richTextBox.SelectionFont.Italic;
                this.underlineToolStripButton.Checked = richTextBox.SelectionFont.Underline;
            }
            this.colorToolStripButton.BackColor = richTextBox.SelectionColor;
        }


        // Method to handle "New" menu item click
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear the rich text box and reset window title
            richTextBox.Clear();
            this.Text = "MyEditor";
        }

        // Method to handle "Open" menu item click
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display the Open File dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Determine the file type based on extension
                RichTextBoxStreamType richTextBoxStreamType = RichTextBoxStreamType.RichText;
                if (openFileDialog.FileName.ToLower().Contains(".txt"))
                {
                    richTextBoxStreamType = RichTextBoxStreamType.PlainText;
                }

                // Load the file into the rich text box and update the window title
                richTextBox.LoadFile(openFileDialog.FileName, richTextBoxStreamType);
                this.Text = "MyEditor (" + openFileDialog.FileName + ")";
            }
        }

        // Method to handle "Save" menu item click
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Use the filename from the Open File dialog if available
            saveFileDialog.FileName = openFileDialog.FileName;

            // Display the Save File dialog
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Determine the file type based on extension
                RichTextBoxStreamType richTextBoxStreamType = RichTextBoxStreamType.RichText;
                if (saveFileDialog.FileName.ToLower().Contains(".txt"))
                {
                    richTextBoxStreamType = RichTextBoxStreamType.PlainText;
                }

                // Save the contents of the rich text box to the file and update the window title
                richTextBox.SaveFile(saveFileDialog.FileName, richTextBoxStreamType);
                this.Text = "MyEditor (" + openFileDialog.FileName + ")";
            }
        }

        // Method to handle "Exit" menu item click
        private void ExitToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }

        // Method to handle "Copy" menu item click
        private void CopyToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            // Copy the selected text to the clipboard
            richTextBox.Copy();
        }

        // Method to handle "Cut" menu item click
        private void CutToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            // Cut the selected text to the clipboard
            richTextBox.Cut();
        }

        // Method to handle "Paste" menu item click
        private void PasteToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            // Paste the contents of the clipboard into the rich text box
            richTextBox.Paste();
        }

        // Method to handle toolbar item clicks
        private void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            FontStyle fontStyle = FontStyle.Regular;
            ToolStripButton toolStripButton = null;

            if (e.ClickedItem == this.boldToolStripButton)
            {
                fontStyle = FontStyle.Bold;
                toolStripButton = this.boldToolStripButton;
            }
            else if (e.ClickedItem == this.italicsToolStripButton)
            {
                fontStyle = FontStyle.Italic;
                toolStripButton = this.italicsToolStripButton;

            }
            else if (e.ClickedItem == this.underlineToolStripButton)
            {
                fontStyle = FontStyle.Underline;
                toolStripButton = this.underlineToolStripButton;
            }


            else if (e.ClickedItem == this.colorToolStripButton)
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox.SelectionColor = colorDialog.Color;
                    colorToolStripButton.BackColor = colorDialog.Color;
                }
            }


            if (fontStyle != FontStyle.Regular)
            {
                toolStripButton.Checked = !toolStripButton.Checked;
                SetSelectionFont(fontStyle, toolStripButton.Checked);
            }
        }


        // Method to set the font style for the selected text
        private void SetSelectionFont(FontStyle fontStyle, bool bSet)
        {
            Font newFont = null;
            Font selectionFont = null;
            selectionFont = richTextBox.SelectionFont;
            if (selectionFont == null)
            {
                selectionFont = richTextBox.Font;
            }
            if (bSet)
            {
                newFont = new Font(selectionFont, selectionFont.Style | fontStyle);
            }
            else
            {
                newFont = new Font(selectionFont, selectionFont.Style & ~fontStyle);
            }
            this.richTextBox.SelectionFont = newFont;
        }
        private void StartTest(object sender, EventArgs e)
        {
            timer.Start();
            toolStripContainer1.Enabled = true;
            testToolStripButton.Enabled = false;
            richTextBox.Focus();
        }

        private void countdownLabel_Click(object sender, EventArgs e)
        {

        }

    }
}
      
    

