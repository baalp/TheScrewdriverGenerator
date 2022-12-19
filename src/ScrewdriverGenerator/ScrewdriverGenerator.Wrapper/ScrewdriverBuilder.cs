using System;
using ScrewdriverGenerator.Model;
using System.IO;
using Kompas6API5;
using Kompas6Constants3D;
using Kompas6Constants;

namespace ScrewdriverGenerator.Wrapper
{
	/// <summary>
	/// Класс формирования деталей и сборки отвертки.
	/// </summary>
	public class ScrewdriverBuilder
	{
		/// <summary>
		/// Объект создания связи с KOMPAS API и с функциями строительства моделей.
		/// </summary>
		private readonly KompasWrapper _kompasWrapper = new KompasWrapper();

		/// <summary>
		/// Объект данных об отвертке.
		/// </summary>
		private ScrewdriverData _screwdriverData = new ScrewdriverData();

		/// <summary>
		/// Объект функций над моделью детали.
		/// </summary>
		private ksPart _ksPart;

		/// <summary>
		/// Объект сбора коллекции объектов детали.
		/// </summary>
		private ksEntityCollection _ksEntityCollection;

		/// <summary>
		/// Объект документа.
		/// </summary>
		private ksDocument3D _ksDocument3D;

		/// <summary>
		/// Путь до папки, в которую экспортируется результат.
		/// </summary>
		private string _savePath;

		/// <summary>
		/// Главная функция, создающая детали для отвертки и их сборку.
		/// </summary>
		/// <param name="screwdriverData">Объект с информацией о создаваемой отвертке.</param>
		/// <param name="outputPath">Путь к папке, в которой создастся папка вывода.</param>
		public void BuildScrewdriver(ScrewdriverData screwdriverData, string outputPath)
		{
			_savePath = outputPath + "\\Screwdriver Generator Output\\";
			Directory.CreateDirectory(_savePath);

			DateTime now = DateTime.Now;
			string timeAddition = now.ToString("yyyyMMddHHmmss");

			_screwdriverData = screwdriverData;
			double T = _screwdriverData.Parameters
				[ScrewdriverParameterType.TipType].Value;
			double H = _screwdriverData.Parameters
				[ScrewdriverParameterType.TipRodHeight].Value;
			double D = _screwdriverData.Parameters
				[ScrewdriverParameterType.WidestPartHandle].Value;
			double Lo = _screwdriverData.Parameters
				[ScrewdriverParameterType.LengthOuterPartRod].Value;
			double Lh = _screwdriverData.Parameters
				[ScrewdriverParameterType.LengthHandle].Value;
			double Li = _screwdriverData.Parameters
				[ScrewdriverParameterType.LengthInnerPartRod].Value;
			double Lf = _screwdriverData.Parameters
				[ScrewdriverParameterType.LengthFixingWings].Value;

			_kompasWrapper.StartKompas();

			_ksDocument3D = _kompasWrapper.CreateDocument(false, true);
			_ksPart = _kompasWrapper.SetDetailProperties
				(_ksPart, _ksDocument3D, timeAddition + "_PartRodScrewdriver", 4737096);
			BuildRod(T, H, D, Lo, Lh, Li, Lf);
			_ksDocument3D.SaveAsEx(_savePath + timeAddition + "_PartRodScrewdriver.m3d", 0);

			_ksDocument3D = _kompasWrapper.CreateDocument(false, true);
			_ksPart = _kompasWrapper.SetDetailProperties
				(_ksPart, _ksDocument3D, timeAddition + "_PartHandleScrewdriver", 3381759);
			BuildHandle(H, D, Lh, Lf);
			_ksDocument3D.SaveAsEx(_savePath + timeAddition + "_PartHandleScrewdriver.m3d", 0);

			_ksDocument3D = _kompasWrapper.CreateDocument(false, false);
			_ksPart = _kompasWrapper.SetDetailProperties
				(_ksPart, _ksDocument3D, timeAddition + "_Screwdriver", 0);
			CreateAssembly(timeAddition);
			_ksDocument3D.SaveAsEx(_savePath + timeAddition + "_Screwdriver.m3d", 0);
		}

