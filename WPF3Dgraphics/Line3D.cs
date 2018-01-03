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

		public void CreateFirstPoint()
		{

		}

		public void CompleteLine()
		{

		}

		MeshGeometry3D MakeLine()
		{
			MeshGeometry3D line = new MeshGeometry3D();

			return line;
		}
	}
}
