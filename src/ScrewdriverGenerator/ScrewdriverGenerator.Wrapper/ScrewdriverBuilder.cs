using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrewdriverGenerator.Model;
using Kompas6API5;
using Kompas6Constants3D;
using Kompas6Constants;

namespace ScrewdriverGenerator.Wrapper
{
    public class ScrewdriverBuilder
    {
        private readonly KompasWrapper _kompasWrapper = new KompasWrapper();

		private ScrewdriverData _screwdriverData = new ScrewdriverData();

		/// <summary>
		/// Деталь.
		/// </summary>
		private ksPart _ksPart;

		private ksDocument3D _ksDocument3D;

		public void BuildScrewdriver(ScrewdriverData screwdriverData)
        {
			_screwdriverData = screwdriverData;
			_kompasWrapper.StartKompas();
			_ksDocument3D = _kompasWrapper.CreateDocument();
            _ksPart = _kompasWrapper.SetDetailProperties(_ksPart, _ksDocument3D);
			double H = _screwdriverData.Parameters[ScrewdriverParameterType.TipRodHeight].Value;
			double Lo = _screwdriverData.Parameters[ScrewdriverParameterType.LengthOuterPartRod].Value;
			double Li = _screwdriverData.Parameters[ScrewdriverParameterType.LengthInnerPartRod].Value;
			BuildRod(H, Lo, Li);
        }

		private void BuildRod(double H, double Lo, double Li)
        {
			//Закрепляющая часть
			ksEntity planeYOZ =
				_ksPart.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeYOZ);
			ksEntity sketchInnerPartRod = CreateSketch(planeYOZ, out var sketchInnerPartRodDefinition);
			ksDocument2D ksDocument2D = sketchInnerPartRodDefinition.BeginEdit();
			int regularPolygon = CreateRegularPolygon(ksDocument2D, 6, 0, 0, 0, H * 5, true);
			sketchInnerPartRodDefinition.EndEdit();
			CreateBaseExtrusion(sketchInnerPartRod, true, Direction_Type.dtNormal, Li);
			
			//Стержень
			ksEntity sketchOuterPartRod = CreateSketch(planeYOZ, out var sketchOuterPartRodDefinition);
			ksDocument2D = sketchOuterPartRodDefinition.BeginEdit();
			//CreateCircle(ksDocument2D, H * 4, 0, 0);
			ksDocument2D.ksCircle(0, 0, H * 2, (short)ksCurveStyleEnum.ksCSNormal);
			sketchOuterPartRodDefinition.EndEdit();
			CreateBaseExtrusion(sketchOuterPartRod, false, Direction_Type.dtReverse, Lo);
			