		/// <summary>
		/// Функция, создающая деталь стержня отвертки.
		/// </summary>
		/// <param name="T">Вид наконечника отвертки.</param>
		/// <param name="H">Толщина наконечника стержня отвертки.</param>
		/// <param name="D">Наибольший диаметр обхвата рукоятки отвертки.</param>
		/// <param name="Lo">Длина внешней части отвертки.</param>
		/// <param name="Lh">Длина рукоятки отвертки.</param>
		/// <param name="Li">Длина внутренней части отвертки.</param>
		/// <param name="Lf">Длина закрепляющих крылышек.</param>
		private void BuildRod
			(double T, double H, double D, double Lo, double Lh, double Li, double Lf)
		{
			//Закрепляющая часть
			ksEntity planeYOZ =
				_ksPart.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeYOZ);
			ksEntity sketchInnerPartRod = _kompasWrapper.CreateSketch
				(_ksPart, planeYOZ, out var sketchInnerPartRodDefinition);
			ksDocument2D ksDocument2D = sketchInnerPartRodDefinition.BeginEdit();
			int regularPolygon = _kompasWrapper.CreateRegularPolygon
				(_kompasWrapper._kompasObject, ksDocument2D, 6, 0, 0, 0, H * 5, false);
			sketchInnerPartRodDefinition.EndEdit();
			_kompasWrapper.CreateBaseExtrusion
				(_ksPart, sketchInnerPartRod, true, Direction_Type.dtNormal, Li);

			//Если значение Lf (Размер закрепляющих крылышек) не пустое.
			if (Lf != -2)
            {
				//Закрепляющие крылышки
				ksEntity planeXOY =
					_ksPart.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeXOY);
				ksEntity sketchFixingWingsRod = _kompasWrapper.CreateSketch
					(_ksPart, planeXOY, out var sketchFixingWingsRodDefinition);
				ksDocument2D = sketchFixingWingsRodDefinition.BeginEdit();
				_kompasWrapper.CreateRectangle
							(_kompasWrapper._kompasObject, ksDocument2D,
							Lf, H * 10, -Lf - (Li - (Lh * 0.5)), -H * 5);
				sketchFixingWingsRodDefinition.EndEdit();
				_kompasWrapper.CreateBaseExtrusion
					(_ksPart, sketchFixingWingsRod, true, Direction_Type.dtMiddlePlane, H);
			}

			//Стержень
			ksEntity sketchOuterPartRod = _kompasWrapper.CreateSketch
				(_ksPart, planeYOZ, out var sketchOuterPartRodDefinition);
			ksDocument2D = sketchOuterPartRodDefinition.BeginEdit();
			ksDocument2D.ksCircle(0, 0, H * 2, (short)ksCurveStyleEnum.ksCSNormal);
			sketchOuterPartRodDefinition.EndEdit();
			_kompasWrapper.CreateBaseExtrusion
				(_ksPart, sketchOuterPartRod, false, Direction_Type.dtReverse, Lo);

