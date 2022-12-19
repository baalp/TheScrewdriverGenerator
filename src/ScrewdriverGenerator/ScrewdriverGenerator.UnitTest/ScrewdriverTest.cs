using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ScrewdriverGenerator.Model;

namespace ScrewdriverGenerator.UnitTest
{
    /// <summary>
    /// Класс тестирования полей класса отвертки.
    /// </summary>
    [TestFixture]
    public class ScrewdriverTest
    {
        /// <summary>
        /// Объект класса отвертки.
        /// </summary>
        private readonly ScrewdriverData _screwdriverData = new ScrewdriverData();

        /// <summary>
        /// Позитивный и негативный тест сеттера Parameters.
        /// </summary>
        [Test(Description = "Позитивный и негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 30, 25, 15, 5, 0, Description = "Позитивный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 30, 25, 15, -2, 0, Description = "Позитивный тест сеттера SetParameters.")]
        [TestCase(4, 0.2, 4, 30, 25, 15, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 4, 4, 30, 25, 15, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 0.5, 30, 25, 15, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 300, 25, 15, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 3, 25, 15, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 2, 25, 15, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 2, 5, 15, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 2, 50, 15, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 2, 25, 5, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 2, 25, 50, -2, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 2, 25, 15, 1, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 2, 25, 15, 20, 1, Description = "Негативный тест сеттера SetParameters.")]
        [TestCase(0, 0.2, 4, 2, 25, 15, -1, 1, Description = "Негативный тест сеттера SetParameters.")]
        public void TestSetParameters_CorrectValue
            (
            double tipType,
            double tipRodHeight,
            double widestPartHandle,
            double lengthOuterPartRod,
            double lengthHandle,
            double lengthInnerPartRod,
            double lengthFixingWings,
            int expected
            )
        {
            _screwdriverData.SetParameters(tipType, tipRodHeight, widestPartHandle, lengthOuterPartRod, lengthHandle, lengthInnerPartRod, lengthFixingWings);
            var actual = _screwdriverData.Errors.Count;
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Позитивный и негативный тест метода GetGroupBoxDescription.")]
        [TestCase(ScrewdriverParameterType.TipRodHeight, ScrewdriverParameterType.TipRodHeight, "Tip rod height (H): 0,1 - 10 mm.", Description = "Позитивный тест сеттера SetParameters.")]
        [TestCase(ScrewdriverParameterType.LengthFixingWings, ScrewdriverParameterType.LengthInnerPartRod, "Length of fixing wings (Lf): 0,6 - 900 mm.", Description = "Позитивный тест сеттера SetParameters.")]
        [TestCase(ScrewdriverParameterType.LengthFixingWings, -1, "", Description = "Негативный тест сеттера SetParameters.")]
        public void TestGetTextBoxDescription_CorrectValue(ScrewdriverParameterType type, ScrewdriverParameterType typeForError, string expected)
        {
            ScrewdriverParameter screwdriverParameter = _screwdriverData.Parameters[type];
            _screwdriverData.Errors.Clear();
            _screwdriverData.Errors.Add(typeForError, "Error");
            var actual = _screwdriverData.GetGroupBoxDescription(screwdriverParameter);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест геттера Value, при допустимом значении.
        /// </summary>
        [Test(Description = "Позитивный тест геттера Value, при допустимом значении.")]
        public void TestValueGet_CorrectValueGood()
        {
            var parameter = new ScrewdriverParameter(2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            const int expected = 2;
            var actual = parameter.Value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест геттера Value, при допустимом пустом значении.
        /// </summary>
        [Test(Description = "Позитивный тест геттера Value, при допустимом пустом значении.")]
        public void TestValueGet_CorrectValueEmpty()
        {
            var parameter = new ScrewdriverParameter(-2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            const int expected = -2;
            var actual = parameter.Value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Негативный тест геттера Value, при слишком малом значении.
        /// </summary>
        [Test(Description = "Негативный тест геттера Value, при слишком малом значении.")]
        public void TestValueGet_IncorrectValueSmall()
        {
            var parameter = new ScrewdriverParameter(2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            parameter.Value = -3;
            var actual = parameter.Value;
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Негативный тест геттера Value, при слишком большом значении.
        /// </summary>
        [Test(Description = "Негативный тест геттера Value, при слишком большом значении.")]
        public void TestValueGet_IncorrectValueBig()
        {
            var parameter = new ScrewdriverParameter(2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            parameter.Value = 20;
            var actual = parameter.Value;
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Негативный тест геттера Value, при обязательном пустом значении.
        /// </summary>
        [Test(Description = "Негативный тест геттера Value, при обязательном пустом значении.")]
        public void TestValueGet_IncorrectValueEmpty()
        {
            var parameter = new ScrewdriverParameter(2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            parameter.Value = -1;
            var actual = parameter.Value;
            var expected = 2;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест геттера и сеттера MinValue.
        /// </summary>
        [Test(Description = "Позитивный тест геттера и сеттера MinValue.")]
        public void TestMinValueGet_CorrectValue()
        {
            var parameter = new ScrewdriverParameter(2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            parameter.MinValue = 0.5;
            const double expected = 0.5;
            var actual = parameter.MinValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест геттера и сеттера MaxValue.
        /// </summary>
        [Test(Description = "Позитивный тест геттера и сеттера MaxValue.")]
        public void TestMaxValueGet_CorrectValue()
        {
            var parameter = new ScrewdriverParameter(2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            parameter.MaxValue = 10;
            const int expected = 10;
            var actual = parameter.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест геттера ScrewdriverParameterType.
        /// </summary>
        [Test(Description = "Позитивный тест геттера ScrewdriverParameterType.")]
        public void TestScrewdriverParameterTypeGet_CorrectValue()
        {
            var parameter = new ScrewdriverParameter(2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            var expected = ScrewdriverParameterType.TipRodHeight;
            var actual = parameter.ScrewdriverParameterType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест геттера MessageAttachment.
        /// </summary>
        [Test(Description = "Позитивный тест геттера MessageAttachment.")]
        public void TestMessageAttachmentGet_CorrectValue()
        {
            var parameter = new ScrewdriverParameter(2, 1, 5,
                "Value", ScrewdriverParameterType.TipRodHeight,
                new Dictionary<ScrewdriverParameterType, string>());
            var expected = "Value";
            var actual = parameter.MessageAttachment;
            Assert.AreEqual(expected, actual);
        }
    }
}
