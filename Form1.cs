using System;
using System.Linq;
using System.Windows.Forms;

namespace Calculator {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        enum Opertions : int {
            PLUS = '+',
            MINUS = '-',
            MULTIPLICATION = '×',
            DIVISION = '÷'
        }


        private void btn0_Click(object sender, EventArgs e) {
            labelResult.Text += '0';
        }

        private void btn00_Click(object sender, EventArgs e) {
            labelResult.Text += "00";
        }

        private void btnDot_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(labelResult.Text)) {
                labelResult.Text = "0.";
            } else if (IsOperator(labelResult.Text.Last())) {
                labelResult.Text += "0.";
            } else if (!DotIsExist()) {
                labelResult.Text += '.';
            }
        }

        private void btnEqual_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(labelResult.Text)) {
                labelResult.Text = Calculate();
            }
        }

        private void btn1_Click(object sender, EventArgs e) {
            labelResult.Text += '1';
        }

        private void btn2_Click(object sender, EventArgs e) {
            labelResult.Text += '2';
        }

        private void btn3_Click(object sender, EventArgs e) {
            labelResult.Text += '3';
        }

        private void btnPlus_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(labelResult.Text))
                return;

            if (!ResultIsEndWithOperation())
                labelResult.Text += (char)Opertions.PLUS;
        }

        private void btn4_Click(object sender, EventArgs e) {
            labelResult.Text += '4';
        }

        private void btn5_Click(object sender, EventArgs e) {
            labelResult.Text += '5';
        }

        private void btn6_Click(object sender, EventArgs e) {
            labelResult.Text += '6';
        }

        private void btnMinus_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(labelResult.Text))
                return;

            if (!ResultIsEndWithOperation())
                labelResult.Text += (char)Opertions.MINUS;
        }

        private void btn7_Click(object sender, EventArgs e) {
            labelResult.Text += '7';
        }

        private void btn8_Click(object sender, EventArgs e) {
            labelResult.Text += '8';
        }

        private void btn9_Click(object sender, EventArgs e) {
            labelResult.Text += '9';
        }

        private void btnMultiplication_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(labelResult.Text))
                return;

            if (!ResultIsEndWithOperation())
                labelResult.Text += (char)Opertions.MULTIPLICATION;
        }

        private void btnClear_Click(object sender, EventArgs e) {
            labelResult.Text = string.Empty;
        }

        private void btnBackspace_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(labelResult.Text)) {
                labelResult.Text = labelResult.Text.Remove(labelResult.Text.Length - 1);
            }
        }

        private void btnDivision_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(labelResult.Text))
                return;

            if (!ResultIsEndWithOperation())
                labelResult.Text += (char)Opertions.DIVISION;
        }

        private bool ResultIsEndWithOperation() {
            return labelResult.Text.Last() == (char)Opertions.PLUS || labelResult.Text.Last() == (char)Opertions.MINUS ||
                labelResult.Text.Last() == (char)Opertions.MULTIPLICATION || labelResult.Text.Last() == (char)Opertions.DIVISION;
        }

        private int IndexPrevOper(string text, int currentOperIndex) {
            for (int i = currentOperIndex - 1; i >= 0; --i) {
                if (text[i] == (char)Opertions.MULTIPLICATION || text[i] == (char)Opertions.DIVISION ||
                    text[i] == (char)Opertions.PLUS || text[i] == (char)Opertions.MINUS) {

                    return i;
                }
            }

            return -1;
        }

        private int IndexNextOper(string text, int currentOperIndex) {
            for (int i = currentOperIndex + 1; i < text.Length; ++i) {
                if (text[i] == (char)Opertions.MULTIPLICATION || text[i] == (char)Opertions.DIVISION ||
                    text[i] == (char)Opertions.PLUS || text[i] == (char)Opertions.MINUS) {

                    return i;
                }
            }

            return text.Length;
        }

        private string PerformOpertion(string text, int operIndex, Opertions opertionType) {
            int indexPrevOper = IndexPrevOper(text, operIndex);
            int indexNextOper = IndexNextOper(text, operIndex);
            double numBeforeOper = Convert.ToDouble(text.Substring(indexPrevOper + 1, operIndex - indexPrevOper - 1));
            double numAfterOper = Convert.ToDouble(text.Substring(operIndex + 1, indexNextOper - (operIndex + 1)));
            text = text.Remove(indexPrevOper + 1, indexNextOper - (indexPrevOper + 1));

            switch (opertionType) {
                case Opertions.MULTIPLICATION:
                    text = text.Insert(indexPrevOper + 1, Convert.ToString(numBeforeOper * numAfterOper));
                    break;
                case Opertions.DIVISION:
                    text = text.Insert(indexPrevOper + 1, Convert.ToString(numBeforeOper / numAfterOper));
                    break;
                case Opertions.PLUS:
                    text = text.Insert(indexPrevOper + 1, Convert.ToString(numBeforeOper + numAfterOper));
                    break;
                case Opertions.MINUS:
                    text = text.Insert(indexPrevOper + 1, Convert.ToString(numBeforeOper - numAfterOper));
                    break;
            }

            return text;
        }

        private string Calculate() {
            string text = labelResult.Text;
            while (!double.TryParse(text, out _)) {
                for (int i = 0; i < text.Length; ++i) {
                    if (text[i] == (char)Opertions.MULTIPLICATION) {
                        text = PerformOpertion(text, i, Opertions.MULTIPLICATION);
                    } else if (text[i] == (char)Opertions.DIVISION) {
                        text = PerformOpertion(text, i, Opertions.DIVISION);
                    }
                }

                for (int i = 0; i < text.Length; ++i) {
                    if (text[i] == (char)Opertions.PLUS) {
                        text = PerformOpertion(text, i, Opertions.PLUS);
                    } else if (text[i] == (char)Opertions.MINUS) {
                        text = PerformOpertion(text, i, Opertions.MINUS);
                    }
                }
            }

            return text;
        }

        private bool IsOperator(char c) {
            return c == (char)Opertions.MULTIPLICATION || c == (char)Opertions.DIVISION ||
                c == (char)Opertions.PLUS || c == (char)Opertions.MINUS;
        }

        private bool DotIsExist() {
            string temp = labelResult.Text;
            int indexPrevOper = IndexPrevOper(temp, temp.Length - 1);
            if (indexPrevOper != -1) {
                temp = temp.Substring(indexPrevOper);
            } else {
                temp = temp.Substring(0);
            }


            return temp.Contains('.');
        }

    }
}
