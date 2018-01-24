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
	public class Ball3D
	{
		List<Ellipse> circles = new List<Ellipse>();
		public List<Line> myLines = new List<Line>();
		List<TextBlock> textBlocks = new List<TextBlock>();
		public Int32[] indices2;

		public GeometryModel3D Ball1 = new GeometryModel3D();

		MeshGeometry3D MakeBall()
		{
			MeshGeometry3D ball = new MeshGeometry3D();
			Point3DCollection corners = new Point3DCollection();
			corners.Add(new Point3D(0, 1, 0));
			corners.Add(new Point3D(0, 0.866, -0.5));
			corners.Add(new Point3D(-0.25, 0.866, -0.433));
			corners.Add(new Point3D(-0.433, 0.866, -0.25));
			corners.Add(new Point3D(-0.5, 0.866, 0));
			corners.Add(new Point3D(-0.433, 0.866, 0.25));
			corners.Add(new Point3D(-0.25, 0.866, 0.433));
			corners.Add(new Point3D(0, 0.866, 0.5));
			corners.Add(new Point3D(0.25, 0.866, 0.433));
			corners.Add(new Point3D(0.433, 0.866, 0.25));
			corners.Add(new Point3D(0.5, 0.866, 0));
			corners.Add(new Point3D(0.433, 0.866, -0.25));
			corners.Add(new Point3D(0.25, 0.866, -0.433));
			corners.Add(new Point3D(0, 0.5, -0.866));
			corners.Add(new Point3D(-0.433, 0.5, -0.75));
			corners.Add(new Point3D(-0.75, 0.5, -0.433));
			corners.Add(new Point3D(-0.866, 0.5, 0));
			corners.Add(new Point3D(-0.75, 0.5, 0.433));
			corners.Add(new Point3D(-0.433, 0.5, 0.75));
			corners.Add(new Point3D(0, 0.5, 0.866));
			corners.Add(new Point3D(0.433, 0.5, 0.75));
			corners.Add(new Point3D(0.75, 0.5, 0.433));
			corners.Add(new Point3D(0.866, 0.5, 0));
			corners.Add(new Point3D(0.75, 0.5, -0.433));
			corners.Add(new Point3D(0.433, 0.5, -0.75));
			corners.Add(new Point3D(0, 0, -1));
			corners.Add(new Point3D(-0.5, -0, -0.866));
			corners.Add(new Point3D(-0.866, 0, -0.5));
			corners.Add(new Point3D(-1, 0, 0));
			corners.Add(new Point3D(-0.866, 0, 0.5));
			corners.Add(new Point3D(-0.5, 0, 0.866));
			corners.Add(new Point3D(0, 0, 1));
			corners.Add(new Point3D(0.5, 0, 0.866));
			corners.Add(new Point3D(0.866, 0, 0.5));
			corners.Add(new Point3D(1, 0, 0));
			corners.Add(new Point3D(0.866, 0, -0.5));
			corners.Add(new Point3D(0.5, 0, -0.866));
			corners.Add(new Point3D(0, -0.5, -0.866));
			corners.Add(new Point3D(-0.433, -0.5, -0.75));
			corners.Add(new Point3D(-0.75, -0.5, -0.433));
			corners.Add(new Point3D(-0.866, -0.5, 0));
			corners.Add(new Point3D(-0.75, -0.5, 0.433));
			corners.Add(new Point3D(-0.433, -0.5, 0.75));
			corners.Add(new Point3D(0, -0.5, 0.866));
			corners.Add(new Point3D(0.433, -0.5, 0.75));
			corners.Add(new Point3D(0.75, -0.5, 0.433));
			corners.Add(new Point3D(0.866, -0.5, 0));
			corners.Add(new Point3D(0.75, -0.5, -0.433));
			corners.Add(new Point3D(0.433, -0.5, -0.75));
			corners.Add(new Point3D(0, -0.866, -0.5));
			corners.Add(new Point3D(-0.25, -0.866, -0.433));
			corners.Add(new Point3D(-0.433, -0.866, -0.25));
			corners.Add(new Point3D(-0.5, -0.866, 0));
			corners.Add(new Point3D(-0.433, -0.866, 0.25));
			corners.Add(new Point3D(-0.25, -0.866, 0.433));
			corners.Add(new Point3D(0, -0.866, 0.5));
			corners.Add(new Point3D(0.25, -0.866, 0.433));
			corners.Add(new Point3D(0.433, -0.8660, 0.25));
			corners.Add(new Point3D(0.5, -0.866, 0));
			corners.Add(new Point3D(0.433, -0.866, -0.25));
			corners.Add(new Point3D(0.25, -0.866, -0.433));
			corners.Add(new Point3D(0, -1, 0));
			ball.Positions = corners;

			Int32[] indices ={

							1, 2, 3,
							1, 3, 4,
							1, 4, 5,
							1, 5, 6,
							1, 6, 7,
							1, 7, 8,
							1, 8, 9,
							1, 9, 10,
							1, 10, 11,
							1, 11, 12,
							1, 12, 13,
							1, 13, 2,
							15, 3, 2,
							2, 14, 15,
							16, 4, 3,
							3, 15, 16,
							17, 5, 4,
							4, 16, 17,
							18, 6, 5,
							5, 17, 18,
							19, 7, 6,
							6, 18, 19,
							20, 8, 7,
							7, 19, 20,
							21, 9, 8,
							8, 20, 21,
							22, 10, 9,
							9, 21, 22,
							23, 11, 10,
							10, 22, 23,
							24, 12, 11,
							11, 23, 24,
							25, 13, 12,
							12, 24, 25,
							14, 2 ,13 ,
							13, 25, 14,
							27, 15, 14,
							14, 26, 27,
							28, 16, 15,
							15, 27, 28,
							29, 17, 16,
							16, 28, 29,
							30, 18, 17,
							17, 29, 30,
							31, 19, 18,
							18, 30, 31,
							32, 20, 19,
							19, 31, 32,
							33, 21, 20,
							20, 32, 33,
							34, 22, 21,
							21, 33, 34,
							35, 23, 22,
							22, 34, 35,
							36, 24, 23,
							23, 35, 36,
							37, 25, 24,
							24, 36, 37,
							26, 14, 25,
							25, 37, 26,
							39, 27, 26,
							26, 38, 39,
							40, 28, 27,
							27, 39, 40,
							41, 29, 28,
							28, 40, 41,
							42, 30, 29,
							29, 41, 42,
							43, 31, 30,
							30, 42, 43,
							44, 32, 31,
							31, 43, 44,
							45, 33, 32,
							32, 44, 45,
							46, 34, 33,
							33, 45, 46,
							47, 35, 34,
							34, 46, 47,
							48, 36, 35,
							35, 47, 48,
							49, 37, 36,
							36, 48, 49,
							38, 26, 37,
							37, 49, 38,
							51, 39, 38,
							38, 50, 51,
							52, 40, 39,
							39, 51, 52,
							53, 41, 40,
							40, 52, 53,
							54, 42, 41,
							41, 53, 54,
							55, 43, 42,
							42, 54, 55,
							56, 44, 43,
							43, 55, 56,
							57, 45, 44,
							44, 56, 57,
							58, 46, 45,
							45, 57, 58,
							59, 47, 46,
							46, 58, 59,
							60, 48, 47,
							47, 59, 60,
							61, 49, 48,
							48, 60, 61,
							50, 38, 49,
							49, 61, 50,
							62, 51, 50,
							62, 52, 51,
							62, 53, 52,
							62, 54, 53,
							62, 55, 54,
							62, 56, 55,
							62, 57, 56,
							62, 58, 57,
							62, 59, 58,
							62, 60, 59,
							62, 61, 60,
							62, 50, 61
			};

			indices2 = indices;

			Int32Collection Triangles =
								  new Int32Collection();
			foreach (Int32 index in indices)
			{
				Triangles.Add(index - 1);
			}
			ball.TriangleIndices = Triangles;

			for (int i = 0; i < ball.Positions.Count; i++)
			{
				// Text for Vertex
				TextBlock text = new TextBlock();
				text.Text = i.ToString();
				text.HorizontalAlignment = HorizontalAlignment.Center;
				text.Background = Brushes.White;

				textBlocks.Add(text);

				// Circles for Vertex
				Ellipse circle = new Ellipse();
				circle.Stroke = System.Windows.Media.Brushes.Transparent;
				circle.Fill = System.Windows.Media.Brushes.Transparent;
				circle.Width = 5;
				circle.Height = 5;

				circles.Add(circle);
			}

			for (int i = 0; i < indices.Length; i++)
			{
				Line line = new Line();
				line.Stroke = System.Windows.Media.Brushes.White;
				line.HorizontalAlignment = HorizontalAlignment.Left;
				line.VerticalAlignment = VerticalAlignment.Center;
				line.StrokeThickness = 1;

				myLines.Add(line);
			}

			return ball;
		}

		public void CreateCube(ref Canvas canvas, Viewport3D viewport)
		{
			MeshGeometry3D cubeMesh = MakeBall();
			Ball1.Geometry = cubeMesh;
			Ball1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));

			int i = 0;
			foreach (var item in textBlocks)
			{
				canvas.Children.Add(item);

				item.RenderTransform = new TranslateTransform
				{
					X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[i]).X + 10,
					Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[i]).Y + 10
				};

				i++;
			}


			// DOTS
			i = 0;
			foreach (var item in circles)
			{
				canvas.Children.Add(item);

				item.RenderTransform = new TranslateTransform
				{
					X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[i]).X - 2.5,
					Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[i]).Y - 2.5
				};

				i++;
			}

			// WIREFRAME
			//i = 0;
			//int j = 1;
			//foreach (var item in myLines)
			//{
			//	canvas.Children.Add(item);

			//	item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[indices2[i]]).X;
			//	item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[indices2[i]]).Y;

			//	item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[indices2[j]]).X;
			//	item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[indices2[j]]).Y;

			//	if (j > myLines.Count - 2)
			//	{
			//		break;
			//	}

			//	i++;
			//	j++;
			//}
		}
	}
}
