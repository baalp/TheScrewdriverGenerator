using System.Collections.Generic;
using System.Linq;

namespace ScrewdriverGenerator.Model
{
    /// <summary>
    /// Класс всех параметров отвертки.
    /// </summary>
    public class ScrewdriverData
    {
        /// <summary>
        /// Библиотека параметров отвертки и их типов.
        /// </summary>
        public Dictionary<ScrewdriverParameterType, ScrewdriverParameter> Parameters { get; set; }

        /// <summary>
        /// Библиотека ошибок данных отвертки и их типа.
        /// </summary>
        public Dictionary<ScrewdriverParameterType, string> Errors { get; set; }

        /// <summary>
        /// Минимальный возможный размер TipRodHeight.
        /// </summary>
        private const double _minTipRodHeight = 0.1;

        /// <summary>
        /// Максимальный возможный размер TipRodHeight.
        /// </summary>
        private const double _maxTipRodHeight = 10;

        /// <summary>
        /// Минимальное возможное соотношение значений TipRodHeight и WidestPartHandle.
        /// </summary>
        private const double _minWidestPartHandleMultiple = 16;

        /// <summary>
        /// Максимальное возможное соотношение значений TipRodHeight и WidestPartHandle.
        /// </summary>
        private const double _maxWidestPartHandleMultiple = 24;

        /// <summary>
        /// Минимальное возможное соотношение значений TipRodHeight и LengthOuterPartRod.
        /// </summary>
        private const double _minLengthOuterPartRodMultiple = 20;

        /// <summary>
        /// Максимальное возможное соотношение значений TipRodHeight и LengthOuterPartRod.
        /// </summary>
        private const double _maxLengthOuterPartRodMultiple = 400;

        /// <summary>
        /// Минимальное возможное соотношение значений WidestPartHandle и LengthHandle.
        /// </summary>
        private const double _minLengthHandleMultiple = 3.75;

        /// <summary>
        /// Максимальное возможное соотношение значений WidestPartHandle и LengthHandle.
        /// </summary>
        private const double _maxLengthHandleMultiple = 7.5;

        /// <summary>
        /// Минимальное возможное соотношение значений LengthHandle и LengthInnerPartRod.
        /// </summary>
        private const double _minLengthInnerPartRodMultiple = 0.5;

        /// <summary>
        /// Максимальное возможное соотношение значений LengthHandle и LengthInnerPartRod.
        /// </summary>
        private const double _maxLengthInnerPartRodMultiple = 0.6;

        /// <summary>
        /// Минимальное возможное соотношение значений LengthHandle и LengthFixingWings.
        /// </summary>
        private const double _minLengthFixingWingsMultiple = 0.1;

        /// <summary>
        /// Максимальное возможное соотношение значений LengthHandle и LengthFixingWings.
        /// </summary>
        private const double _maxLengthFixingWingsMultiple = 0.5;

