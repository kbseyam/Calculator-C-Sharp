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

        private void ConcatToLabelResult(char newChar) {
            if (LabelResult.Text.Length < 15) {
                LabelResult.Text += newChar;
            } else {
                MessageBox.Show("Max is 15 number");
            }
        }

        private void ConcatToLabelResult(string newText) {
            for (int i = 0; i < newText.Length; i++) {
                ConcatToLabelResult(newText[i]);
            }
        }

        private void btn0_Click(object sender, EventArgs e) {
            ConcatToLabelResult('0');
        }

        private void btn00_Click(object sender, EventArgs e) {
            ConcatToLabelResult("00");
        }

        private void btnDot_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(LabelResult.Text)) {
                LabelResult.Text = "0.";
            } else if (IsOperator(LabelResult.Text.Last())) {
                ConcatToLabelResult("0.");
            } else if (!DotIsExist()) {
                ConcatToLabelResult('.');
            }
        }

        private void btnEqual_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(LabelResult.Text)) {
                LabelPrevResult.Text = LabelResult.Text + " =";
                LabelResult.Text = Calculate();
            }
        }

        private void btn1_Click(object sender, EventArgs e) {
            ConcatToLabelResult('1');
        }

        private void btn2_Click(object sender, EventArgs e) {
            ConcatToLabelResult('2');
        }

        private void btn3_Click(object sender, EventArgs e) {
            ConcatToLabelResult('3');
        }

        private void btnPlus_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(LabelResult.Text))
                return;

            if (!ResultIsEndWithOperation())
                ConcatToLabelResult((char)Opertions.PLUS);
        }

        private void btn4_Click(object sender, EventArgs e) {
            ConcatToLabelResult('4');
        }

        private void btn5_Click(object sender, EventArgs e) {
            ConcatToLabelResult('5');
        }

        private void btn6_Click(object sender, EventArgs e) {
            ConcatToLabelResult('6');
        }

        private void btnMinus_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(LabelResult.Text))
                return;

            if (!ResultIsEndWithOperation())
                ConcatToLabelResult((char)Opertions.MINUS);
        }

        private void btn7_Click(object sender, EventArgs e) {
            ConcatToLabelResult('7');
        }

        private void btn8_Click(object sender, EventArgs e) {
            ConcatToLabelResult('8');
        }

        private void btn9_Click(object sender, EventArgs e) {
            ConcatToLabelResult('9');
        }

        private void btnMultiplication_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(LabelResult.Text))
                return;

            if (!ResultIsEndWithOperation())
                ConcatToLabelResult((char)Opertions.MULTIPLICATION);
        }

        private void btnClear_Click(object sender, EventArgs e) {
            LabelResult.Text = string.Empty;
            LabelPrevResult.Text = string.Empty;
        }

        private void btnBackspace_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(LabelResult.Text)) {
                LabelResult.Text = LabelResult.Text.Remove(LabelResult.Text.Length - 1);
            }
        }

        private void btnDivision_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(LabelResult.Text))
                return;

            if (!ResultIsEndWithOperation())
                ConcatToLabelResult((char)Opertions.DIVISION);
        }

        private bool ResultIsEndWithOperation() {
            return LabelResult.Text.Last() == (char)Opertions.PLUS || LabelResult.Text.Last() == (char)Opertions.MINUS ||
                LabelResult.Text.Last() == (char)Opertions.MULTIPLICATION || LabelResult.Text.Last() == (char)Opertions.DIVISION;
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
            string text = LabelResult.Text;
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
            string temp = LabelResult.Text;
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
