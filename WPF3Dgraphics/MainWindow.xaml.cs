﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using Petzold.Media3D;
using System.IO;
using Microsoft.Win32;

namespace WPF3Dgraphics
{
	public partial class MainWindow : Window
	{
		enum Angle
		{
			AngleX,
			AngleY,
			AngleZ
		}

		enum Constraints
		{
			Position,
			Rotation,
			Scale,
			EditObject,
			None
		}
		Storyboard RotCube = new Storyboard();
		DoubleAnimation RotAngle = new DoubleAnimation();
		AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(7, 1, 3), 5);
		GeometryModel3D Cube1 = new GeometryModel3D();
		Viewport3D myViewport = new Viewport3D();
		Model3DGroup modelGroup = new Model3DGroup();
		RotateTransform3D Rotate;
		TranslateTransform3D Position;
		List<Ellipse> circles = new List<Ellipse>();
		List<Line> myLines = new List<Line>();
		List<TextBlock> textBlocks = new List<TextBlock>();
		Angle angle = Angle.AngleX;
		Constraints constraints = Constraints.Position;
		Int32[] indices2;
		List<string> faces = new List<string>();
		double rotationCubeX, rotationCubeY, rotationCubeZ = 0;
		double lastPosX = 0;
		double lastPosDotY = 0;
		double lastPosDotX = 0;
		double lastPosDotZ = 0;
		double cameraX, cameraY, cameraZ;
		double vertexX, vertexY;
		PerspectiveCamera Camera1 = new PerspectiveCamera();
		Line3D line3d = new Line3D();

		//ModelImporter import = new ModelImporter();

		public MainWindow()
		{
			InitializeComponent();
		}

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