        public ScrewdriverData()
        {
            Errors = new Dictionary<ScrewdriverParameterType, string>();

            Parameters = new Dictionary<ScrewdriverParameterType, ScrewdriverParameter>()
            {
                { ScrewdriverParameterType.TipType,
                    new ScrewdriverParameter
                    (
                        0, 
                        0, 
                        2,
                        "Tip type",
                        ScrewdriverParameterType.TipType, 
                        Errors
                    )
                },

                { ScrewdriverParameterType.TipRodHeight,
                    new ScrewdriverParameter
                    (
                        -1, 
                        _minTipRodHeight, 
                        _maxTipRodHeight,
                        "Tip rod height (H)",
                        ScrewdriverParameterType.TipRodHeight, 
                        Errors
                    )
                },

                { ScrewdriverParameterType.WidestPartHandle,
                    new ScrewdriverParameter
                    (
                        -1,
                        _minTipRodHeight * _minWidestPartHandleMultiple,
                        _maxTipRodHeight * _maxWidestPartHandleMultiple,
                        "Widest part of handle (D)",
                        ScrewdriverParameterType.WidestPartHandle, 
                        Errors
                    )
                },

                { ScrewdriverParameterType.LengthOuterPartRod,
                    new ScrewdriverParameter
                    (
                        -1,
                        _minTipRodHeight * _minLengthOuterPartRodMultiple,
                        _maxTipRodHeight * _maxLengthOuterPartRodMultiple,
                        "Length of outer part of rod (Lo)",
                        ScrewdriverParameterType.LengthOuterPartRod, 
                        Errors
                    )
                },

                { ScrewdriverParameterType.LengthHandle,
                    new ScrewdriverParameter
                    (
                        -1,
                        _minTipRodHeight * _minWidestPartHandleMultiple * 
                        _minLengthHandleMultiple,
                        _maxTipRodHeight * _maxWidestPartHandleMultiple * 
                        _maxLengthHandleMultiple,
                        "Length of handle (Lh)",
                        ScrewdriverParameterType.LengthHandle, 
                        Errors
                    )
                },

                { ScrewdriverParameterType.LengthInnerPartRod,
                    new ScrewdriverParameter
                    (
                        -1,
                        _minTipRodHeight * _minWidestPartHandleMultiple * 
                        _minLengthHandleMultiple * _minLengthInnerPartRodMultiple,
                        _maxTipRodHeight * _maxWidestPartHandleMultiple * 
                        _maxLengthHandleMultiple * _maxLengthInnerPartRodMultiple,
                        "Length of inner part of rod (Li)",
                        ScrewdriverParameterType.LengthInnerPartRod, 
                        Errors
                    )
                },

                { ScrewdriverParameterType.LengthFixingWings,
                    new ScrewdriverParameter
                    (
                        -1,
                        _minTipRodHeight * _minWidestPartHandleMultiple *
                        _minLengthHandleMultiple * _minLengthFixingWingsMultiple,
                        _maxTipRodHeight * _maxWidestPartHandleMultiple *
                        _maxLengthHandleMultiple * _maxLengthFixingWingsMultiple,
                        "Length of fixing wings (Lf)",
                        ScrewdriverParameterType.LengthFixingWings,
                        Errors
                    )
                },
            };
        }

        /// <summary>
        /// Устанавливает значения параметров отвертки. Обновляет статус ошибки.
        /// </summary>
        /// <param name="tipType">Вид наконечника отвертки.</param>
        /// <param name="tipRodHeight">Высота наконечника стержня отвертки.</param>
        /// <param name="widestPartHandle">Самая широкая часть рукоятки.</param>
        /// <param name="lengthOuterPartRod">Длина внешней части стержня.</param>
        /// <param name="lengthHandle">Длина рукоятки отвертки.</param>
        /// <param name="lengthInnerPartRod">Длина внутренней части стержня.</param>
        /// <param name="lengthFixingWings">Длина фиксирующих крылышек стержня.</param>
        public void SetParameters
            (
            double tipType,
            double tipRodHeight,
            double widestPartHandle,
            double lengthOuterPartRod,
            double lengthHandle,
            double lengthInnerPartRod,
            double lengthFixingWings
            )
        {
            Errors.Clear();

            Parameters[ScrewdriverParameterType.TipType].Value = tipType;

            Parameters[ScrewdriverParameterType.TipRodHeight].Value = tipRodHeight;
            if (Errors.Any()) return;
            Parameters[ScrewdriverParameterType.WidestPartHandle].MinValue = 
                tipRodHeight * _minWidestPartHandleMultiple;
            Parameters[ScrewdriverParameterType.WidestPartHandle].MaxValue =
                tipRodHeight * _maxWidestPartHandleMultiple;
            Parameters[ScrewdriverParameterType.LengthOuterPartRod].MinValue =
                tipRodHeight * _minLengthOuterPartRodMultiple;
            Parameters[ScrewdriverParameterType.LengthOuterPartRod].MaxValue =
                tipRodHeight * _maxLengthOuterPartRodMultiple;

            Parameters[ScrewdriverParameterType.WidestPartHandle].Value = widestPartHandle;
            if (Errors.Any()) return;
            Parameters[ScrewdriverParameterType.LengthHandle].MinValue =
                widestPartHandle * _minLengthHandleMultiple;
            Parameters[ScrewdriverParameterType.LengthHandle].MaxValue =
                widestPartHandle * _maxLengthHandleMultiple;

            Parameters[ScrewdriverParameterType.LengthOuterPartRod].Value = lengthOuterPartRod;
            if (Errors.Any()) return;

            Parameters[ScrewdriverParameterType.LengthHandle].Value = lengthHandle;
            if (Errors.Any()) return;
            Parameters[ScrewdriverParameterType.LengthInnerPartRod].MinValue =
                lengthHandle * _minLengthInnerPartRodMultiple;
            Parameters[ScrewdriverParameterType.LengthInnerPartRod].MaxValue =
                lengthHandle * _maxLengthInnerPartRodMultiple;
            Parameters[ScrewdriverParameterType.LengthFixingWings].MinValue =
                lengthHandle * _minLengthFixingWingsMultiple;
            Parameters[ScrewdriverParameterType.LengthFixingWings].MaxValue =
                lengthHandle * _maxLengthFixingWingsMultiple;

            Parameters[ScrewdriverParameterType.LengthInnerPartRod].Value = lengthInnerPartRod;
            if (Errors.Any()) return;

            Parameters[ScrewdriverParameterType.LengthFixingWings].Value = lengthFixingWings;
            if (Errors.Any()) return;
        }

