using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrewdriverGenerator.Model;
using System.IO;
using Kompas6API5;
using Kompas6Constants3D;
using Kompas6Constants;

namespace ScrewdriverGenerator.Wrapper
{
	/// <summary>
	/// 
	/// </summary>
	public class ScrewdriverBuilder
	{
		/// <summary>
		/// Объект создания связи с KOMPAS API.
		/// </summary>
		private readonly KompasWrapper _kompasWrapper = new KompasWrapper();

		/// <summary>
		/// Объект с функциями строительства деталей и сборок.
		/// </summary>
		private KompasBuilder _kompasBuilder = new KompasBuilder();

		/// <summary>
		/// Объект данных об отвертке.
		/// </summary>
		private ScrewdriverData _screwdriverData = new ScrewdriverData();

		/// <summary>
		/// Деталь стержня отвертки.
		/// </summary>
		private ksPart _ksPartRod;

		/// <summary>
		/// Деталь рукоятки отвертки.
		/// </summary>
		private ksPart _ksPartHandle;

		/// <summary>
		/// Сборка отвертки.
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
			double T = _screwdriverData.Parameters[ScrewdriverParameterType.TipType].Value;
			double H = _screwdriverData.Parameters[ScrewdriverParameterType.TipRodHeight].Value;
			double D = _screwdriverData.Parameters[ScrewdriverParameterType.WidestPartHandle].Value;
			double Lo = _screwdriverData.Parameters[ScrewdriverParameterType.LengthOuterPartRod].Value;
			double Lh = _screwdriverData.Parameters[ScrewdriverParameterType.LengthHandle].Value;
			double Li = _screwdriverData.Parameters[ScrewdriverParameterType.LengthInnerPartRod].Value;
			double Lf = _screwdriverData.Parameters[ScrewdriverParameterType.LengthFixingWings].Value;

			_kompasWrapper.StartKompas();

			_ksDocument3D = _kompasWrapper.CreateDocument(false, true);
			_ksPartRod = _kompasWrapper.SetDetailProperties
				(_ksPartRod, _ksDocument3D, timeAddition + "_PartRodScrewdriver", 4737096);
			BuildRod(T, H, D, Lo, Lh, Li, Lf);
			_ksDocument3D.SaveAsEx(_savePath + timeAddition + "_PartRodScrewdriver.m3d", 0);

			_ksDocument3D = _kompasWrapper.CreateDocument(false, true);
			_ksPartHandle = _kompasWrapper.SetDetailProperties
				(_ksPartHandle, _ksDocument3D, timeAddition + "_PartHandleScrewdriver", 3381759);
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
		private void BuildRod(double T, double H, double D, double Lo, double Lh, double Li, double Lf)
		{
			//Закрепляющая часть
			ksEntity planeYOZ =
				_ksPartRod.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeYOZ);
			ksEntity sketchInnerPartRod = _kompasBuilder.CreateSketch
				(_ksPartRod, planeYOZ, out var sketchInnerPartRodDefinition);
			ksDocument2D ksDocument2D = sketchInnerPartRodDefinition.BeginEdit();
			int regularPolygon = _kompasBuilder.CreateRegularPolygon
				(_kompasWrapper._kompasObject, ksDocument2D, 6, 0, 0, 0, H * 5, false);
			sketchInnerPartRodDefinition.EndEdit();
			_kompasBuilder.CreateBaseExtrusion
				(_ksPartRod, sketchInnerPartRod, true, Direction_Type.dtNormal, Li);

			if (Lf != -2)
            {
				//Закрепляющие крылышки
				ksEntity planeXOY =
					_ksPartRod.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeXOY);
				ksEntity sketchFixingWingsRod = _kompasBuilder.CreateSketch
					(_ksPartRod, planeXOY, out var sketchFixingWingsRodDefinition);
				ksDocument2D = sketchFixingWingsRodDefinition.BeginEdit();
				_kompasBuilder.CreateRectangle
							(_kompasWrapper._kompasObject, ksDocument2D,
							Lf, H * 10, -Lf - (Li - (Lh * 0.5)), -H * 5);
				sketchFixingWingsRodDefinition.EndEdit();
				_kompasBuilder.CreateBaseExtrusion
					(_ksPartRod, sketchFixingWingsRod, true, Direction_Type.dtMiddlePlane, H);
			}

