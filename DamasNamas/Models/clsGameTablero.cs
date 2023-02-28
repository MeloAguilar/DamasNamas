using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNamas.Models
{
	public class clsGameTablero
	{
		public const int FILAS = 8;
		public const int COLUMNAS = 8;

		List<Square> huecos;

		clsPieza[] piezasBlancas;

		clsPieza[] piezasNegras;

		long tiempo;

		public List<Square> Huecos
		{
			get => huecos;
			set => huecos = value;
		}
		public clsPieza[] PiezasBlancas
		{
			get => piezasBlancas;
			set => piezasBlancas = value;
		}

		public clsPieza[] PiezasNegras
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
			PiezasBlancas = new clsPieza[12];
			PiezasNegras = new clsPieza[12];
			for (int i = 0; i < 12; i++)
			{
				PiezasBlancas[i] = new clsPieza(ColorPieza.Blanco);
				PiezasNegras[i] = new clsPieza(ColorPieza.Negro);
			}
		}



		void SetUpPiezas()
		{//Conteo de piezas
			var contadorPB = 0;
			var contadorPN = 0;
			Huecos = new List<Square>();
			for (int i = 0; i <FILAS; i++)
			{
				for (int j = 0; j < COLUMNAS; j++)
				{

					//Si la pieza está en una de las dos primeras lineas no podrá mopverse, ya que tiene una pieza propia delante
					if (i < 2)
					{

						if ((i%2!=0 && j%2!=0) || (i%2==0 && j%2==0))
						{

							Huecos.Add(new Square(i, j, PiezasBlancas[contadorPB], Colors.SandyBrown, false));
							contadorPB++;
						}
						else
						{

							Huecos.Add(new Square(i, j, null, Colors.SaddleBrown, false));
						}


						//En la tercera linea las piezas no tendrán ninguna delante por lo que podrán moverse
					}
					else if (i == 2)
					{
						if ((i%2!=0 && j%2!=0) || (i%2==0 && j%2==0))
						{

							Huecos.Add(new Square(i, j, PiezasBlancas[contadorPB], Colors.SandyBrown, true));
							contadorPB++;
						}
						else
						{

							Huecos.Add(new Square(i, j, null, Colors.SaddleBrown, false));
						}

					}
					else if (i == 3 && j%2 ==0)
					{
						Huecos.Add(new Square(i, j, null, Colors.SaddleBrown, false));
					}
					else if (i == 3 && j%2 !=0)
					{
						Huecos.Add(new Square(i, j, null, Colors.SandyBrown, false));
					}
					else if (i == 4 && j%2 !=0)
					{
						Huecos.Add(new Square(i, j, null, Colors.SaddleBrown, false));
					}
					else if (i == 4 && j%2 ==0)
					{
						Huecos.Add(new Square(i, j, null, Colors.SandyBrown, false));
					}

					//Igual que el anterior pero al contrario, ya que es para el jugador de arriba
					else if (i > 5 && i < 8)
					{
						if ((i%2!=0 && j%2!=0) || (i%2==0 && j%2==0))
						{
							Huecos.Add(new Square(i, j, PiezasNegras[contadorPN], Colors.SandyBrown, false));
							contadorPN++;
						}
						else
						{

							Huecos.Add(new Square(i, j, null, Colors.SaddleBrown, false));
						}
					}
					else if (i == 5)
					{
						if ((i%2!=0 && j%2!=0) || (i%2==0 && j%2==0))
						{
							Huecos.Add(new Square(i, j, PiezasNegras[contadorPN], Colors.SandyBrown, true));
							contadorPN++;
						}
						else
						{

							Huecos.Add(new Square(i, j, null, Colors.SaddleBrown, false));
						}

					}



				}

			}
		}





	}
}
