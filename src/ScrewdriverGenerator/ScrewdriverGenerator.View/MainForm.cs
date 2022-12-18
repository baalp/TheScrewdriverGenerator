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
    /// <summary>
    /// Класс взаимодействия пользователя с программой через окно MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
		/// Цвет, который принимают элемента интерфейса при отсутствии ошибок.
		/// </summary>
        private Color _colorDefault = Color.FromArgb(255, 255, 255);

        /// <summary>
		/// Цвет, который принимают элемента интерфейса при нахождении ошибок.
		/// </summary>
        private Color _colorError = Color.FromArgb(255, 192, 192);

        /// <summary>
		/// Объект данных об отвертке.
		/// </summary>
        private readonly ScrewdriverData _screwdriverData;

        /// <summary>
		/// Объект, строящий отвертку в Компас-3D.
		/// </summary>
        private readonly ScrewdriverBuilder _screwdriverBuilder;

        /// <summary>
		/// Библиотека, хранящая связь между TextBox и типами данных отвертки.
		/// </summary>
        private readonly Dictionary<ScrewdriverParameterType, TextBox>
            _parameterToTextBox;

        /// <summary>
		/// Переменная, хранящая выбранный тип наконечника отвертки.
		/// </summary>
        private int _selectedTypeOfTip = 0;

        /// <summary>
		/// Стандартный путь для сохранения файлов.
		/// </summary>
        private string defaultChosenPath = "C:\\Temp";

        public MainForm()
        {
            InitializeComponent();

            _screwdriverData = new ScrewdriverData();
            _screwdriverBuilder = new ScrewdriverBuilder();
            LabelChoosenPath.Text = defaultChosenPath;

            //Dictonary зависимости типа данных от его TextBox для изменения его цвета.
            _parameterToTextBox = new Dictionary<ScrewdriverParameterType, TextBox>
            {
                { ScrewdriverParameterType.TipRodHeight, TextBoxTipRodHeight },
                { ScrewdriverParameterType.WidestPartHandle, TextBoxWidestPartOfHandle },
                { ScrewdriverParameterType.LengthOuterPartRod, TextBoxLengthOfOuterPartOfRod },
                { ScrewdriverParameterType.LengthHandle, TextBoxLengthOfHandle },
                { ScrewdriverParameterType.LengthInnerPartRod, TextBoxLengthOfInnerPartOfRod },
                { ScrewdriverParameterType.LengthFixingWings, TextBoxLengthFixingWings }
            };

            // Предотвращение ввода некорректных символов во все TextBox.
            TextBoxTipRodHeight.KeyPress            += PreventInputWrongSymbols;
            TextBoxWidestPartOfHandle.KeyPress      += PreventInputWrongSymbols;
            TextBoxLengthOfOuterPartOfRod.KeyPress  += PreventInputWrongSymbols;
            TextBoxLengthOfHandle.KeyPress          += PreventInputWrongSymbols;
            TextBoxLengthOfInnerPartOfRod.KeyPress  += PreventInputWrongSymbols;
            TextBoxLengthFixingWings.KeyPress       += PreventInputWrongSymbols;

            // Проверка на ввод некорректных значений.
            RadioButtonTypeOfTipFlat.CheckedChanged         += FindError;
            RadioButtonTypeOfTipCross.CheckedChanged        += FindError;
            RadioButtonTypeOfTipTriangular.CheckedChanged   += FindError;
            TextBoxTipRodHeight.TextChanged                 += FindError;
            TextBoxWidestPartOfHandle.TextChanged           += FindError;
            TextBoxLengthOfOuterPartOfRod.TextChanged       += FindError;
            TextBoxLengthOfHandle.TextChanged               += FindError;
            TextBoxLengthOfInnerPartOfRod.TextChanged       += FindError;
            TextBoxLengthFixingWings.TextChanged            += FindError;
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

        /// <summary>
        /// Поиск некорректных данных в TextBox и их запись в ScrewdriverData.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Нажатая на клавиатуре клавиша.</param>
        private void FindError(object sender, EventArgs e)
        {
            double[] _inputValues = new double[7];
            _inputValues[0] = Convert.ToDouble(_selectedTypeOfTip);

            //Проверка TextBox на пустоту.
            int i = 1;
            foreach (var keyValue in _parameterToTextBox)
            {
                if (_parameterToTextBox[keyValue.Key].Text != string.Empty)
                {
                    _inputValues[i] = double.Parse(_parameterToTextBox[keyValue.Key].Text);
                }
                else 
                {
                    if (i == 6)
                    {
                        //Если вводимый параметр необязателен.
                        _inputValues[i] = -2;
                    }
                    else
                    {
                        //Если вводимый параметр обязателен.
                        _inputValues[i] = -1;
                    }
                }
                i++;
            }

            _screwdriverData.SetParameters
                (
                _inputValues[0], _inputValues[1], _inputValues[2], 
                _inputValues[3], _inputValues[4], _inputValues[5],
                _inputValues[6]
                );

            UpdateGUIBecauseFindError(_screwdriverData.Errors);
        }

        /// <summary>
        /// обновление внешнего вида GUI при изменении статуса ошибки.
        /// </summary>
        /// <param name="_errors">Библиотека, хранящая ошибку и тип данных отвертки с ней.</param>
        private void UpdateGUIBecauseFindError(Dictionary<ScrewdriverParameterType, string> _errors)
        {
            foreach (var keyValue in _parameterToTextBox)
            {
                keyValue.Value.Enabled = true;
                keyValue.Value.BackColor = _colorDefault;
            }

            //Обнуление статуса ошибки
            ButtonBuild.Enabled = true;
            ButtonBuild.Text = "Build";
            StatusStripError.BackColor = _colorDefault;
            ToolStripStatusLabelError.Text = "No errors found.";

            GroupBoxTipRodHeight.Text = _screwdriverData.GetGroupBoxDescription
                (_screwdriverData.Parameters[ScrewdriverParameterType.TipRodHeight]);
            GroupBoxWidestPartOfHandle.Text = _screwdriverData.GetGroupBoxDescription
                (_screwdriverData.Parameters[ScrewdriverParameterType.WidestPartHandle]);
            GroupBoxLengthOfOuterPartOfRod.Text = _screwdriverData.GetGroupBoxDescription
                (_screwdriverData.Parameters[ScrewdriverParameterType.LengthOuterPartRod]);
            GroupBoxLengthOfHandle.Text = _screwdriverData.GetGroupBoxDescription
                (_screwdriverData.Parameters[ScrewdriverParameterType.LengthHandle]);
            GroupBoxLengthOfInnerPartOfRod.Text = _screwdriverData.GetGroupBoxDescription
                (_screwdriverData.Parameters[ScrewdriverParameterType.LengthInnerPartRod]);
            GroupBoxLengthFixingWings.Text = _screwdriverData.GetGroupBoxDescription
                (_screwdriverData.Parameters[ScrewdriverParameterType.LengthFixingWings]);

            //Добавление статуса ошибки при ее наличии
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
                    TextBoxLengthFixingWings.Enabled = false;
                }
                else if (_errorKey == ScrewdriverParameterType.LengthInnerPartRod)
                {
                    TextBoxLengthOfInnerPartOfRod.BackColor = _colorError;
                }
                else if (_errorKey == ScrewdriverParameterType.LengthFixingWings)
                {
                    TextBoxLengthFixingWings.BackColor = _colorError;
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
            _screwdriverBuilder.BuildScrewdriver(_screwdriverData, LabelChoosenPath.Text);
        }

        private void ButtonChooseOutputPath_Click(object sender, EventArgs e)
        {
            if (FolderBrowserDialogOutputPath.ShowDialog() == DialogResult.OK)
            {
                LabelChoosenPath.Text = FolderBrowserDialogOutputPath.SelectedPath;
            }
        }
    }
}
