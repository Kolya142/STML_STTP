﻿using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser
{
	public interface PreDrawable
	{
	}
	public struct CText : PreDrawable
	{
		public string text;
		public Font font;
	}
	public struct CImage : PreDrawable
	{
		public byte[] bytes;
	}
	public struct CHr : PreDrawable
	{
	}
}
