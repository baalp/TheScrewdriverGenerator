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

        /// <summary>
        /// Запуск программы Компас-3D.
        /// </summary>
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
        /// <param name="invisible">Видимость создания документа</param>
        /// <param name="typeDoc">Тип документа: true - Деталь, false - сборка</param>
        public ksDocument3D CreateDocument(bool invisible, bool typeDoc)
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
        /// <param name="_ksPart">Деталь, свойства которой устанавливается этой функцией.</param>
        /// <param name="_ksDocument3D">Документ, в котором находится эта деталь.</param>
        /// <param name="name">Имя детали.</param>
        /// <param name="color">Цвет детали: R + G*256 + B*65535</param>
        public ksPart SetDetailProperties
            (ksPart _ksPart, ksDocument3D _ksDocument3D, string name, int color)
        {
            _ksPart = (ksPart)_ksDocument3D.GetPart((short)Part_Type.pTop_Part);
            _ksPart.name = name;
            _ksPart.SetAdvancedColor(color, 0.5, 0.6,
                0.8, 0.8, 1, 0.5);
            _ksPart.Update();
            return _ksPart;
        }
    }
}
