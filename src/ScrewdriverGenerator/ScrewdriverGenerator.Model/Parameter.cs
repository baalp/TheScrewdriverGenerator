using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewdriverGenerator.Model
{
    public class Parameter
    {
        private double _value;
        private readonly double _minValue;
        private readonly double _maxValue;
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

        public Parameter
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
            _minErrorMessage = 
                "Error: " + errorMessageAttachment + " can't be less than " + _minValue + " mm.";
            _maxErrorMessage = 
                "Error: " + errorMessageAttachment + " can't be more than " + _maxValue + " mm.";
            _screwdriverParameterType = screwdriverParameterType;
            Value = value;
        }

        private bool CheckRange(double value)
        {
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