        /// <summary>
        /// Получить описание вида данных отвертки с их границами в виде строки.
        /// </summary>
        /// <param name="screwdriverParameter">
        /// Параметр отвертки, о которой необходимо узнать информацию.</param>
        /// <returns>Строка с описанием данного отвертки.</returns>
        public string GetGroupBoxDescription(ScrewdriverParameter screwdriverParameter)
        {
            if (Errors.Any())
            {
                var _errorKey = Errors.ElementAt(0).Key;

                switch (_errorKey)
                {
                    case ScrewdriverParameterType.TipRodHeight:
                        return GenerateGroupBoxDescription(screwdriverParameter, 1);

                    case ScrewdriverParameterType.WidestPartHandle:
                        return GenerateGroupBoxDescription(screwdriverParameter, 3);

                    case ScrewdriverParameterType.LengthOuterPartRod:
                        return GenerateGroupBoxDescription(screwdriverParameter, 4);

                    case ScrewdriverParameterType.LengthHandle:
                        return GenerateGroupBoxDescription(screwdriverParameter, 4);

                    case ScrewdriverParameterType.LengthInnerPartRod:
                        return GenerateGroupBoxDescription(screwdriverParameter, 6);

                    case ScrewdriverParameterType.LengthFixingWings:
                        return GenerateGroupBoxDescription(screwdriverParameter, 6);

                    default:
                        return string.Empty;
                }
            }
            else
            {
                return GenerateGroupBoxDescription(screwdriverParameter, 6);
            }
        }

        /// <summary>
        /// Генерация сообщения описания вида данных отвертки.
        /// </summary>
        /// <param name="screwdriverParameter">
        /// Параметр отвертки, о котором нужно узнать информацию.</param>
        /// <param name="stopper">Переменная шага, по которой определяется, 
        /// какой вариант сообщения выдаст эта функция.</param>
        /// <returns>Строка с корректным описанием данного отвертки.</returns>
        private string GenerateGroupBoxDescription
            (ScrewdriverParameter screwdriverParameter, int stopper)
        {
            if ((int)screwdriverParameter.ScrewdriverParameterType <= stopper)
            {
                return screwdriverParameter.MessageAttachment + ": "
                    + screwdriverParameter.MinValue + " - " + screwdriverParameter.MaxValue + " mm.";
            }
            else
            {
                return screwdriverParameter.MessageAttachment + ": "
                    + "# - # mm.";
            }
        }
    }
}