			//Создание наконечника
			switch (_screwdriverData.Parameters[ScrewdriverParameterType.TipType].Value)
            {
				//Плоский наконечник
				case 0:
					//Плоскость для создания наконечника стержня
					ksEntity sketchTipShapeGenerator = CreateSketch(CreateOffsetPlane(-Lo, ksObj3dTypeEnum.o3d_planeYOZ), out var sketchTipShapeGeneratorDefinition);
					ksDocument2D = sketchTipShapeGeneratorDefinition.BeginEdit();
					CreateRectangle(ksDocument2D, H * 8, H * 8, H * 0.5, -H * 4);
					CreateRectangle(ksDocument2D, H * 8, H * 8, -H * 8.5, -H * 4);
					sketchTipShapeGeneratorDefinition.EndEdit();
					CutExtrusion(sketchTipShapeGenerator, H * 10, false, 10, true);
					break;

				//Крестовой наконечник
				case 1:
					//Треугольник - усечение для наконечника
					ksEntity planeXOZ = _ksPart.GetDefaultEntity((short)ksObj3dTypeEnum.o3d_planeXOZ);
					ksEntity sketchCrossTipAngle = CreateSketch(planeXOZ, out var sketchCrossTipAngleDefinition);
					ksDocument2D = sketchCrossTipAngleDefinition.BeginEdit();
					ksDocument2D.ksLineSeg(Lo, 0, Lo, H * 2, 1);
					ksDocument2D.ksLineSeg(Lo - H, H * 2, Lo, H * 2, 1);
					ksDocument2D.ksLineSeg(Lo - H, H * 2, Lo, 0, 1);
					ksDocument2D.ksLineSeg(0, 0, 100, 0, 3);
					sketchCrossTipAngleDefinition.EndEdit();
					CutRotated(sketchCrossTipAngle, 60, false);
					//Квадраты - вырезы для создания крестового наконечника
					ksEntity sketchTipShapeCrossGenerator = CreateSketch(CreateOffsetPlane(-Lo, ksObj3dTypeEnum.o3d_planeYOZ), out var sketchTipShapeCrossGeneratorDefinition);
					ksDocument2D = sketchTipShapeCrossGeneratorDefinition.BeginEdit();
					CreateRectangle(ksDocument2D, H * 4, H * 4, H * 0.5, H * 0.5);
					CreateRectangle(ksDocument2D, H * 4, H * 4, -H * 4.5, H * 0.5);
					CreateRectangle(ksDocument2D, H * 4, H * 4, -H * 4.5, -H * 4.5);
					CreateRectangle(ksDocument2D, H * 4, H * 4, H * 0.5, -H * 4.5);
					sketchTipShapeCrossGeneratorDefinition.EndEdit();
					CutExtrusion(sketchTipShapeCrossGenerator, H * 10, false, 10, true);
					break;

				//Треугольный наконечник
				case 2:
					//Три треугольника - вырезы для создания треугольного наконечника
					ksEntity sketchTipShapeTriangleGenerator = CreateSketch(CreateOffsetPlane(-Lo, ksObj3dTypeEnum.o3d_planeYOZ), out var sketchTipShapeTriangleGeneratorDefinition);
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
						GetXorYbyRadiusAndAngle(true,  H * 10, 180, -H * Math.Sqrt(3) / 6, H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 180, -H * Math.Sqrt(3) / 6, H / 2),
						GetXorYbyRadiusAndAngle(true,  H * 10, 60,  -H * Math.Sqrt(3) / 6, H / 2),
						GetXorYbyRadiusAndAngle(false, H * 10, 60,  -H * Math.Sqrt(3) / 6, H / 2),
						1
						);
					sketchTipShapeTriangleGeneratorDefinition.EndEdit();
					CutExtrusion(sketchTipShapeTriangleGenerator, H * 10, false, 10, true);
					break;
			}
		}

		private double GetXorYbyRadiusAndAngle(bool isX, double radius, double angle, double x0 = 0, double y0 = 0)
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

		/// <summary>
		/// Метод создания эскиза
		/// </summary>
		/// <param name="plane">Плоскость</param>
		/// <param name="sketchDefinition">Параметры эскиза</param>
		/// <returns>Указатель на эскиз</returns>
		private ksEntity CreateSketch(ksEntity plane, out ksSketchDefinition sketchDefinition)
		{
			ksEntity sketch = _ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_sketch);
			sketchDefinition = sketch.GetDefinition();
			sketchDefinition.SetPlane(plane);
			sketch.Create();
			return sketch;
		}

		/// <summary>
		/// Метод постройки окружности
		/// </summary>
		/// <param name="document2D">Интерфейс графического документа</param>
		/// <param name="count">Диаметр окружности</param>
		private int CreateRegularPolygon(ksDocument2D document2D, int count,
			double xCenter, double yCenter, double angle, double radius, bool describe)
		{
			//count, xCenter, yCenter, angle, radius, describe
			KompasObject kompas = _kompasWrapper._kompasObject;
			RegularPolygonParam regularPolygonParam =
				kompas.GetParamStruct((short)StructType2DEnum.ko_RegularPolygonParam);
			//RegularPolygonParam regularPolygonParam;
			regularPolygonParam.count = count;
			regularPolygonParam.xc = xCenter;
			regularPolygonParam.yc = yCenter;
			regularPolygonParam.ang = angle;
			regularPolygonParam.radius = radius;
			regularPolygonParam.describe = describe;
			regularPolygonParam.style = (int)ksCurveStyleEnum.ksCSNormal;
			return document2D.ksRegularPolygon(regularPolygonParam, (short)ksCurveStyleEnum.ksCSNormal);
		}

		/// <summary>
		/// Метод создания выдавливанием
		/// </summary>
		/// <param name="direction_Type">Направление выдавливания</param>
		/// <param name="length">Длина выдавливания</param>
		/// <param name="sketch">Эскиз выдавливания</param>
		private void CreateBaseExtrusion(ksEntity sketch, bool forward, Direction_Type direction_Type, double depth, double draftValue = 0, bool draftOutward = false)
		{
			ksEntity extrusion =
				_ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_baseExtrusion);
			ksBaseExtrusionDefinition extrusionDefinition = extrusion.GetDefinition();

			extrusionDefinition.directionType = (short)direction_Type;
			extrusionDefinition.SetSideParam(forward, (short)ksEndTypeEnum.etBlind, depth, draftValue, draftOutward);
			extrusionDefinition.SetSketch(sketch);
			extrusion.Create();
		}

		/// <summary>
		/// Вырезание выдавливанием по эскизу.
		/// </summary>
		/// <param name="sketch">Эскиз.</param>
		/// <param name="height">Высота выдавливания.</param>
		/// <param name="direction">Направление: true - прямое, false - обратное.</param>
		public void CutExtrusion(ksEntity sketch, double height, bool direction, double draftValue = 0, bool draftOutward = false)
		{
			ksEntity entity = _ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_cutExtrusion);
			ksCutExtrusionDefinition definition = entity.GetDefinition();
			if (direction)
			{
				definition.directionType = (short)Direction_Type.dtNormal;
			}
			else
			{
				definition.directionType = (short)Direction_Type.dtReverse;
			}
			definition.cut = true;
			definition.SetSideParam(direction, (short)End_Type.etBlind, height, draftValue, draftOutward);
			definition.SetSketch(sketch);
			entity.Create();
		}

		/// <summary>
		/// Вырезание выдавливанием вращение по эскизу.
		/// </summary>
		/// <param name="sketch">Эскиз.</param>
		/// <param name="height">Высота выдавливания.</param>
		/// <param name="direction">Направление: true - прямое, false - обратное.</param>
		public void CutRotated(ksEntity sketch, double height, bool direction, double draftValue = 0, bool draftOutward = false)
		{
			ksEntity entity = _ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_cutRotated);
			ksCutRotatedDefinition definition = entity.GetDefinition();
			if (direction)
			{
				definition.directionType = (short)Direction_Type.dtNormal;
			}
			else
			{
				definition.directionType = (short)Direction_Type.dtReverse;
			}
			definition.cut = true;
			definition.SetSideParam(true, 360);
			definition.directionType = (short)Direction_Type.dtNormal;
			definition.SetSketch(sketch);
			entity.Create();
		}

		/// <summary>
		/// Метод создания смещённой плоскости
		/// </summary>
		/// <param name="offset">расстояние смещения</param>
		/// <param name="plane">базовая плоскость</param>
		/// <returns>Указатель на смещённую плоскость</returns>
		private ksEntity CreateOffsetPlane(double offset, ksObj3dTypeEnum plane)
		{
			ksEntity defaultPlane =
				_ksPart.GetDefaultEntity((short)plane);
			ksEntity planeOffset =
				_ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_planeOffset);
			ksPlaneOffsetDefinition planeOffsetDefinition = planeOffset.GetDefinition();

			planeOffsetDefinition.direction = true;
			planeOffsetDefinition.offset = offset;

			planeOffsetDefinition.SetPlane(defaultPlane);

			planeOffset.Create();
			return planeOffset;
		}

		/// <summary>
		/// Метод постройки прямоугольник
		/// </summary>
		/// <param name="document2D">Интерфейс графического документа</param>
		/// <param name="width">Ширина квадрата</param>
		/// <param name="height">Высота квадрата</param>
		/// <param name="x">X начальной точки</param>
		/// <param name="y">Y начальной точки</param>
		/// <returns>Указатель на созданный прямоугольник</returns>
		private int CreateRectangle(ksDocument2D document2D, double width,
			double height, double x, double y)
		{
			KompasObject kompas = _kompasWrapper._kompasObject;
			ksRectangleParam param =
				kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
			param.x = x;
			param.y = y;
			param.height = height;
			param.width = width;
			param.style = (int)ksCurveStyleEnum.ksCSNormal;
			return document2D.ksRectangle(param, 0);
		}
	}
}