			//Создание наконечника
			switch (T)
			{
				//Плоский наконечник
				case 0:
					BuildRodTipFlat(ksDocument2D, H, Lo);
					break;

				//Крестовой наконечник
				case 1:
					BuildRodTipCross(ksDocument2D, H, Lo);
					break;

				//Треугольный наконечник
				case 2:
					BuildRodTipTriangle(ksDocument2D, H, Lo);
					break;
			}
		}

		private void BuildRodTipFlat(ksDocument2D ksDocument2D, double H, double Lo)
        {
			ksEntity sketchTipShapeGenerator = _kompasWrapper.CreateSketch
				(_ksPart, _kompasWrapper.CreateOffsetPlane
				(_ksPart, ksObj3dTypeEnum.o3d_planeYOZ, -Lo),
				out var sketchTipShapeGeneratorDefinition);
			ksDocument2D = sketchTipShapeGeneratorDefinition.BeginEdit();
			_kompasWrapper.CreateRectangle
				(_kompasWrapper._kompasObject, ksDocument2D, H * 8, H * 8, H * 0.5, -H * 4);
			_kompasWrapper.CreateRectangle
				(_kompasWrapper._kompasObject, ksDocument2D, H * 8, H * 8, -H * 8.5, -H * 4);
			sketchTipShapeGeneratorDefinition.EndEdit();
			_kompasWrapper.CutExtrusion
				(_ksPart, sketchTipShapeGenerator, false, 
				Direction_Type.dtReverse, H * 10, 10, true);
		}

		private void BuildRodTipCross(ksDocument2D ksDocument2D, double H, double Lo)
        {
			//Треугольник - усечение для наконечника
			ksEntity planeXOZ_ = _ksPart.GetDefaultEntity
				((short)ksObj3dTypeEnum.o3d_planeXOZ);
			ksEntity sketchCrossTipAngle = _kompasWrapper.CreateSketch
				(_ksPart, planeXOZ_, out var sketchCrossTipAngleDefinition);
			ksDocument2D = sketchCrossTipAngleDefinition.BeginEdit();
			ksDocument2D.ksLineSeg(Lo, 0, Lo, H * 2, 1);
			ksDocument2D.ksLineSeg(Lo - H, H * 2, Lo, H * 2, 1);
			ksDocument2D.ksLineSeg(Lo - H, H * 2, Lo, 0, 1);
			ksDocument2D.ksLineSeg(0, 0, 100, 0, 3);
			sketchCrossTipAngleDefinition.EndEdit();
			_kompasWrapper.CutRotated
				(_ksPart, sketchCrossTipAngle, false, Direction_Type.dtNormal, 360);
			//Квадраты - вырезы для создания крестового наконечника
			ksEntity sketchTipShapeCrossGenerator = _kompasWrapper.CreateSketch
				(_ksPart, _kompasWrapper.CreateOffsetPlane
				(_ksPart, ksObj3dTypeEnum.o3d_planeYOZ, -Lo),
				out var sketchTipShapeCrossGeneratorDefinition);
			ksDocument2D = sketchTipShapeCrossGeneratorDefinition.BeginEdit();
			_kompasWrapper.CreateRectangle
				(_kompasWrapper._kompasObject, ksDocument2D, H * 4, H * 4, H * 0.5, H * 0.5);
			_kompasWrapper.CreateRectangle
				(_kompasWrapper._kompasObject, ksDocument2D, H * 4, H * 4, -H * 4.5, H * 0.5);
			_kompasWrapper.CreateRectangle
				(_kompasWrapper._kompasObject, ksDocument2D, H * 4, H * 4, -H * 4.5, -H * 4.5);
			_kompasWrapper.CreateRectangle
				(_kompasWrapper._kompasObject, ksDocument2D, H * 4, H * 4, H * 0.5, -H * 4.5);
			sketchTipShapeCrossGeneratorDefinition.EndEdit();
			_kompasWrapper.CutExtrusion
				(_ksPart, sketchTipShapeCrossGenerator, false,
				Direction_Type.dtReverse, H * 10, 10, true);
		}

		private void BuildRodTipTriangle(ksDocument2D ksDocument2D, double H, double Lo)
        {
			//Три треугольника - вырезы для создания треугольного наконечника
			ksEntity sketchTipShapeTriangleGenerator = _kompasWrapper.CreateSketch
				(_ksPart, _kompasWrapper.CreateOffsetPlane
				(_ksPart, ksObj3dTypeEnum.o3d_planeYOZ, -Lo),
				out var sketchTipShapeTriangleGeneratorDefinition);
			ksDocument2D = sketchTipShapeTriangleGeneratorDefinition.BeginEdit();
			//Точки треугольника - основания треугольного наконечника отвертки
			double _X1 = H * Math.Sqrt(3) / 3;
			double _Y1 = 0;
			double _X2 = -H * Math.Sqrt(3) / 6;
			double _Y2 = -H / 2;
			double _X3 = -H * Math.Sqrt(3) / 6;
			double _Y3 = H / 2;
			//Длина сторон треугольников - усечений треугольного узора наконечника
			double _size = H * 10;

			//Первый треугольник
			//Первый отрезок
			ksDocument2D.ksLineSeg
				(_X1, _Y1, CartesianFromPolar(true, _size, 60, _X1, _Y1),
				CartesianFromPolar(false, _size, 60, _X1, _Y1), 1);
			//Второй отрезок
			ksDocument2D.ksLineSeg
				(_X1, _Y1, CartesianFromPolar(true, _size, 300, _X1, _Y1),
				CartesianFromPolar(false, _size, 300, _X1, _Y1), 1);
			//Перпендикулярный отрезок
			ksDocument2D.ksLineSeg
				(CartesianFromPolar(true, _size, 60, _X1, _Y1),
				CartesianFromPolar(false, _size, 60, _X1, _Y1),
				CartesianFromPolar(true, _size, 300, _X1, _Y1),
				CartesianFromPolar(false, _size, 300, _X1, _Y1), 1);
			//Второй треугольник
			//Первый отрезок
			ksDocument2D.ksLineSeg
				(_X2, _Y2, CartesianFromPolar(true, _size, 300, _X2, _Y2),
				CartesianFromPolar(false, _size, 300, _X2, _Y2), 1);
			//Второй отрезок
			ksDocument2D.ksLineSeg
				(_X2, _Y2, CartesianFromPolar(true, _size, 180, _X2, _Y2),
				CartesianFromPolar(false, _size, 180, _X2, _Y2), 1);
			//Перпендикулярный отрезок
			ksDocument2D.ksLineSeg
				(CartesianFromPolar(true, _size, 300, _X2, _Y2),
				CartesianFromPolar(false, _size, 300, _X2, _Y2),
				CartesianFromPolar(true, _size, 180, _X2, _Y2),
				CartesianFromPolar(false, _size, 180, _X2, _Y2), 1);
			//Третий треугольник
			//Первый отрезок
			ksDocument2D.ksLineSeg
				(_X3, _Y3, CartesianFromPolar(true, _size, 180, _X3, _Y3),
				CartesianFromPolar(false, _size, 180, _X3, _Y3), 1);
			//Второй отрезок
			ksDocument2D.ksLineSeg
				(_X3, _Y3, CartesianFromPolar(true, _size, 60, _X3, _Y3),
				CartesianFromPolar(false, _size, 60, _X3, _Y3), 1);
			//Перпендикулярный отрезок
			ksDocument2D.ksLineSeg
				(CartesianFromPolar(true, _size, 180, _X3, _Y3),
				CartesianFromPolar(false, _size, 180, _X3, _Y3),
				CartesianFromPolar(true, _size, 60, _X3, _Y3),
				CartesianFromPolar(false, _size, 60, _X3, _Y3), 1);

			sketchTipShapeTriangleGeneratorDefinition.EndEdit();
			_kompasWrapper.CutExtrusion
				(_ksPart, sketchTipShapeTriangleGenerator, false,
				Direction_Type.dtReverse, H * 10, 10, true);
		}

		/// <summary>
		/// Функция, создающая деталь рукоятки отвертки.
		/// </summary>
		/// <param name="H">Толщина наконечника стержня отвертки.</param>
		/// <param name="D">Наибольший диаметр обхвата рукоятки отвертки.</param>
		/// <param name="Lh">Длина рукоятки отвертки.</param>
		/// <param name="Lf">Длина закрепляющих крылышек.</param>
		private void BuildHandle(double H, double D, double Lh, double Lf)
		{
			//Создаем рукоять.
			ksEntity planeYOZ = _ksPart.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeYOZ);
			ksEntity sketchHandle = _kompasWrapper.CreateSketch
				(_ksPart, planeYOZ, out var sketchHandleDefinition);
			ksDocument2D ksDocument2D = sketchHandleDefinition.BeginEdit();
			ksDocument2D.ksCircle(0, 0, D / 2, (short)ksCurveStyleEnum.ksCSNormal);
			sketchHandleDefinition.EndEdit();
			_kompasWrapper.CreateBaseExtrusion
				(_ksPart, sketchHandle, true, Direction_Type.dtNormal, Lh);

			//Создаем отверстие под стержень.
			ksEntity sketchHandleHoleForRod = _kompasWrapper.CreateSketch
				(_ksPart, planeYOZ, out var sketchHandleHoleForRodDefinition);
			ksDocument2D = sketchHandleHoleForRodDefinition.BeginEdit();
			_kompasWrapper.CreateRegularPolygon
				(_kompasWrapper._kompasObject, ksDocument2D, 6, 0, 0, 0, H * 5, false);
			sketchHandleHoleForRodDefinition.EndEdit();
			_kompasWrapper.CutExtrusion
				(_ksPart, sketchHandleHoleForRod, false, Direction_Type.dtReverse, Lh / 2);

			//Создаем скругление на конце рукоятки эксизом треугольника.
			ksEntity planeXOZ = _ksPart.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeXOZ);
			ksEntity sketchHandleEndAngle = _kompasWrapper.CreateSketch
				(_ksPart, planeXOZ, out var sketchHandleEndAngleDefinition);
			ksDocument2D = sketchHandleEndAngleDefinition.BeginEdit();
			ksDocument2D.ksLineSeg(-Lh, -D / 4, -Lh, -D / 2, 1);
			ksDocument2D.ksLineSeg(-Lh, -D / 2, -Lh + D / 2, -D / 2, 1);
			ksDocument2D.ksLineSeg(-Lh + D / 2, -D / 2, -Lh, -D / 4, 1);
			ksDocument2D.ksLineSeg(0, 0, 100, 0, 3);
			sketchHandleEndAngleDefinition.EndEdit();
			_kompasWrapper.CutRotated
				(_ksPart, sketchHandleEndAngle, false, Direction_Type.dtNormal, 360);

			//Создаем впадину в конце рукоятки.
			ksEntity sketchHandleStart = _kompasWrapper.CreateSketch
				(_ksPart, planeXOZ, out var sketchHandleStartDefinition);
			ksDocument2D = sketchHandleStartDefinition.BeginEdit();
			_kompasWrapper.CreateRectangle
				(_kompasWrapper._kompasObject, ksDocument2D, Lh / 10 * 4, D / 8, -Lh / 10 * 4, -D / 2);
			ksDocument2D.ksLineSeg(0, 0, -100, 0, 3);
			sketchHandleStartDefinition.EndEdit();
			_kompasWrapper.CutRotated
				(_ksPart, sketchHandleStart, true, Direction_Type.dtNormal, 360);

			//Создаем переход между держащей частью и впадиной в конце рукоятки.
			ksEntity sketchHandleStartAngle = _kompasWrapper.CreateSketch
				(_ksPart, planeXOZ, out var sketchHandleStartAngleDefinition);
			ksDocument2D = sketchHandleStartAngleDefinition.BeginEdit();
			ksDocument2D.ksLineSeg(-Lh / 10 * 4, -D / 2, -Lh / 10 * 4 + -D / 2, -D / 2, 1);
			ksDocument2D.ksLineSeg(-Lh / 10 * 4, -D / 2, -Lh / 10 * 4, -D / 8 * 3, 1);
			ksDocument2D.ksLineSeg(-Lh / 10 * 4 + -D / 2, -D / 2, -Lh / 10 * 4, -D / 8 * 3, 1);
			ksDocument2D.ksLineSeg(0, 0, -100, 0, 3);
			sketchHandleStartAngleDefinition.EndEdit();
			_kompasWrapper.CutRotated
				(_ksPart, sketchHandleStartAngle, true, Direction_Type.dtNormal, 360);

			//Создаем неровную поверхность в держащей части рукоятки.
			ksEntity sketchHandleUnevenness = _kompasWrapper.CreateSketch
				(_ksPart, planeYOZ, out var sketchHandleUnevennessDefinition);
			ksDocument2D = sketchHandleUnevennessDefinition.BeginEdit();
			//TODO: рефаторить (+)
			//Дистанция от середины окружности, вырезающих грани у отвертки.
			double _radius = D / 16 * 23;
			for (int i = 0; i < 360; i += 60)
            {
				ksDocument2D.ksCircle(CartesianFromPolar(true, _radius, i),
				CartesianFromPolar(false, _radius, i), D, 1);
			}
			sketchHandleUnevennessDefinition.EndEdit();
			_kompasWrapper.CutExtrusion(_ksPart, sketchHandleUnevenness, false, 
				Direction_Type.dtReverse, Lh);

			if (Lf != -2)
            {
				//Отверстия под закрепляющие крылышки
				ksEntity planeXOY =
					_ksPart.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeXOY);
				ksEntity sketchFixingWingsHandle = _kompasWrapper.CreateSketch
					(_ksPart, planeXOY, out var sketchFixingWingsHandleDefinition);
				ksDocument2D = sketchFixingWingsHandleDefinition.BeginEdit();
				_kompasWrapper.CreateRectangle
							(_kompasWrapper._kompasObject, ksDocument2D,
							Lf, H * 10, -Lf, -H * 5);
				sketchFixingWingsHandleDefinition.EndEdit();
				_kompasWrapper.CutExtrusion
					(_ksPart, sketchFixingWingsHandle, true, Direction_Type.dtMiddlePlane, H);
			}
		}

		/// <summary>
		/// Функция, создающая сборку из делатей отвертки.
		/// </summary>
		private void CreateAssembly(string timeAddition)
		{
			//Добавляем детали с сборку и соотносим их
			Console.WriteLine(_ksDocument3D.SetPartFromFile(_savePath + timeAddition + 
				"_PartRodScrewdriver.m3d", _ksPart, false));
			Console.WriteLine(_ksDocument3D.SetPartFromFile(_savePath + timeAddition + 
				"_PartHandleScrewdriver.m3d", _ksPart, false));
			_ksPart.UpdatePlacement();
			_ksDocument3D.RebuildDocument();

			//Добавляем сопряжения
			//Ручка
			_ksPart = _ksDocument3D.GetPart(0);
			_ksEntityCollection = _ksPart.EntityCollection((short)Obj3dType.o3d_face);
			ksEntity _ksEntityRodBottom = _ksEntityCollection.GetByIndex(0);
			_ksEntityCollection = _ksPart.EntityCollection((short)Obj3dType.o3d_axisOX);
			ksEntity _ksEntityRodAxis = _ksEntityCollection.GetByIndex(0);

			//Стержень
			_ksPart = _ksDocument3D.GetPart(1);
			_ksEntityCollection = _ksPart.EntityCollection((short)Obj3dType.o3d_face);
			ksEntity _ksEntityHandleBottom = _ksEntityCollection.GetByIndex(8);
			_ksEntityCollection = _ksPart.EntityCollection((short)Obj3dType.o3d_axisOX);
			ksEntity _ksEntityHandleAxis = _ksEntityCollection.GetByIndex(0);

			_ksDocument3D.AddMateConstraint(0, _ksEntityRodBottom, _ksEntityHandleBottom);
			_ksDocument3D.AddMateConstraint(0, _ksEntityRodAxis, _ksEntityHandleAxis, 1);
		}

		/// <summary>
		/// Функция, рассчитывающая координату точки по ее расстоянию и углу от другой точки.
		/// (Переводит из полярных координат в Декартовые).
		/// </summary>
		/// <param name="isX">Рассчитывает ли функция X или Y: true - X, false - Y.</param>
		/// <param name="radius">Расстояние между точками.</param>
		/// <param name="angle">Угол, по которому точка исказилась относительно другой точки.</param>
		/// <param name="x0">Положение по Х для первой точки.</param>
		/// <param name="y0">Положение по Y для первой точки.</param>
		/// <returns>Координату X или Y.</returns>
		private double CartesianFromPolar(bool isX, double radius, double angle, 
			double x0 = 0, double y0 = 0)
		{
			if (isX)
			{
				return x0 + radius * Math.Cos(angle * (Math.PI / 180.0));
			}
			else
			{
				return y0 + radius * Math.Sin(angle * (Math.PI / 180.0));
			}
		}
	}
}
