using System.Collections.Generic;
using System.Threading;
using System;

using static System.Console;
using static Bingo.ConsoleExt;

namespace Bingo
{
	public static class GenerateurCase
	{
		private static readonly Random R = new Random();
		private static readonly Dictionary<uint, bool> Cache = new Dictionary<uint, bool>();
		
		private static uint Next(byte colonne) => (uint) (R.Next(15) + 1 + colonne * 15);

		public static void Reset() => Cache.Clear();

		public static uint Generate(byte colonne)
		{
			uint numero;
			do numero = Next(colonne);
			while (Cache.ContainsKey(numero));

			Cache[numero] = true;
			return numero;
		}
	}
	
	public struct Case
	{
		public static Case Generate(byte colonne)
			=> new Case(colonne, GenerateurCase.Generate(colonne));

		public Case(byte colonne, uint numero)
		{
			Colonne = colonne;
			Numero = numero;
			Sortie = false;
		}

		public override string ToString()
			=> Colonne == 2 && Numero == 0 ? "X" : Numero.ToString();

		public readonly byte Colonne;
		public readonly uint Numero;
		public bool Sortie;
	}

	public class Carte
	{
		public Case this[byte colonne, byte ligne] => Cases[colonne, ligne];

		public readonly Case[,] Cases = new Case[5, 5];

		public bool IsWin
		{
			get
			{
				for (byte i = 0; i < 5; ++i)
				{
					if (this[i, 0].Sortie && this[i, 1].Sortie && this[i, 2].Sortie && this[i, 3].Sortie && this[i, 4].Sortie)
						return true;
					if (this[0, i].Sortie && this[1, i].Sortie && this[2, i].Sortie && this[3, i].Sortie && this[4, i].Sortie)
						return true;
				}

				if (this[0, 0].Sortie && this[1, 1].Sortie && this[2, 2].Sortie && this[3, 3].Sortie && this[4, 4].Sortie)
					return true;
				if (this[0, 4].Sortie && this[1, 3].Sortie && this[2, 2].Sortie && this[3, 2].Sortie && this[4, 1].Sortie)
					return true;

				return false;
			}
		}

		public Carte()
		{
			GenerateurCase.Reset();
			for (byte i = 0; i < 5; ++i)
				for (byte j = 0; j < 5; ++j)
					Cases[j, i] = Case.Generate(i);

			Cases[2, 2] = new Case(2, 0) {Sortie = true};
		}

		public void Print(int left, int top)
		{
			// alt + 201 ╔ alt + 205 ═ alt + 203 ╦ alt + 187 ╗
			// alt + 186 ║ alt + 204 ╠ alt + 206 ╬ alt + 202 ╩
			// alt + 185 ╣ alt + 200 ╚ alt + 188 ╝

			BackgroundColor = IsWin ? ConsoleColor.Gray : ConsoleColor.Black;
			
			SetCursorPosition(left, top);
			Write("╔══╦══╦══╦══╦══╗");
			SetCursorPosition(left, top + 1);
			Write("║ B║ I║ N║ G║ O║");
			SetCursorPosition(left, top + 2);
			Write("╠══╬══╬══╬══╬══╣");

			for (byte i = 0; i < 5; ++i)
			{
				SetCursorPosition(left, top + 3 + i);
				Write("║");

				for (byte j = 0; j < 5; ++j)
				{
					ForegroundColor = this[i, j].Sortie ? ConsoleColor.Green : ConsoleColor.Red;
					Write("{0,2}", this[i, j]);
					ForegroundColor = ConsoleColor.White;
					Write("║");
				}
			}
			
			SetCursorPosition(left, top + 8);
			Write("╚══╩══╩══╩══╩══╝");
		}
	}

	internal static class Program
	{
		private static readonly Dictionary<uint, uint> boulles = new Dictionary<uint, uint>();
		private static readonly Random R = new Random();
		
		public static void Main() => Run(() =>
		{
			ReadU32(150, 3, "Entrez le nombre de joueur", out uint joueurs, 4, 20);

			SetCursorPosition(150, 3);
			Write("Appuyez sur une touche pour commencer!");
			ReadKey(false);
			
			Dictionary<uint, bool> cache = new Dictionary<uint, bool>();
			Carte[] cartes = new Carte[joueurs];
			for (int i = 0; i < joueurs; ++i)
				cartes[i] = new Carte();
			
			int nWinner = 0;
			while (nWinner == 0)
			{
				Clear();
				DisplayBoulles();
				DisplayCartes(cartes);

				Thread.Sleep(600);

				uint numero;
				do numero = (uint) R.Next(75) + 1;
				while (cache.ContainsKey(numero));
				cache[numero] = true;

				if (boulles.ContainsKey(numero)) boulles[numero]++;
				else boulles[numero] = 1;
				
				SetCursorPosition(145, 15);
				Write("Une boulle à été tiré! Numéro {0}", numero);
				DisplayBoulles();

				Thread.Sleep(600);

				nWinner = UpdateCartes(cartes, cache);
			}
			
			Clear();
			DisplayBoulles();
			DisplayCartes(cartes);
			
			SetCursorPosition(145, 15);
			Write("Nous avons {0} gagnant{1}!", nWinner, nWinner > 1 ? "s" : "");
		});

		public static void DisplayCartes(Carte[] cartes)
		{
			const int ligneCompte = 5;
			const int leftOffset = 4, topOffset = 1;
			const int cardWidth = 16 + 1, cardHeight = 8 + 1;
			for (int i = 0; i < cartes.Length; ++i)
			{
				int ligne = i / ligneCompte;
				cartes[i].Print(leftOffset + (i - ligne * ligneCompte) * cardWidth,
					topOffset + ligne * cardHeight);
			}
		}

		public static void DisplayBoulles()
		{
			const uint ligneCompte = 5;
			const uint startLeft = 100, startTop = 5;
			const uint boulleWidth = 6, boulleHeight = 2;

			SetCursorPosition((int) startLeft + 6, (int) startTop - 2);
			Write("Status des Boulles");

			for (uint i = 0; i < 75; ++i)
			{
				int ligne = (int) (i / ligneCompte);
				int left = (int) (startLeft + (i - ligne * ligneCompte) * boulleWidth);
				int top = (int) (startTop + ligne * boulleHeight);

				SetCursorPosition(left, top);
				Write("{0,2}:{1,2}", i + 1, boulles.ContainsKey(i + 1) ? boulles[i + 1] : 0);
			}
		}

		public static int UpdateCartes(IEnumerable<Carte> cartes, Dictionary<uint, bool> cache)
		{
			int nWin = 0;
			foreach (Carte carte in cartes)
			{
				for (byte i = 0; i < 5; ++i)
					for (byte j = 0; j < 5; ++j)
						if (!carte[i, j].Sortie && cache.ContainsKey(carte[i, j].Numero))
							carte.Cases[i, j].Sortie = true;

				if (carte.IsWin) ++nWin;
			}

			return nWin;
		}
	}
}