using System;
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
		Storyboard RotCube = new Storyboard();
		DoubleAnimation RotAngle = new DoubleAnimation();
		AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(7, 1, 3), 5);
		GeometryModel3D Cube2 = new GeometryModel3D();
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
		Cube3D cube3d = new Cube3D();
		Ball3D ball3d = new Ball3D();
		Wireframe wireframe = new Wireframe();
		MoveObject moveObject = new MoveObject();
		Vertex vertex = new Vertex();

		public int CurrentObjectSelected { get; set; }

		List<GeometryModel3D> modelsInScene = new List<GeometryModel3D>();
		
		public MainWindow()
		{
			InitializeComponent();
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

		private void Canvas1_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Point point = Mouse.GetPosition(Canvas1);

			if (e.ChangedButton == MouseButton.Left) // Left mousebutton pressed
			{
				if (constraints == Constraints.EditObject)
				{
					MeshGeometry3D mesh;
					mesh = (MeshGeometry3D)modelsInScene[CurrentObjectSelected].Geometry;
					int i = 0;
					foreach (var item in circles)
					{
						item.RenderTransform = new TranslateTransform
						{
							X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).X - 2.5,
							Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).Y - 2.5
						};

						if(vertex.VertexPressed((int)point.X, (int)point.Y,
							(int)circles[i].RenderTransform.Value.OffsetX, (int)circles[i].RenderTransform.Value.OffsetY))
						{
							vertex.DetectDotVertex(i, false, ref circles);
							vertex.SelectedVertex = i;
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
					moveObject.MoveTheObject(modelsInScene[CurrentObjectSelected], point, ref Canvas1, lastPosX, rotationCubeX);
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
					MeshGeometry3D modelMesh;
					modelMesh = (MeshGeometry3D)modelsInScene[CurrentObjectSelected].Geometry;
					
					int[] selected = new int[modelMesh.Positions.Count];
					int number = 0;
					//while (selected < modelMesh.Positions.Count)
					//{
						if (circles[vertex.SelectedVertex].Fill == System.Windows.Media.Brushes.Red)
						{
							if (comboBox.SelectedIndex == 0)
							{
								MoveVertex(modelMesh, vertex.SelectedVertex, true, false, false);
							}
							else if (comboBox.SelectedIndex == 1)
							{
							MoveVertex(modelMesh, vertex.SelectedVertex, false, true, false);
							}
							else if (comboBox.SelectedIndex == 2)
							{
							MoveVertex(modelMesh, vertex.SelectedVertex, false, false, true);
							}
						}
					//	selected++;
					//}
				}
				else if (constraints == Constraints.None)
				{

				}
			}
			else if (e.MiddleButton == MouseButtonState.Pressed) // Middle mousebutton Pressed
			{
				Camera1.LookDirection = new Vector3D(cameraX, cameraY + (point.Y * 0.002), cameraZ + (point.X * 0.002));
				
				if(modelsInScene.Count != 0)
				{
					foreach (var model in modelsInScene)
					{
						wireframe.DrawWireFrame(model, myViewport, ball3d.indices2, ball3d.myLines);
					}
				}

				// Update DOTS
				MeshGeometry3D mesh;
				mesh = (MeshGeometry3D)modelsInScene[CurrentObjectSelected].Geometry;
				int i = 0;
				foreach (var item in circles)
				{

					item.RenderTransform = new TranslateTransform
					{
						X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).X - 2.5,
						Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).Y - 2.5
					};
					i++;
				}
			}
		}

		// Moving selected Vertex(red)
		void MoveVertex(MeshGeometry3D geoModel, int vert, bool x, bool y, bool z)
		{
			Point point = Mouse.GetPosition(Canvas1);
			
			//foreach (var vert in vert)
			//{
				if (x == true)
				{
					double finalX = geoModel.Positions[vert].Y + (point.Y - lastPosDotX);

					geoModel.Positions[vert] = new Point3D
					(finalX * 0.01,
					geoModel.Positions[vert].Y,
					geoModel.Positions[vert].Z);
					lastPosDotX = geoModel.Positions[vert].X;
				}
				else if (y == true)
				{
					double finalY = geoModel.Positions[vert].Y + (point.Y - lastPosDotY);

					geoModel.Positions[vert] = new Point3D
					(geoModel.Positions[vert].X,
					finalY * 0.01,
					geoModel.Positions[vert].Z);
					lastPosDotY = geoModel.Positions[vert].Y;
				}
				else if (z == true)
				{
					double finalZ = geoModel.Positions[vert].Y + (point.Y - lastPosDotZ);

					geoModel.Positions[vert] = new Point3D
					(geoModel.Positions[vert].X,
					geoModel.Positions[vert].Y,
					finalZ * 0.01);
					lastPosDotZ = geoModel.Positions[vert].Z;
				}
			//}

			//RefreshFrame(System.Windows.Media.Brushes.Transparent);

			if (modelsInScene.Count != 0)
			{
				foreach (var model in modelsInScene)
				{
					wireframe.DrawWireFrame(model, myViewport, ball3d.indices2, ball3d.myLines);
				}
			}

			//foreach (var vertic in geoModel.Positions)
			//{
			if (constraints == Constraints.EditObject)
				{
				// Update Dots
				//foreach (var item in circles)
				//{
				//	item.RenderTransform = new TranslateTransform
				//	{
				//		X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, geoModel.Positions[item]).X - 2.5,
				//		Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, geoModel.Positions[item]).Y - 2.5
				//	};
				//}
				MeshGeometry3D mesh;
				mesh = (MeshGeometry3D)modelsInScene[CurrentObjectSelected].Geometry;
				int i = 0;
				foreach (var item in circles)
				{

					item.RenderTransform = new TranslateTransform
					{
						X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).X - 2.5,
						Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).Y - 2.5
					};
					i++;
				}
			}
			//}
		}

		private void Canvas1_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Released)
			{
				
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

			if (modelsInScene.Count != 0)
			{
				foreach (var model in modelsInScene)
				{
					wireframe.DrawWireFrame(model, myViewport, ball3d.indices2, ball3d.myLines);
				}
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Left)
			{
				
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

			if (modelsInScene.Count != 0)
			{
				foreach (var model in modelsInScene)
				{
					wireframe.DrawWireFrame(model, myViewport, ball3d.indices2, ball3d.myLines);
				}

				if (constraints == Constraints.EditObject)
				{
					// Update DOTS
					MeshGeometry3D mesh;
					mesh = (MeshGeometry3D)modelsInScene[CurrentObjectSelected].Geometry;
					int i = 0;
					foreach (var item in circles)
					{

						item.RenderTransform = new TranslateTransform
						{
							X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).X - 2.5,
							Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).Y - 2.5
						};
						i++;
					}
				}
			}
		}

		private void SaveModelButton_Click(object sender, RoutedEventArgs e)
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
					myStream.Close();
					ObjCreation obj = new ObjCreation();
					obj.CreateObjFile(saveFileDialog.FileName, modelsInScene[CurrentObjectSelected], modelsInScene[CurrentObjectSelected].ToString());
				}
			}
		}

		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			if (true)
			{
				modelGroup.Children.Add(cube3d.Cube1);
				cube3d.CreateCube(ref Canvas1, myViewport);
				wireframe.CreateModelWire(cube3d.Cube1, myViewport, ref Canvas1, cube3d.indices2, cube3d.myLines);
				modelsInScene.Add(cube3d.Cube1);
				circles = cube3d.circles;
				CurrentObjectSelected = modelsInScene.Count - 1;
			}
			else
			{

			}

			LoadButton.Visibility = Visibility.Hidden;
		}

		private void LinesButton_Click(object sender, RoutedEventArgs e)
		{
			line3d.OnOrOff = true;
		}

		private void BallButton_Click(object sender, RoutedEventArgs e)
		{
			// a sphere will be created here

			modelGroup.Children.Add(ball3d.Ball1);
			ball3d.CreateCube(ref Canvas1, myViewport);
			wireframe.CreateModelWire(ball3d.Ball1, myViewport, ref Canvas1, ball3d.indices2, ball3d.myLines);
			modelsInScene.Add(ball3d.Ball1);
			circles = ball3d.circles;
			CurrentObjectSelected = modelsInScene.Count - 1;
		}

		private void SubDivButton_Click(object sender, RoutedEventArgs e)
		{
			// Sub Divide current model
		}

		// W.I.P.
		void ScaleObject()
		{
			ScaleTransform3D scale = new ScaleTransform3D();
			Transform3DGroup myTransform3DGroup = new Transform3DGroup();

			scale.CenterX = 5;
			myTransform3DGroup.Children.Add(scale);

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
				new TranslateTransform3D(Cube2.Transform.Value.OffsetX,
										Cube2.Transform.Value.OffsetY,
										Cube2.Transform.Value.OffsetZ);

			// Add the rotation transform to a Transform3DGroup
			Transform3DGroup myTransform3DGroup = new Transform3DGroup();
			myTransform3DGroup.Children.Add(myRotateTransform3D);

			// Adding current position (so it doesn't reset)
			myTransform3DGroup.Children.Add(newPosition);
			Cube2.Transform = myTransform3DGroup;

			//vertexX = myViewport.ActualWidth * (1 - vertexX);
			//myLine.X2 = vertexX;

			lastPosX = point.X;

			MeshGeometry3D cubeMesh;
			cubeMesh = (MeshGeometry3D)Cube2.Geometry;

			Debug.WriteLine(Cube2.Transform.Value.OffsetX);
		}

		// Moving the WHOLE object, (Only x Position ATM)
		void MoveObject()
		{
			TranslateTransform3D newPosition;
			Point point = Mouse.GetPosition(Canvas1);
			newPosition = new TranslateTransform3D(Cube2.Transform.Value.OffsetX + ((point.X - lastPosX) * 0.0001), 0, 0);
			
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

			Cube2.Transform = myTransform3DGroup;
		}

		

		// Just for fun/test
		void RotateCubeAnimation()
		{
			axis = new AxisAngleRotation3D(new Vector3D(1, 0, 1), 0);
			Cube2.Transform = new RotateTransform3D(axis);
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
