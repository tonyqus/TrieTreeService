using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BluePrint.Dictionary;
using System.Text.RegularExpressions;
using System.Net;
using BluePrint.SegmentFramework;

namespace DictionaryQuery
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<POSType> ConvertValueToPOS(int posValue)
        {
            List<POSType> result = new List<POSType>();
            var values = Enum.GetValues(typeof(POSType));
            foreach (POSType value in values)
            {
                if ((posValue & (int)value) == (int)value)
                {
                    result.Add(value);
                }
            }
            return result;
        }
        string ConvertPOSTypesToString(List<POSType> posList)
        {
            StringBuilder sb = new StringBuilder();
            foreach(POSType pos in posList)
            {
                if(pos!=POSType.UNKNOWN&&pos!=POSType.NEWLINE)
                    sb.AppendFormat("{0}({1})|",pos.Description(),pos.ToString());
            }

            return sb.ToString(0,sb.Length-1);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex("dict://([\\d\\.]+):(\\d{3,5})", RegexOptions.Compiled);
            string addr = tbDictAddress.Text.Trim();
            var matches = regex.Matches(addr);
                        string serverAddr=matches[0].Groups[1].Value;
            int port = Int32.Parse(matches[0].Groups[2].Value);
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(serverAddr), port);

            
            int posValue = 0;
            
            foreach (var selectedObject in checkedListBox1.CheckedItems)
            {
                string value = (string)selectedObject;
                var values = Enum.GetValues(typeof(POSType));
                foreach (var v in values)
                {
                    if (((System.Enum)v).Description() == value)
                    {
                        POSType pos = (POSType)v;
                        posValue |= ((int)pos);
                        break;
                    }
                }
            }
            using (DictionaryServiceClient dsc = new DictionaryServiceClient())
            {
                dsc.Connect(serverAddress);
                TrieTreeResult result = null;
                if(radioButton1.Checked)
                    result = dsc.MaximumMatch(tbWord.Text.Trim(), posValue);
                else if(radioButton2.Checked)
                    result = dsc.ReverseMaximumMatch(tbWord.Text.Trim(), posValue);
                else
                    result = dsc.ExactMatch(tbWord.Text.Trim(), posValue);

                if (result != null)
                {
                    string resultText = result.Word;
                    int originalLength1=0, originalLength2=0;
                    
                    if (result.Frequency != 0)
                    {
                        originalLength1 = resultText.Length;
                        resultText += ", 频率：" + result.Frequency;
                     
                    }
                    if (result.POS > 0)
                    {
                        originalLength2 = resultText.Length;
                        resultText += ", 类型：" + ConvertPOSTypesToString(ConvertValueToPOS(result.POS));

                    }
                    richTextBox1.Text = resultText;
                    richTextBox1.ForeColor = System.Drawing.Color.Black;
                    if (originalLength1 > 0)
                    {
                        richTextBox1.Select(originalLength1 + 1, 4);
                        richTextBox1.SelectionColor = System.Drawing.Color.Blue;
                    }
                    if (originalLength2 > 0)
                    {
                        richTextBox1.Select(originalLength2 + 1, 4);
                        richTextBox1.SelectionColor = System.Drawing.Color.Blue;
                    }
                }
                else
                {
                    richTextBox1.ForeColor = System.Drawing.Color.Red;
                    richTextBox1.Text = "未找到合适词";
                }
            }
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            var values =Enum.GetValues(typeof(POSType));
            foreach(var value in values)
            {
                if((POSType)value!=POSType.NEWLINE)
                    checkedListBox1.Items.Add(((System.Enum)value).Description());
            }
        }
    }
}
