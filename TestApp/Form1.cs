using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace TestApp
{
    public partial class Form1 : Form
    {
        private string[] testData = new string[] {
            "String1",
            "String2",
            "String3",
            "String4",
            "String5",
            "String6",
            "String7",
            "String8",
            "String9",
        };

        public Form1()
        {
            InitializeComponent();
            InitTestData();
        }

        private void InitTestData()
        {
            for (int i = 0; i < testData.Length; i++)
            {
                ComboBoxCheckItem item = new ComboBoxCheckItem(testData[i], i);
                simpleCheckListComboBox1.Items.Add(item);
            }
            
            simpleCheckListComboBox1.DisplayMember = "Name";
            simpleCheckListComboBox1.ValuesSeparator = "; ";
            simpleCheckListComboBox1.MaxDropDownItems = 7;
            simpleCheckListComboBox1.TextChanged += SimpleCheckListComboBox1_TextChanged;
            
        }

        private void SimpleCheckListComboBox1_TextChanged(object sender, EventArgs e)
        {
            Form1_Click(sender, e);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            foreach (var item in simpleCheckListComboBox1.CheckedItems)
            {
                label1.Text += item + " - ";
            }            
        }
    }
}