			//Стержень
			ksEntity sketchOuterPartRod = _kompasBuilder.CreateSketch
				(_ksPartRod, planeYOZ, out var sketchOuterPartRodDefinition);
			ksDocument2D = sketchOuterPartRodDefinition.BeginEdit();
			//CreateCircle(ksDocument2D, H * 4, 0, 0);
			ksDocument2D.ksCircle(0, 0, H * 2, (short)ksCurveStyleEnum.ksCSNormal);
			sketchOuterPartRodDefinition.EndEdit();
			_kompasBuilder.CreateBaseExtrusion
				(_ksPartRod, sketchOuterPartRod, false, Direction_Type.dtReverse, Lo);

			//Создание наконечника
			switch (T)
			{
				//Плоский наконечник
				case 0:
					//Плоскость для создания наконечника стержня
					ksEntity sketchTipShapeGenerator = _kompasBuilder.CreateSketch
						(_ksPartRod, _kompasBuilder.CreateOffsetPlane
						(_ksPartRod, ksObj3dTypeEnum.o3d_planeYOZ, -Lo), 
						out var sketchTipShapeGeneratorDefinition);
					ksDocument2D = sketchTipShapeGeneratorDefinition.BeginEdit();
					_kompasBuilder.CreateRectangle
						(_kompasWrapper._kompasObject, ksDocument2D, H * 8, H * 8, H * 0.5, -H * 4);
					_kompasBuilder.CreateRectangle
						(_kompasWrapper._kompasObject, ksDocument2D, H * 8, H * 8, -H * 8.5, -H * 4);
					sketchTipShapeGeneratorDefinition.EndEdit();
					_kompasBuilder.CutExtrusion
						(_ksPartRod, sketchTipShapeGenerator, false, Direction_Type.dtReverse, H * 10, 10, true);
					break;

				//Крестовой наконечник
				case 1:
					//Треугольник - усечение для наконечника
					ksEntity planeXOZ_ = _ksPartRod.GetDefaultEntity
						((short)ksObj3dTypeEnum.o3d_planeXOZ);
					ksEntity sketchCrossTipAngle = _kompasBuilder.CreateSketch
						(_ksPartRod, planeXOZ_, out var sketchCrossTipAngleDefinition);
					ksDocument2D = sketchCrossTipAngleDefinition.BeginEdit();
					ksDocument2D.ksLineSeg(Lo, 0, Lo, H * 2, 1);
					ksDocument2D.ksLineSeg(Lo - H, H * 2, Lo, H * 2, 1);
					ksDocument2D.ksLineSeg(Lo - H, H * 2, Lo, 0, 1);
					ksDocument2D.ksLineSeg(0, 0, 100, 0, 3);
					sketchCrossTipAngleDefinition.EndEdit();
					_kompasBuilder.CutRotated
						(_ksPartRod, sketchCrossTipAngle, false, Direction_Type.dtNormal, 360);
					//Квадраты - вырезы для создания крестового наконечника
					ksEntity sketchTipShapeCrossGenerator = _kompasBuilder.CreateSketch
						(_ksPartRod, _kompasBuilder.CreateOffsetPlane
						(_ksPartRod, ksObj3dTypeEnum.o3d_planeYOZ, -Lo), 
						out var sketchTipShapeCrossGeneratorDefinition);
					ksDocument2D = sketchTipShapeCrossGeneratorDefinition.BeginEdit();
					_kompasBuilder.CreateRectangle
						(_kompasWrapper._kompasObject, ksDocument2D, H * 4, H * 4, H * 0.5, H * 0.5);
					_kompasBuilder.CreateRectangle
						(_kompasWrapper._kompasObject, ksDocument2D, H * 4, H * 4, -H * 4.5, H * 0.5);
					_kompasBuilder.CreateRectangle
						(_kompasWrapper._kompasObject, ksDocument2D, H * 4, H * 4, -H * 4.5, -H * 4.5);
					_kompasBuilder.CreateRectangle
						(_kompasWrapper._kompasObject, ksDocument2D, H * 4, H * 4, H * 0.5, -H * 4.5);
					sketchTipShapeCrossGeneratorDefinition.EndEdit();
					_kompasBuilder.CutExtrusion
						(_ksPartRod, sketchTipShapeCrossGenerator, false, 
						Direction_Type.dtReverse, H * 10, 10, true);
					break;

				//Треугольный наконечник
				case 2:
					//Три треугольника - вырезы для создания треугольного наконечника
					ksEntity sketchTipShapeTriangleGenerator = _kompasBuilder.CreateSketch
						(_ksPartRod, _kompasBuilder.CreateOffsetPlane
						(_ksPartRod, ksObj3dTypeEnum.o3d_planeYOZ, -Lo), 
						out var sketchTipShapeTriangleGeneratorDefinition);
					ksDocument2D = sketchTipShapeTriangleGeneratorDefinition.BeginEdit();
					//Первый треугольник
					//Первый отрезок
					ksDocument2D.ksLineSeg
						(
						H * Math.Sqrt(3) / 3,
						0,
						GetXorYbyRadiusAndAngle(true, H * 10, 60, H * Math.Sqrt(3) / 3, 0),
						GetXorYbyRadiusAndAngle(false, H * 10, 60, H * Math.Sqrt(3) / 3, 0),
						1
						);
					//Второй отрезок
					ksDocument2D.ksLineSeg
						(
						H * Math.Sqrt(3) / 3,
						0,
						GetXorYbyRadiusAndAngle(true, H * 10, 300, H * Math.Sqrt(3) / 3, 0),
						GetXorYbyRadiusAndAngle(false, H * 10, 300, H * Math.Sqrt(3) / 3, 0),
						1
						);
					//Перпендикулярный отрезок
					ksDocument2D.ksLineSeg
						(
						GetXorYbyRadiusAndAngle(true, H * 10, 60, H * Math.Sqrt(3) / 3, 0),
						GetXorYbyRadiusAndAngle(false, H * 10, 60, H * Math.Sqrt(3) / 3, 0),
						GetXorYbyRadiusAndAngle(true, H * 10, 300, H * Math.Sqrt(3) / 3, 0),
						GetXorYbyRadiusAndAngle(false, H * 10, 300, H * Math.Sqrt(3) / 3, 0),
						1
						);
					//Второй треугольник
					//Первый отрезок
					ksDocument2D.ksLineSeg
						(
						-H * Math.Sqrt(3) / 6,
						-H / 2,
						GetXorYbyRadiusAndAngle(true, H * 10, 300, -H * Math.Sqrt(3) / 6, -H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 300, -H * Math.Sqrt(3) / 6, -H / 2),
						1
						);
					//Второй отрезок
					ksDocument2D.ksLineSeg
						(
						-H * Math.Sqrt(3) / 6,
						-H / 2,
						GetXorYbyRadiusAndAngle(true, H * 10, 180, -H * Math.Sqrt(3) / 6, -H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 180, -H * Math.Sqrt(3) / 6, -H / 2),
						1
						);
					//Перпендикулярный отрезок
					ksDocument2D.ksLineSeg
						(
						GetXorYbyRadiusAndAngle(true, H * 10, 300, -H * Math.Sqrt(3) / 6, -H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 300, -H * Math.Sqrt(3) / 6, -H / 2),
						GetXorYbyRadiusAndAngle(true, H * 10, 180, -H * Math.Sqrt(3) / 6, -H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 180, -H * Math.Sqrt(3) / 6, -H / 2),
						1
						);
					//Третий треугольник
					//Первый отрезок
					ksDocument2D.ksLineSeg
						(
						-H * Math.Sqrt(3) / 6,
						H / 2,
						GetXorYbyRadiusAndAngle(true, H * 10, 180, -H * Math.Sqrt(3) / 6, H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 180, -H * Math.Sqrt(3) / 6, H / 2),
						1
						);
					//Второй отрезок
					ksDocument2D.ksLineSeg
						(
						-H * Math.Sqrt(3) / 6,
						H / 2,
						GetXorYbyRadiusAndAngle(true, H * 10, 60, -H * Math.Sqrt(3) / 6, H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 60, -H * Math.Sqrt(3) / 6, H / 2),
						1
						);
					//Перпендикулярный отрезок
					ksDocument2D.ksLineSeg
						(
						GetXorYbyRadiusAndAngle(true, H * 10, 180, -H * Math.Sqrt(3) / 6, H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 180, -H * Math.Sqrt(3) / 6, H / 2),
						GetXorYbyRadiusAndAngle(true, H * 10, 60, -H * Math.Sqrt(3) / 6, H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 60, -H * Math.Sqrt(3) / 6, H / 2),
						1
						);
					sketchTipShapeTriangleGeneratorDefinition.EndEdit();
					_kompasBuilder.CutExtrusion
						(_ksPartRod, sketchTipShapeTriangleGenerator, false, 
						Direction_Type.dtReverse, H * 10, 10, true);
					break;
			}
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
			//Создаем рукоять
			ksEntity planeYOZ = _ksPartHandle.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeYOZ);
			ksEntity sketchHandle = _kompasBuilder.CreateSketch
				(_ksPartHandle, planeYOZ, out var sketchHandleDefinition);
			ksDocument2D ksDocument2D = sketchHandleDefinition.BeginEdit();
			ksDocument2D.ksCircle(0, 0, D / 2, (short)ksCurveStyleEnum.ksCSNormal);
			sketchHandleDefinition.EndEdit();
			_kompasBuilder.CreateBaseExtrusion
				(_ksPartHandle, sketchHandle, true, Direction_Type.dtNormal, Lh);

			//Создаем отверстие под стержень
			ksEntity sketchHandleHoleForRod = _kompasBuilder.CreateSketch
				(_ksPartHandle, planeYOZ, out var sketchHandleHoleForRodDefinition);
			ksDocument2D = sketchHandleHoleForRodDefinition.BeginEdit();
			_kompasBuilder.CreateRegularPolygon
				(_kompasWrapper._kompasObject, ksDocument2D, 6, 0, 0, 0, H * 5, false);
			sketchHandleHoleForRodDefinition.EndEdit();
			_kompasBuilder.CutExtrusion
				(_ksPartHandle, sketchHandleHoleForRod, false, Direction_Type.dtReverse, Lh / 2);

			//Создаем скругление на конце рукоятки эксизом треугольника
			ksEntity planeXOZ = _ksPartHandle.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeXOZ);
			ksEntity sketchHandleEndAngle = _kompasBuilder.CreateSketch
				(_ksPartHandle, planeXOZ, out var sketchHandleEndAngleDefinition);
			ksDocument2D = sketchHandleEndAngleDefinition.BeginEdit();
			ksDocument2D.ksLineSeg(-Lh, -D / 4, -Lh, -D / 2, 1);
			ksDocument2D.ksLineSeg(-Lh, -D / 2, -Lh + D / 2, -D / 2, 1);
			ksDocument2D.ksLineSeg(-Lh + D / 2, -D / 2, -Lh, -D / 4, 1);
			ksDocument2D.ksLineSeg(0, 0, 100, 0, 3);
			sketchHandleEndAngleDefinition.EndEdit();
			_kompasBuilder.CutRotated
				(_ksPartHandle, sketchHandleEndAngle, false, Direction_Type.dtNormal, 360);

			//Создаем впадину в конце рукоятки
			ksEntity sketchHandleStart = _kompasBuilder.CreateSketch
				(_ksPartHandle, planeXOZ, out var sketchHandleStartDefinition);
			ksDocument2D = sketchHandleStartDefinition.BeginEdit();
			_kompasBuilder.CreateRectangle
				(_kompasWrapper._kompasObject, ksDocument2D, Lh / 10 * 4, D / 8, -Lh / 10 * 4, -D / 2);
			ksDocument2D.ksLineSeg(0, 0, -100, 0, 3);
			sketchHandleStartDefinition.EndEdit();
			_kompasBuilder.CutRotated
				(_ksPartHandle, sketchHandleStart, true, Direction_Type.dtNormal, 360);

			//Создаем переход между держащей частью и впадиной в конце рукоятки
			ksEntity sketchHandleStartAngle = _kompasBuilder.CreateSketch
				(_ksPartHandle, planeXOZ, out var sketchHandleStartAngleDefinition);
			ksDocument2D = sketchHandleStartAngleDefinition.BeginEdit();
			ksDocument2D.ksLineSeg(-Lh / 10 * 4, -D / 2, -Lh / 10 * 4 + -D / 2, -D / 2, 1);
			ksDocument2D.ksLineSeg(-Lh / 10 * 4, -D / 2, -Lh / 10 * 4, -D / 8 * 3, 1);
			ksDocument2D.ksLineSeg(-Lh / 10 * 4 + -D / 2, -D / 2, -Lh / 10 * 4, -D / 8 * 3, 1);
			ksDocument2D.ksLineSeg(0, 0, -100, 0, 3);
			sketchHandleStartAngleDefinition.EndEdit();
			_kompasBuilder.CutRotated
				(_ksPartHandle, sketchHandleStartAngle, true, Direction_Type.dtNormal, 360);

			//Создаем неровную поверхность в держащей части рукоятки
			ksEntity sketchHandleUnevenness = _kompasBuilder.CreateSketch
				(_ksPartHandle, planeYOZ, out var sketchHandleUnevennessDefinition);
			ksDocument2D = sketchHandleUnevennessDefinition.BeginEdit();
			//TODO: рефаторить
			ksDocument2D.ksCircle(GetXorYbyRadiusAndAngle(true, D / 16 * 23, 0), 
				GetXorYbyRadiusAndAngle(false, D / 16 * 23, 0), D, (short)ksCurveStyleEnum.ksCSNormal);
			ksDocument2D.ksCircle(GetXorYbyRadiusAndAngle(true, D / 16 * 23, 60), 
				GetXorYbyRadiusAndAngle(false, D / 16 * 23, 60), D, (short)ksCurveStyleEnum.ksCSNormal);
			ksDocument2D.ksCircle(GetXorYbyRadiusAndAngle(true, D / 16 * 23, 120), 
				GetXorYbyRadiusAndAngle(false, D / 16 * 23, 120), D, (short)ksCurveStyleEnum.ksCSNormal);
			ksDocument2D.ksCircle(GetXorYbyRadiusAndAngle(true, D / 16 * 23, 180), 
				GetXorYbyRadiusAndAngle(false, D / 16 * 23, 180), D, (short)ksCurveStyleEnum.ksCSNormal);
			ksDocument2D.ksCircle(GetXorYbyRadiusAndAngle(true, D / 16 * 23, 240), 
				GetXorYbyRadiusAndAngle(false, D / 16 * 23, 240), D, (short)ksCurveStyleEnum.ksCSNormal);
			ksDocument2D.ksCircle(GetXorYbyRadiusAndAngle(true, D / 16 * 23, 300), 
				GetXorYbyRadiusAndAngle(false, D / 16 * 23, 300), D, (short)ksCurveStyleEnum.ksCSNormal);
			sketchHandleUnevennessDefinition.EndEdit();
			_kompasBuilder.CutExtrusion(_ksPartHandle, sketchHandleUnevenness, false, 
				Direction_Type.dtReverse, Lh);

			if (Lf != -2)
            {
				//Отверстия под закрепляющие крылышки
				ksEntity planeXOY =
					_ksPartHandle.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeXOY);
				ksEntity sketchFixingWingsHandle = _kompasBuilder.CreateSketch
					(_ksPartHandle, planeXOY, out var sketchFixingWingsHandleDefinition);
				ksDocument2D = sketchFixingWingsHandleDefinition.BeginEdit();
				_kompasBuilder.CreateRectangle
							(_kompasWrapper._kompasObject, ksDocument2D,
							Lf, H * 10, -Lf, -H * 5);
				sketchFixingWingsHandleDefinition.EndEdit();
				_kompasBuilder.CutExtrusion
					(_ksPartHandle, sketchFixingWingsHandle, true, Direction_Type.dtMiddlePlane, H);
			}
		}

		/// <summary>
		/// Функция, создающая сборку из делатей отвертки.
		/// </summary>
		private void CreateAssembly(string timeAddition)
		{
			//Добавляем детали с сборку и соотносим их
			Console.WriteLine(_ksDocument3D.SetPartFromFile(_savePath + timeAddition + "_PartRodScrewdriver.m3d", _ksPart, false));
			Console.WriteLine(_ksDocument3D.SetPartFromFile(_savePath + timeAddition + "_PartHandleScrewdriver.m3d", _ksPart, false));
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
		/// </summary>
		/// <param name="isX">Рассчитывает ли функция X или Y: true - X, false - Y.</param>
		/// <param name="radius">Расстояние между точками.</param>
		/// <param name="angle">Угол, по которому точка исказилась относительно другой точки.</param>
		/// <param name="x0">Положение по Х для первой точки.</param>
		/// <param name="y0">Положение по Y для первой точки.</param>
		private double GetXorYbyRadiusAndAngle(bool isX, double radius, double angle, 
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
