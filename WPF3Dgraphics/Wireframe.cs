using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace WPF3Dgraphics
{
	public class Wireframe
	{
		public bool WireframeOnOrOff { get; set; }

		List<Ellipse> circles = new List<Ellipse>();
		List<Line> myLines = new List<Line>();
		List<TextBlock> textBlocks = new List<TextBlock>();

		public void DrawWireFrame(GeometryModel3D geoModel, Viewport3D myViewport, Int32[] indices, List<Line> lines)
		{
			MeshGeometry3D mesh;
			mesh = (MeshGeometry3D)geoModel.Geometry;

			int i = 0;
			foreach (var item in textBlocks)
			{
				item.RenderTransform = new TranslateTransform
				{
					X = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).X,
					Y = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[i]).Y
				};

				i++;
			}

			// WIREFRAME
			i = 0;
			int j = 1;
			foreach (var item in lines)
			{
				item.X1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[indices[i] -1]).X;
				item.Y1 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[indices[i] -1]).Y;

				item.X2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[indices[j] -1]).X;
				item.Y2 = Petzold.Media3D.ViewportInfo.Point3DtoPoint2D(myViewport, mesh.Positions[indices[j] -1]).Y;

				if (j > lines.Count - 2)
				{
					break;
				}

				i++;
				j++;
			}
		}
	}
}
