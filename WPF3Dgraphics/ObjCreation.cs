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
		public void CreateObjFile(string filePath, GeometryModel3D geoModel, string groupName)
		{
			
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

			// Faces in proper places
			File.AppendAllText(@filePath, "g " + groupName + Environment.NewLine);
			File.AppendAllText(@filePath, "f ");
			int index = 0;
			int i3 = 0;
			int polygons = 0;
			int triangles = 1;
			foreach(var face in cubeMesh.TriangleIndices)
			{
				i3++;
				File.AppendAllText(@filePath, (face + 1).ToString() + " ");

				if(index == cubeMesh.TriangleIndices.Count - 1)
				{
					break;
				}
				if(i3 > 2)
				{
					File.AppendAllText(@filePath, Environment.NewLine);
					File.AppendAllText(@filePath, "f ");
					i3 = 0;
					triangles++;
				}
				index++;
			}
			File.AppendAllText(@filePath, Environment.NewLine);

			// Amount of polygons and or triangles
			File.AppendAllText(@filePath, "# " + polygons.ToString("0") + " polygons");
			if(triangles > 0)
			{
				File.AppendAllText(@filePath, " - " + triangles + " triangles");
			}

			File.AppendAllText(@filePath, Environment.NewLine);
			File.AppendAllText(@filePath, Environment.NewLine);
		}
	}
}
