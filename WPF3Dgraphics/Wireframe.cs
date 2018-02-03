using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace WPF3Dgraphics
{
	public class Wireframe
	{
		public bool WireframeOnOrOff { get; set; }

		//List<Line> myLines = new List<Line>();
		//List<TextBlock> textBlocks = new List<TextBlock>();

		public void CreateModelWire(GeometryModel3D geoModel, Viewport3D viewport, ref Canvas canvas, Int32[] indices, List<Line> lines)
		{
			MeshGeometry3D mesh;
			mesh = (MeshGeometry3D)geoModel.Geometry;

			//for (int ij = 0; ij < indices.Length; ij++)
			//{
			//	Line line = new Line();
			//	line.Stroke = System.Windows.Media.Brushes.White;
			//	line.HorizontalAlignment = HorizontalAlignment.Left;
			//	line.VerticalAlignment = VerticalAlignment.Center;
			//	line.StrokeThickness = 1;

			//	lines.Add(line);
			//}

			int i = 0;
			int j = 1;
			foreach (var item in lines)
			{
				canvas.Children.Add(item);

				item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, mesh.Positions[indices[i]]).X;
				item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, mesh.Positions[indices[i]]).Y;

				item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, mesh.Positions[indices[j]]).X;
				item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, mesh.Positions[indices[j]]).Y;

				if (j > lines.Count - 100)
				{
					break;
				}

				i++;
				j++;
			}
		}

		public void DrawWireFrame(GeometryModel3D geoModel, Viewport3D myViewport, Int32[] indices, List<Line> lines)
		{
			MeshGeometry3D mesh;
			mesh = (MeshGeometry3D)geoModel.Geometry;

			int i = 0;
			//foreach (var item in textBlocks)
			//{
			//	item.RenderTransform = new TranslateTransform
			//	{
			//		X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).X,
			//		Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).Y
			//	};

			//	i++;
			//}

			// WIREFRAME
			i = 0;
			int j = 1;
			foreach (var item in lines)
			{
				item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[indices[i] -1]).X;
				item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[indices[i] -1]).Y;

				item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[indices[j] -1]).X;
				item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[indices[j] -1]).Y;

				if (j > lines.Count - 2)
				{
					break;
				}

				i++;
				j++;
			}
		}
	}
}
