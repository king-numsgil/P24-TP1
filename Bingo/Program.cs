using System.Collections.Generic;
using System;

using static System.Console;

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
		public static void Main()
		{
			SetWindowSize(200, 40);

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
				new Carte()
			};

			const int ligneCompte = 5;
			const int leftOffset = 4, topOffset = 1;
			const int cardWidth = 16 + 1, cardHeight = 8 + 1;
			for (int i = 0; i < cartes.Length; ++i)
			{
				int ligne = i / ligneCompte;
				cartes[i].Print(leftOffset + (i - ligne * ligneCompte) * cardWidth,
					topOffset + ligne * cardHeight);
			}

			ReadLine();
		}
	}
}