using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Mouse;

namespace Browser
{
	static class Parser
	{
		public static List<PreDrawable> Parse(string code, Font font)
		{
			List<PreDrawable> Commands = new List<PreDrawable>();
			foreach (var line in code.Split("\n"))
			{
				if (line.Length == 0)
				{
					continue;
				}

				var sp = line.Split(new[] { ": " }, StringSplitOptions.None);
				if (sp.Length == 0)
				{
					continue;
				}

				if (sp.Length == 1)
				{
					if (sp[0] == "HR")
					{
						Commands.Add(new CHr());
					}
				}

				if (sp.Length == 2)
				{
					if (sp[0] == "Text")
					{
						Commands.Add(new CText { text = sp[1].Replace("<gh>", ":").Replace("<a>", "<").Replace("<b>", ">"), font=font }) ;
					}
					else if (sp[0] == "Image")
					{
						Commands.Add(new CImage {bytes = File.ReadAllBytes(sp[0])});
					}
				}

				if (sp.Length == 3)
				{
					if (sp[0] == "Button")
					{
						Commands.Add(new CButton { text = sp[1].Replace("<gh>", ":").Replace("<a>", "<").Replace("<b>", ">"), font = font, page = sp[2] });
					}
				}
			}
			return Commands;
		}
	}
}
