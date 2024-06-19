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
		SttpClient client = new SttpClient(port: 91);
		client.GenerateHeader("/", "BetterFly");
		string recived = client.Send();
		Console.WriteLine(recived);
		Console.WriteLine(recived.Length);

		Program program = new Program();
		// Create a new window
		RenderWindow window = new RenderWindow(new VideoMode(800, 600), "SFML.Net Window");
		window.Closed += (sender, e) => ((RenderWindow)sender).Close();
		window.MouseWheelScrolled += program.Window_MouseWheelScrolled;
		Font font = new Font("WhiteRabbit-47pD.ttf");

		// Create a circle shape
		List<PreDrawable> preDrawables = Parser.Parse(
			recived
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
				/*Vector2i Pos = Mouse.GetPosition(window);
				  FloatRect floatRect = this.label.GetLocalBounds();
				  if ( floatRect.Left < Pos.X && Pos.X < floatRect.Left + floatRect.Width &&
				      floatRect.Top < Pos.Y && Pos.Y < floatRect.Top + floatRect.Height) */
				(Drawable?, float, string?) drawable = CreateDrawable.CreateFromPreDrawable(preDrawable);
				if (drawable.Item1 != null)
				{
					if (drawable.Item1 is Transformable transformable)
					{
						transformable.Position = new Vector2f(transformable.Position.X, y + program.scroll);
					}
					if (preDrawable.GetType() == typeof(CButton))
					{
						string page = drawable.Item3;
						Vector2i Pos = Mouse.GetPosition(window);
						FloatRect floatRect = (drawable.Item1 as Text).GetGlobalBounds();
						if (floatRect.Contains(Pos))
						{

							if (drawable.Item1 is Text text)
							{
								text.FillColor = Color.Blue;
							}
							if (Mouse.IsButtonPressed(Mouse.Button.Left))
							{
								client.GenerateHeader(drawable.Item3, "BetterFly");
								recived = client.Send();
								Console.WriteLine(recived);
								Console.WriteLine(recived.Length);
								preDrawables = Parser.Parse(recived, font);
							}
						}
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
