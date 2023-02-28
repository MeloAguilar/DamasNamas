using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNamas.Models
{
	public class Square
	{
		int posX;
		int posY;


		bool esPosibleMover;

		Color color;

		private clsPieza pieza;

		public clsPieza Pieza
		{
			get { return pieza; }
			set { pieza = value; }
		}

		public int PosX { get; set; }
		public Color Color { get => color; set => color = value; }
		public int PosY { get; set; }
		public bool EsPosibleMover { get => esPosibleMover; set => esPosibleMover = value; }



		//Constructor para insertar directamente una pieza.
		public Square(int posX, int posY, clsPieza _pieza, Color color, bool esposible)
		{

			PosX=posX;
			PosY=posY;
			Pieza = _pieza;
			Color= color;
			EsPosibleMover = esposible;

		}


		//Constructor para insertar un square sin pieza.
		public Square(int posX, int posY, Color color)
		{
			PosX=posX;
			PosY=posY;
			Pieza = null;
			EsPosibleMover=false;
			Color= color;
		}

	}
}
