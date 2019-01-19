using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace CalculatorWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string equalMultiPress = string.Empty;
        bool isOperand, equalPressed = false;
        string lastOp = string.Empty;
        char[] ops = { '+', '-', '*', '/' };
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if((e.KeyCode>=Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) || (e.KeyCode>=Keys.D0&&e.KeyCode<=Keys.D9))
            {
                string tmp = "btn";
                tmp += e.KeyCode.ToString().Substring(e.KeyCode.ToString().Length - 1);
                (MainGrp.Controls[tmp] as Button).PerformClick();
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Multiply: (MainGrp.Controls["multBtn"] as Button).PerformClick();break;
                    case Keys.Divide: (MainGrp.Controls["divBtn"] as Button).PerformClick(); break;
                    case Keys.Subtract: (MainGrp.Controls["minusBtn"] as Button).PerformClick(); break;
                    case Keys.Add: (MainGrp.Controls["plusBtn"] as Button).PerformClick(); break;
                    case Keys.Oemplus: (MainGrp.Controls["equalBtn"] as Button).PerformClick(); break;
                    case Keys.Back: (MainGrp.Controls["delBtn"] as Button).PerformClick(); break;
                    case Keys.C: (MainGrp.Controls["cBtn"] as Button).PerformClick(); break;
                    case Keys.OemPeriod:
                    case Keys.N: (MainGrp.Controls["negativeBtn"] as Button).PerformClick(); break;
                    case Keys.Decimal: (MainGrp.Controls["pointBtn"] as Button).PerformClick(); break;
                }
            }
        }
        private void equalBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (sender as Button).Text.ToLower();
                switch (btn)
                {
                    case "c":
                        MainDisplayLbl.Text = string.Empty;
                        allOpsLbl.Text = string.Empty;
                        break;
                    case "<<":
                        if (MainDisplayLbl.Text != string.Empty)
                            MainDisplayLbl.Text = MainDisplayLbl.Text.Remove(MainDisplayLbl.Text.Length - 1);
                        break;
                    case ".":
                        if (!MainDisplayLbl.Text.Contains('.') && MainDisplayLbl.Text != string.Empty)
                            MainDisplayLbl.Text += btn;
                        break;
                    case "±":
                        if (!MainDisplayLbl.Text.Contains('-')) MainDisplayLbl.Text = MainDisplayLbl.Text.Insert(0, "-");
                        else MainDisplayLbl.Text = MainDisplayLbl.Text.Remove(0, 1);
                        break;
                    case "-":
                    case "+":
                    case "*":
                    case "/":
                        if (!isOperand && MainDisplayLbl.Text != string.Empty)
                        {
                            if (MainDisplayLbl.Text != string.Empty)
                                if (MainDisplayLbl.Text.Last() == '.') MainDisplayLbl.Text = MainDisplayLbl.Text.Remove(MainDisplayLbl.Text.Length - 1);
                            if (equalPressed) allOpsLbl.Text = MainDisplayLbl.Text;
                            else allOpsLbl.Text += MainDisplayLbl.Text;
                            allOpsLbl.Text += btn;
                            lastOp = btn;
                            MainDisplayLbl.Text = string.Empty;
                            isOperand = true;
                        }
                        else if (isOperand && !equalPressed)
                        {
                            allOpsLbl.Text = allOpsLbl.Text.Remove(allOpsLbl.Text.Length - 1);
                            allOpsLbl.Text += btn;
                        }

                        break;
                    case "=":
                        if (equalPressed && double.TryParse(allOpsLbl.Text.Last().ToString(), out double result))
                        {
                            try
                            {
                                allOpsLbl.Text += allOpsLbl.Text.Substring(allOpsLbl.Text.LastIndexOfAny(ops));
                                MainDisplayLbl.Text = new DataTable().Compute(allOpsLbl.Text, null).ToString();
                            }
                            catch (Exception) { }
                            return;
                        }

                        if (allOpsLbl.Text != string.Empty)
                        {
                            equalPressed = true;
                            allOpsLbl.Text += MainDisplayLbl.Text;
                            if (MainDisplayLbl.Text == string.Empty) allOpsLbl.Text = allOpsLbl.Text.Remove(allOpsLbl.Text.Length - 1);
                            MainDisplayLbl.Text = new DataTable().Compute(allOpsLbl.Text, null).ToString();
                            equalMultiPress = allOpsLbl.Text;
                        }
                        break;
                    default:
                        isOperand = false;
                        equalPressed = false;
                        MainDisplayLbl.Text += btn;
                        break;
                }
            }
            catch (Exception) { }
            equalBtn.Focus();
        }
    }
}
