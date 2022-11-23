using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewdriverGenerator.Model
{
    class ScrewdriverData
    {
        public Dictionary<ScrewdriverParameterType, ScrewdriverParameter> Parameters { get; set; }
        public Dictionary<ScrewdriverParameterType, string> Errors { get; set; }
        public ScrewdriverData()
        {
            Errors = new Dictionary<ScrewdriverParameterType, string>();

            var _minTipRodHeight = 0.1;
            var _maxTipRodHeight = 10;
            var _minWidestPartHandleMultiple = 16;
            var _maxWidestPartHandleMultiple = 24;
            var _minLengthOuterPartRodMultiple = 20;
            var _maxLengthOuterPartRodMultiple = 400;
            var _minLengthHandleMultiple = 3.75;
            var _maxLengthHandleMultiple = 7.5;
            var _minLengthInnerPartRodMultiple = 0.5;
            var _maxLengthInnerPartRodMultiple = 0.6;

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
                    new ScrewdriverParameter(1, 1.6, 240,"Length of handle",
                    ScrewdriverParameterType.LengthHandle, Errors)},

                { ScrewdriverParameterType.LengthInnerPartRod,
                    new ScrewdriverParameter(1, 1.6, 240,"Length of inner part of rod",
                    ScrewdriverParameterType.LengthInnerPartRod, Errors)},
            };
        }
    }
}
