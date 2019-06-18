using System;

namespace Bingo
{
	enum Colonne
	{
		B = 0,
		I = 1,
		N = 2,
		G = 3,
		O = 4
	}

	struct Case
	{
		public static Case Generate(Colonne colonne)
		{
			return new Case(colonne, (uint) (
				colonne == Colonne.B ? R.Next(1,  15) :
				colonne == Colonne.I ? R.Next(16, 30) :
				colonne == Colonne.N ? R.Next(31, 45) :
				colonne == Colonne.G ? R.Next(46, 60) :
				R.Next(61, 75)));
		}

		public Case(Colonne colonne, uint numero)
		{
			Colonne = colonne;
			Numero = numero;
		}

		public readonly Colonne Colonne;
		public readonly uint Numero;
		
		private static readonly Random R = new Random();
	}

	class Carte
	{
		public uint this[Colonne colonne, int ligne] => Cases[(int) colonne, ligne].Numero;

		public readonly Case[,] Cases = new Case[5, 5];

		public Carte()
		{
			for (int i = 0; i < 5; ++i)
				for (int j = 0; j < 5; ++j)
					Cases[j, i] = Case.Generate((Colonne) i);

			Cases[2, 2] = new Case(Colonne.N, 0);
		}

		public void Print(int left, int top)
		{
			// alt + 201 ╔ alt + 205 ═ alt + 203 ╦ alt + 187 ╗
			// alt + 186 ║ alt + 204 ╠ alt + 206 ╬ alt + 202 ╩
			// alt + 185 ╣ alt + 200 ╚ alt + 188 ╝
			
			Console.SetCursorPosition(left, top);
			Console.Write("╔══╦══╦══╦══╦══╗");
			Console.SetCursorPosition(left, top + 1);
			Console.Write("║B ║I ║N ║G ║O ║");
			Console.SetCursorPosition(left, top + 2);
			Console.Write("╠══╬══╬══╬══╬══╣");

			for (int i = 0; i < 5; ++i)
			{
				Colonne c = (Colonne) i;
				Console.SetCursorPosition(left, top + 3 + i);
				Console.Write("║{0,2}║{1,2}║{2,2}║{3,2}║{4,2}║", this[c, 0], this[c, 1], this[c, 2], this[c, 3], this[c, 4]);
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