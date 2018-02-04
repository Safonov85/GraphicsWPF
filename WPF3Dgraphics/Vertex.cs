using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WPF3Dgraphics
{
	public class Vertex
	{
		public void DetectDotVertex(int item, bool ctrlPressed, ref List<Ellipse> circles)
		{
			// Clear all reds, MAKE BLUE
			if (ctrlPressed == false)
			{
				int i = 0;
				while (i < circles.Count - 1)
				{
					circles[i].Stroke = System.Windows.Media.Brushes.Blue;
					circles[i].Fill = System.Windows.Media.Brushes.Blue;
					i++;
				}
			}
			if (circles[item].Fill != System.Windows.Media.Brushes.Red)
			{
				circles[item].Stroke = System.Windows.Media.Brushes.Red;
				circles[item].Fill = System.Windows.Media.Brushes.Red;
			}
			else
			{
				circles[item].Stroke = System.Windows.Media.Brushes.Blue;
				circles[item].Fill = System.Windows.Media.Brushes.Blue;
			}

		}

		// Detecteing if the blue vertex is pressed outside it or inside it
		public bool VertexPressed(int xMousePos, int yMousePos, int xVert, int yVert)
		{
			if (xMousePos > (xVert - 5) && xMousePos < (xVert + 5)
			&& yMousePos > (yVert - 5) && yMousePos < (yVert + 5))
			{
				return true;
			}

			return false;
		}
	}
}
