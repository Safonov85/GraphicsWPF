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
//using HelixToolkit.Wpf;


namespace WPF3Dgraphics
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
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
			Scale
		}
		Storyboard RotCube = new Storyboard();
		DoubleAnimation RotAngle = new DoubleAnimation();
		AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(7, 1, 3), 5);
		GeometryModel3D Cube1 = new GeometryModel3D();
		Viewport3D myViewport = new Viewport3D();
		Model3DGroup modelGroup = new Model3DGroup();
		RotateTransform3D Rotate;
		TranslateTransform3D Position;
		List<Line> myLines = new List<Line>();
		List<TextBlock> textBlocks = new List<TextBlock>();
		Angle angle = Angle.AngleX;
		Constraints constraints = Constraints.Position;
		Int32[] indices2;
		double rotationCubeX, rotationCubeY, rotationCubeZ = 0;
		double lastPosX = 0;
		double cameraX, cameraY, cameraZ;
		double vertexX, vertexY;
		PerspectiveCamera Camera1 = new PerspectiveCamera();

		// The camera's current location.
		private double CameraPhi = Math.PI / 6.0;       // 30 degrees
		private double CameraTheta = Math.PI / 6.0;     // 30 degrees

		private double CameraR = 13.0;

		//ModelImporter import = new ModelImporter();

		public MainWindow()
		{
			InitializeComponent();
		}

		MeshGeometry3D MCube()
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
				TextBlock text = new TextBlock();
				text.Text = i.ToString();
				text.HorizontalAlignment = HorizontalAlignment.Center;
				text.Background = Brushes.White;

				textBlocks.Add(text);
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

			//DirectionalLight DirLight1 = new DirectionalLight();
			//DirLight1.Color = Colors.White;
			//DirLight1.Direction = new Vector3D(-1, -1, -1);

			//SpotLight DirLight1 = new SpotLight();
			//DirLight1.Color = Colors.White;
			//DirLight1.Direction = new Vector3D(0, 0, 0);
			//DirLight1.InnerConeAngle = 0;
			//DirLight1.OuterConeAngle = 50;
			//DirLight1.Position = new Point3D(5, -5, 5);

			PointLight DirLight1 = new PointLight();
			DirLight1.Color = Colors.White;
			DirLight1.Position = new Point3D(5, 5, 5);

			PointLight DirLight2 = new PointLight();
			DirLight2.Color = Colors.White;
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
			

			//text.Text = "5";
			//text.HorizontalAlignment = HorizontalAlignment.Center;
			//text.Background = Brushes.White;
			//Canvas1.Children.Add(text);
			
			// Anti-Aliasing OFF!!!!!!!!!! FOR 3D 
			RenderOptions.SetEdgeMode((DependencyObject)myViewport, EdgeMode.Aliased);

			// Anti-Aliasing OFF!!!!!!!!!! FOR 2D 
			RenderOptions.SetEdgeMode(Canvas1, EdgeMode.Aliased);

			MoveButton.Background = Brushes.Coral;
		}

		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			CreateCube();
		}

		void CreateCube()
		{
			MeshGeometry3D cubeMesh = MCube();
			Cube1.Geometry = cubeMesh;
			Cube1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));

			int i = 0;
			foreach (var item in textBlocks)
			{
				Canvas1.Children.Add(item);

				item.RenderTransform = new TranslateTransform
				{
					X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).X,
					Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[i]).Y
				};

				i++;
			}

			i = 0;
			int j = 1;
			foreach (var item in myLines)
			{
				Canvas1.Children.Add(item);

				item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[i]]).X;
				item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[i]]).Y;

				item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[j]]).X;
				item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[indices2[j]]).Y;

				if(j > myLines.Count -2)
				{
					break;
				}

				i++;
				j++;
			}

			//Thread.Sleep(1000);
			//modelGroup.Children.RemoveAt(0);
		}

		private void LoadButton2_Click(object sender, RoutedEventArgs e)
		{
			MoveObject();
		}

		private void LoadButton3_Click(object sender, RoutedEventArgs e)
		{
			//MoveObject();
			MoveVertex();
		}

		private void Canvas1_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if(e.ChangedButton == MouseButton.Left) // Left mousebutton pressed
			{
				Cube1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
			}
			else if (e.ChangedButton == MouseButton.Middle) // Middle mousebutton pressed
			{

			}
		}

		private void Canvas1_MouseMove(object sender, MouseEventArgs e)
		{
			if(e.LeftButton == MouseButtonState.Pressed)
			{
				if(constraints == Constraints.Position)
				{
					MoveObject();
				}
				else if(constraints == Constraints.Rotation)
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
				else if(constraints == Constraints.Scale)
				{
					ScaleObject();
				}
			}
			else if(e.MiddleButton == MouseButtonState.Pressed) // Middle mousebutton Pressed
			{
				Point point = Mouse.GetPosition(Canvas1);
				Camera1.LookDirection = new Vector3D(cameraX, cameraY + (point.Y * 0.002), cameraZ + (point.X * 0.002));
				//Camera1.UpDirection = new Vector3D(Camera1.UpDirection.X, Camera1.UpDirection.Y + 1, Camera1.UpDirection.Z +4);
				
				MeshGeometry3D cubeMesh;
				cubeMesh = (MeshGeometry3D)Cube1.Geometry;

				Debug.WriteLine(Camera1.UpDirection.Z);

				//text.RenderTransform = new TranslateTransform
				//{
				//	X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[1]).X,
				//	Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[1]).Y
				//};


				//myLine.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[1]).X;
				//myLine.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[1]).Y;

				//myLine.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[0]).X;
				//myLine.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[0]).Y;

				//myLine2.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[0]).X;
				//myLine2.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[0]).Y;

				//myLine2.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[3]).X;
				//myLine2.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, cubeMesh.Positions[3]).Y;

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
			constraints = Constraints.Position;
		}

		private void RotateButton_Click(object sender, RoutedEventArgs e)
		{
			MoveButton.Background = Brushes.Gray;
			RotateButton.Background = Brushes.Coral;
			ScaleButton.Background = Brushes.Gray;
			constraints = Constraints.Rotation;
		}

		private void ScaleButton_Click(object sender, RoutedEventArgs e)
		{
			MoveButton.Background = Brushes.Gray;
			RotateButton.Background = Brushes.Gray;
			ScaleButton.Background = Brushes.Coral;
			constraints = Constraints.Scale;
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Left)
			{
				Camera1.UpDirection = new Vector3D(4, 6, 7);

				//// Calculate the camera's position in Cartesian coordinates.
				//double y = CameraR * Math.Sin(CameraPhi);
				//double hyp = CameraR * Math.Cos(CameraPhi);
				//double x = hyp * Math.Cos(CameraTheta);
				//double z = hyp * Math.Sin(CameraTheta);
				//Camera1.Position = new Point3D(x, y, z);

				//// Look toward the origin.
				//Camera1.LookDirection = new Vector3D(-x, -y, -z);

				//// Set the Up direction.
				//Camera1.UpDirection = new Vector3D(0, 0, 0);
			}
		}

		void ScaleObject()
		{
			ScaleTransform3D scale = new ScaleTransform3D();
			Transform3DGroup myTransform3DGroup = new Transform3DGroup();

			scale.CenterX = 5;
			myTransform3DGroup.Children.Add(scale);
			Cube1.Transform = myTransform3DGroup;

		}

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

		// Moving the WHOLE object
		void MoveObject()
		{
			TranslateTransform3D newPosition;
			Point point = Mouse.GetPosition(Canvas1);

			
			newPosition = new TranslateTransform3D(Cube1.Transform.Value.OffsetX + ((point.X - lastPosX) * 0.0001), 0, 0);
			

			//if (direction == "+")
			//{
			//	newPosition = new TranslateTransform3D(Cube1.Transform.Value.OffsetX + 0.1, 0, 0);
			//}
			//else
			//{
			//	newPosition = new TranslateTransform3D(Cube1.Transform.Value.OffsetX - 0.1, 0, 0);
			//}
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

		void MoveVertex()
		{
			MeshGeometry3D cubeMesh;
			cubeMesh = (MeshGeometry3D)Cube1.Geometry;
			int i = 5;

			cubeMesh.Positions[i] = new Point3D(cubeMesh.Positions[i].X, cubeMesh.Positions[i].Y + 0.1, cubeMesh.Positions[i].Z);

			// kinda unnecessary
			Cube1.Geometry = cubeMesh;

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
