using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewdriverGenerator.Model
{
    /// <summary>
    /// Перечисление параметров генерируемой отвертки
    /// </summary>
    public enum ScrewdriverParameterType
    {
        /// <summary>
        /// Вид наконечника отвертки: 0 = Плоская; 1 = Крестовая; 2 = Треугольная
        /// </summary>
        TipType,

        /// <summary>
        /// H – Высота наконечника стержня отвертки
        /// </summary>
        TipRodHeight,

        /// <summary>
        /// D – Самая широкая часть рукоятки
        /// </summary>
        WidestPartofHandle,

        /// <summary>
        /// Lo – Длина внешней части стержня
        /// </summary>
        LengthofOuterPartofRod,

        /// <summary>
        /// Lh – Длина рукоятки отвертки
        /// </summary>
        LengthHandle,

        /// <summary>
        /// Li – Длина внутренней части стержня
        /// </summary>
        LengthofInnerPartofRod
    }
}
