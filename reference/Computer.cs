using System;
using System.Runtime.CompilerServices;
namespace ThomokuSL5 {
public class Computer
{
	// Made seedable for deterministic verification against the F# port.
	public static Random random = new Random();

	private int laudankoko = 40;

	private int[,] PRIORITY = new int[64, 64];

	private int maxsyvyys = 5;

	private int puhe = 1;

	public int vuoro;

	public int kokovuoro;

	public int arpamaara;

	private int alotyyli = 2;

	private int alotyyli2 = 5;

	public int suora1x;

	public int suora1y;

	public int suora2x;

	public int suora2y;

	[field: CompilerGenerated]
	public int[,] A
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public string CmpComString
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public Computer()
	{
		alotyyli = arpa5();
		if (alotyyli == 0)
		{
			alotyyli = 4;
		}
		alotyyli2 = arpa();
	}

	public int arpa()
	{
		Convert.ToInt32(random.NextDouble() * 3.0);
		return Convert.ToInt32(random.NextDouble() * 3.0) + 1;
	}

	public int arpa4()
	{
		return Convert.ToInt32(random.NextDouble() * 3.0) + 1;
	}

	public int arpa5()
	{
		return Convert.ToInt32(random.NextDouble() * 3.0);
	}

	public void prionollaus()
	{
		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				PRIORITY[j, i] = 0;
			}
		}
	}

	public int TarkistaVoitto()
	{
		int[,] array = new int[64, 64];
		int num = 1;
		int num2 = 40;
		int num3 = vuoro;
		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				array[j, i] = 0;
			}
		}
		for (int k = 0; k < 40; k++)
		{
			for (int l = 0; l < 40; l++)
			{
				array[l + 8, k + 8] = A[l, k];
			}
		}
		for (int m = 8; m < num2 + 8; m++)
		{
			for (int n = 8; n < num2 + 8; n++)
			{
				int num4;
				for (num4 = 0; array[n + num4, m] == num3; num4++)
				{
				}
				if (num4 > 4 && (num == 1 || (num4 == 5 && array[n - 1, m] != num3)))
				{
					suora1x = n - 8;
					suora1y = m - 8;
					suora2x = n + num4 - 9;
					suora2y = m - 8;
					return num3;
				}
			}
		}
		for (int num5 = 8; num5 < num2 + 8; num5++)
		{
			for (int num6 = 8; num6 < num2 + 8; num6++)
			{
				int num7;
				for (num7 = 0; array[num6, num5 + num7] == num3; num7++)
				{
				}
				if (num7 > 4 && (num == 1 || (num7 == 5 && array[num6, num5 - 1] != num3)))
				{
					suora1x = num6 - 8;
					suora1y = num5 - 8;
					suora2x = num6 - 8;
					suora2y = num5 + num7 - 9;
					return num3;
				}
			}
		}
		for (int num8 = 8; num8 < num2 + 8; num8++)
		{
			for (int num9 = 8; num9 < num2 + 8; num9++)
			{
				int num10;
				for (num10 = 0; array[num9 + num10, num8 + num10] == num3; num10++)
				{
				}
				if (num10 > 4 && (num == 1 || (num10 == 5 && array[num9 - 1, num8 - 1] != num3)))
				{
					suora1x = num9 - 8;
					suora1y = num8 - 8;
					suora2x = num9 + num10 - 9;
					suora2y = num8 + num10 - 9;
					return num3;
				}
			}
		}
		for (int num11 = 8; num11 < num2 + 8; num11++)
		{
			for (int num12 = 8; num12 < num2 + 8; num12++)
			{
				int num13;
				for (num13 = 0; array[num12 - num13, num11 + num13] == num3; num13++)
				{
				}
				if (num13 > 4 && (num == 1 || (num13 == 5 && array[num12 + 1, num11 - 1] != num3)))
				{
					suora1x = num12 - 8;
					suora1y = num11 - 8;
					suora2x = num12 - num13 - 7;
					suora2y = num11 + num13 - 9;
					return num3;
				}
			}
		}
		return 0;
	}

	private void taulukko1(int[,] AA, int V1, int V2)
	{
		AA[0, 0] = V2;
		AA[0, 1] = V2;
		AA[0, 2] = 0;
		AA[0, 3] = V2;
		AA[0, 4] = V2;
		AA[0, 5] = 5;
		AA[0, 6] = 5;
		AA[0, 7] = 5;
		AA[1, 0] = V2;
		AA[1, 1] = V2;
		AA[1, 2] = V2;
		AA[1, 3] = 0;
		AA[1, 4] = V2;
		AA[1, 5] = 5;
		AA[1, 6] = 5;
		AA[1, 7] = 5;
		AA[2, 0] = V2;
		AA[2, 1] = V2;
		AA[2, 2] = V2;
		AA[2, 3] = V2;
		AA[2, 4] = 0;
		AA[2, 5] = 5;
		AA[2, 6] = 5;
		AA[2, 7] = 5;
		AA[3, 0] = V1;
		AA[3, 1] = V1;
		AA[3, 2] = 0;
		AA[3, 3] = V1;
		AA[3, 4] = V1;
		AA[3, 5] = 5;
		AA[3, 6] = 5;
		AA[3, 7] = 5;
		AA[4, 0] = V1;
		AA[4, 1] = V1;
		AA[4, 2] = V1;
		AA[4, 3] = 0;
		AA[4, 4] = V1;
		AA[4, 5] = 5;
		AA[4, 6] = 5;
		AA[4, 7] = 5;
		AA[5, 0] = V1;
		AA[5, 1] = V1;
		AA[5, 2] = V1;
		AA[5, 3] = V1;
		AA[5, 4] = 0;
		AA[5, 5] = 5;
		AA[5, 6] = 5;
		AA[5, 7] = 5;
	}

	private void fastend(int[] PISTEET)
	{
		PISTEET[0] = 2000;
		PISTEET[1] = 4000;
		PISTEET[2] = 4000;
		PISTEET[3] = 200;
		PISTEET[4] = 400;
		PISTEET[5] = 400;
	}

	private void taulukko2(int[,] AA, int[,] BB, int V1, int V2)
	{
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				BB[i, j] = 5;
			}
		}
		AA[0, 0] = V2;
		AA[0, 1] = V2;
		AA[0, 2] = V2;
		AA[0, 3] = V2;
		AA[0, 4] = 0;
		AA[1, 0] = V2;
		AA[1, 1] = V2;
		AA[1, 2] = V2;
		AA[1, 3] = 0;
		AA[1, 4] = V2;
		AA[2, 0] = V2;
		AA[2, 1] = V2;
		AA[2, 2] = 0;
		AA[2, 3] = V2;
		AA[2, 4] = V2;
		AA[3, 0] = V2;
		AA[3, 1] = V2;
		AA[3, 2] = 0;
		AA[3, 3] = V2;
		AA[3, 4] = 0;
		AA[4, 0] = V2;
		AA[4, 1] = V2;
		AA[4, 2] = 0;
		AA[4, 3] = V2;
		AA[4, 4] = 0;
		AA[5, 0] = V2;
		AA[5, 1] = 0;
		AA[5, 2] = V2;
		AA[5, 3] = 0;
		AA[5, 4] = V2;
		AA[6, 0] = V2;
		AA[6, 1] = V2;
		AA[6, 2] = 0;
		AA[6, 3] = 0;
		AA[6, 4] = V2;
		AA[7, 0] = V2;
		AA[7, 1] = V2;
		AA[7, 2] = 0;
		AA[7, 3] = 0;
		AA[7, 4] = V2;
		AA[8, 0] = V2;
		AA[8, 1] = V2;
		AA[8, 2] = V2;
		AA[8, 3] = 0;
		AA[8, 4] = 0;
		AA[9, 0] = V2;
		AA[9, 1] = V2;
		AA[9, 2] = V2;
		AA[9, 3] = 0;
		AA[9, 4] = 0;
		AA[10, 0] = V2;
		AA[10, 1] = 0;
		AA[10, 2] = V2;
		AA[10, 3] = V2;
		AA[10, 4] = 0;
		AA[11, 0] = V2;
		AA[11, 1] = 0;
		AA[11, 2] = V2;
		AA[11, 3] = V2;
		AA[11, 4] = 0;
		BB[3, 2] = V1;
		BB[3, 4] = V2;
		BB[4, 2] = V2;
		BB[4, 4] = V1;
		BB[5, 1] = V2;
		BB[5, 3] = V1;
		BB[6, 2] = V2;
		BB[6, 3] = V1;
		BB[7, 2] = V1;
		BB[7, 3] = V2;
		BB[8, 3] = V2;
		BB[8, 4] = V1;
		BB[9, 3] = V1;
		BB[9, 4] = V2;
		BB[10, 1] = V1;
		BB[10, 4] = V2;
		BB[11, 1] = V2;
		BB[11, 4] = V1;
	}

	private void taulukko3(int[,] AA, int[,] BB, int V1, int V2)
	{
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				BB[i, j] = 5;
			}
		}
		AA[0, 0] = 0;
		AA[0, 1] = V2;
		AA[0, 2] = V2;
		AA[0, 3] = V2;
		AA[0, 4] = 0;
		AA[0, 5] = 0;
		AA[1, 0] = 0;
		AA[1, 1] = V2;
		AA[1, 2] = V2;
		AA[1, 3] = 0;
		AA[1, 4] = V2;
		AA[1, 5] = 0;
		AA[2, 0] = 0;
		AA[2, 1] = V2;
		AA[2, 2] = V2;
		AA[2, 3] = 0;
		AA[2, 4] = 0;
		AA[2, 5] = 0;
		AA[3, 0] = 0;
		AA[3, 1] = V2;
		AA[3, 2] = V2;
		AA[3, 3] = 0;
		AA[3, 4] = 0;
		AA[3, 5] = 0;
		AA[4, 0] = 0;
		AA[4, 1] = V2;
		AA[4, 2] = 0;
		AA[4, 3] = V2;
		AA[4, 4] = 0;
		AA[4, 5] = 0;
		AA[5, 0] = 0;
		AA[5, 1] = V2;
		AA[5, 2] = 0;
		AA[5, 3] = V2;
		AA[5, 4] = 0;
		AA[5, 5] = 0;
		AA[6, 0] = 0;
		AA[6, 1] = V2;
		AA[6, 2] = 0;
		AA[6, 3] = 0;
		AA[6, 4] = V2;
		AA[6, 5] = 0;
		BB[2, 0] = V1;
		BB[2, 3] = V2;
		BB[2, 4] = V1;
		BB[2, 5] = V1;
		BB[3, 0] = V1;
		BB[3, 3] = V1;
		BB[3, 4] = V2;
		BB[3, 5] = V1;
		BB[4, 0] = V1;
		BB[4, 2] = V2;
		BB[4, 4] = V1;
		BB[4, 5] = V1;
		BB[5, 0] = V1;
		BB[5, 2] = V1;
		BB[5, 4] = V2;
		BB[5, 5] = V1;
		BB[6, 0] = V1;
		BB[6, 2] = V2;
		BB[6, 3] = V1;
		BB[6, 5] = V1;
	}

	private void taulukko4(int[,] AA, int V1, int V2)
	{
		AA[0, 0] = 3;
		AA[0, 1] = V2;
		AA[0, 2] = V2;
		AA[0, 3] = 0;
		AA[0, 4] = V2;
		AA[0, 5] = 5;
		AA[0, 6] = 5;
		AA[0, 7] = 5;
		AA[1, 0] = V2;
		AA[1, 1] = V2;
		AA[1, 2] = 0;
		AA[1, 3] = V2;
		AA[1, 4] = 3;
		AA[1, 5] = 5;
		AA[1, 6] = 5;
		AA[1, 7] = 5;
		AA[2, 0] = V2;
		AA[2, 1] = V2;
		AA[2, 2] = V2;
		AA[2, 3] = 0;
		AA[2, 4] = 3;
		AA[2, 5] = 5;
		AA[2, 6] = 5;
		AA[2, 7] = 5;
		AA[3, 0] = V1;
		AA[3, 1] = V2;
		AA[3, 2] = V2;
		AA[3, 3] = V2;
		AA[3, 4] = 3;
		AA[3, 5] = 0;
		AA[3, 6] = 5;
		AA[3, 7] = 5;
		AA[4, 0] = V2;
		AA[4, 1] = V2;
		AA[4, 2] = 3;
		AA[4, 3] = V2;
		AA[4, 4] = 0;
		AA[4, 5] = 5;
		AA[4, 6] = 5;
		AA[4, 7] = 5;
		AA[5, 0] = V2;
		AA[5, 1] = 3;
		AA[5, 2] = V2;
		AA[5, 3] = V2;
		AA[5, 4] = 0;
		AA[5, 5] = 5;
		AA[5, 6] = 5;
		AA[5, 7] = 5;
		AA[6, 0] = V2;
		AA[6, 1] = V2;
		AA[6, 2] = 0;
		AA[6, 3] = 0;
		AA[6, 4] = V2;
		AA[6, 5] = 5;
		AA[6, 6] = 5;
		AA[6, 7] = 5;
		AA[7, 0] = V2;
		AA[7, 1] = 0;
		AA[7, 2] = V2;
		AA[7, 3] = 0;
		AA[7, 4] = V2;
		AA[7, 5] = 5;
		AA[7, 6] = 5;
		AA[7, 7] = 5;
		AA[8, 0] = V2;
		AA[8, 1] = 0;
		AA[8, 2] = V1;
		AA[8, 3] = V1;
		AA[8, 4] = V1;
		AA[8, 5] = 0;
		AA[8, 6] = 0;
		AA[8, 7] = 5;
		AA[9, 0] = 3;
		AA[9, 1] = 0;
		AA[9, 2] = V1;
		AA[9, 3] = V1;
		AA[9, 4] = V1;
		AA[9, 5] = 0;
		AA[9, 6] = 3;
		AA[9, 7] = 5;
		AA[10, 0] = 0;
		AA[10, 1] = V1;
		AA[10, 2] = 0;
		AA[10, 3] = V1;
		AA[10, 4] = V1;
		AA[10, 5] = 0;
		AA[10, 6] = 5;
		AA[10, 7] = 5;
		AA[11, 0] = 3;
		AA[11, 1] = 3;
		AA[11, 2] = V1;
		AA[11, 3] = 0;
		AA[11, 4] = V1;
		AA[11, 5] = V1;
		AA[11, 6] = 3;
		AA[11, 7] = 3;
		AA[12, 0] = 3;
		AA[12, 1] = 3;
		AA[12, 2] = V1;
		AA[12, 3] = V1;
		AA[12, 4] = 0;
		AA[12, 5] = V1;
		AA[12, 6] = 3;
		AA[12, 7] = 3;
		AA[13, 0] = V2;
		AA[13, 1] = 3;
		AA[13, 2] = V1;
		AA[13, 3] = V1;
		AA[13, 4] = V1;
		AA[13, 5] = 0;
		AA[13, 6] = V2;
		AA[13, 7] = 3;
		AA[14, 0] = 3;
		AA[14, 1] = V2;
		AA[14, 2] = V2;
		AA[14, 3] = 0;
		AA[14, 4] = 3;
		AA[14, 5] = 5;
		AA[14, 6] = 5;
		AA[14, 7] = 5;
		AA[15, 0] = 3;
		AA[15, 1] = V2;
		AA[15, 2] = 0;
		AA[15, 3] = V2;
		AA[15, 4] = 3;
		AA[15, 5] = 5;
		AA[15, 6] = 5;
		AA[15, 7] = 5;
		AA[16, 0] = 3;
		AA[16, 1] = V2;
		AA[16, 2] = 0;
		AA[16, 3] = 0;
		AA[16, 4] = V2;
		AA[16, 5] = 3;
		AA[16, 6] = 5;
		AA[16, 7] = 5;
		AA[17, 0] = 3;
		AA[17, 1] = V2;
		AA[17, 2] = 3;
		AA[17, 3] = V2;
		AA[17, 4] = 0;
		AA[17, 5] = 3;
		AA[17, 6] = 5;
		AA[17, 7] = 5;
		AA[18, 0] = 3;
		AA[18, 1] = V2;
		AA[18, 2] = V2;
		AA[18, 3] = 3;
		AA[18, 4] = 0;
		AA[18, 5] = 3;
		AA[18, 6] = 5;
		AA[18, 7] = 5;
		AA[19, 0] = V2;
		AA[19, 1] = V1;
		AA[19, 2] = V1;
		AA[19, 3] = V1;
		AA[19, 4] = 0;
		AA[19, 5] = 0;
		AA[19, 6] = 5;
		AA[19, 7] = 5;
		AA[20, 0] = V2;
		AA[20, 1] = V1;
		AA[20, 2] = V1;
		AA[20, 3] = 0;
		AA[20, 4] = V1;
		AA[20, 5] = 0;
		AA[20, 6] = 5;
		AA[20, 7] = 5;
		AA[21, 0] = V2;
		AA[21, 1] = V1;
		AA[21, 2] = 0;
		AA[21, 3] = V1;
		AA[21, 4] = V1;
		AA[21, 5] = 0;
		AA[21, 6] = 5;
		AA[21, 7] = 5;
		AA[22, 0] = V2;
		AA[22, 1] = 0;
		AA[22, 2] = V1;
		AA[22, 3] = V1;
		AA[22, 4] = V1;
		AA[22, 5] = 0;
		AA[22, 6] = 5;
		AA[22, 7] = 5;
		AA[23, 0] = 3;
		AA[23, 1] = V1;
		AA[23, 2] = V1;
		AA[23, 3] = 0;
		AA[23, 4] = 3;
		AA[23, 5] = 5;
		AA[23, 6] = 5;
		AA[23, 7] = 5;
		AA[24, 0] = 3;
		AA[24, 1] = V1;
		AA[24, 2] = 0;
		AA[24, 3] = V1;
		AA[24, 4] = 3;
		AA[24, 5] = 5;
		AA[24, 6] = 5;
		AA[24, 7] = 5;
		AA[25, 0] = 3;
		AA[25, 1] = V1;
		AA[25, 2] = 0;
		AA[25, 3] = 0;
		AA[25, 4] = V1;
		AA[25, 5] = 3;
		AA[25, 6] = 5;
		AA[25, 7] = 5;
		AA[26, 0] = 3;
		AA[26, 1] = V1;
		AA[26, 2] = 3;
		AA[26, 3] = V1;
		AA[26, 4] = 0;
		AA[26, 5] = 3;
		AA[26, 6] = 5;
		AA[26, 7] = 5;
		AA[27, 0] = 3;
		AA[27, 1] = V1;
		AA[27, 2] = V1;
		AA[27, 3] = 3;
		AA[27, 4] = 0;
		AA[27, 5] = 3;
		AA[27, 6] = 5;
		AA[27, 7] = 5;
		AA[28, 0] = 3;
		AA[28, 1] = V2;
		AA[28, 2] = V2;
		AA[28, 3] = 3;
		AA[28, 4] = 3;
		AA[28, 5] = 0;
		AA[28, 6] = 3;
		AA[28, 7] = 5;
		AA[29, 0] = 3;
		AA[29, 1] = V2;
		AA[29, 2] = 0;
		AA[29, 3] = 3;
		AA[29, 4] = 5;
		AA[29, 5] = 5;
		AA[29, 6] = 5;
		AA[29, 7] = 5;
		AA[30, 0] = 3;
		AA[30, 1] = V2;
		AA[30, 2] = 3;
		AA[30, 3] = 0;
		AA[30, 4] = 3;
		AA[30, 5] = 5;
		AA[30, 6] = 5;
		AA[30, 7] = 5;
		AA[31, 0] = 3;
		AA[31, 1] = V1;
		AA[31, 2] = 0;
		AA[31, 3] = 3;
		AA[31, 4] = 5;
		AA[31, 5] = 5;
		AA[31, 6] = 5;
		AA[31, 7] = 5;
		AA[32, 0] = 3;
		AA[32, 1] = V1;
		AA[32, 2] = 3;
		AA[32, 3] = 0;
		AA[32, 4] = 3;
		AA[32, 5] = 5;
		AA[32, 6] = 5;
		AA[32, 7] = 5;
		AA[33, 0] = 3;
		AA[33, 1] = V2;
		AA[33, 2] = 3;
		AA[33, 3] = 0;
		AA[33, 4] = 3;
		AA[33, 5] = V2;
		AA[33, 6] = 3;
		AA[33, 7] = 5;
		AA[34, 0] = 3;
		AA[34, 1] = V2;
		AA[34, 2] = 3;
		AA[34, 3] = 3;
		AA[34, 4] = 0;
		AA[34, 5] = V2;
		AA[34, 6] = 3;
		AA[34, 7] = 5;
		AA[35, 0] = 3;
		AA[35, 1] = V2;
		AA[35, 2] = 3;
		AA[35, 3] = 3;
		AA[35, 4] = 0;
		AA[35, 5] = 3;
		AA[35, 6] = 5;
		AA[35, 7] = 5;
		AA[36, 0] = 3;
		AA[36, 1] = V1;
		AA[36, 2] = V1;
		AA[36, 3] = 3;
		AA[36, 4] = 3;
		AA[36, 5] = 0;
		AA[36, 6] = 3;
		AA[36, 7] = 5;
		AA[37, 0] = 3;
		AA[37, 1] = V1;
		AA[37, 2] = 3;
		AA[37, 3] = 0;
		AA[37, 4] = 3;
		AA[37, 5] = V1;
		AA[37, 6] = 3;
		AA[37, 7] = 5;
		AA[38, 0] = 3;
		AA[38, 1] = V1;
		AA[38, 2] = 3;
		AA[38, 3] = 3;
		AA[38, 4] = 0;
		AA[38, 5] = V1;
		AA[38, 6] = 3;
		AA[38, 7] = 5;
		AA[39, 0] = V1;
		AA[39, 1] = 0;
		AA[39, 2] = V1;
		AA[39, 3] = 0;
		AA[39, 4] = V1;
		AA[39, 5] = 0;
		AA[39, 6] = V1;
		AA[39, 7] = 5;
	}

	private void attacker(int[] PISTEET)
	{
		PISTEET[0] = 2000;
		PISTEET[1] = 2000;
		PISTEET[2] = 2000;
		PISTEET[3] = 2000;
		PISTEET[4] = 2000;
		PISTEET[5] = 2000;
		PISTEET[6] = 1900;
		PISTEET[7] = 1900;
		PISTEET[8] = 4000;
		PISTEET[9] = 4000;
		PISTEET[10] = 4000;
		PISTEET[11] = 30;
		PISTEET[12] = 30;
		PISTEET[13] = 30;
		PISTEET[14] = 500;
		PISTEET[15] = 270;
		PISTEET[16] = 490;
		PISTEET[17] = 490;
		PISTEET[18] = 490;
		PISTEET[19] = 350;
		PISTEET[20] = 350;
		PISTEET[21] = 350;
		PISTEET[22] = 350;
		PISTEET[23] = 200;
		PISTEET[24] = 200;
		PISTEET[25] = 190;
		PISTEET[26] = 190;
		PISTEET[27] = 80;
		PISTEET[28] = 80;
		PISTEET[29] = 60;
		PISTEET[30] = 50;
		PISTEET[31] = 40;
		PISTEET[32] = 25;
		PISTEET[33] = 9;
		PISTEET[34] = 10;
		PISTEET[35] = 10;
		PISTEET[36] = 10;
		PISTEET[37] = 4;
		PISTEET[38] = 2;
		PISTEET[39] = 2000;
	}

	private void defender(int[] PISTEET)
	{
		PISTEET[0] = 1500;
		PISTEET[1] = 1500;
		PISTEET[2] = 1500;
		PISTEET[3] = 1450;
		PISTEET[4] = 1400;
		PISTEET[5] = 1400;
		PISTEET[6] = 1000;
		PISTEET[7] = 1000;
		PISTEET[8] = 2200;
		PISTEET[9] = 2200;
		PISTEET[10] = 2200;
		PISTEET[11] = 80;
		PISTEET[12] = 80;
		PISTEET[13] = 80;
		PISTEET[14] = 400;
		PISTEET[15] = 220;
		PISTEET[16] = 385;
		PISTEET[17] = 385;
		PISTEET[18] = 385;
		PISTEET[19] = 410;
		PISTEET[20] = 400;
		PISTEET[21] = 400;
		PISTEET[22] = 400;
		PISTEET[23] = 385;
		PISTEET[24] = 198;
		PISTEET[25] = 380;
		PISTEET[26] = 380;
		PISTEET[27] = 90;
		PISTEET[28] = 80;
		if (kokovuoro > 10)
		{
			PISTEET[31] = 45;
			PISTEET[32] = 35;
			PISTEET[29] = 30;
			PISTEET[30] = 15;
		}
		else
		{
			PISTEET[31] = 30;
			PISTEET[32] = 15;
			PISTEET[29] = 45;
			PISTEET[30] = 35;
		}
		PISTEET[33] = 8;
		PISTEET[34] = 10;
		PISTEET[35] = 10;
		PISTEET[36] = 10;
		PISTEET[37] = 4;
		PISTEET[38] = 4;
		PISTEET[39] = 2000;
	}

	private void preasure(int[] PISTEET)
	{
		PISTEET[0] = 1700;
		PISTEET[1] = 1700;
		PISTEET[2] = 1700;
		PISTEET[3] = 1600;
		PISTEET[4] = 1700;
		PISTEET[5] = 1700;
		PISTEET[6] = 1700;
		PISTEET[7] = 1700;
		PISTEET[8] = 2000;
		PISTEET[9] = 2000;
		PISTEET[10] = 2000;
		PISTEET[11] = 50;
		PISTEET[12] = 50;
		PISTEET[13] = 50;
		PISTEET[14] = 500;
		PISTEET[15] = 270;
		PISTEET[16] = 490;
		PISTEET[17] = 490;
		PISTEET[18] = 490;
		PISTEET[19] = 480;
		PISTEET[20] = 380;
		PISTEET[21] = 380;
		PISTEET[22] = 380;
		PISTEET[23] = 240;
		PISTEET[24] = 130;
		PISTEET[25] = 220;
		PISTEET[26] = 210;
		PISTEET[27] = 90;
		PISTEET[28] = 100;
		PISTEET[29] = 80;
		PISTEET[30] = 50;
		PISTEET[31] = 30;
		PISTEET[32] = 25;
		PISTEET[33] = 9;
		PISTEET[34] = 10;
		PISTEET[35] = 10;
		PISTEET[36] = 10;
		PISTEET[37] = 3;
		PISTEET[38] = 2;
		PISTEET[39] = 2000;
	}

	private void tbuilder(int[] PISTEET)
	{
		PISTEET[0] = 1000;
		PISTEET[1] = 1000;
		PISTEET[2] = 950;
		PISTEET[3] = 1000;
		PISTEET[4] = 1000;
		PISTEET[5] = 1000;
		PISTEET[6] = 900;
		PISTEET[7] = 900;
		PISTEET[8] = 2020;
		PISTEET[9] = 2000;
		PISTEET[10] = 2000;
		PISTEET[11] = 10;
		PISTEET[12] = 10;
		PISTEET[13] = 10;
		PISTEET[14] = 420;
		PISTEET[15] = 211;
		PISTEET[16] = 490;
		PISTEET[17] = 490;
		PISTEET[18] = 490;
		PISTEET[19] = 350;
		PISTEET[20] = 350;
		PISTEET[21] = 350;
		PISTEET[22] = 350;
		PISTEET[23] = 200;
		PISTEET[24] = 102;
		PISTEET[25] = 110;
		PISTEET[26] = 140;
		PISTEET[27] = 80;
		PISTEET[28] = 90;
		PISTEET[29] = 50;
		PISTEET[30] = 35;
		PISTEET[31] = 34;
		PISTEET[32] = 26;
		PISTEET[33] = 14;
		PISTEET[34] = 23;
		PISTEET[35] = 23;
		PISTEET[36] = 12;
		PISTEET[37] = 4;
		PISTEET[38] = 3;
		PISTEET[39] = 2000;
	}

	private void haepriority(int syvyys, int rivi0, int suunta0, int X, int Y, int[,] PRIORITY)
	{
		if (suunta0 == 0)
		{
			if (rivi0 == 3 && PRIORITY[X + 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X + 2, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X + 1, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 1, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X + 2, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 7 && PRIORITY[X + 3, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 8 && PRIORITY[X + 3, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 9 && PRIORITY[X + 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 10 && PRIORITY[X + 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 11 && PRIORITY[X + 1, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 1, Y] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 1)
		{
			if (rivi0 == 3 && PRIORITY[X, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X, Y + 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 1] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 7 && PRIORITY[X, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 8 && PRIORITY[X, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 9 && PRIORITY[X, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 10 && PRIORITY[X, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 11 && PRIORITY[X, Y + 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 1] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 2)
		{
			if (rivi0 == 3 && PRIORITY[X + 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X + 2, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X + 1, Y + 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 1, Y + 1] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X + 2, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 7 && PRIORITY[X + 3, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 8 && PRIORITY[X + 3, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 9 && PRIORITY[X + 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 10 && PRIORITY[X + 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 11 && PRIORITY[X + 1, Y + 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 1, Y + 1] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 3)
		{
			if (rivi0 == 3 && PRIORITY[X + 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X + 2, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X + 1, Y - 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 1, Y - 1] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X + 2, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 7 && PRIORITY[X + 3, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 8 && PRIORITY[X + 3, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 9 && PRIORITY[X + 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 10 && PRIORITY[X + 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 11 && PRIORITY[X + 1, Y - 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 1, Y - 1] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 4)
		{
			if (rivi0 == 3 && PRIORITY[X - 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X - 2, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X - 1, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 1, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X - 2, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 7 && PRIORITY[X - 3, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 8 && PRIORITY[X - 3, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 9 && PRIORITY[X - 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 10 && PRIORITY[X - 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 11 && PRIORITY[X - 1, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 1, Y] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 5)
		{
			if (rivi0 == 3 && PRIORITY[X, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X, Y - 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 1] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 7 && PRIORITY[X, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 8 && PRIORITY[X, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 9 && PRIORITY[X, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 10 && PRIORITY[X, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 11 && PRIORITY[X, Y - 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 1] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 6)
		{
			if (rivi0 == 3 && PRIORITY[X - 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X - 2, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X - 1, Y - 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 1, Y - 1] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X - 2, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 7 && PRIORITY[X - 3, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 8 && PRIORITY[X - 3, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 9 && PRIORITY[X - 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 10 && PRIORITY[X - 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 11 && PRIORITY[X - 1, Y - 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 1, Y - 1] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 7)
		{
			if (rivi0 == 3 && PRIORITY[X - 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X - 2, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X - 1, Y + 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 1, Y + 1] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X - 2, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 7 && PRIORITY[X - 3, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 8 && PRIORITY[X - 3, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 9 && PRIORITY[X - 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 10 && PRIORITY[X - 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 11 && PRIORITY[X - 1, Y + 1] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 1, Y + 1] = 9500 - syvyys * 1000;
			}
		}
	}

	private void haepriority2(int syvyys, int rivi0, int suunta0, int X, int Y, int[,] PRIORITY)
	{
		if (suunta0 == 0)
		{
			if (rivi0 == 2 && PRIORITY[X + 3, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 3 && PRIORITY[X + 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X + 2, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X + 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X + 2, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 1)
		{
			if (rivi0 == 2 && PRIORITY[X, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 3 && PRIORITY[X, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y + 2] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 2)
		{
			if (rivi0 == 2 && PRIORITY[X + 3, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 3 && PRIORITY[X + 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X + 2, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X + 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X + 2, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y + 2] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 3)
		{
			if (rivi0 == 2 && PRIORITY[X + 3, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 3, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 3 && PRIORITY[X + 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X + 2, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X + 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X + 2, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X + 2, Y - 2] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 4)
		{
			if (rivi0 == 2 && PRIORITY[X - 3, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 3 && PRIORITY[X - 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X - 2, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X - 4, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X - 2, Y] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 5)
		{
			if (rivi0 == 2 && PRIORITY[X, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 3 && PRIORITY[X, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X, Y - 2] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 6)
		{
			if (rivi0 == 2 && PRIORITY[X - 3, Y - 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y - 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 3 && PRIORITY[X - 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X - 2, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y - 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X - 4, Y - 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y - 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X - 2, Y - 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y - 2] = 9500 - syvyys * 1000;
			}
		}
		if (suunta0 == 7)
		{
			if (rivi0 == 2 && PRIORITY[X - 3, Y + 3] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 3, Y + 3] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 3 && PRIORITY[X - 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 4 && PRIORITY[X - 2, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y + 2] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 5 && PRIORITY[X - 4, Y + 4] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 4, Y + 4] = 9500 - syvyys * 1000;
			}
			if (rivi0 == 6 && PRIORITY[X - 2, Y + 2] < 9500 - syvyys * 1000)
			{
				PRIORITY[X - 2, Y + 2] = 9500 - syvyys * 1000;
			}
		}
	}

	private int pikahaku(int[,] B, int V2, int haettava)
	{
		V2 = ((V2 != 1) ? 1 : 2);
		int[,] array = new int[4, 8];
		int num = 0;
		int num2 = 5;
		if (haettava == 1)
		{
			num = 3;
			num2 = 5;
			array[0, 0] = V2;
			array[0, 1] = V2;
			array[0, 2] = V2;
			array[0, 3] = V2;
			array[0, 4] = 0;
			array[1, 0] = V2;
			array[1, 1] = V2;
			array[1, 2] = V2;
			array[1, 3] = 0;
			array[1, 4] = V2;
			array[2, 0] = V2;
			array[2, 1] = V2;
			array[2, 2] = 0;
			array[2, 3] = V2;
			array[2, 4] = V2;
		}
		if (haettava == 2)
		{
			num = 2;
			num2 = 6;
			array[0, 0] = 0;
			array[0, 1] = V2;
			array[0, 2] = V2;
			array[0, 3] = V2;
			array[0, 4] = 0;
			array[0, 5] = 0;
			array[1, 0] = 0;
			array[1, 1] = V2;
			array[1, 2] = V2;
			array[1, 3] = 0;
			array[1, 4] = V2;
			array[1, 5] = 0;
		}
		for (int i = 0; i < num; i++)
		{
			for (int j = 8; j < laudankoko + 8; j++)
			{
				for (int k = 8; k < laudankoko + 8; k++)
				{
					if (B[k, j] == array[i, 0] && B[k + 1, j] == array[i, 1] && B[k + 2, j] == array[i, 2] && B[k + 3, j] == array[i, 3] && B[k + 4, j] == array[i, 4])
					{
						if (num2 == 5)
						{
							return 9;
						}
						if (B[k + 5, j] == array[i, 5])
						{
							return 9;
						}
					}
					if (B[k, j] == array[i, 0] && B[k, j + 1] == array[i, 1] && B[k, j + 2] == array[i, 2] && B[k, j + 3] == array[i, 3] && B[k, j + 4] == array[i, 4])
					{
						if (num2 == 5)
						{
							return 9;
						}
						if (B[k, j + 5] == array[i, 5])
						{
							return 9;
						}
					}
					if (B[k, j] == array[i, 0] && B[k + 1, j + 1] == array[i, 1] && B[k + 2, j + 2] == array[i, 2] && B[k + 3, j + 3] == array[i, 3] && B[k + 4, j + 4] == array[i, 4])
					{
						if (num2 == 5)
						{
							return 9;
						}
						if (B[k + 5, j + 5] == array[i, 5])
						{
							return 9;
						}
					}
					if (B[k, j] == array[i, 0] && B[k + 1, j - 1] == array[i, 1] && B[k + 2, j - 2] == array[i, 2] && B[k + 3, j - 3] == array[i, 3] && B[k + 4, j - 4] == array[i, 4])
					{
						if (num2 == 5)
						{
							return 9;
						}
						if (B[k + 5, j - 5] == array[i, 5])
						{
							return 9;
						}
					}
					if (B[k, j] == array[i, 0] && B[k - 1, j] == array[i, 1] && B[k - 2, j] == array[i, 2] && B[k - 3, j] == array[i, 3] && B[k - 4, j] == array[i, 4])
					{
						if (num2 == 5)
						{
							return 9;
						}
						if (B[k - 5, j] == array[i, 5])
						{
							return 9;
						}
					}
					if (B[k, j] == array[i, 0] && B[k, j - 1] == array[i, 1] && B[k, j - 2] == array[i, 2] && B[k, j - 3] == array[i, 3] && B[k, j - 4] == array[i, 4])
					{
						if (num2 == 5)
						{
							return 9;
						}
						if (B[k, j - 5] == array[i, 5])
						{
							return 9;
						}
					}
					if (B[k, j] == array[i, 0] && B[k - 1, j - 1] == array[i, 1] && B[k - 2, j - 2] == array[i, 2] && B[k - 3, j - 3] == array[i, 3] && B[k - 4, j - 4] == array[i, 4])
					{
						if (num2 == 5)
						{
							return 9;
						}
						if (B[k - 5, j - 5] == array[i, 5])
						{
							return 9;
						}
					}
					if (B[k, j] == array[i, 0] && B[k - 1, j + 1] == array[i, 1] && B[k - 2, j + 2] == array[i, 2] && B[k - 3, j + 3] == array[i, 3] && B[k - 4, j + 4] == array[i, 4])
					{
						if (num2 == 5)
						{
							return 9;
						}
						if (B[k - 5, j + 5] == array[i, 5])
						{
							return 9;
						}
					}
				}
			}
		}
		return 0;
	}

	private int torjuntahaku(int[,] B, int V2, int[,] PRIORITY)
	{
		int[,] array = new int[4, 8];
		int num = 1;
		array[0, 0] = 0;
		array[0, 1] = V2;
		array[0, 2] = V2;
		array[0, 3] = V2;
		array[0, 4] = 0;
		array[0, 5] = 0;
		for (int i = 0; i < num; i++)
		{
			for (int j = 8; j < laudankoko + 8; j++)
			{
				for (int k = 8; k < laudankoko + 8; k++)
				{
					if (B[k, j] == array[i, 0] && B[k + 1, j] == array[i, 1] && B[k + 2, j] == array[i, 2] && B[k + 3, j] == array[i, 3] && B[k + 4, j] == array[i, 4] && B[k + 5, j] == array[i, 5])
					{
						B[k + 4, j] = V2;
						B[k + 5, j] = V2;
						int num2 = pikahaku(B, V2, 2);
						B[k + 4, j] = 0;
						B[k + 5, j] = 0;
						if (num2 == 9)
						{
							PRIORITY[k + 4, j] = 1000;
						}
					}
					if (B[k, j] == array[i, 0] && B[k, j + 1] == array[i, 1] && B[k, j + 2] == array[i, 2] && B[k, j + 3] == array[i, 3] && B[k, j + 4] == array[i, 4] && B[k, j + 5] == array[i, 5])
					{
						B[k, j + 4] = V2;
						B[k, j + 5] = V2;
						int num3 = pikahaku(B, V2, 2);
						B[k, j + 4] = 0;
						B[k, j + 5] = 0;
						if (num3 == 9)
						{
							PRIORITY[k, j + 4] = 1000;
						}
					}
					if (B[k, j] == array[i, 0] && B[k + 1, j + 1] == array[i, 1] && B[k + 2, j + 2] == array[i, 2] && B[k + 3, j + 3] == array[i, 3] && B[k + 4, j + 4] == array[i, 4] && B[k + 5, j + 5] == array[i, 5])
					{
						B[k + 4, j + 4] = V2;
						B[k + 5, j + 5] = V2;
						int num4 = pikahaku(B, V2, 2);
						B[k + 4, j + 4] = 0;
						B[k + 5, j + 5] = 0;
						if (num4 == 9)
						{
							PRIORITY[k + 4, j + 4] = 1000;
						}
					}
					if (B[k, j] == array[i, 0] && B[k + 1, j - 1] == array[i, 1] && B[k + 2, j - 2] == array[i, 2] && B[k + 3, j - 3] == array[i, 3] && B[k + 4, j - 4] == array[i, 4] && B[k + 5, j - 5] == array[i, 5])
					{
						B[k + 4, j - 4] = V2;
						B[k + 5, j - 5] = V2;
						int num5 = pikahaku(B, V2, 2);
						B[k + 4, j - 4] = 0;
						B[k + 5, j - 5] = 0;
						if (num5 == 9)
						{
							PRIORITY[k + 4, j - 4] = 1000;
						}
					}
					if (B[k, j] == array[i, 0] && B[k - 1, j] == array[i, 1] && B[k - 2, j] == array[i, 2] && B[k - 3, j] == array[i, 3] && B[k - 4, j] == array[i, 4] && B[k - 5, j] == array[i, 5])
					{
						B[k - 4, j] = V2;
						B[k - 5, j] = V2;
						int num6 = pikahaku(B, V2, 2);
						B[k - 4, j] = 0;
						B[k - 5, j] = 0;
						if (num6 == 9)
						{
							PRIORITY[k - 4, j] = 1000;
						}
					}
					if (B[k, j] == array[i, 0] && B[k, j - 1] == array[i, 1] && B[k, j - 2] == array[i, 2] && B[k, j - 3] == array[i, 3] && B[k, j - 4] == array[i, 4] && B[k, j - 5] == array[i, 5])
					{
						B[k, j - 4] = V2;
						B[k, j - 5] = V2;
						int num7 = pikahaku(B, V2, 2);
						B[k, j - 4] = 0;
						B[k, j - 5] = 0;
						if (num7 == 9)
						{
							PRIORITY[k, j - 4] = 1000;
						}
					}
					if (B[k, j] == array[i, 0] && B[k - 1, j - 1] == array[i, 1] && B[k - 2, j - 2] == array[i, 2] && B[k - 3, j - 3] == array[i, 3] && B[k - 4, j - 4] == array[i, 4] && B[k - 5, j - 5] == array[i, 5])
					{
						B[k - 4, j - 4] = V2;
						B[k - 5, j - 5] = V2;
						int num8 = pikahaku(B, V2, 2);
						B[k - 4, j - 4] = 0;
						B[k - 5, j - 5] = 0;
						if (num8 == 9)
						{
							PRIORITY[k - 4, j - 4] = 1000;
						}
					}
					if (B[k, j] == array[i, 0] && B[k - 1, j + 1] == array[i, 1] && B[k - 2, j + 2] == array[i, 2] && B[k - 3, j + 3] == array[i, 3] && B[k - 4, j + 4] == array[i, 4] && B[k - 5, j + 5] == array[i, 5])
					{
						B[k - 4, j + 4] = V2;
						B[k - 5, j + 5] = V2;
						int num9 = pikahaku(B, V2, 2);
						B[k - 4, j + 4] = 0;
						B[k - 5, j + 5] = 0;
						if (num9 == 9)
						{
							PRIORITY[k - 4, j + 4] = 1000;
						}
					}
				}
			}
		}
		return 0;
	}

	private int rekursiohaku(int syvyys, int syvyysmax, int[,] AA, int[,] BB, int[,] B, int[,] PRIORITY, int rivi0, int suunta0, int X, int Y, int puoli2)
	{
		if (syvyys > syvyysmax)
		{
			return 0;
		}
		for (int i = 0; i < 12; i++)
		{
			if (syvyys == 0)
			{
				rivi0 = i;
			}
			for (int j = 8; j < laudankoko + 8; j++)
			{
				if (syvyys == 0)
				{
					Y = j;
				}
				for (int k = 8; k < laudankoko + 8; k++)
				{
					if (syvyys == 0)
					{
						X = k;
					}
					if (B[k, j] == AA[i, 0] && B[k + 1, j] == AA[i, 1] && B[k + 2, j] == AA[i, 2] && B[k + 3, j] == AA[i, 3] && B[k + 4, j] == AA[i, 4])
					{
						if (i < 3 && syvyys > 0)
						{
							int num = pikahaku(B, puoli2, 1);
							if (num == 9)
							{
								return 0;
							}
							syvyysmax = syvyys - 1;
							haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 0;
						}
						for (int l = 0; l < 5; l++)
						{
							if (BB[i, l] != 5)
							{
								B[k + l, j] = BB[i, l];
							}
						}
						rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int m = 0; m < 5; m++)
						{
							if (BB[i, m] != 5)
							{
								B[k + m, j] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k, j + 1] == AA[i, 1] && B[k, j + 2] == AA[i, 2] && B[k, j + 3] == AA[i, 3] && B[k, j + 4] == AA[i, 4])
					{
						if (i < 3 && syvyys > 0)
						{
							int num2 = pikahaku(B, puoli2, 1);
							if (num2 == 9)
							{
								return 0;
							}
							syvyysmax = syvyys - 1;
							haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 1;
						}
						for (int n = 0; n < 5; n++)
						{
							if (BB[i, n] != 5)
							{
								B[k, j + n] = BB[i, n];
							}
						}
						rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num3 = 0; num3 < 5; num3++)
						{
							if (BB[i, num3] != 5)
							{
								B[k, j + num3] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k + 1, j + 1] == AA[i, 1] && B[k + 2, j + 2] == AA[i, 2] && B[k + 3, j + 3] == AA[i, 3] && B[k + 4, j + 4] == AA[i, 4])
					{
						if (i < 3 && syvyys > 0)
						{
							int num4 = pikahaku(B, puoli2, 1);
							if (num4 == 9)
							{
								return 0;
							}
							syvyysmax = syvyys - 1;
							haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 2;
						}
						for (int num5 = 0; num5 < 5; num5++)
						{
							if (BB[i, num5] != 5)
							{
								B[k + num5, j + num5] = BB[i, num5];
							}
						}
						rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num6 = 0; num6 < 5; num6++)
						{
							if (BB[i, num6] != 5)
							{
								B[k + num6, j + num6] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k + 1, j - 1] == AA[i, 1] && B[k + 2, j - 2] == AA[i, 2] && B[k + 3, j - 3] == AA[i, 3] && B[k + 4, j - 4] == AA[i, 4])
					{
						if (i < 3 && syvyys > 0)
						{
							int num7 = pikahaku(B, puoli2, 1);
							if (num7 == 9)
							{
								return 0;
							}
							syvyysmax = syvyys - 1;
							haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 3;
						}
						for (int num8 = 0; num8 < 5; num8++)
						{
							if (BB[i, num8] != 5)
							{
								B[k + num8, j - num8] = BB[i, num8];
							}
						}
						rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num9 = 0; num9 < 5; num9++)
						{
							if (BB[i, num9] != 5)
							{
								B[k + num9, j - num9] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k - 1, j] == AA[i, 1] && B[k - 2, j] == AA[i, 2] && B[k - 3, j] == AA[i, 3] && B[k - 4, j] == AA[i, 4])
					{
						if (i < 3 && syvyys > 0)
						{
							int num10 = pikahaku(B, puoli2, 1);
							if (num10 == 9)
							{
								return 0;
							}
							syvyysmax = syvyys - 1;
							haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 4;
						}
						for (int num11 = 0; num11 < 5; num11++)
						{
							if (BB[i, num11] != 5)
							{
								B[k - num11, j] = BB[i, num11];
							}
						}
						rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num12 = 0; num12 < 5; num12++)
						{
							if (BB[i, num12] != 5)
							{
								B[k - num12, j] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k, j - 1] == AA[i, 1] && B[k, j - 2] == AA[i, 2] && B[k, j - 3] == AA[i, 3] && B[k, j - 4] == AA[i, 4])
					{
						if (i < 3 && syvyys > 0)
						{
							int num13 = pikahaku(B, puoli2, 1);
							if (num13 == 9)
							{
								return 0;
							}
							syvyysmax = syvyys - 1;
							haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 5;
						}
						for (int num14 = 0; num14 < 5; num14++)
						{
							if (BB[i, num14] != 5)
							{
								B[k, j - num14] = BB[i, num14];
							}
						}
						rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num15 = 0; num15 < 5; num15++)
						{
							if (BB[i, num15] != 5)
							{
								B[k, j - num15] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k - 1, j - 1] == AA[i, 1] && B[k - 2, j - 2] == AA[i, 2] && B[k - 3, j - 3] == AA[i, 3] && B[k - 4, j - 4] == AA[i, 4])
					{
						if (i < 3 && syvyys > 0)
						{
							int num16 = pikahaku(B, puoli2, 1);
							if (num16 == 9)
							{
								return 0;
							}
							syvyysmax = syvyys - 1;
							haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 6;
						}
						for (int num17 = 0; num17 < 5; num17++)
						{
							if (BB[i, num17] != 5)
							{
								B[k - num17, j - num17] = BB[i, num17];
							}
						}
						rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num18 = 0; num18 < 5; num18++)
						{
							if (BB[i, num18] != 5)
							{
								B[k - num18, j - num18] = 0;
							}
						}
					}
					if (B[k, j] != AA[i, 0] || B[k - 1, j + 1] != AA[i, 1] || B[k - 2, j + 2] != AA[i, 2] || B[k - 3, j + 3] != AA[i, 3] || B[k - 4, j + 4] != AA[i, 4])
					{
						continue;
					}
					if (i < 3 && syvyys > 0)
					{
						int num19 = pikahaku(B, puoli2, 1);
						if (num19 == 9)
						{
							return 0;
						}
						syvyysmax = syvyys - 1;
						haepriority(syvyys, rivi0, suunta0, X, Y, PRIORITY);
						return syvyys;
					}
					if (syvyys == 0)
					{
						suunta0 = 7;
					}
					for (int num20 = 0; num20 < 5; num20++)
					{
						if (BB[i, num20] != 5)
						{
							B[k - num20, j + num20] = BB[i, num20];
						}
					}
					rekursiohaku(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
					for (int num21 = 0; num21 < 5; num21++)
					{
						if (BB[i, num21] != 5)
						{
							B[k - num21, j + num21] = 0;
						}
					}
				}
			}
		}
		return 0;
	}

	private int rekursiohaku2(int syvyys, int syvyysmax, int[,] AA, int[,] BB, int[,] B, int[,] PRIORITY, int rivi0, int suunta0, int X, int Y, int puoli2)
	{
		if (syvyys > syvyysmax)
		{
			return 0;
		}
		for (int i = 0; i < 7; i++)
		{
			if (syvyys == 0)
			{
				rivi0 = i;
			}
			for (int j = 8; j < laudankoko + 8; j++)
			{
				if (syvyys == 0)
				{
					Y = j;
				}
				for (int k = 8; k < laudankoko + 8; k++)
				{
					if (syvyys == 0)
					{
						X = k;
					}
					if (B[k, j] == AA[i, 0] && B[k + 1, j] == AA[i, 1] && B[k + 2, j] == AA[i, 2] && B[k + 3, j] == AA[i, 3] && B[k + 4, j] == AA[i, 4] && B[k + 5, j] == AA[i, 5])
					{
						if (i < 2 && syvyys > 0)
						{
							syvyysmax = syvyys - 1;
							haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 0;
						}
						for (int l = 0; l < 6; l++)
						{
							if (BB[i, l] != 5)
							{
								B[k + l, j] = BB[i, l];
							}
						}
						rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int m = 0; m < 6; m++)
						{
							if (BB[i, m] != 5)
							{
								B[k + m, j] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k, j + 1] == AA[i, 1] && B[k, j + 2] == AA[i, 2] && B[k, j + 3] == AA[i, 3] && B[k, j + 4] == AA[i, 4] && B[k, j + 5] == AA[i, 5])
					{
						if (i < 2 && syvyys > 0)
						{
							syvyysmax = syvyys - 1;
							haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 1;
						}
						for (int n = 0; n < 6; n++)
						{
							if (BB[i, n] != 5)
							{
								B[k, j + n] = BB[i, n];
							}
						}
						rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num = 0; num < 6; num++)
						{
							if (BB[i, num] != 5)
							{
								B[k, j + num] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k + 1, j + 1] == AA[i, 1] && B[k + 2, j + 2] == AA[i, 2] && B[k + 3, j + 3] == AA[i, 3] && B[k + 4, j + 4] == AA[i, 4] && B[k + 5, j + 5] == AA[i, 5])
					{
						if (i < 2 && syvyys > 0)
						{
							syvyysmax = syvyys - 1;
							haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 2;
						}
						for (int num2 = 0; num2 < 6; num2++)
						{
							if (BB[i, num2] != 5)
							{
								B[k + num2, j + num2] = BB[i, num2];
							}
						}
						rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num3 = 0; num3 < 6; num3++)
						{
							if (BB[i, num3] != 5)
							{
								B[k + num3, j + num3] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k + 1, j - 1] == AA[i, 1] && B[k + 2, j - 2] == AA[i, 2] && B[k + 3, j - 3] == AA[i, 3] && B[k + 4, j - 4] == AA[i, 4] && B[k + 5, j - 5] == AA[i, 5])
					{
						if (i < 2 && syvyys > 0)
						{
							syvyysmax = syvyys - 1;
							haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 3;
						}
						for (int num4 = 0; num4 < 6; num4++)
						{
							if (BB[i, num4] != 5)
							{
								B[k + num4, j - num4] = BB[i, num4];
							}
						}
						rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num5 = 0; num5 < 6; num5++)
						{
							if (BB[i, num5] != 5)
							{
								B[k + num5, j - num5] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k - 1, j] == AA[i, 1] && B[k - 2, j] == AA[i, 2] && B[k - 3, j] == AA[i, 3] && B[k - 4, j] == AA[i, 4] && B[k - 5, j] == AA[i, 5])
					{
						if (i < 2 && syvyys > 0)
						{
							syvyysmax = syvyys - 1;
							haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 4;
						}
						for (int num6 = 0; num6 < 6; num6++)
						{
							if (BB[i, num6] != 5)
							{
								B[k - num6, j] = BB[i, num6];
							}
						}
						rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num7 = 0; num7 < 6; num7++)
						{
							if (BB[i, num7] != 5)
							{
								B[k - num7, j] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k, j - 1] == AA[i, 1] && B[k, j - 2] == AA[i, 2] && B[k, j - 3] == AA[i, 3] && B[k, j - 4] == AA[i, 4] && B[k, j - 5] == AA[i, 5])
					{
						if (i < 2 && syvyys > 0)
						{
							syvyysmax = syvyys - 1;
							haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 5;
						}
						for (int num8 = 0; num8 < 6; num8++)
						{
							if (BB[i, num8] != 5)
							{
								B[k, j - num8] = BB[i, num8];
							}
						}
						rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num9 = 0; num9 < 6; num9++)
						{
							if (BB[i, num9] != 5)
							{
								B[k, j - num9] = 0;
							}
						}
					}
					if (B[k, j] == AA[i, 0] && B[k - 1, j - 1] == AA[i, 1] && B[k - 2, j - 2] == AA[i, 2] && B[k - 3, j - 3] == AA[i, 3] && B[k - 4, j - 4] == AA[i, 4] && B[k - 5, j - 5] == AA[i, 5])
					{
						if (i < 2 && syvyys > 0)
						{
							syvyysmax = syvyys - 1;
							haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY);
							return syvyys;
						}
						if (syvyys == 0)
						{
							suunta0 = 6;
						}
						for (int num10 = 0; num10 < 6; num10++)
						{
							if (BB[i, num10] != 5)
							{
								B[k - num10, j - num10] = BB[i, num10];
							}
						}
						rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
						for (int num11 = 0; num11 < 6; num11++)
						{
							if (BB[i, num11] != 5)
							{
								B[k - num11, j - num11] = 0;
							}
						}
					}
					if (B[k, j] != AA[i, 0] || B[k - 1, j + 1] != AA[i, 1] || B[k - 2, j + 2] != AA[i, 2] || B[k - 3, j + 3] != AA[i, 3] || B[k - 4, j + 4] != AA[i, 4] || B[k - 5, j + 5] != AA[i, 5])
					{
						continue;
					}
					if (i < 2 && syvyys > 0)
					{
						syvyysmax = syvyys - 1;
						haepriority2(syvyys, rivi0, suunta0, X, Y, PRIORITY);
						return syvyys;
					}
					if (syvyys == 0)
					{
						suunta0 = 7;
					}
					for (int num12 = 0; num12 < 6; num12++)
					{
						if (BB[i, num12] != 5)
						{
							B[k - num12, j + num12] = BB[i, num12];
						}
					}
					rekursiohaku2(syvyys + 1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
					for (int num13 = 0; num13 < 6; num13++)
					{
						if (BB[i, num13] != 5)
						{
							B[k - num13, j + num13] = 0;
						}
					}
				}
			}
		}
		return 0;
	}

	private void lisaapriority(int[,] PRIORITY, int[,] AA, int C, int D, int suunta, int[,] B, int rivi, int[] PISTEET)
	{
		if (((AA[rivi, 0] == 3 && B[C, D] == 0) || AA[rivi, 0] == 5 || B[C, D] == AA[rivi, 0]) && ((AA[rivi, 1] == 3 && B[C + 1, D] == 0) || AA[rivi, 1] == 5 || B[C + 1, D] == AA[rivi, 1]) && ((AA[rivi, 2] == 3 && B[C + 2, D] == 0) || AA[rivi, 2] == 5 || B[C + 2, D] == AA[rivi, 2]) && ((AA[rivi, 3] == 3 && B[C + 3, D] == 0) || AA[rivi, 3] == 5 || B[C + 3, D] == AA[rivi, 3]) && ((AA[rivi, 4] == 3 && B[C + 4, D] == 0) || AA[rivi, 4] == 5 || B[C + 4, D] == AA[rivi, 4]) && ((AA[rivi, 5] == 3 && B[C + 5, D] == 0) || AA[rivi, 5] == 5 || B[C + 5, D] == AA[rivi, 5]) && ((AA[rivi, 6] == 3 && B[C + 6, D] == 0) || AA[rivi, 6] == 5 || B[C + 6, D] == AA[rivi, 6]) && ((AA[rivi, 7] == 3 && B[C + 7, D] == 0) || AA[rivi, 7] == 5 || B[C + 7, D] == AA[rivi, 7]))
		{
			for (int i = 0; i <= 7; i++)
			{
				if (AA[rivi, i] == 0 && B[C + i, D] == 0 && C + i > -1 && C + i < laudankoko + 8 && D > -1 && D < laudankoko + 8)
				{
					PRIORITY[C + i, D] += PISTEET[rivi];
				}
			}
		}
		if (((AA[rivi, 0] == 3 && B[C, D] == 0) || AA[rivi, 0] == 5 || B[C, D] == AA[rivi, 0]) && ((AA[rivi, 1] == 3 && B[C, D + 1] == 0) || AA[rivi, 1] == 5 || B[C, D + 1] == AA[rivi, 1]) && ((AA[rivi, 2] == 3 && B[C, D + 2] == 0) || AA[rivi, 2] == 5 || B[C, D + 2] == AA[rivi, 2]) && ((AA[rivi, 3] == 3 && B[C, D + 3] == 0) || AA[rivi, 3] == 5 || B[C, D + 3] == AA[rivi, 3]) && ((AA[rivi, 4] == 3 && B[C, D + 4] == 0) || AA[rivi, 4] == 5 || B[C, D + 4] == AA[rivi, 4]) && ((AA[rivi, 5] == 3 && B[C, D + 5] == 0) || AA[rivi, 5] == 5 || B[C, D + 5] == AA[rivi, 5]) && ((AA[rivi, 6] == 3 && B[C, D + 6] == 0) || AA[rivi, 6] == 5 || B[C, D + 6] == AA[rivi, 6]) && ((AA[rivi, 7] == 3 && B[C, D + 7] == 0) || AA[rivi, 7] == 5 || B[C, D + 7] == AA[rivi, 7]))
		{
			for (int j = 0; j <= 7; j++)
			{
				if (AA[rivi, j] == 0 && B[C, D + j] == 0 && C > -1 && C < laudankoko + 8 && D + j > -1 && D + j < laudankoko + 8)
				{
					PRIORITY[C, D + j] += PISTEET[rivi];
				}
			}
		}
		if (((AA[rivi, 0] == 3 && B[C, D] == 0) || AA[rivi, 0] == 5 || B[C, D] == AA[rivi, 0]) && ((AA[rivi, 1] == 3 && B[C + 1, D + 1] == 0) || AA[rivi, 1] == 5 || B[C + 1, D + 1] == AA[rivi, 1]) && ((AA[rivi, 2] == 3 && B[C + 2, D + 2] == 0) || AA[rivi, 2] == 5 || B[C + 2, D + 2] == AA[rivi, 2]) && ((AA[rivi, 3] == 3 && B[C + 3, D + 3] == 0) || AA[rivi, 3] == 5 || B[C + 3, D + 3] == AA[rivi, 3]) && ((AA[rivi, 4] == 3 && B[C + 4, D + 4] == 0) || AA[rivi, 4] == 5 || B[C + 4, D + 4] == AA[rivi, 4]) && ((AA[rivi, 5] == 3 && B[C + 5, D + 5] == 0) || AA[rivi, 5] == 5 || B[C + 5, D + 5] == AA[rivi, 5]) && ((AA[rivi, 6] == 3 && B[C + 6, D + 6] == 0) || AA[rivi, 6] == 5 || B[C + 6, D + 6] == AA[rivi, 6]) && ((AA[rivi, 7] == 3 && B[C + 7, D + 7] == 0) || AA[rivi, 7] == 5 || B[C + 7, D + 7] == AA[rivi, 7]))
		{
			for (int k = 0; k <= 7; k++)
			{
				if (AA[rivi, k] == 0 && B[C + k, D + k] == 0 && C + k > -1 && C + k < laudankoko + 8 && D + k > -1 && D + k < laudankoko + 8)
				{
					PRIORITY[C + k, D + k] += PISTEET[rivi];
				}
			}
		}
		if (((AA[rivi, 0] == 3 && B[C, D] == 0) || AA[rivi, 0] == 5 || B[C, D] == AA[rivi, 0]) && ((AA[rivi, 1] == 3 && B[C + 1, D - 1] == 0) || AA[rivi, 1] == 5 || B[C + 1, D - 1] == AA[rivi, 1]) && ((AA[rivi, 2] == 3 && B[C + 2, D - 2] == 0) || AA[rivi, 2] == 5 || B[C + 2, D - 2] == AA[rivi, 2]) && ((AA[rivi, 3] == 3 && B[C + 3, D - 3] == 0) || AA[rivi, 3] == 5 || B[C + 3, D - 3] == AA[rivi, 3]) && ((AA[rivi, 4] == 3 && B[C + 4, D - 4] == 0) || AA[rivi, 4] == 5 || B[C + 4, D - 4] == AA[rivi, 4]) && ((AA[rivi, 5] == 3 && B[C + 5, D - 5] == 0) || AA[rivi, 5] == 5 || B[C + 5, D - 5] == AA[rivi, 5]) && ((AA[rivi, 6] == 3 && B[C + 6, D - 6] == 0) || AA[rivi, 6] == 5 || B[C + 6, D - 6] == AA[rivi, 6]) && ((AA[rivi, 7] == 3 && B[C + 7, D - 7] == 0) || AA[rivi, 7] == 5 || B[C + 7, D - 7] == AA[rivi, 7]))
		{
			for (int l = 0; l <= 7; l++)
			{
				if (AA[rivi, l] == 0 && B[C + l, D - l] == 0 && C + l > -1 && C + l < laudankoko + 8 && D - l > -1 && D - l < laudankoko + 8)
				{
					PRIORITY[C + l, D - l] += PISTEET[rivi];
				}
			}
		}
		if (((AA[rivi, 0] == 3 && B[C, D] == 0) || AA[rivi, 0] == 5 || B[C, D] == AA[rivi, 0]) && ((AA[rivi, 1] == 3 && B[C - 1, D] == 0) || AA[rivi, 1] == 5 || B[C - 1, D] == AA[rivi, 1]) && ((AA[rivi, 2] == 3 && B[C - 2, D] == 0) || AA[rivi, 2] == 5 || B[C - 2, D] == AA[rivi, 2]) && ((AA[rivi, 3] == 3 && B[C - 3, D] == 0) || AA[rivi, 3] == 5 || B[C - 3, D] == AA[rivi, 3]) && ((AA[rivi, 4] == 3 && B[C - 4, D] == 0) || AA[rivi, 4] == 5 || B[C - 4, D] == AA[rivi, 4]) && ((AA[rivi, 5] == 3 && B[C - 5, D] == 0) || AA[rivi, 5] == 5 || B[C - 5, D] == AA[rivi, 5]) && ((AA[rivi, 6] == 3 && B[C - 6, D] == 0) || AA[rivi, 6] == 5 || B[C - 6, D] == AA[rivi, 6]) && ((AA[rivi, 7] == 3 && B[C - 7, D] == 0) || AA[rivi, 7] == 5 || B[C - 7, D] == AA[rivi, 7]))
		{
			for (int m = 0; m <= 7; m++)
			{
				if (AA[rivi, m] == 0 && B[C - m, D] == 0 && C - m > -1 && C - m < laudankoko + 8 && D > -1 && D < laudankoko + 8)
				{
					PRIORITY[C - m, D] += PISTEET[rivi];
				}
			}
		}
		if (((AA[rivi, 0] == 3 && B[C, D] == 0) || AA[rivi, 0] == 5 || B[C, D] == AA[rivi, 0]) && ((AA[rivi, 1] == 3 && B[C, D - 1] == 0) || AA[rivi, 1] == 5 || B[C, D - 1] == AA[rivi, 1]) && ((AA[rivi, 2] == 3 && B[C, D - 2] == 0) || AA[rivi, 2] == 5 || B[C, D - 2] == AA[rivi, 2]) && ((AA[rivi, 3] == 3 && B[C, D - 3] == 0) || AA[rivi, 3] == 5 || B[C, D - 3] == AA[rivi, 3]) && ((AA[rivi, 4] == 3 && B[C, D - 4] == 0) || AA[rivi, 4] == 5 || B[C, D - 4] == AA[rivi, 4]) && ((AA[rivi, 5] == 3 && B[C, D - 5] == 0) || AA[rivi, 5] == 5 || B[C, D - 5] == AA[rivi, 5]) && ((AA[rivi, 6] == 3 && B[C, D - 6] == 0) || AA[rivi, 6] == 5 || B[C, D - 6] == AA[rivi, 6]) && ((AA[rivi, 7] == 3 && B[C, D - 7] == 0) || AA[rivi, 7] == 5 || B[C, D - 7] == AA[rivi, 7]))
		{
			for (int n = 0; n <= 7; n++)
			{
				if (AA[rivi, n] == 0 && B[C, D - n] == 0 && C > -1 && C < laudankoko + 8 && D - n > -1 && D - n < laudankoko + 8)
				{
					PRIORITY[C, D - n] += PISTEET[rivi];
				}
			}
		}
		if (((AA[rivi, 0] == 3 && B[C, D] == 0) || AA[rivi, 0] == 5 || B[C, D] == AA[rivi, 0]) && ((AA[rivi, 1] == 3 && B[C - 1, D - 1] == 0) || AA[rivi, 1] == 5 || B[C - 1, D - 1] == AA[rivi, 1]) && ((AA[rivi, 2] == 3 && B[C - 2, D - 2] == 0) || AA[rivi, 2] == 5 || B[C - 2, D - 2] == AA[rivi, 2]) && ((AA[rivi, 3] == 3 && B[C - 3, D - 3] == 0) || AA[rivi, 3] == 5 || B[C - 3, D - 3] == AA[rivi, 3]) && ((AA[rivi, 4] == 3 && B[C - 4, D - 4] == 0) || AA[rivi, 4] == 5 || B[C - 4, D - 4] == AA[rivi, 4]) && ((AA[rivi, 5] == 3 && B[C - 5, D - 5] == 0) || AA[rivi, 5] == 5 || B[C - 5, D - 5] == AA[rivi, 5]) && ((AA[rivi, 6] == 3 && B[C - 6, D - 6] == 0) || AA[rivi, 6] == 5 || B[C - 6, D - 6] == AA[rivi, 6]) && ((AA[rivi, 7] == 3 && B[C - 7, D - 7] == 0) || AA[rivi, 7] == 5 || B[C - 7, D - 7] == AA[rivi, 7]))
		{
			for (int num = 0; num <= 7; num++)
			{
				if (AA[rivi, num] == 0 && B[C - num, D - num] == 0 && C - num > -1 && C - num < laudankoko + 8 && D - num > -1 && D - num < laudankoko + 8)
				{
					PRIORITY[C - num, D - num] += PISTEET[rivi];
				}
			}
		}
		if (((AA[rivi, 0] != 3 || B[C, D] != 0) && AA[rivi, 0] != 5 && B[C, D] != AA[rivi, 0]) || ((AA[rivi, 1] != 3 || B[C - 1, D + 1] != 0) && AA[rivi, 1] != 5 && B[C - 1, D + 1] != AA[rivi, 1]) || ((AA[rivi, 2] != 3 || B[C - 2, D + 2] != 0) && AA[rivi, 2] != 5 && B[C - 2, D + 2] != AA[rivi, 2]) || ((AA[rivi, 3] != 3 || B[C - 3, D + 3] != 0) && AA[rivi, 3] != 5 && B[C - 3, D + 3] != AA[rivi, 3]) || ((AA[rivi, 4] != 3 || B[C - 4, D + 4] != 0) && AA[rivi, 4] != 5 && B[C - 4, D + 4] != AA[rivi, 4]) || ((AA[rivi, 5] != 3 || B[C - 5, D + 5] != 0) && AA[rivi, 5] != 5 && B[C - 5, D + 5] != AA[rivi, 5]) || ((AA[rivi, 6] != 3 || B[C - 6, D + 6] != 0) && AA[rivi, 6] != 5 && B[C - 6, D + 6] != AA[rivi, 6]) || ((AA[rivi, 7] != 3 || B[C - 7, D + 7] != 0) && AA[rivi, 7] != 5 && B[C - 7, D + 7] != AA[rivi, 7]))
		{
			return;
		}
		for (int num2 = 0; num2 <= 7; num2++)
		{
			if (AA[rivi, num2] == 0 && B[C - num2, D + num2] == 0 && C - num2 > -1 && C - num2 < laudankoko + 8 && D + num2 > -1 && D + num2 < laudankoko + 8)
			{
				PRIORITY[C - num2, D + num2] += PISTEET[rivi];
			}
		}
	}

	public int alotus()
	{
		int[,] array = new int[64, 64];
		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				array[j, i] = 0;
			}
		}
		for (int k = 0; k < laudankoko; k++)
		{
			for (int l = 0; l < laudankoko; l++)
			{
				array[l + 8, k + 8] = A[l, k];
			}
		}
		switch (kokovuoro)
		{
		case 0:
			PRIORITY[laudankoko / 2 + 8, laudankoko / 2 + 8] = 500;
			return 1;
		case 1:
			if (alotyyli == 2 || alotyyli == 3)
			{
				for (int num29 = 8; num29 < laudankoko + 8; num29++)
				{
					for (int num30 = 8; num30 < laudankoko + 8; num30++)
					{
						if (array[num30, num29] != 1)
						{
							continue;
						}
						if (num30 < laudankoko / 2 + 7)
						{
							if (num29 < laudankoko / 2 + 7)
							{
								PRIORITY[num30 + 1, num29 + 1] = 500;
							}
							else
							{
								PRIORITY[num30 + 1, num29 - 1] = 500;
							}
						}
						else if (num29 < laudankoko / 2 + 7)
						{
							PRIORITY[num30 - 1, num29 + 1] = 500;
						}
						else
						{
							PRIORITY[num30 - 1, num29 - 1] = 500;
						}
					}
				}
				return 1;
			}
			if (alotyyli == 4)
			{
				for (int num31 = 8; num31 < laudankoko + 8; num31++)
				{
					for (int num32 = 8; num32 < laudankoko + 8; num32++)
					{
						if (array[num32, num31] != 1)
						{
							continue;
						}
						if (num32 < laudankoko / 2 + 7)
						{
							if (num31 < laudankoko / 2 + 7)
							{
								PRIORITY[num32 + 2, num31 + 1] = 500;
								PRIORITY[num32 + 1, num31 + 2] = 500;
							}
							else
							{
								PRIORITY[num32 + 2, num31 - 1] = 500;
								PRIORITY[num32 + 1, num31 - 2] = 500;
							}
						}
						else if (num31 < laudankoko / 2 + 7)
						{
							PRIORITY[num32 - 2, num31 + 1] = 500;
							PRIORITY[num32 - 1, num31 + 2] = 500;
						}
						else
						{
							PRIORITY[num32 - 2, num31 - 1] = 500;
							PRIORITY[num32 - 1, num31 - 2] = 500;
						}
					}
				}
				return 1;
			}
			return 0;
		case 2:
		{
			for (int num33 = 8; num33 < laudankoko + 8; num33++)
			{
				for (int num34 = 8; num34 < laudankoko + 8; num34++)
				{
					if (array[num34, num33] != 1)
					{
						continue;
					}
					switch (alotyyli)
					{
					case 2:
						if (array[num34 - 1, num33 - 1] == 2)
						{
							PRIORITY[num34 - 2, num33 - 2] = 500;
							return 1;
						}
						if (array[num34, num33 - 1] == 2)
						{
							PRIORITY[num34, num33 - 2] = 500;
							return 1;
						}
						if (array[num34 + 1, num33 - 1] == 2)
						{
							PRIORITY[num34 + 2, num33 - 2] = 500;
							return 1;
						}
						if (array[num34 - 1, num33] == 2)
						{
							PRIORITY[num34 - 2, num33] = 500;
							return 1;
						}
						if (array[num34 + 1, num33] == 2)
						{
							PRIORITY[num34 + 2, num33] = 500;
							return 1;
						}
						if (array[num34 - 1, num33 + 1] == 2)
						{
							PRIORITY[num34 - 2, num33 + 2] = 500;
							return 1;
						}
						if (array[num34, num33 + 1] == 2)
						{
							PRIORITY[num34, num33 + 2] = 500;
							return 1;
						}
						if (array[num34 + 1, num33 + 1] == 2)
						{
							PRIORITY[num34 + 2, num33 + 2] = 500;
							return 1;
						}
						if (array[num34 - 2, num33 - 2] == 2)
						{
							PRIORITY[num34 - 3, num33 - 3] = 500;
							return 1;
						}
						if (array[num34 + 2, num33 - 2] == 2)
						{
							PRIORITY[num34 + 3, num33 - 3] = 500;
							return 1;
						}
						if (array[num34 + 2, num33 + 2] == 2)
						{
							PRIORITY[num34 + 3, num33 + 3] = 500;
							return 1;
						}
						if (array[num34 - 2, num33 + 2] == 2)
						{
							PRIORITY[num34 - 3, num33 + 3] = 500;
							return 1;
						}
						if (array[num34, num33 - 2] == 2)
						{
							PRIORITY[num34 - 2, num33 - 2] = 500;
							PRIORITY[num34 + 2, num33 - 2] = 500;
							return 1;
						}
						if (array[num34 - 2, num33] == 2)
						{
							PRIORITY[num34 - 2, num33 - 2] = 500;
							PRIORITY[num34 - 2, num33 + 2] = 500;
							return 1;
						}
						if (array[num34 + 2, num33] == 2)
						{
							PRIORITY[num34 + 2, num33 - 2] = 500;
							PRIORITY[num34 + 2, num33 + 2] = 500;
							return 1;
						}
						if (array[num34, num33 + 2] == 2)
						{
							PRIORITY[num34 - 2, num33 + 2] = 500;
							PRIORITY[num34 + 2, num33 + 2] = 500;
							return 1;
						}
						if (array[num34 - 1, num33 - 2] == 2)
						{
							PRIORITY[num34 - 2, num33 - 2] = 500;
							PRIORITY[num34 + 1, num33 - 1] = 500;
							PRIORITY[num34 - 2, num33] = 100;
							return 1;
						}
						if (array[num34 - 1, num33 + 2] == 2)
						{
							PRIORITY[num34 - 2, num33 + 2] = 500;
							PRIORITY[num34 + 1, num33 + 1] = 500;
							PRIORITY[num34, num33 + 2] = 100;
							return 1;
						}
						if (array[num34 + 1, num33 - 2] == 2)
						{
							PRIORITY[num34 + 2, num33 - 2] = 500;
							PRIORITY[num34 - 1, num33 - 1] = 500;
							PRIORITY[num34 + 2, num33] = 100;
							return 1;
						}
						if (array[num34 + 1, num33 + 2] == 2)
						{
							PRIORITY[num34 + 2, num33 + 2] = 500;
							PRIORITY[num34 - 1, num33 + 1] = 500;
							PRIORITY[num34, num33 + 2] = 100;
							return 1;
						}
						if (array[num34 + 2, num33 + 1] == 2)
						{
							PRIORITY[num34 + 2, num33 + 2] = 500;
							PRIORITY[num34 + 1, num33 - 1] = 500;
							PRIORITY[num34, num33 + 2] = 100;
							return 1;
						}
						if (array[num34 - 2, num33 - 1] == 2)
						{
							PRIORITY[num34 - 2, num33 - 2] = 500;
							PRIORITY[num34 - 1, num33 + 1] = 500;
							PRIORITY[num34, num33 - 2] = 100;
							return 1;
						}
						if (array[num34 + 2, num33 - 1] == 2)
						{
							PRIORITY[num34 + 2, num33 - 2] = 500;
							PRIORITY[num34 + 1, num33 + 1] = 500;
							PRIORITY[num34, num33 - 2] = 100;
							return 1;
						}
						if (array[num34 - 2, num33 + 1] == 2)
						{
							PRIORITY[num34 - 2, num33 + 2] = 500;
							PRIORITY[num34 - 1, num33 - 1] = 500;
							PRIORITY[num34, num33 + 2] = 100;
							return 1;
						}
						return 0;
					case 3:
					{
						if (array[num34 - 1, num33 - 1] == 2)
						{
							PRIORITY[num34 + 1, num33 + 1] = 500;
							return 1;
						}
						if (array[num34 + 1, num33 - 1] == 2)
						{
							PRIORITY[num34 - 1, num33 + 1] = 500;
							return 1;
						}
						if (array[num34 - 1, num33 + 1] == 2)
						{
							PRIORITY[num34 + 1, num33 - 1] = 500;
							return 1;
						}
						if (array[num34 + 1, num33 + 1] == 2)
						{
							PRIORITY[num34 - 1, num33 - 1] = 500;
							return 1;
						}
						if (array[num34, num33 - 1] == 2)
						{
							PRIORITY[num34 - 1, num33 + 1] = 500;
							PRIORITY[num34 + 1, num33 + 1] = 500;
							return 1;
						}
						if (array[num34 - 1, num33] == 2)
						{
							PRIORITY[num34 + 1, num33 + 1] = 500;
							PRIORITY[num34 + 1, num33 - 1] = 500;
							return 1;
						}
						if (array[num34 + 1, num33] == 2)
						{
							PRIORITY[num34 - 1, num33 + 1] = 500;
							PRIORITY[num34 - 1, num33 - 1] = 500;
							return 1;
						}
						if (array[num34, num33 + 1] == 2)
						{
							PRIORITY[num34 + 1, num33 - 1] = 500;
							PRIORITY[num34 - 1, num33 - 1] = 500;
							return 1;
						}
						for (int num37 = 8; num37 < laudankoko + 8; num37++)
						{
							for (int num38 = 8; num38 < laudankoko + 8; num38++)
							{
								if (array[num38, num37] != 2)
								{
									continue;
								}
								if (num38 > num34)
								{
									if (num37 > num33)
									{
										PRIORITY[num34 - 1, num33 - 1] = 500;
										return 1;
									}
									PRIORITY[num34 - 1, num33 + 1] = 500;
									return 1;
								}
								if (num37 > num33)
								{
									PRIORITY[num34 + 1, num33 - 1] = 500;
									return 1;
								}
								PRIORITY[num34 + 1, num33 + 1] = 500;
								return 1;
							}
						}
						return 0;
					}
					case 4:
					{
						for (int num35 = 8; num35 < laudankoko + 8; num35++)
						{
							for (int num36 = 8; num36 < laudankoko + 8; num36++)
							{
								if (array[num36, num35] != 2)
								{
									continue;
								}
								if (num36 > num34)
								{
									if (num35 > num33)
									{
										if (num34 + 1 == num36 && num33 + 1 == num35)
										{
											PRIORITY[num34, num33 - 2] = 500;
											PRIORITY[num34 - 2, num33] = 500;
											return 1;
										}
										PRIORITY[num34 - 2, num33 - 2] = 500;
										return 1;
									}
									if (num34 + 1 == num36 && num33 - 1 == num35)
									{
										PRIORITY[num34, num33 + 2] = 500;
										PRIORITY[num34 - 2, num33] = 500;
										return 1;
									}
									PRIORITY[num34 - 2, num33 + 2] = 500;
									return 1;
								}
								if (num35 > num33)
								{
									if (num34 - 1 == num36 && num33 + 1 == num35)
									{
										PRIORITY[num34, num33 - 2] = 500;
										PRIORITY[num34 + 2, num33] = 500;
										return 1;
									}
									PRIORITY[num34 + 2, num33 - 2] = 500;
									return 1;
								}
								if (num34 - 1 == num36 && num33 - 1 == num35)
								{
									PRIORITY[num34, num33 + 2] = 500;
									PRIORITY[num34 + 2, num33] = 500;
									return 1;
								}
								PRIORITY[num34 + 2, num33 + 2] = 500;
								return 1;
							}
						}
						return 0;
					}
					default:
						return 0;
					}
				}
			}
			break;
		}
		case 3:
		{
			if (alotyyli == 2)
			{
				for (int num25 = 8; num25 < laudankoko + 8; num25++)
				{
					for (int num26 = 8; num26 < laudankoko + 8; num26++)
					{
						if (array[num26, num25] == 2 && ((array[num26 - 1, num25 - 1] == 1 && array[num26 + 1, num25 + 1] == 1) || (array[num26 + 1, num25 - 1] == 1 && array[num26 - 1, num25 + 1] == 1)))
						{
							PRIORITY[num26, num25 + 1] = 500;
							PRIORITY[num26, num25 - 1] = 500;
							PRIORITY[num26 + 1, num25] = 500;
							PRIORITY[num26 - 1, num25] = 500;
							return 1;
						}
					}
				}
			}
			if (alotyyli != 3)
			{
				break;
			}
			for (int num27 = 8; num27 < laudankoko + 8; num27++)
			{
				for (int num28 = 8; num28 < laudankoko + 8; num28++)
				{
					if (array[num28, num27] == 2)
					{
						if ((array[num28 - 1, num27 - 1] == 1 || array[num28 - 1, num27 - 1] == 0) && array[num28 + 1, num27 + 1] == 0)
						{
							PRIORITY[num28 + 1, num27 + 1] = 500;
							return 1;
						}
						if ((array[num28 - 1, num27 + 1] == 1 || array[num28 - 1, num27 + 1] == 0) && array[num28 + 1, num27 - 1] == 0)
						{
							PRIORITY[num28 + 1, num27 - 1] = 500;
							return 1;
						}
						if ((array[num28 + 1, num27 - 1] == 1 || array[num28 + 1, num27 - 1] == 0) && array[num28 - 1, num27 + 1] == 0)
						{
							PRIORITY[num28 - 1, num27 + 1] = 500;
							return 1;
						}
						if ((array[num28 + 1, num27 + 1] == 1 || array[num28 + 1, num27 + 1] == 0) && array[num28 - 1, num27 - 1] == 0)
						{
							PRIORITY[num28 - 1, num27 - 1] = 500;
							return 1;
						}
					}
				}
			}
			break;
		}
		case 4:
		{
			int num18 = pikahaku(array, 1, 2);
			if (num18 == 9)
			{
				return 0;
			}
			for (int num19 = 8; num19 < laudankoko + 8; num19++)
			{
				for (int num20 = 8; num20 < laudankoko + 8; num20++)
				{
					if (array[num20, num19] != 1)
					{
						continue;
					}
					switch (alotyyli)
					{
					case 2:
					{
						for (int num23 = 8; num23 < laudankoko + 8; num23++)
						{
							for (int num24 = 8; num24 < laudankoko + 8; num24++)
							{
								if (array[num24, num23] != 2)
								{
									continue;
								}
								if (array[num20 + 2, num19 + 2] == 1 && (num24 != num20 + 1 || num23 != num19 + 1))
								{
									if (num24 > num23)
									{
										if (array[num20, num19 + 2] == 0)
										{
											PRIORITY[num20, num19 + 2] = 500;
											return 1;
										}
									}
									else if (array[num20 + 2, num19] == 0)
									{
										PRIORITY[num20 + 2, num19] = 500;
										return 1;
									}
								}
								if (array[num20 - 2, num19 + 2] == 1 && (num24 != num20 - 1 || num23 != num19 + 1))
								{
									if (num24 > laudankoko + 12 - num23)
									{
										if (array[num20 - 2, num19] == 0)
										{
											PRIORITY[num20 - 2, num19] = 500;
											return 1;
										}
									}
									else if (array[num20, num19 + 2] == 0)
									{
										PRIORITY[num20, num19 + 2] = 500;
										return 1;
									}
								}
								if (array[num20, num19 + 2] == 1 && (num24 != num20 || num23 != num19 + 1))
								{
									if (num24 > num20)
									{
										if (array[num20 - 1, num19 + 1] == 0)
										{
											PRIORITY[num20 - 1, num19 + 1] = 500;
											return 1;
										}
									}
									else if (array[num20 + 1, num19 + 1] == 0)
									{
										PRIORITY[num20 + 1, num19 + 1] = 500;
										return 1;
									}
								}
								if (array[num20 + 2, num19] != 1 || (num24 == num20 + 1 && num23 == num19))
								{
									continue;
								}
								if (num23 > num19)
								{
									if (array[num20 + 1, num19 - 1] == 0)
									{
										PRIORITY[num20 + 1, num19 - 1] = 500;
										return 1;
									}
								}
								else if (array[num20 + 1, num19 + 1] == 0)
								{
									PRIORITY[num20 + 1, num19 + 1] = 500;
									return 1;
								}
							}
						}
						return 0;
					}
					case 3:
						if (array[num20 - 1, num19 - 1] == 1)
						{
							if (array[num20 - 2, num19 - 2] == 2 && array[num20 + 1, num19 + 1] == 0)
							{
								PRIORITY[num20 + 1, num19 + 1] = 500;
								return 1;
							}
							if (array[num20 + 1, num19 + 1] == 2 && array[num20 - 2, num19 - 2] == 0)
							{
								PRIORITY[num20 - 2, num19 - 2] = 500;
								return 1;
							}
						}
						if (array[num20 - 1, num19 + 1] == 1)
						{
							if (array[num20 - 2, num19 + 2] == 2 && array[num20 + 1, num19 - 1] == 0)
							{
								PRIORITY[num20 + 1, num19 - 1] = 500;
								return 1;
							}
							if (array[num20 + 1, num19 - 1] == 2 && array[num20 - 2, num19 + 2] == 0)
							{
								PRIORITY[num20 - 2, num19 + 2] = 500;
								return 1;
							}
						}
						if (array[num20 + 1, num19 - 1] == 1)
						{
							if (array[num20 + 2, num19 - 2] == 2 && array[num20 - 1, num19 + 1] == 0)
							{
								PRIORITY[num20 - 1, num19 + 1] = 500;
								return 1;
							}
							if (array[num20 - 1, num19 + 1] == 2 && array[num20 + 2, num19 - 2] == 0)
							{
								PRIORITY[num20 + 2, num19 - 2] = 500;
								return 1;
							}
						}
						if (array[num20 + 1, num19 + 1] == 1)
						{
							if (array[num20 + 2, num19 + 2] == 2 && array[num20 - 1, num19 - 1] == 0)
							{
								PRIORITY[num20 - 1, num19 - 1] = 500;
								return 1;
							}
							if (array[num20 - 1, num19 - 1] == 2 && array[num20 + 2, num19 + 2] == 0)
							{
								PRIORITY[num20 + 2, num19 + 2] = 500;
								return 1;
							}
						}
						return 0;
					case 4:
					{
						if (array[num20 + 2, num19] == 1)
						{
							if (array[num20 - 1, num19 + 1] == 2)
							{
								if (array[num20, num19 - 2] == 0)
								{
									PRIORITY[num20, num19 - 2] = 500;
									return 1;
								}
								if (array[num20, num19 - 2] != 0 && array[num20, num19 + 2] == 0)
								{
									PRIORITY[num20, num19 + 2] = 400;
									return 1;
								}
							}
							if (array[num20 - 1, num19 - 1] == 2)
							{
								if (array[num20, num19 + 2] == 0)
								{
									PRIORITY[num20, num19 + 2] = 500;
									return 1;
								}
								if (array[num20, num19 + 2] != 0 && array[num20, num19 - 2] == 0)
								{
									PRIORITY[num20, num19 - 2] = 400;
									return 1;
								}
							}
							if (array[num20 + 3, num19 + 1] == 2)
							{
								if (array[num20 + 2, num19 - 2] == 0)
								{
									PRIORITY[num20 + 2, num19 - 2] = 500;
									return 1;
								}
								if (array[num20 + 2, num19 - 2] != 0 && array[num20 + 2, num19 + 2] == 0)
								{
									PRIORITY[num20 + 2, num19 + 2] = 400;
									return 1;
								}
							}
							if (array[num20 + 3, num19 - 1] == 2)
							{
								if (array[num20 + 2, num19 + 2] == 0)
								{
									PRIORITY[num20 + 2, num19 + 2] = 500;
									return 1;
								}
								if (array[num20 + 2, num19 + 2] != 0 && array[num20 + 2, num19 - 2] == 0)
								{
									PRIORITY[num20 + 2, num19 - 2] = 400;
									return 1;
								}
							}
						}
						if (array[num20 - 2, num19] == 1)
						{
							if (array[num20 + 1, num19 + 1] == 2)
							{
								if (array[num20, num19 - 2] == 0)
								{
									PRIORITY[num20, num19 - 2] = 500;
									return 1;
								}
								if (array[num20, num19 - 2] != 0 && array[num20, num19 + 2] == 0)
								{
									PRIORITY[num20, num19 + 2] = 400;
									return 1;
								}
							}
							if (array[num20 + 1, num19 - 1] == 2)
							{
								if (array[num20, num19 + 2] == 0)
								{
									PRIORITY[num20, num19 + 2] = 500;
									return 1;
								}
								if (array[num20, num19 + 2] != 0 && array[num20, num19 - 2] == 0)
								{
									PRIORITY[num20, num19 - 2] = 400;
									return 1;
								}
							}
							if (array[num20 - 3, num19 + 1] == 2)
							{
								if (array[num20 - 2, num19 - 2] == 0)
								{
									PRIORITY[num20 - 2, num19 - 2] = 500;
									return 1;
								}
								if (array[num20 - 2, num19 - 2] != 0 && array[num20 - 2, num19 + 2] == 0)
								{
									PRIORITY[num20 - 2, num19 + 2] = 400;
									return 1;
								}
							}
							if (array[num20 - 3, num19 - 1] == 2)
							{
								if (array[num20 - 2, num19 + 2] == 0)
								{
									PRIORITY[num20 - 2, num19 + 2] = 500;
									return 1;
								}
								if (array[num20 - 2, num19 + 2] != 0 && array[num20 - 2, num19 - 2] == 0)
								{
									PRIORITY[num20 - 2, num19 - 2] = 400;
									return 1;
								}
							}
						}
						if (array[num20, num19 + 2] == 1)
						{
							if (array[num20 + 1, num19 - 1] == 2)
							{
								if (array[num20 - 2, num19] == 0)
								{
									PRIORITY[num20 - 2, num19] = 500;
									return 1;
								}
								if (array[num20 - 2, num19] != 0 && array[num20 + 2, num19] == 0)
								{
									PRIORITY[num20 + 2, num19] = 400;
									return 1;
								}
							}
							if (array[num20 - 1, num19 - 1] == 2)
							{
								if (array[num20 + 2, num19] == 0)
								{
									PRIORITY[num20 + 2, num19] = 500;
									return 1;
								}
								if (array[num20 + 2, num19] != 0 && array[num20 - 2, num19] == 0)
								{
									PRIORITY[num20 - 2, num19] = 400;
									return 1;
								}
							}
							if (array[num20 + 1, num19 + 3] == 2)
							{
								if (array[num20 - 2, num19 + 2] == 0)
								{
									PRIORITY[num20 - 2, num19] = 500;
									return 1;
								}
								if (array[num20 - 2, num19 + 2] != 0 && array[num20 + 2, num19] == 0)
								{
									PRIORITY[num20 + 2, num19 + 2] = 400;
									return 1;
								}
							}
							if (array[num20 - 1, num19 + 3] == 2)
							{
								if (array[num20 + 2, num19 + 2] == 0)
								{
									PRIORITY[num20 + 2, num19 + 2] = 500;
									return 1;
								}
								if (array[num20 + 2, num19 + 2] != 0 && array[num20 - 2, num19 + 2] == 0)
								{
									PRIORITY[num20 - 2, num19 + 2] = 400;
									return 1;
								}
							}
						}
						if (array[num20, num19 - 2] == 1)
						{
							if (array[num20 + 1, num19 + 1] == 2)
							{
								if (array[num20 - 2, num19] == 0)
								{
									PRIORITY[num20 - 2, num19] = 500;
									return 1;
								}
								if (array[num20 - 2, num19] != 0 && array[num20 + 2, num19] == 0)
								{
									PRIORITY[num20 + 2, num19] = 400;
									return 1;
								}
							}
							if (array[num20 - 1, num19 + 1] == 2)
							{
								if (array[num20 + 2, num19] == 0)
								{
									PRIORITY[num20 + 2, num19] = 500;
									return 1;
								}
								if (array[num20 + 2, num19] != 0 && array[num20 - 2, num19] == 0)
								{
									PRIORITY[num20 - 2, num19] = 400;
									return 1;
								}
							}
							if (array[num20 + 1, num19 - 3] == 2)
							{
								if (array[num20 - 2, num19 - 2] == 0)
								{
									PRIORITY[num20 - 2, num19 - 2] = 500;
									return 1;
								}
								if (array[num20 - 2, num19 - 2] != 0 && array[num20 + 2, num19 - 2] == 0)
								{
									PRIORITY[num20 + 2, num19 - 2] = 400;
									return 1;
								}
							}
							if (array[num20 - 1, num19 - 3] == 2)
							{
								if (array[num20 + 2, num19 - 2] == 0)
								{
									PRIORITY[num20 + 2, num19 - 2] = 500;
									return 1;
								}
								if (array[num20 + 2, num19 - 2] != 0 && array[num20 - 2, num19 - 2] == 0)
								{
									PRIORITY[num20 - 2, num19 - 2] = 400;
									return 1;
								}
							}
						}
						for (int num21 = 8; num21 < laudankoko + 8; num21++)
						{
							for (int num22 = 8; num22 < laudankoko + 8; num22++)
							{
								if (array[num22, num21] != 2)
								{
									continue;
								}
								if (array[num20 - 2, num19 - 2] == 1)
								{
									if (num22 > num21)
									{
										if (array[num20 - 2, num19] == 0)
										{
											PRIORITY[num20 - 2, num19] = 500;
											return 1;
										}
									}
									else if (array[num20, num19 - 2] == 0)
									{
										PRIORITY[num20, num19 - 2] = 500;
										return 1;
									}
								}
								if (array[num20 - 2, num19 + 2] == 1)
								{
									if (num22 > laudankoko + 14 - num21)
									{
										if (array[num20 - 2, num19] == 0)
										{
											PRIORITY[num20 - 2, num19] = 500;
											return 1;
										}
									}
									else if (array[num20, num19 + 2] == 0)
									{
										PRIORITY[num20, num19 + 2] = 500;
										return 1;
									}
								}
								if (array[num20 + 2, num19 - 2] == 1)
								{
									if (num22 > laudankoko + 14 - num21)
									{
										if (array[num20, num19 - 2] == 0)
										{
											PRIORITY[num20, num19 - 2] = 500;
											return 1;
										}
									}
									else if (array[num20 + 2, num19] == 0)
									{
										PRIORITY[num20 + 2, num19] = 500;
										return 1;
									}
								}
								if (array[num20 + 2, num19 + 2] != 1)
								{
									continue;
								}
								if (num22 > num21)
								{
									if (array[num20, num19 + 2] == 0)
									{
										PRIORITY[num20, num19 + 2] = 500;
										return 1;
									}
								}
								else if (array[num20 + 2, num19] == 0)
								{
									PRIORITY[num20 + 2, num19] = 500;
									return 1;
								}
							}
						}
						return 0;
					}
					default:
						return 0;
					}
				}
			}
			return 0;
		}
		case 5:
		{
			if (alotyyli == 2)
			{
				int num10 = pikahaku(array, 2, 2);
				if (num10 == 9)
				{
					return 0;
				}
				for (int num11 = 8; num11 < laudankoko + 8; num11++)
				{
					for (int num12 = 8; num12 < laudankoko + 8; num12++)
					{
						if (array[num12, num11] != 2)
						{
							continue;
						}
						if (array[num12 - 1, num11 - 1] == 1 && array[num12 + 1, num11 - 1] == 1 && array[num12 + 1, num11 + 1] == 1)
						{
							if (array[num12 - 1, num11] == 0)
							{
								PRIORITY[num12 - 1, num11] = 500;
								return 1;
							}
							if (array[num12, num11 + 1] == 0)
							{
								PRIORITY[num12, num11 + 1] = 500;
								return 1;
							}
						}
						if (array[num12 - 1, num11 - 1] == 1 && array[num12 - 1, num11 + 1] == 1 && array[num12 + 1, num11 + 1] == 1)
						{
							if (array[num12 + 1, num11] == 0)
							{
								PRIORITY[num12 + 1, num11] = 500;
								return 1;
							}
							if (array[num12, num11 - 1] == 0)
							{
								PRIORITY[num12, num11 - 1] = 500;
								return 1;
							}
						}
						if (array[num12 - 1, num11 - 1] == 1 && array[num12 + 1, num11 - 1] == 1 && array[num12 - 1, num11 + 1] == 1)
						{
							if (array[num12 + 1, num11] == 0)
							{
								PRIORITY[num12 + 1, num11] = 500;
								return 1;
							}
							if (array[num12, num11 + 1] == 0)
							{
								PRIORITY[num12, num11 + 1] = 500;
								return 1;
							}
						}
						if (array[num12 + 1, num11 - 1] == 1 && array[num12 - 1, num11 + 1] == 1 && array[num12 + 1, num11 + 1] == 1)
						{
							if (array[num12 - 1, num11] == 0)
							{
								PRIORITY[num12 - 1, num11] = 500;
								return 1;
							}
							if (array[num12, num11 - 1] == 0)
							{
								PRIORITY[num12, num11 - 1] = 500;
								return 1;
							}
						}
					}
				}
				return 0;
			}
			if (alotyyli != 3)
			{
				return 0;
			}
			int num13 = pikahaku(array, 2, 2);
			if (num13 == 9)
			{
				return 0;
			}
			for (int num14 = 8; num14 < laudankoko + 8; num14++)
			{
				for (int num15 = 8; num15 < laudankoko + 8; num15++)
				{
					if (array[num15, num14] != 2)
					{
						continue;
					}
					for (int num16 = 8; num16 < laudankoko + 8; num16++)
					{
						for (int num17 = 8; num17 < laudankoko + 8; num17++)
						{
							if (array[num17, num16] != 1)
							{
								continue;
							}
							if (array[num15 - 1, num14 - 1] == 2 && (num17 != num15 - 2 || num16 != num14 - 2) && (num17 != num15 + 1 || num16 != num14 + 1))
							{
								if (array[num15 - 2, num14 - 2] == 1)
								{
									if (num17 > num16)
									{
										if (array[num15, num14 + 1] == 0)
										{
											PRIORITY[num15, num14 + 1] = 500;
											return 1;
										}
									}
									else if (array[num15 + 1, num14] == 0)
									{
										PRIORITY[num15 + 1, num14] = 500;
										return 1;
									}
								}
								if (array[num15 + 1, num14 + 1] == 1)
								{
									if (num17 > num16)
									{
										if (array[num15 - 2, num14 - 1] == 0)
										{
											PRIORITY[num15 - 2, num14 - 1] = 500;
											return 1;
										}
									}
									else if (array[num15 - 1, num14 - 2] == 0)
									{
										PRIORITY[num15 - 1, num14 - 2] = 500;
										return 1;
									}
								}
							}
							if (array[num15 - 1, num14 + 1] == 2 && (num17 != num15 - 2 || num16 != num14 + 2) && (num17 != num15 + 1 || num16 != num14 - 1))
							{
								if (array[num15 - 2, num14 + 2] == 1)
								{
									if (num17 > laudankoko + 14 - num16)
									{
										if (array[num15, num14 - 1] == 0)
										{
											PRIORITY[num15, num14 - 1] = 500;
											return 1;
										}
									}
									else if (array[num15 + 1, num14] == 0)
									{
										PRIORITY[num15 + 1, num14] = 500;
										return 1;
									}
								}
								if (array[num15 + 1, num14 - 1] == 1)
								{
									if (num17 > laudankoko + 14 - num16)
									{
										if (array[num15 - 2, num14 + 1] == 0)
										{
											PRIORITY[num15 - 2, num14 + 1] = 500;
											return 1;
										}
									}
									else if (array[num15 - 1, num14 + 2] == 0)
									{
										PRIORITY[num15 - 1, num14 + 2] = 500;
										return 1;
									}
								}
							}
							if (array[num15 + 1, num14 - 1] == 2 && (num17 != num15 + 2 || num16 != num14 - 2) && (num17 != num15 - 1 || num16 != num14 + 1))
							{
								if (array[num15 + 2, num14 - 2] == 1)
								{
									if (num17 > laudankoko + 14 - num16)
									{
										if (array[num15 - 1, num14] == 0)
										{
											PRIORITY[num15 - 1, num14] = 500;
											return 1;
										}
									}
									else if (array[num15, num14 + 1] == 0)
									{
										PRIORITY[num15, num14 + 1] = 500;
										return 1;
									}
								}
								if (array[num15 - 1, num14 + 1] == 1)
								{
									if (num17 > laudankoko + 14 - num16)
									{
										if (array[num15 + 1, num14 - 2] == 0)
										{
											PRIORITY[num15 + 1, num14 - 2] = 500;
											return 1;
										}
									}
									else if (array[num15 + 2, num14 - 1] == 0)
									{
										PRIORITY[num15 + 2, num14 - 1] = 500;
										return 1;
									}
								}
							}
							if (array[num15 + 1, num14 + 1] != 2 || (num17 == num15 + 2 && num16 == num14 + 2) || (num17 == num15 - 1 && num16 == num14 - 1))
							{
								continue;
							}
							if (array[num15 + 2, num14 + 2] == 1)
							{
								if (num17 > num16)
								{
									if (array[num15 - 1, num14] == 0)
									{
										PRIORITY[num15 - 1, num14] = 500;
										return 1;
									}
								}
								else if (array[num15, num14 - 1] == 0)
								{
									PRIORITY[num15, num14 - 1] = 500;
									return 1;
								}
							}
							if (array[num15 - 1, num14 - 1] != 1)
							{
								continue;
							}
							if (num17 > num16)
							{
								if (array[num15 + 1, num14 + 1] == 0)
								{
									PRIORITY[num15 + 1, num14 + 2] = 500;
									return 1;
								}
							}
							else if (array[num15 + 2, num14 + 1] == 0)
							{
								PRIORITY[num15 + 2, num14 + 1] = 500;
								return 1;
							}
						}
					}
				}
			}
			return 0;
		}
		case 6:
			switch (alotyyli)
			{
			case 2:
			{
				int num7 = pikahaku(array, 1, 2);
				if (num7 == 9)
				{
					return 0;
				}
				for (int num8 = 8; num8 < laudankoko + 8; num8++)
				{
					for (int num9 = 8; num9 < laudankoko + 8; num9++)
					{
						if (array[num9, num8] != 1)
						{
							continue;
						}
						if (array[num9 - 2, num8] == 1 && array[num9, num8 - 2] == 1 && array[num9 - 1, num8 - 1] == 2)
						{
							if (array[num9, num8 - 1] == 2 && array[num9 + 1, num8 - 1] == 0)
							{
								PRIORITY[num9 + 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9 - 1, num8] == 2 && array[num9 - 1, num8 + 1] == 0)
							{
								PRIORITY[num9 - 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 + 2, num8] == 1 && array[num9, num8 - 2] == 1 && array[num9 + 1, num8 - 1] == 2)
						{
							if (array[num9 + 1, num8] == 2 && array[num9 + 1, num8 + 1] == 0)
							{
								PRIORITY[num9 + 1, num8 + 1] = 500;
								return 1;
							}
							if (array[num9, num8 - 1] == 2 && array[num9 - 1, num8 - 1] == 0)
							{
								PRIORITY[num9 - 1, num8 - 1] = 500;
								return 1;
							}
						}
						if (array[num9 + 2, num8] == 1 && array[num9, num8 + 2] == 1 && array[num9 + 1, num8 + 1] == 2)
						{
							if (array[num9 + 1, num8] == 2 && array[num9 + 1, num8 - 1] == 0)
							{
								PRIORITY[num9 + 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9, num8 + 1] == 2 && array[num9 - 1, num8 + 1] == 0)
							{
								PRIORITY[num9 - 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 - 2, num8] == 1 && array[num9, num8 + 2] == 1 && array[num9 - 1, num8 + 1] == 2)
						{
							if (array[num9 - 1, num8] == 2 && array[num9 - 1, num8 - 1] == 0)
							{
								PRIORITY[num9 - 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9, num8 + 1] == 2 && array[num9 + 1, num8 + 1] == 0)
							{
								PRIORITY[num9 + 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 - 2, num8] == 1 && array[num9, num8 - 2] == 1 && array[num9 - 1, num8 - 1] == 2)
						{
							if (array[num9 - 3, num8 - 1] == 2 && array[num9 + 1, num8 - 1] == 0)
							{
								PRIORITY[num9 + 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9 - 1, num8 - 3] == 2 && array[num9 - 1, num8 + 1] == 0)
							{
								PRIORITY[num9 - 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 + 2, num8] == 1 && array[num9, num8 - 2] == 1 && array[num9 + 1, num8 - 1] == 2)
						{
							if (array[num9 + 1, num8 - 3] == 2 && array[num9 + 1, num8 + 1] == 0)
							{
								PRIORITY[num9 + 1, num8 + 1] = 500;
								return 1;
							}
							if (array[num9 + 3, num8 - 1] == 2 && array[num9 - 1, num8 - 1] == 0)
							{
								PRIORITY[num9 - 1, num8 - 1] = 500;
								return 1;
							}
						}
						if (array[num9 + 2, num8] == 1 && array[num9, num8 + 2] == 1 && array[num9 + 1, num8 + 1] == 2)
						{
							if (array[num9 + 1, num8 - 3] == 2 && array[num9 + 1, num8 - 1] == 0)
							{
								PRIORITY[num9 + 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9 + 3, num8 + 1] == 2 && array[num9 - 1, num8 + 1] == 0)
							{
								PRIORITY[num9 - 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 - 2, num8] == 1 && array[num9, num8 + 2] == 1 && array[num9 - 1, num8 + 1] == 2)
						{
							if (array[num9 - 1, num8 + 3] == 2 && array[num9 - 1, num8 - 1] == 0)
							{
								PRIORITY[num9 - 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9 - 3, num8 + 1] == 2 && array[num9 + 1, num8 + 1] == 0)
							{
								PRIORITY[num9 + 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 - 2, num8] == 1 && array[num9, num8 - 2] == 1 && array[num9 - 1, num8 - 1] == 2)
						{
							if (array[num9 - 2, num8 - 1] == 2 && array[num9 + 1, num8 - 1] == 0)
							{
								PRIORITY[num9 + 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9 - 1, num8 - 2] == 2 && array[num9 - 1, num8 + 1] == 0)
							{
								PRIORITY[num9 - 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 + 2, num8] == 1 && array[num9, num8 - 2] == 1 && array[num9 + 1, num8 - 1] == 2)
						{
							if (array[num9 + 1, num8 - 2] == 2 && array[num9 + 1, num8 + 1] == 0)
							{
								PRIORITY[num9 + 1, num8 + 1] = 500;
								return 1;
							}
							if (array[num9 + 2, num8 - 1] == 2 && array[num9 - 1, num8 - 1] == 0)
							{
								PRIORITY[num9 - 1, num8 - 1] = 500;
								return 1;
							}
						}
						if (array[num9 + 2, num8] == 1 && array[num9, num8 + 2] == 1 && array[num9 + 1, num8 + 1] == 2)
						{
							if (array[num9 + 1, num8 - 2] == 2 && array[num9 + 1, num8 - 1] == 0)
							{
								PRIORITY[num9 + 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9 + 2, num8 + 1] == 2 && array[num9 - 1, num8 + 1] == 0)
							{
								PRIORITY[num9 - 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 - 2, num8] == 1 && array[num9, num8 + 2] == 1 && array[num9 - 1, num8 + 1] == 2)
						{
							if (array[num9 - 1, num8 + 2] == 2 && array[num9 - 1, num8 - 1] == 0)
							{
								PRIORITY[num9 - 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9 - 2, num8 + 1] == 2 && array[num9 + 1, num8 + 1] == 0)
							{
								PRIORITY[num9 + 1, num8 + 1] = 500;
								return 1;
							}
						}
						if (array[num9 - 1, num8 + 1] == 1 && array[num9 + 1, num8 + 1] == 1 && array[num9, num8 + 1] == 2)
						{
							if (array[num9, num8 + 2] == 0)
							{
								PRIORITY[num9, num8 + 2] = 500;
								return 1;
							}
							if (array[num9, num8 + 2] == 2)
							{
								if (array[num9 + 2, num8 + 2] == 0 && array[num9 - 1, num8 - 1] == 0)
								{
									PRIORITY[num9 - 1, num8 - 1] = 500;
									return 1;
								}
								if (array[num9 - 2, num8 + 2] == 0 && array[num9 + 1, num8 - 1] == 0)
								{
									PRIORITY[num9 + 1, num8 - 1] = 500;
									return 1;
								}
							}
						}
						if (array[num9 + 1, num8 - 1] == 1 && array[num9 + 1, num8 + 1] == 1 && array[num9 + 1, num8] == 2)
						{
							if (array[num9 + 2, num8] == 0)
							{
								PRIORITY[num9 + 2, num8] = 500;
								return 1;
							}
							if (array[num9 + 2, num8] == 2)
							{
								if (array[num9 + 2, num8 - 2] == 0 && array[num9 - 1, num8 + 1] == 0)
								{
									PRIORITY[num9 - 1, num8 + 1] = 500;
									return 1;
								}
								if (array[num9 + 2, num8 + 2] == 0 && array[num9 - 1, num8 - 1] == 0)
								{
									PRIORITY[num9 - 1, num8 - 1] = 500;
									return 1;
								}
							}
						}
						if (array[num9 + 1, num8 - 1] == 1 && array[num9 - 1, num8 - 1] == 1 && array[num9, num8 - 1] == 2)
						{
							if (array[num9, num8 - 2] == 0)
							{
								PRIORITY[num9, num8 - 2] = 500;
								return 1;
							}
							if (array[num9, num8 - 2] == 2)
							{
								if (array[num9 + 2, num8 - 2] == 0 && array[num9 - 1, num8 + 1] == 0)
								{
									PRIORITY[num9 - 1, num8 + 1] = 500;
									return 1;
								}
								if (array[num9 - 2, num8 - 2] == 0 && array[num9 + 1, num8 + 1] == 0)
								{
									PRIORITY[num9 + 1, num8 + 1] = 500;
									return 1;
								}
							}
						}
						if (array[num9 - 1, num8 + 1] != 1 || array[num9 - 1, num8 - 1] != 1 || array[num9 - 1, num8] != 2)
						{
							continue;
						}
						if (array[num9 - 2, num8] == 0)
						{
							PRIORITY[num9 - 2, num8] = 500;
							return 1;
						}
						if (array[num9 - 2, num8] == 2)
						{
							if (array[num9 + 2, num8 - 2] == 0 && array[num9 + 1, num8 - 1] == 0)
							{
								PRIORITY[num9 + 1, num8 - 1] = 500;
								return 1;
							}
							if (array[num9 - 2, num8 - 2] == 0 && array[num9 + 1, num8 + 1] == 0)
							{
								PRIORITY[num9 + 1, num8 + 1] = 500;
								return 1;
							}
						}
					}
				}
				return 0;
			}
			case 3:
			{
				int num2 = pikahaku(array, 1, 2);
				if (num2 == 9)
				{
					return 0;
				}
				for (int num3 = 8; num3 < laudankoko + 8; num3++)
				{
					for (int num4 = 8; num4 < laudankoko + 8; num4++)
					{
						if (array[num4, num3] != 1)
						{
							continue;
						}
						for (int num5 = 8; num5 < laudankoko + 8; num5++)
						{
							for (int num6 = 8; num6 < laudankoko + 8; num6++)
							{
								if (array[num6, num5] != 2)
								{
									continue;
								}
								if (array[num4 - 1, num3 - 1] == 1 && (num6 != num4 - 2 || num5 != num3 - 2) && (num6 != num4 + 1 || num5 != num3 + 1))
								{
									if (array[num4 - 2, num3 - 2] == 2)
									{
										if (num6 > num5)
										{
											if (array[num4, num3 + 1] == 0)
											{
												PRIORITY[num4, num3 + 1] = 500;
												return 1;
											}
										}
										else if (array[num4 + 1, num3] == 0)
										{
											PRIORITY[num4 + 1, num3] = 500;
											return 1;
										}
									}
									if (array[num4 + 1, num3 + 1] == 2)
									{
										if (num6 > num5)
										{
											if (array[num4 - 2, num3 - 1] == 0)
											{
												PRIORITY[num4 - 2, num3 - 1] = 500;
												return 1;
											}
										}
										else if (array[num4 - 1, num3 - 2] == 0)
										{
											PRIORITY[num4 - 1, num3 - 2] = 500;
											return 1;
										}
									}
								}
								if (array[num4 - 1, num3 + 1] == 1 && (num6 != num4 - 2 || num5 != num3 + 2) && (num6 != num4 + 1 || num5 != num3 - 1))
								{
									if (array[num4 - 2, num3 + 2] == 2)
									{
										if (num6 > laudankoko + 14 - num5)
										{
											if (array[num4, num3 - 1] == 0)
											{
												PRIORITY[num4, num3 - 1] = 500;
												return 1;
											}
										}
										else if (array[num4 + 1, num3] == 0)
										{
											PRIORITY[num4 + 1, num3] = 500;
											return 1;
										}
									}
									if (array[num4 + 1, num3 - 1] == 2)
									{
										if (num6 > laudankoko + 14 - num5)
										{
											if (array[num4 - 2, num3 + 1] == 0)
											{
												PRIORITY[num4 - 2, num3 + 1] = 500;
												return 1;
											}
										}
										else if (array[num4 - 1, num3 + 2] == 0)
										{
											PRIORITY[num4 - 1, num3 + 2] = 500;
											return 1;
										}
									}
								}
								if (array[num4 + 1, num3 - 1] == 1 && (num6 != num4 + 2 || num5 != num3 - 2) && (num6 != num4 - 1 || num5 != num3 + 1))
								{
									if (array[num4 + 2, num3 - 2] == 2)
									{
										if (num6 > laudankoko + 14 - num5)
										{
											if (array[num4 - 1, num3] == 0)
											{
												PRIORITY[num4 - 1, num3] = 500;
												return 1;
											}
										}
										else if (array[num4, num3 + 1] == 0)
										{
											PRIORITY[num4, num3 + 1] = 500;
											return 1;
										}
									}
									if (array[num4 - 1, num3 + 1] == 2)
									{
										if (num6 > laudankoko + 14 - num5)
										{
											if (array[num4 + 1, num3 - 2] == 0)
											{
												PRIORITY[num4 + 1, num3 - 2] = 500;
												return 1;
											}
										}
										else if (array[num4 + 2, num3 - 1] == 0)
										{
											PRIORITY[num4 + 2, num3 - 1] = 500;
											return 1;
										}
									}
								}
								if (array[num4 + 1, num3 + 1] != 1 || (num6 == num4 + 2 && num5 == num3 + 2) || (num6 == num4 - 1 && num5 == num3 - 1))
								{
									continue;
								}
								if (array[num4 + 2, num3 + 2] == 2)
								{
									if (num6 > num5)
									{
										if (array[num4 - 1, num3] == 0)
										{
											PRIORITY[num4 - 1, num3] = 500;
											return 1;
										}
									}
									else if (array[num4, num3 - 1] == 0)
									{
										PRIORITY[num4, num3 - 1] = 500;
										return 1;
									}
								}
								if (array[num4 - 1, num3 - 1] != 2)
								{
									continue;
								}
								if (num6 > num5)
								{
									if (array[num4 + 1, num3 + 2] == 0)
									{
										PRIORITY[num4 + 1, num3 + 2] = 500;
										return 1;
									}
								}
								else if (array[num4 + 2, num3 + 1] == 0)
								{
									PRIORITY[num4 + 2, num3 + 1] = 500;
									return 1;
								}
							}
						}
					}
				}
				return 0;
			}
			case 4:
			{
				int num = pikahaku(array, 1, 2);
				if (num == 9)
				{
					return 0;
				}
				for (int m = 8; m < laudankoko + 8; m++)
				{
					for (int n = 8; n < laudankoko + 8; n++)
					{
						if (array[n, m] == 1)
						{
							if (array[n - 2, m - 2] == 0 && array[n, m - 2] == 1 && array[n - 2, m] == 1)
							{
								PRIORITY[n - 2, m - 2] = 500;
								return 1;
							}
							if (array[n - 2, m - 2] == 1 && array[n, m - 2] == 0 && array[n - 2, m] == 1)
							{
								PRIORITY[n, m - 2] = 500;
								return 1;
							}
							if (array[n - 2, m - 2] == 1 && array[n, m - 2] == 1 && array[n - 2, m] == 0)
							{
								PRIORITY[n - 2, m] = 500;
								return 1;
							}
							if (array[n, m - 2] == 0 && array[n + 2, m - 2] == 1 && array[n + 2, m] == 1)
							{
								PRIORITY[n, m - 2] = 500;
								return 1;
							}
							if (array[n, m - 2] == 1 && array[n + 2, m - 2] == 0 && array[n + 2, m] == 1)
							{
								PRIORITY[n + 2, m - 2] = 500;
								return 1;
							}
							if (array[n, m - 2] == 1 && array[n + 2, m - 2] == 1 && array[n + 2, m] == 0)
							{
								PRIORITY[n + 2, m - 2] = 500;
								return 1;
							}
							if (array[n + 2, m] == 0 && array[n + 2, m + 2] == 1 && array[n, m + 2] == 1)
							{
								PRIORITY[n + 2, m] = 500;
								return 1;
							}
							if (array[n + 2, m] == 1 && array[n + 2, m + 2] == 0 && array[n, m + 2] == 1)
							{
								PRIORITY[n + 2, m + 2] = 500;
								return 1;
							}
							if (array[n + 2, m] == 1 && array[n + 2, m + 2] == 1 && array[n, m + 2] == 0)
							{
								PRIORITY[n, m + 2] = 500;
								return 1;
							}
							if (array[n - 2, m] == 0 && array[n - 2, m + 2] == 1 && array[n + 2, m + 2] == 1)
							{
								PRIORITY[n - 2, m] = 500;
								return 1;
							}
							if (array[n - 2, m] == 1 && array[n - 2, m + 2] == 0 && array[n + 2, m + 2] == 1)
							{
								PRIORITY[n - 2, m + 2] = 500;
								return 1;
							}
							if (array[n - 2, m] == 1 && array[n - 2, m + 2] == 1 && array[n + 2, m + 2] == 0)
							{
								PRIORITY[n + 2, m + 2] = 500;
								return 1;
							}
						}
					}
				}
				return 0;
			}
			default:
				return 0;
			}
		default:
			return 0;
		}
		return 0;
	}

	public Pelipaikka haeparas()
	{
		int num = 0;
		int num2 = 0;
		int y = 0;
		int x = 0;
		for (int i = 0; i < laudankoko + 8; i++)
		{
			for (int j = 0; j < laudankoko + 8; j++)
			{
				int num3 = arpa();
				if (arpamaara == 0)
				{
					num2 = 0;
				}
				if (arpamaara == 1)
				{
					num2 = 7 + num3;
				}
				if (arpamaara == 2)
				{
					num2 = Convert.ToInt32(0.1 * (double)num);
				}
				if (PRIORITY[j, i] < -1)
				{
					PRIORITY[j, i] = 65536 + PRIORITY[j, i];
				}
				if (PRIORITY[j, i] + num3 + num2 > num && j >= 8 && i >= 8 && A[j - 8, i - 8] == 0)
				{
					num = PRIORITY[j, i] + num3;
					y = j - 8;
					x = i - 8;
				}
			}
		}
		Pelipaikka pelipaikka = new Pelipaikka();
		pelipaikka.X = x;
		pelipaikka.Y = y;
		return pelipaikka;
	}

	public int mietipaikka()
	{
		int suunta = 0;
		int puoli = 2;
		int[] pISTEET = new int[64];
		int num = 0;
		int suunta2 = 0;
		int rivi = 0;
		int[,] array = new int[64, 64];
		int[,] array2 = new int[64, 64];
		int[,] aA = new int[64, 8];
		int[,] aA2 = new int[16, 8];
		int[,] bB = new int[16, 8];
		int num2 = vuoro;
		int num3;
		if (vuoro == 1)
		{
			num3 = 2;
			double num4 = 0.85;
			if (kokovuoro < 16)
			{
				num4 = (double)kokovuoro / 17.0;
			}
			if (random.NextDouble() > num4)
			{
				num3 = 1;
			}
		}
		else
		{
			num3 = 2;
			double num5 = 0.85;
			if (kokovuoro < 16)
			{
				num5 = 0.92;
			}
			if (random.NextDouble() > num5)
			{
				num3 = 1;
			}
		}
		if (random.NextDouble() > 0.96)
		{
			num3 = 3;
		}
		if (random.NextDouble() > 0.98)
		{
			num3 = 4;
		}
		int syvyysmax = maxsyvyys;
		int num6 = arpa4();
		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				array[j, i] = 0;
			}
		}
		for (int k = 0; k < laudankoko; k++)
		{
			for (int l = 0; l < laudankoko; l++)
			{
				array[l + 8, k + 8] = A[l, k];
			}
		}
		fastend(pISTEET);
		if (num2 == 1)
		{
			taulukko1(aA, 2, 1);
		}
		if (num2 == 2)
		{
			taulukko1(aA, 1, 2);
		}
		for (int m = 0; m < 6; m++)
		{
			for (int n = 8; n < laudankoko + 8; n++)
			{
				for (int num7 = 8; num7 < laudankoko + 8; num7++)
				{
					lisaapriority(PRIORITY, aA, num7, n, suunta, array, m, pISTEET);
				}
			}
		}
		for (int num8 = 8; num8 < laudankoko + 8; num8++)
		{
			for (int num9 = 8; num9 < laudankoko + 8; num9++)
			{
				if (PRIORITY[num9, num8] <= 1)
				{
					continue;
				}
				if (PRIORITY[num9, num8] < 1000 && puhe == 1)
				{
					if (num6 <= 1)
					{
						CmpComString = "Computer: I have no options...";
					}
					if (num6 == 2)
					{
						CmpComString = "Computer: Now you had the straight four. :(";
					}
					if (num6 == 3)
					{
						CmpComString = "Computer: I must place the mark here.";
					}
					if (num6 >= 4)
					{
						CmpComString = "Computer: There are no question marks.";
					}
				}
				return 2;
			}
		}
		if (maxsyvyys != 0)
		{
			if (num2 == 1)
			{
				taulukko2(aA2, bB, 2, 1);
				puoli = 2;
			}
			if (num2 == 2)
			{
				taulukko2(aA2, bB, 1, 2);
				puoli = 1;
			}
			for (int num10 = 0; num10 < 64; num10++)
			{
				for (int num11 = 0; num11 < 64; num11++)
				{
					array2[num10, num11] = array[num10, num11];
				}
			}
			int x = 0;
			int y = 0;
			rekursiohaku(0, syvyysmax, aA2, bB, array2, PRIORITY, rivi, suunta2, x, y, num2);
			for (int num12 = 8; num12 < laudankoko + 8; num12++)
			{
				for (int num13 = 8; num13 < laudankoko + 8; num13++)
				{
					if (PRIORITY[num13, num12] > 1 && PRIORITY[num13, num12] > num)
					{
						num = PRIORITY[num13, num12];
					}
				}
			}
			for (int num14 = 8; num14 < laudankoko + 8; num14++)
			{
				for (int num15 = 8; num15 < laudankoko + 8; num15++)
				{
					if (PRIORITY[num15, num14] > 1 && PRIORITY[num15, num14] < num)
					{
						PRIORITY[num15, num14] = 0;
					}
				}
			}
			for (int num16 = 8; num16 < laudankoko + 8; num16++)
			{
				for (int num17 = 8; num17 < laudankoko + 8; num17++)
				{
					if (PRIORITY[num17, num16] <= 1)
					{
						continue;
					}
					if (puhe == 1)
					{
						if (num6 <= 1)
						{
							CmpComString = "Computer: GG!";
						}
						if (num6 == 2)
						{
							CmpComString = "Computer: I won!!! :D";
						}
						if (num6 == 3)
						{
							CmpComString = "Computer: Almost too easy...";
						}
						if (num6 >= 4)
						{
							CmpComString = "Computer: You must be sleeping.";
						}
					}
					return 1;
				}
			}
			int num18 = pikahaku(array, num2, 2);
			if (num18 == 9)
			{
				torjuntahaku(array, num2, PRIORITY);
				for (int num19 = 8; num19 < laudankoko + 8; num19++)
				{
					for (int num20 = 8; num20 < laudankoko + 8; num20++)
					{
						if (PRIORITY[num20, num19] > 1)
						{
							if (puhe == 1)
							{
								CmpComString = "Computer: Hmm... Hmm...";
							}
							return 1;
						}
					}
				}
				if (puhe == 1)
				{
					if (num6 <= 1)
					{
						CmpComString = "Computer: I'm under a threat.";
					}
					if (num6 == 2)
					{
						CmpComString = "Computer: Ohh... Help me! :)";
					}
					if (num6 == 3)
					{
						CmpComString = "Computer: Tough game...";
					}
					if (num6 >= 4)
					{
						CmpComString = "Computer: You got the straight three.";
					}
				}
			}
			if (num18 != 9)
			{
				rivi = 0;
				suunta2 = 0;
				x = 0;
				y = 0;
				syvyysmax = maxsyvyys;
				for (int num21 = 0; num21 < 64; num21++)
				{
					for (int num22 = 0; num22 < 64; num22++)
					{
						array2[num21, num22] = array[num21, num22];
					}
				}
				if (num2 == 1)
				{
					taulukko2(aA2, bB, 1, 2);
				}
				if (num2 == 2)
				{
					taulukko2(aA2, bB, 2, 1);
				}
				rekursiohaku(0, syvyysmax, aA2, bB, array2, PRIORITY, rivi, suunta2, x, y, puoli);
				for (int num23 = 8; num23 < laudankoko + 8; num23++)
				{
					for (int num24 = 8; num24 < laudankoko + 8; num24++)
					{
						if (PRIORITY[num24, num23] > 1 && PRIORITY[num24, num23] > num)
						{
							num = PRIORITY[num24, num23];
						}
					}
				}
				for (int num25 = 8; num25 < laudankoko + 8; num25++)
				{
					for (int num26 = 8; num26 < laudankoko + 8; num26++)
					{
						if (PRIORITY[num26, num25] > 1 && PRIORITY[num26, num25] < num)
						{
							PRIORITY[num26, num25] = 0;
						}
					}
				}
				for (int num27 = 8; num27 < laudankoko + 8; num27++)
				{
					for (int num28 = 8; num28 < laudankoko + 8; num28++)
					{
						if (PRIORITY[num28, num27] <= 1)
						{
							continue;
						}
						if (puhe == 1)
						{
							if (num6 <= 1)
							{
								CmpComString = "Computer: Let's ruin your game here...";
							}
							if (num6 == 2)
							{
								CmpComString = "Computer: Hmm... I see your point.";
							}
							if (num6 == 3)
							{
								CmpComString = "Computer: I can read your mind. ;)";
							}
							if (num6 >= 4)
							{
								CmpComString = "Computer: You have a little trap here.";
							}
						}
						return 1;
					}
				}
				rivi = 0;
				suunta2 = 0;
				x = 0;
				y = 0;
				syvyysmax = maxsyvyys;
				for (int num29 = 0; num29 < 64; num29++)
				{
					for (int num30 = 0; num30 < 64; num30++)
					{
						array2[num29, num30] = array[num29, num30];
					}
				}
				if (num2 == 1)
				{
					taulukko3(aA2, bB, 2, 1);
				}
				if (num2 == 2)
				{
					taulukko3(aA2, bB, 1, 2);
				}
				rekursiohaku2(0, syvyysmax - 1, aA2, bB, array2, PRIORITY, rivi, suunta2, x, y, num2);
				for (int num31 = 8; num31 < laudankoko + 8; num31++)
				{
					for (int num32 = 8; num32 < laudankoko + 8; num32++)
					{
						if (PRIORITY[num32, num31] > 1 && PRIORITY[num32, num31] > num)
						{
							num = PRIORITY[num32, num31];
						}
					}
				}
				for (int num33 = 8; num33 < laudankoko + 8; num33++)
				{
					for (int num34 = 8; num34 < laudankoko + 8; num34++)
					{
						if (PRIORITY[num34, num33] > 1 && PRIORITY[num34, num33] < num)
						{
							PRIORITY[num34, num33] = 0;
						}
					}
				}
				for (int num35 = 8; num35 < laudankoko + 8; num35++)
				{
					for (int num36 = 8; num36 < laudankoko + 8; num36++)
					{
						if (PRIORITY[num36, num35] <= 1)
						{
							continue;
						}
						if (puhe == 1)
						{
							if (num6 <= 1)
							{
								CmpComString = "Computer: Now be careful!";
							}
							if (num6 == 2)
							{
								CmpComString = "Computer: Hehehee...";
							}
							if (num6 == 3)
							{
								CmpComString = "Computer: Hmmmmm... Let's see...";
							}
							if (num6 >= 4)
							{
								CmpComString = "Computer: I recommend to concentrate.";
							}
						}
						return 1;
					}
				}
				bool flag = true;
				if (num2 == 1)
				{
					taulukko3(aA2, bB, 1, 2);
				}
				if (num2 == 2)
				{
					taulukko3(aA2, bB, 2, 1);
				}
				for (int num37 = 8; num37 < laudankoko + 8; num37++)
				{
					for (int num38 = 8; num38 < laudankoko + 8; num38++)
					{
						if (PRIORITY[num38, num37] > 1)
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					rekursiohaku2(0, syvyysmax - 2, aA2, bB, array2, PRIORITY, rivi, suunta2, x, y, puoli);
					for (int num39 = 8; num39 < laudankoko + 8; num39++)
					{
						for (int num40 = 8; num40 < laudankoko + 8; num40++)
						{
							if (PRIORITY[num40, num39] > 1)
							{
								PRIORITY[num40, num39] /= 2;
							}
						}
					}
				}
			}
		}
		if (num3 == 1)
		{
			attacker(pISTEET);
		}
		if (num3 == 2)
		{
			defender(pISTEET);
		}
		if (num3 == 3)
		{
			preasure(pISTEET);
		}
		if (num3 == 4)
		{
			tbuilder(pISTEET);
		}
		if (num2 == 1)
		{
			taulukko4(aA, 2, 1);
		}
		if (num2 == 2)
		{
			taulukko4(aA, 1, 2);
		}
		for (int num41 = 0; num41 < 40; num41++)
		{
			for (int num42 = 8; num42 < laudankoko + 8; num42++)
			{
				for (int num43 = 8; num43 < laudankoko + 8; num43++)
				{
					lisaapriority(PRIORITY, aA, num43, num42, suunta, array, num41, pISTEET);
				}
			}
		}
		for (int num44 = 8; num44 < laudankoko + 8; num44++)
		{
			for (int num45 = 8; num45 < laudankoko + 8; num45++)
			{
				if (PRIORITY[num45, num44] > 1)
				{
					return 1;
				}
			}
		}
		for (int num46 = 9; num46 < laudankoko + 7; num46++)
		{
			for (int num47 = 9; num47 < laudankoko + 7; num47++)
			{
				if (array[num47, num46] == 0)
				{
					PRIORITY[num47, num46] = 50;
					if (array[num47 + 1, num46] != 0)
					{
						PRIORITY[num47, num46] += 100;
					}
					if (array[num47 - 1, num46] != 0)
					{
						PRIORITY[num47, num46] += 100;
					}
					if (array[num47, num46 - 1] != 0)
					{
						PRIORITY[num47, num46] += 100;
					}
					if (array[num47, num46 + 1] != 0)
					{
						PRIORITY[num47, num46] += 100;
					}
					if (array[num47 + 1, num46 + 1] != 0)
					{
						PRIORITY[num47, num46] += 100;
					}
					if (array[num47 - 1, num46 + 1] != 0)
					{
						PRIORITY[num47, num46] += 100;
					}
					if (array[num47 + 1, num46 - 1] != 0)
					{
						PRIORITY[num47, num46] += 100;
					}
					if (array[num47 - 1, num46 - 1] != 0)
					{
						PRIORITY[num47, num46] += 100;
					}
				}
			}
		}
		if (puhe == 1)
		{
			if (num6 <= 1)
			{
				CmpComString = "Computer: You have no good places!";
			}
			if (num6 == 2)
			{
				CmpComString = "Computer: Try to create something, please.";
			}
			if (num6 == 3)
			{
				CmpComString = "Computer: Not going very well...";
			}
			if (num6 >= 4)
			{
				CmpComString = "Computer: Boring...";
			}
		}
		return 0;
	}
}
public class Pelipaikka
{
	[field: CompilerGenerated]
	public int Y
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public int X
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}
}
}
