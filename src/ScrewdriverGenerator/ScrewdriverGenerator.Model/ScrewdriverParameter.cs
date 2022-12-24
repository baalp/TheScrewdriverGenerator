using System.Collections.Generic;

namespace ScrewdriverGenerator.Model
{
    /// <summary>
    /// Класс одного из параметров отвертки.
    /// </summary>
    public class ScrewdriverParameter
    {
        /// <summary>
        /// Значение параметра.
        /// </summary>
        private double _value;

        /// <summary>
        /// Минимально возможное значение параметра.
        /// </summary>
        private double _minValue;

        /// <summary>
        /// Максимально возможное значение параметра.
        /// </summary>
        private double _maxValue;

        /// <summary>
        /// Имя параметра, прибавляемое к сообщениям об этом значении.
        /// </summary>
        private readonly string _messageAttachment;

        /// <summary>
        /// Сообщение об ошибке пустого значения параметра.
        /// </summary>
        private readonly string _emptyErrorMessage;

        /// <summary>
        /// Сообщение об ошибке о слишком маленьком значении.
        /// </summary>
        private readonly string _minErrorMessage;

        /// <summary>
        /// Сообщение об ошибке о слишком большом значении.
        /// </summary>
        private readonly string _maxErrorMessage;

        /// <summary>
        /// Тип параметра.
        /// </summary>
        private readonly ScrewdriverParameterType _screwdriverParameterType;

        /// <summary>
        /// Библиотека ошибок параметра.
        /// </summary>
        private readonly Dictionary<ScrewdriverParameterType, string> _errors;

        /// <summary>
        /// Значение параметра.
        /// </summary>
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

        /// <summary>
        /// Минимально возможное значение параметра.
        /// </summary>
        public double MinValue
        {
            get => _minValue;
            set 
            {
                _minValue = value;
            }
        }

        /// <summary>
        /// Максимально возможное значение параметра.
        /// </summary>
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
            }
        }

        /// <summary>
        /// Тип параметра.
        /// </summary>
        public ScrewdriverParameterType ScrewdriverParameterType
        {
            get => _screwdriverParameterType;
        }

        /// <summary>
        /// Имя параметра, прибавляемое к сообщениям об этом значении.
        /// </summary>
        public string MessageAttachment
        {
            get => _messageAttachment;
        }

        /// <summary>
        /// Библиотека ошибок параметра.
        /// </summary>
        public Dictionary<ScrewdriverParameterType, string> Errors
        {
            get => _errors;
        }

        /// <summary>
        /// Один из параметров данных отвертки.
        /// </summary>
        /// <param name="value">Значение параметра.</param>
        /// <param name="minValue">Минимально возможное значение параметра.</param>
        /// <param name="maxValue">Максимально возможное значение параметра.</param>
        /// <param name="messageAttachment">
        /// Название параметра, используемое в различных сообщениях.</param>
        /// <param name="screwdriverParameterType">Тип параметра отвертки.</param>
        /// <param name="errors">
        /// Библиотека с ошибками, возникшими при заполнении параметра.</param>
        public ScrewdriverParameter
            (
            double value, 
            double minValue, 
            double maxValue,
            string messageAttachment,
            ScrewdriverParameterType screwdriverParameterType,
            Dictionary<ScrewdriverParameterType, string> errors
            )
        {
            _errors = errors;
            _minValue = minValue;
            _maxValue = maxValue;
            _messageAttachment = messageAttachment;
            _emptyErrorMessage =
                "Error: " + messageAttachment + " has not been entered.";
            _minErrorMessage = 
                "Error: " + messageAttachment + " is less than the permissible value.";
            _maxErrorMessage = 
                "Error: " + messageAttachment + " is greater than the allowable.";
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
            //Реакция на необязательное пустое значение
            if (value == -2.0)
            {
                return true;
            }
            //Реакция на обязательное пустое значение
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
