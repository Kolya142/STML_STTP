using Browser;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

class Program
{
	float scroll = 0;
	private void Window_MouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
	{
		scroll += e.Delta * 10;
	}
	static void Main()
	{
		Program program = new Program();
		// Create a new window
		RenderWindow window = new RenderWindow(new VideoMode(800, 600), "SFML.Net Window");
		window.Closed += (sender, e) => ((RenderWindow)sender).Close();
		window.MouseWheelScrolled += program.Window_MouseWheelScrolled;
		Font font = new Font("WhiteRabbit-47pD.ttf");

		// Create a circle shape
		List<PreDrawable> preDrawables = Parser.Parse(
			"Text: Hello, world\n" +
			"Text: Hello, STML\n" +
			"Text: See Replacements<gh> \n" +
			"Text: <a>gh<b> - \"<gh>\"\n" +
			"Text: <a>a<b> - \"<a>\"\n" +
			"Text: <a>b<b> - \"<b>\"\n" +
			"Text: Yeah, This code doesn't ship with STTP, but it's cool\n"
			,
			font
			);
		int op = 0;

		// Main loop
		while (window.IsOpen)
		{
			op = 0;
			window.DispatchEvents();
			View view = window.GetView();
			view.Size = new Vector2f(window.Size.X, window.Size.Y);
			view.Center = view.Size / 2;
			window.SetView(view);
			// Console.WriteLine(window.Size);

			// Clear the window with a color
			window.Clear(Color.Black);
			if (program.scroll > 0)
			{
				program.scroll = 0;
			}
			float y = 0;

			foreach (PreDrawable preDrawable in preDrawables)
			{
				(Drawable?, float) drawable = CreateDrawable.CreateFromPreDrawable(preDrawable);
				if (drawable.Item1 != null)
				{
					if (drawable.Item1 is Transformable transformable)
					{
						transformable.Position = new Vector2f(transformable.Position.X, y + program.scroll);
					}
					// Console.WriteLine(scroll);
					// Console.WriteLine($"{drawable.Item2}, {y}");
					window.Draw(drawable.Item1);
					y += drawable.Item2 + 14;
				}
			}

			if (program.scroll < -y+50)
			{
				program.scroll = -y+50;
			}

			// Display the contents of the window
			window.Display();
		}
	}
}
