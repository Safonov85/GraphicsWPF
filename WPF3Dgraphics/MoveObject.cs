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
	public class MoveObject
	{
		// Moving the WHOLE object, (Only x Position ATM)
		public void MoveTheObject(GeometryModel3D geoModel, Point point, ref Canvas canvas, double lastPosX, double rotationX)
		{
			TranslateTransform3D newPosition;
			point = Mouse.GetPosition(canvas);
			newPosition = new TranslateTransform3D(geoModel.Transform.Value.OffsetX + ((point.X - lastPosX) * 0.0001), 0, 0);

			AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
			RotateTransform3D myRotateTransform3D = new RotateTransform3D();
			myAxisAngleRotation3d.Axis = new Vector3D(1, 0, 0);
			myAxisAngleRotation3d.Angle = rotationX;
			myRotateTransform3D.Rotation = myAxisAngleRotation3d;

			Transform3DGroup myTransform3DGroup = new Transform3DGroup();

			// Adding current rotation
			myTransform3DGroup.Children.Add(myRotateTransform3D);

			// Adding current position
			myTransform3DGroup.Children.Add(newPosition);

			geoModel.Transform = myTransform3DGroup;
		}
	}
}
