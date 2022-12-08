using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewdriverGenerator.Model
{
    public class ScrewdriverParameter
    {
        private double _value;
        private double _minValue;
        private double _maxValue;
        private readonly string _emptyErrorMessage;
        private readonly string _minErrorMessage;
        private readonly string _maxErrorMessage;
        private readonly ScrewdriverParameterType _screwdriverParameterType;
        private readonly Dictionary<ScrewdriverParameterType, string> _errors;

        public double Value
        {
            get => _value;
            set
            {
                if (CheckRange(value))
                {
                    _value = value;
                }
            }
        }

        public double MinValue
        {
            get => _minValue;
            set 
            {
                _minValue = value;
            }
        }

        public double MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
            }
        }

        public ScrewdriverParameterType ScrewdriverParameterType
        {
            get => _screwdriverParameterType;
        }

        /// <summary>
        /// Один из параметров данных отвертки.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        /// <param name="minValue">Минимально возможное значение параметра.</param>
        /// <param name="maxValue">Максимально возможное значение параметра.</param>
        /// <param name="errorMessageAttachment">
        /// Название параметра, используемое в сообщениях ошибки.</param>
        /// <param name="screwdriverParameterType">Тип параметра отвертки.</param>
        /// <param name="errors">
        /// Библиотека с ошибками, возникшими при заполнении параметра.</param>
        public ScrewdriverParameter
            (
            double value, 
            double minValue, 
            double maxValue,
            string errorMessageAttachment,
            ScrewdriverParameterType screwdriverParameterType,
            Dictionary<ScrewdriverParameterType, string> errors
            )
        {
            _errors = errors;
            _minValue = minValue;
            _maxValue = maxValue;
            _emptyErrorMessage =
                "Error: " + errorMessageAttachment + " has not been entered.";
            _minErrorMessage = 
                "Error: " + errorMessageAttachment + " is less than the permissible value.";
            _maxErrorMessage = 
                "Error: " + errorMessageAttachment + " is greater than the allowable.";
            _screwdriverParameterType = screwdriverParameterType;
            Value = value;
        }

        /// <summary>
        /// Проверка корректности параметра на основе переменных минимума и максимума.
        /// </summary>
        /// <param name="value">Значение, подаваемое в параметр.</param>
        /// <returns>True - значение можно добавить в параметр, false - нельзя.</returns>
        private bool CheckRange(double value)
        {
            if (value == -1.0)
            {
                _errors.Add(_screwdriverParameterType, _emptyErrorMessage);
                return false;
            }
            if (value < _minValue)
            {
                _errors.Add(_screwdriverParameterType, _minErrorMessage);
                return false;
            }
            else if (value > _maxValue)
            {
                _errors.Add(_screwdriverParameterType, _maxErrorMessage);
                return false;
            }
            else return true;
        }
    }
}
