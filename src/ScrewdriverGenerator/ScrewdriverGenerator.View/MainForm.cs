using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrewdriverGenerator.View
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Проверка на ввод некорректных символов во все TextBox.
            TextBoxTipRodHeight.KeyPress            += PreventInputWrongSymbols;
            TextBoxWidestPartOfHandle.KeyPress      += PreventInputWrongSymbols;
            TextBoxLengthOfOuterPartOfRod.KeyPress  += PreventInputWrongSymbols;
            TextBoxLengthOfHandle.KeyPress          += PreventInputWrongSymbols;
            TextBoxLengthOfInnerPartOfRod.KeyPress  += PreventInputWrongSymbols;
        }

        /// <summary>
        /// Запрет ввода некорректных символов. Исключение - запятая на разделение дробной части.
        /// </summary>
        /// <param name="sender">TextBox - Инициатор события.</param>
        /// <param name="e">Нажатая на клавиатуре клавиша.</param>
        private static void PreventInputWrongSymbols(object sender, KeyPressEventArgs e)
        {
            if(
                !char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                !(
                    e.KeyChar == ',' &&
                    ((TextBox)sender).Text.IndexOf(",", StringComparison.Ordinal) == -1
                )
            )
            {
                e.Handled = true;
            }
        }
    }
}
