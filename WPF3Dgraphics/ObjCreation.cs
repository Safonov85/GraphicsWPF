using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WPF3Dgraphics
{
	public class ObjCreation
	{
		public void CreateObjFile(string filePath, GeometryModel3D geoModel)
		{
			string groupName = "cube";
			MeshGeometry3D cubeMesh;
			cubeMesh = (MeshGeometry3D)geoModel.Geometry;
			string[] objText = new string[]
				{
				"# 3ds Max Wavefront OBJ Exporter - (c)2017",
				"# File Created: " + DateTime.Now.ToString().Replace("-", "."),
				"",
				"#",
				"# object " + groupName,
				"#",
				""
				};

			foreach (var line in objText)
			{
				File.AppendAllText(@filePath, line + Environment.NewLine);
			}

			// Vertices Positions Added to OBJ file
			foreach (var point in cubeMesh.Positions)
			{
				File.AppendAllText(@filePath, "v  " + ((decimal)point.X).ToString("0.0000").Replace(',', '.'));
				File.AppendAllText(@filePath, " " + point.Y.ToString("0.0000").Replace(',', '.'));
				File.AppendAllText(@filePath, " " + point.Z.ToString("0.0000").Replace(',', '.') + Environment.NewLine);
			}
			File.AppendAllText(@filePath, "# " + cubeMesh.Positions.Count + " vertices" + Environment.NewLine + Environment.NewLine);

			File.AppendAllText(@filePath, "g " + groupName + Environment.NewLine);

			foreach(var face in cubeMesh.TriangleIndices)
			{
				File.AppendAllText(@filePath, "f " + face);
			}
		}
	}
}
