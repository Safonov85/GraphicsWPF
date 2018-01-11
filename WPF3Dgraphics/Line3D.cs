using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WPF3Dgraphics
{
	public class Line3D
	{
		public double Position2D_X { get; set; }
		public double Position2D_Y { get; set; }

		public double Position3D_X { get; set; }
		public double Position3D_Y { get; set; }
		public double Position3D_Z { get; set; }

		public List<Point3D> LinePoints = new List<Point3D>();

		public bool OnOrOff { get; set; }

		public Line3D()
		{
			OnOrOff = false;
		}

		public void CreateFirstPoint()
		{

		}

		public void AddNewPoint(double x, double y, double z)
		{
			LinePoints.Add(new Point3D(x, y, z));
		}

		public void CompleteLine()
		{

		}

		// create points (not really a mesh ATM)
		MeshGeometry3D MakeLine(Point3D[] points)
		{
			MeshGeometry3D line = new MeshGeometry3D();
			Point3DCollection corners = new Point3DCollection();
			foreach(var point in points)
			{
				corners.Add(point);
			}
			line.Positions = corners;

			return line;
		}
	}
}
