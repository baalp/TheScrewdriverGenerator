using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;
using Kompas6Constants3D;
using System.Runtime.InteropServices;

namespace ScrewdriverGenerator.Wrapper
{
    class KompasWrapper
    {
        /// <summary>
        /// Объект Компас API.
        /// </summary>
        public KompasObject _kompasObject;

        public void StartKompas()
        {
            try
            {
                if (_kompasObject != null)
                {
                    _kompasObject.Visible = true;
                    _kompasObject.ActivateControllerAPI();
                }
                if (_kompasObject != null) return;
                var kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
                _kompasObject = (KompasObject)Activator.CreateInstance(kompasType);
                StartKompas();
                if (_kompasObject == null)
                {
                    throw new Exception("Не удается открыть Компас-3D.");
                }
            }
            catch (COMException)
            {
                _kompasObject = null;
                StartKompas();
            }
        }

        /// <summary>
        /// Создание документа в Компас-3D.
        /// </summary>
        public ksDocument3D CreateDocument()
        {
            try
            {
                ksDocument3D _ksDocument3D;
                _ksDocument3D = (ksDocument3D)_kompasObject.Document3D();
                _ksDocument3D.Create();
                _ksDocument3D = (ksDocument3D)_kompasObject.ActiveDocument3D();
                return _ksDocument3D;
            }
            catch
            {
                throw new ArgumentException("Не удается построить деталь");
            }
        }

        /// <summary>
        /// Установка свойств детали: цвета и имени.
        /// </summary>
        public ksPart SetDetailProperties(ksPart _ksPart, ksDocument3D _ksDocument3D)
        {
            _ksPart = (ksPart)_ksDocument3D.GetPart((short)Part_Type.pTop_Part);
            _ksPart.name = "Screwdriver";
            _ksPart.SetAdvancedColor(14211288, 0.5, 0.6,
                0.8, 0.8, 1, 0.5);
            _ksPart.Update();
            return _ksPart;
        }
    }
}
