using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNamas.Models
{
	partial class clsGameTablero : ObservableObject
	{
		public const int FILAS = 8;
		public const int COLUMNAS = 8;

		List<Square> huecos;

		String[] piezasBlancas;

		String[] piezasNegras;

		long tiempo;

		public List<Square> Huecos
		{
			get => huecos;
			set => huecos = value;
		}
		public String[] PiezasBlancas
		{
			get => piezasBlancas;
			set => piezasBlancas = value;
		}

		public String[] PiezasNegras
		{
			get => piezasNegras;
			set => piezasNegras = value;
		}

		public long Tiempo { get { return tiempo; } set { tiempo = value; } }

		public clsGameTablero()
		{
			//Rellena los arrays de piezas
			CreatePiezas();
			Tiempo = 0;
			//Rellena el tablero
			SetUpPiezas();
		}



		private void CreatePiezas()
		{
			PiezasBlancas = new String[12];
			PiezasNegras = new String[12];
			for (int i = 0; i < 12; i++)
			{
				PiezasBlancas[i] = "piezablanca";
				PiezasNegras[i] = "piezanegra";
			}
		}




		void SetUpPiezas()
		{
			var contadorPB = 0;
			var contadorPN = 0;

			Huecos = new List<Square>();
			for (int fila = 0; fila<FILAS; fila++)
			{
				for (int columna = 0; columna < COLUMNAS; columna++)
				{
					if (fila < 3)
					{
						if ((fila%2==0 && columna%2==0) || (fila%2!=0 && columna%2!=0))
						{
							Huecos.Add(new Square(fila, columna, PiezasBlancas[contadorPB], "lightynone"));
							contadorPB++;
						}
						else
						{
							Huecos.Add(new Square(fila, columna, "shadynone", "shadynone"));
						}
					}
					else if (fila == 3)
					{
						if (columna%2==0)
						{
							Huecos.Add(new Square(fila, columna, "shadynone", "shadynone"));

						}
						else
						{
							Huecos.Add(new Square(fila, columna, "lightynone", "lightynone"));

						}
					}
					else if (fila == 4)
					{
						if (columna%2!=0)
						{
							Huecos.Add(new Square(fila, columna, "shadynone", "shadynone"));
						}
						else
						{
							Huecos.Add(new Square(fila, columna, "lightynone", "lightynone"));
						}
					}
					else if (fila>4)
					{
						if ((fila%2==0 && columna%2==0) || (fila%2!=0 && columna%2!=0))
						{
							Huecos.Add(new Square(fila, columna, PiezasNegras[contadorPN], "lightynone"));
							contadorPN++;
						}
						else
						{
							Huecos.Add(new Square(fila, columna, "shadynone", "shadynone"));
						}
					}
				}
			}
		}

		//void SetUpPiezas()
		//{//Conteo de piezas
		//	var contadorPB = 0;
		//	var contadorPN = 0;
		//	Huecos = new List<Square>();
		//	for (int fila = 0; fila <FILAS; fila++)
		//	{
		//		for (int j = 0; j < COLUMNAS; j++)
		//		{

		//			//Si la pieza está en una de las dos primeras lineas no podrá mopverse, ya que tiene una pieza propia delante
		//			if (fila < 2)
		//			{

		//				if ((fila%2!=0 && j%2!=0) || (fila%2==0 && j%2==0))
		//				{

		//					Huecos.Add(new Square(fila, j, PiezasBlancas[contadorPB], Colors.SandyBrown));
		//					contadorPB++;
		//				}
		//				else
		//				{

		//					Huecos.Add(new Square(fila, j, "", Colors.SaddleBrown));
		//				}


		//				//En la tercera linea las piezas no tendrán ninguna delante por lo que podrán moverse
		//			}
		//			else if (fila == 2)
		//			{
		//				if ((fila%2!=0 && j%2!=0) || (fila%2==0 && j%2==0))
		//				{

		//					Huecos.Add(new Square(fila, j, PiezasBlancas[contadorPB], Colors.SandyBrown));
		//					contadorPB++;
		//				}
		//				else
		//				{

		//					Huecos.Add(new Square(fila, j, "", Colors.SaddleBrown));
		//				}

		//			}
		//			else if (fila == 3 && j%2 ==0)
		//			{
		//				Huecos.Add(new Square(fila, j, "", Colors.SaddleBrown));
		//			}
		//			else if (fila == 3 && j%2 !=0)
		//			{
		//				Huecos.Add(new Square(fila, j, "none", Colors.SandyBrown));
		//			}	
		//			else if (fila == 4 && j%2 !=0)
		//			{
		//				Huecos.Add(new Square(fila, j, "", Colors.SaddleBrown));
		//			}
		//			else if (fila == 4 && j%2 ==0)
		//			{
		//				Huecos.Add(new Square(fila, j, "none", Colors.SandyBrown));
		//			}

		//			//Igual que el anterior pero al contrario, ya que es para el jugador de arriba
		//			else if (fila > 5 && fila < 8)
		//			{
		//				if ((fila%2!=0 && j%2!=0) || (fila%2==0 && j%2==0))
		//				{
		//					Huecos.Add(new Square(fila, j, PiezasNegras[contadorPN], Colors.SandyBrown));
		//					contadorPN++;
		//				}
		//				else
		//				{

		//					Huecos.Add(new Square(fila, j, "", Colors.SaddleBrown));
		//				}
		//			}
		//			else if (fila == 5)
		//			{
		//				if ((fila%2!=0 && j%2!=0) || (fila%2==0 && j%2==0))
		//				{
		//					Huecos.Add(new Square(fila, j, PiezasNegras[contadorPN], Colors.SandyBrown));
		//					contadorPN++;
		//				}
		//				else
		//				{

		//					Huecos.Add(new Square(fila, j, "", Colors.SaddleBrown));
		//				}

		//			}



		//		}

		//	}
		//}





	}
}
