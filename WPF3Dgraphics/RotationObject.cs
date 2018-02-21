using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace WPF3Dgraphics
{
	public class RotationObject
	{
		// Rotating WHOLE object
		public void RotateObject(Angle angle, Canvas canvas, GeometryModel3D geoModel, double rotationCubeX, double lastPosX)
		{
			RotateTransform3D myRotateTransform3D = new RotateTransform3D();
			AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
			myAxisAngleRotation3d.Axis = new Vector3D(1, 0, 0);
			Point point = Mouse.GetPosition(canvas);

			if (angle == Angle.AngleX)
			{
				myAxisAngleRotation3d.Angle = rotationCubeX + (point.X - lastPosX);
				rotationCubeX = myAxisAngleRotation3d.Angle;
			}
			myRotateTransform3D.Rotation = myAxisAngleRotation3d;

			TranslateTransform3D newPosition =
				new TranslateTransform3D(geoModel.Transform.Value.OffsetX,
										geoModel.Transform.Value.OffsetY,
										geoModel.Transform.Value.OffsetZ);

			// Add the rotation transform to a Transform3DGroup
			Transform3DGroup myTransform3DGroup = new Transform3DGroup();
			myTransform3DGroup.Children.Add(myRotateTransform3D);

			// Adding current position (so it doesn't reset)
			myTransform3DGroup.Children.Add(newPosition);
			geoModel.Transform = myTransform3DGroup;

			//vertexX = myViewport.ActualWidth * (1 - vertexX);
			//myLine.X2 = vertexX;

			lastPosX = point.X;

			MeshGeometry3D cubeMesh;
			cubeMesh = (MeshGeometry3D)geoModel.Geometry;
		}

		// Just for fun/test
		//void RotateCubeAnimation()
		//{
		//	axis = new AxisAngleRotation3D(new Vector3D(1, 0, 1), 0);
		//	Cube2.Transform = new RotateTransform3D(axis);
		//	RotAngle.From = 0;
		//	RotAngle.To = 360;
		//	RotAngle.Duration = new Duration(TimeSpan.FromSeconds(1.0));
		//	RotAngle.RepeatBehavior = RepeatBehavior.Forever;
		//	NameScope.SetNameScope(Canvas1, new NameScope());
		//	Canvas1.RegisterName("cubeaxis", axis);
		//	Storyboard.SetTargetName(RotAngle, "cubeaxis");
		//	Storyboard.SetTargetProperty(RotAngle, new PropertyPath(AxisAngleRotation3D.AngleProperty));
		//	RotCube.Children.Add(RotAngle);
		//	RotCube.Begin(Canvas1);
		//}
	}
}
