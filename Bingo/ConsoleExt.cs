using System.Collections.Generic;
using System.Linq;
using System;

namespace Bingo
{
	public static class ConsoleExt
	{
		public static void ClearRect(int left, int top, int width, int height)
		{
			for (int y = 0; y < height; ++y)
			{
				Console.SetCursorPosition(left, top + y);
				for (int x = 0; x < width; ++x)
					Console.Write(' ');
			}
		}

		public static void Fill(int length, char filler)
		{
			for (int i = 0; i < length; ++i)
				Console.Write(filler);
		}

		public static void ReadI32(int left, int top, string question, out int result)
		{
			string tmp = string.Empty;
			do
			{
				if (tmp != string.Empty)
				{
					Console.SetCursorPosition(left + 2, top - 1);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Entrée Invalide!");
				}

				Console.ForegroundColor = ConsoleColor.White;
				Console.BackgroundColor = ConsoleColor.Black;
				ClearRect(left, top, 60, 1);
				Console.SetCursorPosition(left, top);
				Console.Write(question + " : ");
				tmp = Console.ReadLine()?.Trim();
			} while (!int.TryParse(tmp, out result));

			ClearRect(left, top - 1, 60, 3);
		}
		
		public static void ReadU32(int left, int top, string question, out uint result, uint min = 0, uint max = 300)
		{
			string tmp = string.Empty;
			do
			{
				if (tmp != string.Empty)
				{
					Console.SetCursorPosition(left + 2, top - 1);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Entrée Invalide!");
				}

				Console.ForegroundColor = ConsoleColor.White;
				Console.BackgroundColor = ConsoleColor.Black;
				ClearRect(left, top, 60, 1);
				Console.SetCursorPosition(left, top);
				Console.Write(question + " : ");
				tmp = Console.ReadLine()?.Trim();
			} while (!uint.TryParse(tmp, out result) || result < min || result > max);

			ClearRect(left, top - 1, 60, 3);
		}

		public static void ReadChar(int left, int top, string question, out char result, params char[] candidats)
		{
			string tmp = string.Empty;
			result = '\0';
			do
			{
				if (tmp != string.Empty && !Validate(result, candidats))
				{
					Console.SetCursorPosition(left + 2, top - 1);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Entrée Invalide!");
				}

				Console.ForegroundColor = ConsoleColor.White;
				Console.BackgroundColor = ConsoleColor.Black;
				ClearRect(left, top, 60, 1);
				Console.SetCursorPosition(left, top);
				Console.Write(question + " : ");
				tmp = Console.ReadLine()?.Trim().ToUpper();
			} while (!char.TryParse(tmp, out result) || !Validate(result, candidats));

			ClearRect(left, top - 1, 60, 3);
		}

		public static ConsoleKey ReadKey(int left, int top, string question,
			params ConsoleKey[] candidats)
		{
			ConsoleKey key;
			do
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.BackgroundColor = ConsoleColor.Black;
				ClearRect(left, top, question.Length + 2, 1);
				Console.SetCursorPosition(left, top);
				Console.Write(question);
				key = Console.ReadKey(false).Key;
			} while (!Validate(key, candidats));

			return key;
		}

		private static bool Validate<TCheck>(TCheck variable, params TCheck[] candidats) =>
			candidats.Any(c => EqualityComparer<TCheck>.Default.Equals(variable , c));

		public static void Run(Action runner)
		{
			Console.SetWindowSize(200, 40);
			
			bool running = true;
			while (running)
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.BackgroundColor = ConsoleColor.Black;
				Console.Clear();
				runner();

				const string exit_msg = "Voullez-vous recommencer? (O/N)";
				int left = Console.WindowWidth - exit_msg.Length - 2, top = Console.WindowHeight - 2;
				running = ReadKey(left, top, exit_msg, ConsoleKey.O, ConsoleKey.N) == ConsoleKey.O;
			}
		}
	}
}