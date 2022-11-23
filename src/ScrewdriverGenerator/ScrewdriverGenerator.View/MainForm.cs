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

            // Предотвращение ввода некорректных символов во все TextBox.
            TextBoxTipRodHeight.KeyPress            += PreventInputWrongSymbols;
            TextBoxWidestPartOfHandle.KeyPress      += PreventInputWrongSymbols;
            TextBoxLengthOfOuterPartOfRod.KeyPress  += PreventInputWrongSymbols;
            TextBoxLengthOfHandle.KeyPress          += PreventInputWrongSymbols;
            TextBoxLengthOfInnerPartOfRod.KeyPress  += PreventInputWrongSymbols;

            // Проверка на ввод некорректных значений.
            TextBoxTipRodHeight.TextChanged             += FindError;
            TextBoxWidestPartOfHandle.TextChanged       += FindError;
            TextBoxLengthOfOuterPartOfRod.TextChanged   += FindError;
            TextBoxLengthOfHandle.TextChanged           += FindError;
            TextBoxLengthOfInnerPartOfRod.TextChanged   += FindError;
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

        private void FindError(object sender, EventArgs e)
        {

        }
    }
}
