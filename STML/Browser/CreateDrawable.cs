using Browser;
using SFML.Graphics;
using SFML.System;

static class CreateDrawable
{
	public static (Drawable?, float) CreateFromPreDrawable(PreDrawable preDrawable)
	{
		if (preDrawable.GetType() == typeof(CImage))
		{
			return CreateImage((CImage)preDrawable);
		}
		if (preDrawable.GetType() == typeof(CText))
		{
			return CreateText((CText)preDrawable);
		}
		if (preDrawable.GetType() == typeof(CHr))
		{
			return CreateHr((CHr)preDrawable);
		}
		return (null, 0f); // Return 0 for the float if Drawable is null
	}

	public static (Drawable?, float) CreateImage(CImage pd)
	{
		using (var stream = new System.IO.MemoryStream(pd.bytes))
		{
			Texture texture = new Texture(stream);
			Sprite sp = new Sprite(texture);
			return (sp, sp.GetLocalBounds().Height);
		}
	}

	public static (Drawable?, float) CreateText(CText pd)
	{
		Text text = new Text(pd.text, pd.font);
		return (text, text.GetLocalBounds().Height);
	}

	public static (Drawable?, float) CreateHr(CHr _)
	{
		RectangleShape rect = new RectangleShape(new Vector2f(100000000000, 3));
		return (rect, rect.Size.Y);
	}
}