		private void Window_Loaded(object sender,
								  RoutedEventArgs e)
		{
			Rotate = new RotateTransform3D(axis);

			PointLight DirLight1 = new PointLight();
			DirLight1.Color = Colors.White;
			DirLight1.Position = new Point3D(5, 5, 5);

			PointLight DirLight2 = new PointLight();
			DirLight2.Color = Colors.Gray;
			DirLight2.Position = new Point3D(5, -5, 5);


			cameraX = -2;
			cameraY = -2;
			cameraZ = -3;
			Camera1.FarPlaneDistance = 30;
			Camera1.NearPlaneDistance = 1;
			Camera1.FieldOfView = 45;
			Camera1.Position = new Point3D(2, 2, 3);
			Camera1.LookDirection = new Vector3D(cameraX, cameraY, cameraZ);
			Camera1.UpDirection = new Vector3D(0, 1, 0);


			modelGroup.Children.Add(Cube1);
			modelGroup.Children.Add(DirLight1);
			modelGroup.Children.Add(DirLight2);

			ModelVisual3D modelsVisual = new ModelVisual3D();
			modelsVisual.Content = modelGroup;


			myViewport.Camera = Camera1;
			myViewport.Children.Add(modelsVisual);
			this.Canvas1.Children.Add(myViewport);
			myViewport.Height = 500;
			myViewport.Width = 500;

			// Anti-Aliasing OFF!!!!!!!!!! FOR 3D 
			RenderOptions.SetEdgeMode((DependencyObject)myViewport, EdgeMode.Aliased);

			// Anti-Aliasing OFF!!!!!!!!!! FOR 2D 
			RenderOptions.SetEdgeMode(Canvas1, EdgeMode.Aliased);

			MoveButton.Background = Brushes.Coral;
		}

		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			CreateCube();
			LoadButton.Visibility = Visibility.Hidden;
		}

		// Only needs to be used once
		void CreateCube()
		{
			MeshGeometry3D cubeMesh = MakeCube();
			Cube1.Geometry = cubeMesh;
			Cube1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));

			int i = 0;
			foreach (var item in textBlocks)
			{
				Canvas1.Children.Add(item);

				item.RenderTransform = new TranslateTransform
				{
					X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).X + 10,
					Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).Y + 10
				};

				i++;
			}


			// DOTS
			i = 0;
			foreach (var item in circles)
			{
				Canvas1.Children.Add(item);

				item.RenderTransform = new TranslateTransform
				{
					X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).X - 2.5,
					Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).Y - 2.5
				};

				i++;
			}

			if (constraints == Constraints.EditObject)
			{
				int item = 0;
				while (item < circles.Count - 1)
				{
					circles[item].Stroke = System.Windows.Media.Brushes.Blue;
					circles[item].Fill = System.Windows.Media.Brushes.Blue;
					item++;
				}
			}

			// WIREFRAME
			i = 0;
			int j = 1;
			foreach (var item in myLines)
			{
				Canvas1.Children.Add(item);

				item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[i]]).X;
				item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[i]]).Y;

				item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[j]]).X;
				item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[j]]).Y;

				if (j > myLines.Count - 2)
				{
					break;
				}

				i++;
				j++;
			}
		}

		private void Canvas1_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Point point = Mouse.GetPosition(Canvas1);

			if (e.ChangedButton == MouseButton.Left) // Left mousebutton pressed
			{
				if (constraints == Constraints.EditObject)
				{
					int i = 0;
					foreach (var item in circles)
					{

						MeshGeometry3D cubeMesh;
						cubeMesh = (MeshGeometry3D)Cube1.Geometry;
						item.RenderTransform = new TranslateTransform
						{
							X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).X - 2.5,
							Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).Y - 2.5

							//DetectDotVertex(point.X, point.Y,
						};

						if (VertexPressed((int)point.X, (int)point.Y,
							(int)circles[i].RenderTransform.Value.OffsetX, (int)circles[i].RenderTransform.Value.OffsetY))
						{
							DetectDotVertex(i, false); // bool if CTRL is pressed or not
							break;
						}
						i++;
					}
				}

				if(line3d.OnOrOff == true)
				{
					double x = 0, y = 0, z = 1;
					LineRange range;
					Petzold.Media3D.ViewportInfo.Point2DtoPoint3D(myViewport, point, out range);
					line3d.AddNewPoint(x, y, z);
				} 

			}
			else if (e.ChangedButton == MouseButton.Middle) // Middle mousebutton pressed
			{

			}
			else if(e.ChangedButton == MouseButton.Right)
			{
				if(line3d.OnOrOff == true)
				{
					line3d.OnOrOff = false;
				}
			}
		}

		private void Canvas1_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = Mouse.GetPosition(Canvas1);
			LabelCanvasX.Content = "X: " + point.X;
			LabelCanvasY.Content = "Y: " + point.Y;

			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (constraints == Constraints.Position)
				{
					MoveObject();
				}
				else if (constraints == Constraints.Rotation)
				{
					// X angle
					if (angle == Angle.AngleX)
					{
						RotateObject(Angle.AngleX);
					}
					// Y angle
					else if (angle == Angle.AngleY)
					{
						RotateObject(Angle.AngleY);
					}
					// Z angle
					else if (angle == Angle.AngleZ)
					{
						RotateObject(Angle.AngleZ);
					}
				}
				else if (constraints == Constraints.Scale)
				{
					ScaleObject();
				}
				else if (constraints == Constraints.EditObject)
				{
					MeshGeometry3D cubeMesh;
					cubeMesh = (MeshGeometry3D)Cube1.Geometry;
					int selected = 0;
					while (selected < cubeMesh.Positions.Count)
					{
						if (circles[selected].Fill == System.Windows.Media.Brushes.Red)
						{
							if (comboBox.SelectedIndex == 0)
							{
								MoveVertex(selected, true, false, false);
							}
							else if (comboBox.SelectedIndex == 1)
							{
								MoveVertex(selected, false, true, false);
							}
							else if (comboBox.SelectedIndex == 2)
							{
								MoveVertex(selected, false, false, true);
							}
						}
						selected++;
					}
				}
				else if (constraints == Constraints.None)
				{

				}
			}
			else if (e.MiddleButton == MouseButtonState.Pressed) // Middle mousebutton Pressed
			{
				Camera1.LookDirection = new Vector3D(cameraX, cameraY + (point.Y * 0.002), cameraZ + (point.X * 0.002));
				//Camera1.UpDirection = new Vector3D(Camera1.UpDirection.X, Camera1.UpDirection.Y + 1, Camera1.UpDirection.Z +4);

				DrawWireFrame();
			}
		}

		void DrawWireFrame()
		{
			MeshGeometry3D cubeMesh;
			cubeMesh = (MeshGeometry3D)Cube1.Geometry;

			int i = 0;
			foreach (var item in textBlocks)
			{
				item.RenderTransform = new TranslateTransform
				{
					X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).X,
					Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).Y
				};

				i++;
			}

			if (constraints == Constraints.EditObject)
			{
				// Vertex DOTS
				i = 0;
				foreach (var item in circles)
				{
					item.RenderTransform = new TranslateTransform
					{
						X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).X - 2.5,
						Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).Y - 2.5
					};

					i++;
				}
			}

			// WIREFRAME
			i = 0;
			int j = 1;
			foreach (var item in myLines)
			{
				item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[i]]).X;
				item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[i]]).Y;

				item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[j]]).X;
				item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[j]]).Y;

				if (j > myLines.Count - 2)
				{
					break;
				}

				i++;
				j++;
			}



		}

		void DetectDotVertex(int item, bool ctrlPressed)
		{
			// Clear all reds, MAKE BLUE
			if (ctrlPressed == false)
			{
				int i = 0;
				while (i < circles.Count - 1)
				{
					circles[i].Stroke = System.Windows.Media.Brushes.Blue;
					circles[i].Fill = System.Windows.Media.Brushes.Blue;
					i++;
				}
			}
			if (circles[item].Fill != System.Windows.Media.Brushes.Red)
			{
				circles[item].Stroke = System.Windows.Media.Brushes.Red;
				circles[item].Fill = System.Windows.Media.Brushes.Red;
			}
			else
			{
				circles[item].Stroke = System.Windows.Media.Brushes.Blue;
				circles[item].Fill = System.Windows.Media.Brushes.Blue;
			}

		}

		// Detecteing if the blue vertex is pressed outside it or inside it
		bool VertexPressed(int xMousePos, int yMousePos, int xVert, int yVert)
		{
			if (xMousePos > (xVert - 5) && xMousePos < (xVert + 5)
			&& yMousePos > (yVert - 5) && yMousePos < (yVert + 5))
			{
				return true;
			}

			return false;
		}

		private void Canvas1_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Released)
			{
				Cube1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));
			}
		}

		private void MoveButton_Click(object sender, RoutedEventArgs e)
		{
			MoveButton.Background = Brushes.Coral;
			RotateButton.Background = Brushes.Gray;
			ScaleButton.Background = Brushes.Gray;
			EditObjectButton.Background = Brushes.Gray;
			NoneButton.Background = Brushes.Gray;
			constraints = Constraints.Position;

			RefreshFrame(System.Windows.Media.Brushes.Transparent);
		}

		private void RotateButton_Click(object sender, RoutedEventArgs e)
		{
			MoveButton.Background = Brushes.Gray;
			RotateButton.Background = Brushes.Coral;
			ScaleButton.Background = Brushes.Gray;
			EditObjectButton.Background = Brushes.Gray;
			NoneButton.Background = Brushes.Gray;
			constraints = Constraints.Rotation;

			RefreshFrame(System.Windows.Media.Brushes.Transparent);
		}

		private void ScaleButton_Click(object sender, RoutedEventArgs e)
		{
			MoveButton.Background = Brushes.Gray;
			RotateButton.Background = Brushes.Gray;
			ScaleButton.Background = Brushes.Coral;
			EditObjectButton.Background = Brushes.Gray;
			NoneButton.Background = Brushes.Gray;
			constraints = Constraints.Scale;

			RefreshFrame(System.Windows.Media.Brushes.Transparent);
		}

		private void EditObjectButton_Click(object sender, RoutedEventArgs e)
		{
			MoveButton.Background = Brushes.Gray;
			RotateButton.Background = Brushes.Gray;
			ScaleButton.Background = Brushes.Gray;
			EditObjectButton.Background = Brushes.Coral;
			NoneButton.Background = Brushes.Gray;
			constraints = Constraints.EditObject;

			RefreshFrame(System.Windows.Media.Brushes.Blue);
		}

		private void NoneButton_Click(object sender, RoutedEventArgs e)
		{
			MoveButton.Background = Brushes.Gray;
			RotateButton.Background = Brushes.Gray;
			ScaleButton.Background = Brushes.Gray;
			EditObjectButton.Background = Brushes.Gray;
			NoneButton.Background = Brushes.Coral;
			constraints = Constraints.None;

			RefreshFrame(System.Windows.Media.Brushes.Transparent);
		}

		// Refreshing the frame to make sure new condition is met
		void RefreshFrame(Brush brush)
		{
			int item = 0;
			while (item < circles.Count - 1)
			{
				circles[item].Stroke = brush;
				circles[item].Fill = brush;
				item++;
			}

			DrawWireFrame();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Left)
			{
				//Camera1.UpDirection = new Vector3D(4, 6, 7);
			}
		}

		private void Canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			// Delta acts like a bool (even though it's an int) 
			if (e.Delta == 120)
			{
				Camera1.FieldOfView -= 1;
			}
			else
			{
				Camera1.FieldOfView += 1;
			}

			DrawWireFrame();
		}

		private async void SaveModelButton_Click(object sender, RoutedEventArgs e)
		{
			Stream myStream;
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;

			if (saveFileDialog.ShowDialog() == true)
			{
				if ((myStream = saveFileDialog.OpenFile()) != null)
				{
					//WriteTextAsync("Testing write async method");
					myStream.Close();
					ObjCreation obj = new ObjCreation();
					obj.CreateObjFile(saveFileDialog.FileName, Cube1);
				}
			}
		}

		private void LinesButton_Click(object sender, RoutedEventArgs e)
		{
			line3d.OnOrOff = true;
		}

		// W.I.P.
		void ScaleObject()
		{
			ScaleTransform3D scale = new ScaleTransform3D();
			Transform3DGroup myTransform3DGroup = new Transform3DGroup();

			scale.CenterX = 5;
			myTransform3DGroup.Children.Add(scale);
			Cube1.Transform = myTransform3DGroup;

		}

		// Rotating WHOLE object
		void RotateObject(Angle angle)
		{
			RotateTransform3D myRotateTransform3D = new RotateTransform3D();
			AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
			myAxisAngleRotation3d.Axis = new Vector3D(1, 0, 0);
			Point point = Mouse.GetPosition(Canvas1);

			if (angle == Angle.AngleX)
			{
				myAxisAngleRotation3d.Angle = rotationCubeX + (point.X - lastPosX);
				rotationCubeX = myAxisAngleRotation3d.Angle;
			}
			myRotateTransform3D.Rotation = myAxisAngleRotation3d;

			TranslateTransform3D newPosition =
				new TranslateTransform3D(Cube1.Transform.Value.OffsetX,
										Cube1.Transform.Value.OffsetY,
										Cube1.Transform.Value.OffsetZ);

			// Add the rotation transform to a Transform3DGroup
			Transform3DGroup myTransform3DGroup = new Transform3DGroup();
			myTransform3DGroup.Children.Add(myRotateTransform3D);

			// Adding current position (so it doesn't reset)
			myTransform3DGroup.Children.Add(newPosition);
			Cube1.Transform = myTransform3DGroup;

			//vertexX = myViewport.ActualWidth * (1 - vertexX);
			//myLine.X2 = vertexX;

			lastPosX = point.X;

			MeshGeometry3D cubeMesh;
			cubeMesh = (MeshGeometry3D)Cube1.Geometry;

			Debug.WriteLine(Cube1.Transform.Value.OffsetX);
		}

		// Moving the WHOLE object, (Only x Position ATM)
		void MoveObject()
		{
			TranslateTransform3D newPosition;
			Point point = Mouse.GetPosition(Canvas1);
			newPosition = new TranslateTransform3D(Cube1.Transform.Value.OffsetX + ((point.X - lastPosX) * 0.0001), 0, 0);
			
			AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
			RotateTransform3D myRotateTransform3D = new RotateTransform3D();
			myAxisAngleRotation3d.Axis = new Vector3D(1, 0, 0);
			myAxisAngleRotation3d.Angle = rotationCubeX;
			myRotateTransform3D.Rotation = myAxisAngleRotation3d;

			Transform3DGroup myTransform3DGroup = new Transform3DGroup();

			// Adding current rotation
			myTransform3DGroup.Children.Add(myRotateTransform3D);

			// Adding current position
			myTransform3DGroup.Children.Add(newPosition);

			Cube1.Transform = myTransform3DGroup;
		}

		// Moving selected Vertex(red)
		void MoveVertex(int vert, bool x, bool y, bool z)
		{
			MeshGeometry3D cubeMesh;
			cubeMesh = (MeshGeometry3D)Cube1.Geometry;

			Point point = Mouse.GetPosition(Canvas1);



			if (x == true)
			{
				double finalX = cubeMesh.Positions[vert].Y + (point.Y - lastPosDotX);

				cubeMesh.Positions[vert] = new Point3D
				(finalX * 0.01,
				cubeMesh.Positions[vert].Y,
				cubeMesh.Positions[vert].Z);
				lastPosDotX = cubeMesh.Positions[vert].X;
			}
			else if (y == true)
			{
				double finalY = cubeMesh.Positions[vert].Y + (point.Y - lastPosDotY);

				cubeMesh.Positions[vert] = new Point3D
				(cubeMesh.Positions[vert].X,
				finalY * 0.01,
				cubeMesh.Positions[vert].Z);
				lastPosDotY = cubeMesh.Positions[vert].Y;
			}
			else if (z == true)
			{
				double finalZ = cubeMesh.Positions[vert].Y + (point.Y - lastPosDotZ);

				cubeMesh.Positions[vert] = new Point3D
				(cubeMesh.Positions[vert].X,
				cubeMesh.Positions[vert].Y,
				finalZ * 0.01);
				lastPosDotZ = cubeMesh.Positions[vert].Z;
			}


			// Which direction to move in
			//cubeMesh.Positions[vert] = new Point3D(cubeMesh.Positions[vert].X + x, cubeMesh.Positions[vert].Y + y, cubeMesh.Positions[vert].Z + z);

			// kinda unnecessary
			Cube1.Geometry = cubeMesh;

			if (constraints == Constraints.EditObject)
			{
				// Update Dots
				vert = 0;
				foreach (var item in circles)
				{
					item.RenderTransform = new TranslateTransform
					{
						X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[vert]).X - 2.5,
						Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[vert]).Y - 2.5
					};

					vert++;
				}
			}

			// Update Text Numbers
			int i = 0;
			foreach (var item in textBlocks)
			{
				item.RenderTransform = new TranslateTransform
				{
					X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).X,
					Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).Y
				};

				i++;
			}

			// Update Wireframe
			vert = 0;
			int j = 1;
			foreach (var item in myLines)
			{
				item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[vert]]).X;
				item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[vert]]).Y;

				item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[j]]).X;
				item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[j]]).Y;

				if (j > myLines.Count - 2)
				{
					break;
				}

				vert++;
				j++;
			}
		}

		// Just for fun/test
		void RotateCubeAnimation()
		{
			axis = new AxisAngleRotation3D(new Vector3D(1, 0, 1), 0);
			Cube1.Transform = new RotateTransform3D(axis);
			RotAngle.From = 0;
			RotAngle.To = 360;
			RotAngle.Duration = new Duration(TimeSpan.FromSeconds(1.0));
			RotAngle.RepeatBehavior = RepeatBehavior.Forever;
			NameScope.SetNameScope(Canvas1, new NameScope());
			Canvas1.RegisterName("cubeaxis", axis);
			Storyboard.SetTargetName(RotAngle, "cubeaxis");
			Storyboard.SetTargetProperty(RotAngle, new PropertyPath(AxisAngleRotation3D.AngleProperty));
			RotCube.Children.Add(RotAngle);
			RotCube.Begin(Canvas1);
		}
	}
}
