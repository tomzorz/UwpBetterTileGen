using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace UwpBetterTileGen
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var setupSuccess = Setup();
			if (setupSuccess)
			{
				Generate();
			}
			Console.ReadKey();
		}

		private static void Generate()
		{
			Console.WriteLine();
			WriteSpecial("Starting generation...", MessageType.Information, true);
			Console.WriteLine();
			Directory.CreateDirectory("output");
			foreach (var resizeDescription in ResizeDescriptions)
			{
				foreach (var description in resizeDescription.Value)
				{
					using (var inStream = new MemoryStream(FileContents[resizeDescription.Key]))
					{
						using (var outStream = File.OpenWrite("output\\" + description.Name + Extension))
						{
							Console.Write($"Generating {description.Name} from {resizeDescription.Key}... ");
							using (var imageFactory = new ImageFactory())
							{
								imageFactory.Load(inStream)
											.Resize(new Size(description.Width, description.Height))
											.Format(Format)
											.Quality(100)
											.Save(outStream);
							}
							WriteSpecial("done!", MessageType.Success, true);
						}
					}
				}
			}
			Console.WriteLine();
			Console.WriteLine("Generation completed! Press any key to quit...");
		}

		private static readonly ISupportedImageFormat Format = new PngFormat();
		private const string Extension = ".png";

		private static readonly Dictionary<string, List<ResizeDescription>> ResizeDescriptions = new Dictionary<string, List<ResizeDescription>>
		{
			["square_large"] = new List<ResizeDescription>
			{
				//310x310
				new ResizeDescription(1240, 1240, "Square310x310Logo.scale-400"),
				new ResizeDescription( 620,  620, "Square310x310Logo.scale-200"),
				new ResizeDescription( 465,  465, "Square310x310Logo.scale-150"),
				new ResizeDescription( 388,  388, "Square310x310Logo.scale-125"),
				new ResizeDescription( 310,  310, "Square310x310Logo.scale-100"),
				//150x150
				new ResizeDescription(600, 600, "Square150x150Logo.scale-400"),
				new ResizeDescription(300, 300, "Square150x150Logo.scale-200"),
				new ResizeDescription(225, 225, "Square150x150Logo.scale-150"),
				new ResizeDescription(188, 188, "Square150x150Logo.scale-125"),
				new ResizeDescription(150, 150, "Square150x150Logo.scale-100"),
			},
			["square_small"] = new List<ResizeDescription>
			{
				//71x71
				new ResizeDescription(284, 284, "Square71x71Logo.scale-400"),
				new ResizeDescription(142, 142, "Square71x71Logo.scale-200"),
				new ResizeDescription(107, 107, "Square71x71Logo.scale-150"),
				new ResizeDescription( 89,  89, "Square71x71Logo.scale-125"),
				new ResizeDescription( 71,  71, "Square71x71Logo.scale-100"),
				//44x44
				new ResizeDescription(176, 176, "Square44x44Logo.scale-400"),
				new ResizeDescription( 88,  88, "Square44x44Logo.scale-200"),
				new ResizeDescription( 66,  66, "Square44x44Logo.scale-150"),
				new ResizeDescription( 55,  55, "Square44x44Logo.scale-125"),
				new ResizeDescription( 44,  44, "Square44x44Logo.scale-100"),
				//44x44 targetsize
				new ResizeDescription(256, 256, "Square44x44Logo.targetsize-256"),
				new ResizeDescription( 48,  48, "Square44x44Logo.targetsize-48"),
				new ResizeDescription( 24,  24, "Square44x44Logo.targetsize-24"),
				new ResizeDescription( 16,  16, "Square44x44Logo.targetsize-16"),
				//44x44 targetsize altform unplated
				new ResizeDescription(256, 256, "Square44x44Logo.targetsize-256_altform-unplated"),
				new ResizeDescription( 48,  48, "Square44x44Logo.targetsize-48_altform-unplated"),
				new ResizeDescription( 24,  24, "Square44x44Logo.targetsize-24_altform-unplated"),
				new ResizeDescription( 16,  16, "Square44x44Logo.targetsize-16_altform-unplated"),
				//store logo
				new ResizeDescription(200, 200, "StoreLogo.scale-400"),
				new ResizeDescription(100, 100, "StoreLogo.scale-200"),
				new ResizeDescription( 75,  75, "StoreLogo.scale-150"),
				new ResizeDescription( 63,  63, "StoreLogo.scale-125"),
				new ResizeDescription( 50,  50, "StoreLogo.scale-100"),
			},
			["wide"] = new List<ResizeDescription>
			{
				new ResizeDescription(1240, 600, "Wide310x150Logo.scale-400"),
				new ResizeDescription( 620, 300, "Wide310x150Logo.scale-200"),
				new ResizeDescription( 465, 225, "Wide310x150Logo.scale-150"),
				new ResizeDescription( 388, 188, "Wide310x150Logo.scale-125"),
				new ResizeDescription( 310, 150, "Wide310x150Logo.scale-100"),
			},
			["splash"] = new List<ResizeDescription>
			{
				new ResizeDescription(2480, 1200, "SplashScreen.scale-400"),
				new ResizeDescription(1240,  600, "SplashScreen.scale-200"),
				new ResizeDescription( 930,  450, "SplashScreen.scale-150"),
				new ResizeDescription( 775,  375, "SplashScreen.scale-125"),
				new ResizeDescription( 620,  300, "SplashScreen.scale-100"),
			}
		};

		private static readonly Dictionary<string, byte[]> FileContents = new Dictionary<string, byte[]>();

		private static bool Setup()
		{
			Console.WriteLine("Welcome to");
			Console.WriteLine("           __   __   ___ ___ ___  ___  __  ___         ___  __   ___      ");
			Console.WriteLine("|  | |  | |__) |__) |__   |   |  |__  |__)  |  | |    |__  / _` |__  |\\ | ");
			Console.WriteLine("\\__/ |/\\| |    |__) |___  |   |  |___ |  \\  |  | |___ |___ \\__> |___ | \\| ");
			Console.WriteLine();
			WriteSpecial("Loading source files...", MessageType.Information, true);
			Console.WriteLine();
			foreach (var key in ResizeDescriptions.Keys)
			{
				var fn = key + Extension;
				Console.Write($"Looking for source file {fn}: ");
				if (File.Exists(fn))
				{
					try
					{
						FileContents[key] = File.ReadAllBytes(fn);
						WriteSpecial("File found and loaded successfully!", MessageType.Success, true);
					}
					catch (Exception e)
					{
						WriteSpecial("Couldn't load file!", MessageType.Failure, true);
						Console.WriteLine(e.ToString());
						return false;
					}
				}
				else
				{
					WriteSpecial("File not found!", MessageType.Failure, true);
					return false;
				}
			}
			return true;
		}

		private static void WriteSpecial(string message, MessageType type, bool addNewLine = false)
		{
			var pBg = Console.BackgroundColor;
			var pFg = Console.ForegroundColor;
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (type)
			{
				case MessageType.Information:
					Console.ForegroundColor = ConsoleColor.White;
					Console.BackgroundColor = ConsoleColor.DarkBlue;
					break;
				case MessageType.Failure:
					Console.ForegroundColor = ConsoleColor.White;
					Console.BackgroundColor = ConsoleColor.DarkRed;
					break;
				case MessageType.Success:
					Console.ForegroundColor = ConsoleColor.White;
					Console.BackgroundColor = ConsoleColor.DarkGreen;
					break;
			}
			Console.Write(message);
			Console.ForegroundColor = pFg;
			Console.BackgroundColor = pBg;
			if (addNewLine) Console.WriteLine();
		}

		private enum MessageType
		{
			Information,
			Failure,
			Success
		}
	}
}