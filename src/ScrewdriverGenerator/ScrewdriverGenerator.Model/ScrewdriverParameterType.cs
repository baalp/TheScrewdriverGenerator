
namespace ScrewdriverGenerator.Model
{
    /// <summary>
    /// Перечисление параметров генерируемой отвертки.
    /// </summary>
    public enum ScrewdriverParameterType
    {
        /// <summary>
        /// Вид наконечника отвертки: 0 = Плоская; 1 = Крестовая; 2 = Треугольная.
        /// </summary>
        TipType,

        /// <summary>
        /// H – Высота наконечника стержня отвертки.
        /// </summary>
        TipRodHeight,

        /// <summary>
        /// D – Самая широкая часть рукоятки.
        /// </summary>
        WidestPartHandle,

        /// <summary>
        /// Lo – Длина внешней части стержня.
        /// </summary>
        LengthOuterPartRod,

        /// <summary>
        /// Lh – Длина рукоятки отвертки.
        /// </summary>
        LengthHandle,

        /// <summary>
        /// Li – Длина внутренней части стержня.
        /// </summary>
        LengthInnerPartRod,

        /// <summary>
        /// Lf – Длина закрепляющих крылышек.
        /// </summary>
        LengthFixingWings
    }
}
