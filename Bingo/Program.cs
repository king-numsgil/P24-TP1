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
		}

		public override string ToString()
			=> Colonne == 2 && Numero == 0 ? "X" : Numero.ToString();

		public readonly byte Colonne;
		public readonly uint Numero;
	}

	public class Carte
	{
		public Case this[byte colonne, byte ligne] => Cases[colonne, ligne];

		public readonly Case[,] Cases = new Case[5, 5];

		public Carte()
		{
			GenerateurCase.Reset();
			for (byte i = 0; i < 5; ++i)
				for (byte j = 0; j < 5; ++j)
					Cases[j, i] = Case.Generate(i);

			Cases[2, 2] = new Case(2, 0);
		}

		public void Print(int left, int top)
		{
			// alt + 201 ╔ alt + 205 ═ alt + 203 ╦ alt + 187 ╗
			// alt + 186 ║ alt + 204 ╠ alt + 206 ╬ alt + 202 ╩
			// alt + 185 ╣ alt + 200 ╚ alt + 188 ╝
			
			SetCursorPosition(left, top);
			Write("╔══╦══╦══╦══╦══╗");
			SetCursorPosition(left, top + 1);
			Write("║ B║ I║ N║ G║ O║");
			SetCursorPosition(left, top + 2);
			Write("╠══╬══╬══╬══╬══╣");

			for (byte i = 0; i < 5; ++i)
			{
				SetCursorPosition(left, top + 3 + i);
				Write("║{0,2}║{1,2}║{2,2}║{3,2}║{4,2}║",
					this[i, 0], this[i, 1], this[i, 2], this[i, 3], this[i, 4]);
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
			
			/*int nWinner = 0;
			while (nWinner == 0)
			{*/
				Clear();
				DisplayBoulles();
				
				const int ligneCompte = 5;
				const int leftOffset = 4, topOffset = 1;
				const int cardWidth = 16 + 1, cardHeight = 8 + 1;
				for (int i = 0; i < cartes.Length; ++i)
				{
					int ligne = i / ligneCompte;
					cartes[i].Print(leftOffset + (i - ligne * ligneCompte) * cardWidth,
						topOffset + ligne * cardHeight);
				}

				/*Thread.Sleep(500);
			}*/
		});

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

		public static int NumWinner(Carte[] cartes)
		{
			return 0;
		}
	}
}