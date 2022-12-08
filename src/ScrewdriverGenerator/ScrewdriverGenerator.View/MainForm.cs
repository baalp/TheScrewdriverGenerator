using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScrewdriverGenerator.Model;
using ScrewdriverGenerator.Wrapper;

namespace ScrewdriverGenerator.View
{
    public partial class MainForm : Form
    {
        private Color _colorDefault = Color.FromArgb(255, 255, 255);
        private Color _colorError = Color.FromArgb(255, 192, 192);

        private readonly ScrewdriverData _screwdriverData;
        private readonly ScrewdriverBuilder _screwdriverBuilder;

        private readonly Dictionary<ScrewdriverParameterType, TextBox>
            _parameterToTextBox;

        private int _selectedTypeOfTip = 0;

        public MainForm()
        {
            InitializeComponent();

            _screwdriverData = new ScrewdriverData();
            _screwdriverBuilder = new ScrewdriverBuilder();

            //Dictonary зависимости типа данных от его TextBox для изменения его цвета.
            _parameterToTextBox = new Dictionary<ScrewdriverParameterType, TextBox>
            {
                { ScrewdriverParameterType.TipRodHeight, TextBoxTipRodHeight },
                { ScrewdriverParameterType.WidestPartHandle, TextBoxWidestPartOfHandle },
                { ScrewdriverParameterType.LengthOuterPartRod, TextBoxLengthOfOuterPartOfRod },
                { ScrewdriverParameterType.LengthHandle, TextBoxLengthOfHandle },
                { ScrewdriverParameterType.LengthInnerPartRod, TextBoxLengthOfInnerPartOfRod }
            };

            // Предотвращение ввода некорректных символов во все TextBox.
            TextBoxTipRodHeight.KeyPress            += PreventInputWrongSymbols;
            TextBoxWidestPartOfHandle.KeyPress      += PreventInputWrongSymbols;
            TextBoxLengthOfOuterPartOfRod.KeyPress  += PreventInputWrongSymbols;
            TextBoxLengthOfHandle.KeyPress          += PreventInputWrongSymbols;
            TextBoxLengthOfInnerPartOfRod.KeyPress  += PreventInputWrongSymbols;

            // Проверка на ввод некорректных значений.
            RadioButtonTypeOfTipFlat.CheckedChanged         += FindError;
            RadioButtonTypeOfTipCross.CheckedChanged        += FindError;
            RadioButtonTypeOfTipTriangular.CheckedChanged   += FindError;
            TextBoxTipRodHeight.TextChanged                 += FindError;
            TextBoxWidestPartOfHandle.TextChanged           += FindError;
            TextBoxLengthOfOuterPartOfRod.TextChanged       += FindError;
            TextBoxLengthOfHandle.TextChanged               += FindError;
            TextBoxLengthOfInnerPartOfRod.TextChanged       += FindError;
            Load += FindError;
        }

        /// <summary>
        /// Запрет ввода некорректных символов. Исключение - запятая на разделение дробной части.
        /// </summary>
        /// <param name="sender">TextBox - Инициатор события.</param>
        /// <param name="e">Нажатая на клавиатуре клавиша.</param>
        private static void PreventInputWrongSymbols(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Length >= 8 && !(e.KeyChar == (char)8))
            {
                e.Handled = true;
                return;
            }

            if 
            (
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
            double[] _inputValues = new double[6];
            _inputValues[0] = Convert.ToDouble(_selectedTypeOfTip);

            //Проверка TextBox на пустоту
            int i = 1;
            foreach (var keyValue in _parameterToTextBox)
            {
                if (_parameterToTextBox[keyValue.Key].Text != string.Empty)
                {
                    _inputValues[i] = double.Parse(_parameterToTextBox[keyValue.Key].Text);
                }
                else 
                {
                    _inputValues[i] = -1;
                }
                i++;
            }

            _screwdriverData.SetParameters
                (
                _inputValues[0], _inputValues[1], _inputValues[2], 
                _inputValues[3], _inputValues[4], _inputValues[5]
                );

            UpdateGUIBecauseFindError(_screwdriverData.Errors);
        }

