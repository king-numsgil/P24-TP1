using System;

namespace Bingo
{
	struct Case
	{
		public static Case Generate(byte colonne)
		{
			return new Case(colonne, (uint) (
				colonne == 0 ? R.Next(1,  15) :
				colonne == 1 ? R.Next(16, 30) :
				colonne == 2 ? R.Next(31, 45) :
				colonne == 3 ? R.Next(46, 60) :
				R.Next(61, 75)));
		}

		public Case(byte colonne, uint numero)
		{
			Colonne = colonne;
			Numero = numero;
		}

		public override string ToString()
		{
			return Colonne == 2 && Numero == 0 ? "X" : Numero.ToString();
		}

		public readonly byte Colonne;
		public readonly uint Numero;
		
		private static readonly Random R = new Random();
	}

	class Carte
	{
		public Case this[byte colonne, int ligne] => Cases[colonne, ligne];

		public readonly Case[,] Cases = new Case[5, 5];

		public Carte()
		{
			for (byte i = 0; i < 5; ++i)
				for (int j = 0; j < 5; ++j)
					Cases[j, i] = Case.Generate(i);

			Cases[2, 2] = new Case(2, 0);
		}

		public void Print(int left, int top)
		{
			// alt + 201 ╔ alt + 205 ═ alt + 203 ╦ alt + 187 ╗
			// alt + 186 ║ alt + 204 ╠ alt + 206 ╬ alt + 202 ╩
			// alt + 185 ╣ alt + 200 ╚ alt + 188 ╝
			
			Console.SetCursorPosition(left, top);
			Console.Write("╔══╦══╦══╦══╦══╗");
			Console.SetCursorPosition(left, top + 1);
			Console.Write("║ B║ I║ N║ G║ O║");
			Console.SetCursorPosition(left, top + 2);
			Console.Write("╠══╬══╬══╬══╬══╣");

			for (byte i = 0; i < 5; ++i)
			{
				Console.SetCursorPosition(left, top + 3 + i);
				Console.Write("║{0,2}║{1,2}║{2,2}║{3,2}║{4,2}║", this[i, 0], this[i, 1], this[i, 2], this[i, 3], this[i, 4]);
			}
			
			Console.SetCursorPosition(left, top + 8);
			Console.Write("╚══╩══╩══╩══╩══╝");
		}
	}

	static class Program
	{
		public static void Main()
		{
			Console.SetWindowSize(200, 40);

			Carte[] cartes = {
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte(),
				new Carte()
			};

			for (int i = 0; i < cartes.Length; ++i)
			{
				int ligne = i / 5;
				cartes[i].Print(4 + (i - ligne * 5) * 17, 1 + ligne * 9);
			}

			Console.ReadLine();
		}
	}
}