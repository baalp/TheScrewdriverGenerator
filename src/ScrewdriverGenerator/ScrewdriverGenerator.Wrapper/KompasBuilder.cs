using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;
using Kompas6Constants3D;
using Kompas6Constants;

namespace ScrewdriverGenerator.Wrapper
{
    class KompasBuilder
    {

		/// <summary>
		/// Создать новый эскиз.
		/// </summary>
		/// <param name="ksPart">Документ детали, в котором нужно создать эскиз.</param>
		/// <param name="plane">Плоскость, на которой нужно создать эскиз.</param>
		/// <param name="sketchDefinition">Определение эскиза</param>
		/// <returns>Указатель на созданный эскиз</returns>
		public ksEntity CreateSketch
			(ksPart ksPart, ksEntity plane, out ksSketchDefinition sketchDefinition)
		{
			ksEntity sketch = ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_sketch);
			sketchDefinition = sketch.GetDefinition();
			sketchDefinition.SetPlane(plane);
			sketch.Create();
			return sketch;
		}

		/// <summary>
		/// Создать выдавливание на основе эскиза.
		/// </summary>
		/// <param name="ksPart">Документ детали, в котором нужно создать выдавливание.</param>
		/// <param name="sketch">Эскиз, который нужно выдавить.</param>
		/// <param name="forward">Направление выдавливания: true - вперед, false - назад.</param>
		/// <param name="direction_Type">Вариант направления выдавливания.</param>
		/// <param name="depth">Длина выдавливания.</param>
		/// <param name="draftValue">Угол наклона выдавливания.</param>
		/// <param name="draftOutward">Направление уклона: true - уклон внутрь, false - уклон наружу.</param>
		public void CreateBaseExtrusion
			(ksPart ksPart, ksEntity sketch, bool forward, Direction_Type direction_Type, 
			double depth, double draftValue = 0, bool draftOutward = false)
		{
			ksEntity extrusion =
				ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_baseExtrusion);
			ksBaseExtrusionDefinition extrusionDefinition = extrusion.GetDefinition();

			extrusionDefinition.directionType = (short)direction_Type;
			extrusionDefinition.SetSideParam(forward, (short)ksEndTypeEnum.etBlind, 
				depth, draftValue, draftOutward);
			extrusionDefinition.SetSketch(sketch);
			extrusion.Create();
		}

		/// <summary>
		/// Вырезание выдавливанием по эскизу.
		/// </summary>
		/// <param name="ksPart">Документ детали, в котором нужно сделать выдавливание.</param>
		/// <param name="sketch">Эскиз, который нужно выдавить.</param>
		/// <param name="forward">Направление выдавливания: true - вперед, false - назад.</param>
		/// <param name="direction_Type">Вариант направления выдавливания.</param>
		/// <param name="depth">Длина выдавливания.</param>
		/// <param name="draftValue">Угол наклона выдавливания.</param>
		/// <param name="draftOutward">Направление уклона: true - уклон внутрь, false - уклон наружу.</param>
		public void CutExtrusion(ksPart ksPart, ksEntity sketch, bool forward, 
			Direction_Type direction_Type, double depth, double draftValue = 0, 
			bool draftOutward = false)
		{
			ksEntity entity = ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_cutExtrusion);
			ksCutExtrusionDefinition definition = entity.GetDefinition();
			definition.cut = true;
			definition.directionType = (short)direction_Type;
			definition.SetSideParam(forward, (short)End_Type.etBlind, depth, draftValue, draftOutward);
			definition.SetSketch(sketch);
			entity.Create();
		}

		/// <summary>
		/// Постройка правильного многоугольника по его центру и углу наклона.
		/// </summary>
		/// <param name="kompasObject">Объект Компас API.</param>
		/// <param name="document2D">
		/// Документ, в который необходимо экспортировать многоугольник.</param>
		/// <param name="count">Количество углов многоугольника.</param>
		/// <param name="xCenter">Координата Х центра многоугольника.</param>
		/// <param name="yCenter">Координата Y центра многоугольника.</param>
		/// <param name="angle">Наклон будущего многоугольника.</param>
		/// <param name="radius">Радиус окружности, формирующей многоугольник.</param>
		/// <param name="describe">Правило формирования: false - вписанный, true - описанный.</param>
		/// <returns>Указатель на созданный многоугольник</returns>
		public int CreateRegularPolygon(KompasObject kompasObject, ksDocument2D document2D, int count,
			double xCenter, double yCenter, double angle, double radius, bool describe)
		{
			RegularPolygonParam regularPolygonParam =
				kompasObject.GetParamStruct((short)StructType2DEnum.ko_RegularPolygonParam);
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
		/// Вырезание вращением по эскизу вокруг линии оси.
		/// </summary>
		/// <param name="ksPart">Документ детали, в котором нужно создать вырезание.</param>
		/// <param name="sketch">Эскиз, по которому произойдет вырезание</param>
		/// <param name="forward">Направление вырезания: true - прямое, false - обратное.</param>
		/// <param name="direction_Type">Тип направления вращения.</param>
		/// <param name="angle">Угол выреза.</param>
		public void CutRotated(ksPart ksPart, ksEntity sketch, bool forward,
			Direction_Type direction_Type, double angle)
		{
			ksEntity entity = ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_cutRotated);
			ksCutRotatedDefinition definition = entity.GetDefinition();
			definition.cut = true;
			definition.SetSideParam(forward, angle);
			definition.directionType = (short)direction_Type;
			definition.SetSketch(sketch);
			entity.Create();
		}

		/// <summary>
		/// Постройка прямоугольника по его сторонам.
		/// </summary>
		/// <param name="kompasObject">Объект Компас API.</param>
		/// <param name="document2D">
		/// Документ, в который необходимо экспортировать прямоугольник</param>
		/// <param name="width">Ширина прямоугольника</param>
		/// <param name="height">Высота прямоугольника</param>
		/// <param name="x">X начальной точки</param>
		/// <param name="y">Y начальной точки</param>
		/// <returns>Указатель на созданный прямоугольник</returns>
		public int CreateRectangle(KompasObject kompasObject, ksDocument2D document2D, double width,
			double height, double x, double y)
		{
			ksRectangleParam param =
				kompasObject.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
			param.x = x;
			param.y = y;
			param.height = height;
			param.width = width;
			param.style = (int)ksCurveStyleEnum.ksCSNormal;
			return document2D.ksRectangle(param, 0);
		}

		/// <summary>
		/// Метод создания смещённой плоскости.
		/// </summary>
		/// <param name="ksPart">Деталь, в которой создается смещенная плоскость.</param>
		/// <param name="plane">Плоскость, на основе которой создается новая плоскость.</param>
		/// <param name="offset">Расстояние между существующей и генерируемой плоскости.</param>
		/// <returns>Указатель на смещённую плоскость.</returns>
		public ksEntity CreateOffsetPlane(ksPart ksPart, ksObj3dTypeEnum plane, double offset)
		{
			ksEntity defaultPlane =
				ksPart.GetDefaultEntity((short)plane);
			ksEntity planeOffset =
				ksPart.NewEntity((short)ksObj3dTypeEnum.o3d_planeOffset);
			ksPlaneOffsetDefinition planeOffsetDefinition = planeOffset.GetDefinition();

			planeOffsetDefinition.direction = true;
			planeOffsetDefinition.offset = offset;

			planeOffsetDefinition.SetPlane(defaultPlane);

			planeOffset.Create();
			return planeOffset;
		}
	}
}