        private void UpdateGUIBecauseFindError(Dictionary<ScrewdriverParameterType, string> _errors)
        {
            foreach (var keyValue in _parameterToTextBox)
            {
                keyValue.Value.Enabled = true;
                keyValue.Value.BackColor = _colorDefault;
            }

            ButtonBuild.Enabled = true;
            ButtonBuild.Text = "Build";
            StatusStripError.BackColor = _colorDefault;
            ToolStripStatusLabelError.Text = "No errors found.";

            GroupBoxTipRodHeight.Text = _screwdriverData.GetGroupBoxDescription(_screwdriverData.Parameters[ScrewdriverParameterType.TipRodHeight]);
            GroupBoxWidestPartOfHandle.Text = _screwdriverData.GetGroupBoxDescription(_screwdriverData.Parameters[ScrewdriverParameterType.WidestPartHandle]);
            GroupBoxLengthOfOuterPartOfRod.Text = _screwdriverData.GetGroupBoxDescription(_screwdriverData.Parameters[ScrewdriverParameterType.LengthOuterPartRod]);
            GroupBoxLengthOfHandle.Text = _screwdriverData.GetGroupBoxDescription(_screwdriverData.Parameters[ScrewdriverParameterType.LengthHandle]);
            GroupBoxLengthOfInnerPartOfRod.Text = _screwdriverData.GetGroupBoxDescription(_screwdriverData.Parameters[ScrewdriverParameterType.LengthInnerPartRod]);

            if (_errors.Any())
            {
                var _errorKey = _errors.ElementAt(0).Key;
                var _errorValue = _errors.ElementAt(0).Value;

                StatusStripError.BackColor = _colorError;
                ToolStripStatusLabelError.Text = _errorValue;
                ButtonBuild.Enabled = false;
                ButtonBuild.Text = "Correct all errors to build a model";

                if (_errorKey == ScrewdriverParameterType.TipRodHeight)
                {
                    TextBoxTipRodHeight.BackColor = _colorError;
                    TextBoxWidestPartOfHandle.Enabled = false;
                    TextBoxLengthOfOuterPartOfRod.Enabled = false;
                    TextBoxLengthOfHandle.Enabled = false;
                    TextBoxLengthOfInnerPartOfRod.Enabled = false;
                }
                else if (_errorKey == ScrewdriverParameterType.WidestPartHandle)
                {
                    TextBoxWidestPartOfHandle.BackColor = _colorError;
                    TextBoxLengthOfHandle.Enabled = false;
                    TextBoxLengthOfInnerPartOfRod.Enabled = false;
                }
                else if (_errorKey == ScrewdriverParameterType.LengthOuterPartRod)
                {
                    TextBoxLengthOfOuterPartOfRod.BackColor = _colorError;
                    TextBoxLengthOfInnerPartOfRod.Enabled = false;
                }
                else if (_errorKey == ScrewdriverParameterType.LengthHandle)
                {
                    TextBoxLengthOfHandle.BackColor = _colorError;
                    TextBoxLengthOfInnerPartOfRod.Enabled = false;
                }
                else if (_errorKey == ScrewdriverParameterType.LengthInnerPartRod)
                {
                    TextBoxLengthOfInnerPartOfRod.BackColor = _colorError;
                }
            }
            return;
        }

        private void RadioButtonTypeOfTipFlat_CheckedChanged(object sender, EventArgs e)
        {
            _selectedTypeOfTip = RadioButtonTypeOfTipFlat.TabIndex;
        }

        private void RadioButtonTypeOfTipCross_CheckedChanged(object sender, EventArgs e)
        {
            _selectedTypeOfTip = RadioButtonTypeOfTipCross.TabIndex;
        }

        private void RadioButtonTypeOfTipTriangular_CheckedChanged(object sender, EventArgs e)
        {
            _selectedTypeOfTip = RadioButtonTypeOfTipTriangular.TabIndex;
        }

        private void ButtonBuild_Click(object sender, EventArgs e)
        {
            _screwdriverBuilder.BuildScrewdriver(_screwdriverData);
        }
    }
}
