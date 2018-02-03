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
	public class Cube3D
	{

		// Shapes for 2D layer (might be moved to separate class)
		public List<Ellipse> circles = new List<Ellipse>();
		public List<Line> myLines = new List<Line>();
		public List<TextBlock> textBlocks = new List<TextBlock>();
		public Int32[] indices2;

		public GeometryModel3D Cube1 = new GeometryModel3D();

		MeshGeometry3D MakeCube()
		{
			MeshGeometry3D cube = new MeshGeometry3D();
			Point3DCollection corners = new Point3DCollection();
			corners.Add(new Point3D(0.5, 0.5, 0.5));
			corners.Add(new Point3D(-0.5, 0.5, 0.5));
			corners.Add(new Point3D(-0.5, -0.5, 0.5));
			corners.Add(new Point3D(0.5, -0.5, 0.5));
			corners.Add(new Point3D(0.5, 0.5, -0.5));
			corners.Add(new Point3D(-0.5, 0.5, -0.5));
			corners.Add(new Point3D(-0.5, -0.5, -0.5));
			corners.Add(new Point3D(0.5, -0.5, -0.5));
			cube.Positions = corners;

			Int32[] indices ={
			                    //front
			                      0,1,2,
						 0,2,3,
			                   //back
			                      4,7,6,
						 4,6,5,
			                   //Right
			                      4,0,3,
						 4,3,7,
			                   //Left
			                      1,5,6,
						 1,6,2,
			                   //Top
			                      1,0,4,
						 1,4,5,
			                   //Bottom
			                      2,6,7,
						 2,7,3
					  };

			indices2 = indices;

			Int32Collection Triangles =
								  new Int32Collection();
			foreach (Int32 index in indices)
			{
				Triangles.Add(index);
			}
			cube.TriangleIndices = Triangles;

			for (int i = 0; i < cube.Positions.Count; i++)
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

			return cube;
		}

		public void CreateCube(ref Canvas canvas, Viewport3D viewport)
		{
			MeshGeometry3D cubeMesh = MakeCube();
			Cube1.Geometry = cubeMesh;
			Cube1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));

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
			i = 0;
			int j = 1;
			foreach (var item in myLines)
			{
				canvas.Children.Add(item);

				item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[indices2[i]]).X;
				item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[indices2[i]]).Y;

				item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[indices2[j]]).X;
				item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(viewport, cubeMesh.Positions[indices2[j]]).Y;

				if (j > myLines.Count - 2)
				{
					break;
				}

				i++;
				j++;
			}
		}
	}
}
