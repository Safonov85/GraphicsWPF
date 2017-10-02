using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using HelixToolkit.Wpf;


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
		Storyboard RotCube = new Storyboard();
		DoubleAnimation RotAngle = new DoubleAnimation();
		AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(7, 1, 3), 5);
		GeometryModel3D Cube1 = new GeometryModel3D();
		RotateTransform3D Rotate;
		TranslateTransform3D Position;
		Line myLine;
		Angle angle = Angle.AngleX;
		double rotationCubeX = 0;
		double lastPosX = 0;


		ModelImporter import = new ModelImporter();

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

			Int32Collection Triangles =
								  new Int32Collection();
			foreach (Int32 index in indices)
			{
				Triangles.Add(index);
			}
			cube.TriangleIndices = Triangles;


			return cube;
		}

		private void Window_Loaded(object sender,
								  RoutedEventArgs e)
		{
			Rotate = new RotateTransform3D(axis);

			DirectionalLight DirLight1 = new DirectionalLight();
			DirLight1.Color = Colors.White;
			DirLight1.Direction = new Vector3D(-1, -1, -1);

			PerspectiveCamera Camera1 = new PerspectiveCamera();
			Camera1.FarPlaneDistance = 30;
			Camera1.NearPlaneDistance = 1;
			Camera1.FieldOfView = 45;
			Camera1.Position = new Point3D(2, 2, 3);
			Camera1.LookDirection = new Vector3D(-2, -2, -3);
			Camera1.UpDirection = new Vector3D(0, 1, 0);

			Model3DGroup modelGroup = new Model3DGroup();
			modelGroup.Children.Add(Cube1);
			modelGroup.Children.Add(DirLight1);
			ModelVisual3D modelsVisual = new ModelVisual3D();
			modelsVisual.Content = modelGroup;

			Viewport3D myViewport = new Viewport3D();
			myViewport.Camera = Camera1;
			myViewport.Children.Add(modelsVisual);
			this.Canvas1.Children.Add(myViewport);
			myViewport.Height = 500;
			myViewport.Width = 500;
			
			
			myLine = new Line();
			myLine.Stroke = System.Windows.Media.Brushes.Black;
			myLine.X1 = 1;
			myLine.Y1 = 1;
			myLine.X2 = 500;
			myLine.Y2 = 3;
			myLine.HorizontalAlignment = HorizontalAlignment.Left;
			myLine.VerticalAlignment = VerticalAlignment.Center;
			myLine.StrokeThickness = 3;
			
			Canvas1.Children.Add(myLine);
			
			// Anti-Aliasing OFF!!!!!!!!!!
			RenderOptions.SetEdgeMode((DependencyObject)myViewport, EdgeMode.Aliased);
		}

		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			//LoadMyModel();
			CreateCube();
		}

		void LoadMyModel()
		{
			//ModelVisual3D device3D = new ModelVisual3D();
			
			//Model3DGroup group = import.Load("C:\\3D stuff\\Hospital Game\\OBJ\\Markus_01.obj");

			//Model3D device = group;

			//device3D.Content = device;

			//viewPort3d.Children.Add(device3D);
		}

		void CreateCube()
		{
			MeshGeometry3D cubeMesh = MCube();
			//cubeMesh.Normals = 0;
			Cube1.Geometry = cubeMesh;
			Cube1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));

			
			
			//Canvas.SetTop(myViewport, 0);
			//Canvas.SetLeft(myViewport, 0);
			//this.Width = myViewport.Width;
			//this.Height = myViewport.Height;

			//Cube1.Transform = Rotate;

			//RotAngle.From = 0;
			//RotAngle.To = 360;
			//RotAngle.Duration = new Duration(TimeSpan.FromSeconds(5.0));
			//RotAngle.RepeatBehavior = RepeatBehavior.Forever;
			//NameScope.SetNameScope(Canvas1, new NameScope());
			//Canvas1.RegisterName("cubeaxis", axis);
			//Storyboard.SetTargetName(RotAngle, "cubeaxis");
			//Storyboard.SetTargetProperty(RotAngle, new PropertyPath(AxisAngleRotation3D.AngleProperty));
			//RotCube.Children.Add(RotAngle);
			//RotCube.Begin(Canvas1);
		}

		private void LoadButton2_Click(object sender, RoutedEventArgs e)
		{
			//RotateCubeAnimation();
			MoveObject();
		}

		private void Canvas1_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//MoveObject();
			//MoveVertex();
			if(e.ChangedButton == 0) // Left mousebutton pressed
			{

			}
		}

		private void Canvas1_MouseMove(object sender, MouseEventArgs e)
		{
			if(e.LeftButton == MouseButtonState.Pressed)
			{
				
				
				//System.Diagnostics.Debug.WriteLine(point.X.ToString());

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
		}

		void RotateObject(Angle angle)
		{
			RotateTransform3D myRotateTransform3D = new RotateTransform3D();
			AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
			myAxisAngleRotation3d.Axis = new Vector3D(200, 0, 0);
			Point point = Mouse.GetPosition(Canvas1);

			if (angle == Angle.AngleX)
			{
				myAxisAngleRotation3d.Angle = rotationCubeX + (point.X - lastPosX);
				rotationCubeX = myAxisAngleRotation3d.Angle;
			}
			myRotateTransform3D.Rotation = myAxisAngleRotation3d;

			// Add the rotation transform to a Transform3DGroup
			Transform3DGroup myTransform3DGroup = new Transform3DGroup();
			myTransform3DGroup.Children.Add(myRotateTransform3D);
			Cube1.Transform = myTransform3DGroup;
			//RotateTransform3D myRotateTransform = new RotateTransform3D();
			//RotateTransform3D myRotateTransform = new RotateTransform3D(new AxisAngleRotation3D(
			//	new Vector3D(0, 0, 1), 30));
			//TranslateTransform3D newPosition = new TranslateTransform3D(Cube1.Transform.Value.OffsetX + 0.1, 0, 0);
			//myRotateTransform.CenterZ = 20;
			//Cube1.Transform.Transform(new Point3D(3, 5, 2));
			//Cube1.Transform = newPosition;
			//Cube1.Transform = myRotateTransform;
			//Cube1.Transform.Value.Rotate(new Quaternion(3, 4, 5, 5));

			//System.Diagnostics.Debug.WriteLine(myRotateTransform3D.CenterX.ToString());
			//System.Diagnostics.Debug.WriteLine(myRotateTransform.CenterY.ToString());
			//System.Diagnostics.Debug.WriteLine(myRotateTransform.CenterZ.ToString());
			//Cube1.Transform.Value.RotateAt(new Quaternion(3, 0, 0, 2), new Point3D(0, 3, 0));

			lastPosX = point.X;
		}

		// Moving the WHOLE object
		void MoveObject()
		{
			TranslateTransform3D newPosition = new TranslateTransform3D(Cube1.Transform.Value.OffsetX + 0.1, 0, 0);

			Cube1.Transform = newPosition;
		}

		void MoveVertex()
		{
			MeshGeometry3D cubeMesh;
			cubeMesh = (MeshGeometry3D)Cube1.Geometry;
			int i = 5;

			cubeMesh.Positions[i] = new Point3D(cubeMesh.Positions[i].X, cubeMesh.Positions[i].Y + 0.1, cubeMesh.Positions[i].Z);

			// kinda unnecessary
			Cube1.Geometry = cubeMesh;
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
