using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpBetterTileGen
{
	public class ResizeDescription
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public string Name { get; private set; }

		public ResizeDescription(int width, int height, string name)
		{
			Width = width;
			Height = height;
			Name = name;
		}
	}
}