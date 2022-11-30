using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewdriverGenerator.Model
{
    public class ScrewdriverData
    {
        public Dictionary<ScrewdriverParameterType, ScrewdriverParameter> Parameters { get; set; }
        public Dictionary<ScrewdriverParameterType, string> Errors { get; set; }

        private double _minTipRodHeight = 0.1;
        private double _maxTipRodHeight = 10;
        private double _minWidestPartHandleMultiple = 16;
        private double _maxWidestPartHandleMultiple = 24;
        private double _minLengthOuterPartRodMultiple = 20;
        private double _maxLengthOuterPartRodMultiple = 400;
        private double _minLengthHandleMultiple = 3.75;
        private double _maxLengthHandleMultiple = 7.5;
        private double _minLengthInnerPartRodMultiple = 0.5;
        private double _maxLengthInnerPartRodMultiple = 0.6;

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
                        "Tip rod height",
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
                        "Widest part of handle",
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
                        "Length of outer part of rod",
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
                        "Length of handle",
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
                        "Length of inner part of rod",
                        ScrewdriverParameterType.LengthInnerPartRod, 
                        Errors
                    )
                },
            };
        }

        public void SetParameters
            (
            double tipType,
            double tipRodHeight,
            double widestPartHandle,
            double lengthOuterPartRod,
            double lengthHandle,
            double lengthInnerPartRod
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

            Parameters[ScrewdriverParameterType.LengthInnerPartRod].Value = lengthInnerPartRod;
            if (Errors.Any()) return;
        }
    }
}
