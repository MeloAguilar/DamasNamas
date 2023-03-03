using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNamas.Models
{
	partial class Square : ObservableObject
	{
		int posX;
		int posY;


		bool esPosibleMover;

		
		Color color;

		private clsPieza pieza;

		public clsPieza Pieza
		{
			get { return pieza; }
			set { 
				pieza = value;
				OnPropertyChanged("Pieza");
			}
		}



		public Color ColorFondo { get { return color; } set { color= value; OnPropertyChanged(nameof(ColorFondo)); } }

		public int PosX { get; set; }
		public int PosY { get; set; }
		public bool EsPosibleMover { get => esPosibleMover; 
			set 
			{ 
				esPosibleMover = value; 
				OnPropertyChanged("Pieza"); 
			} 
		}



		//Constructor para insertar directamente una pieza.
		public Square(int posX, int posY, clsPieza _pieza, Color color, bool esposible)
		{

			PosX=posX;
			PosY=posY;
			Pieza = _pieza;
			ColorFondo= color;
			EsPosibleMover = esposible;

		}


		//Constructor para insertar un square sin pieza.
		public Square(int posX, int posY, Color color)
		{
			PosX=posX;
			PosY=posY;
			Pieza = null;
			EsPosibleMover=false;
			ColorFondo= color;
		}

		//Constructor copia
		public Square(Square _square)
		{
			PosX=_square.PosX;
			PosY=_square.PosY;
			Pieza = _square.Pieza;
			EsPosibleMover=_square.EsPosibleMover;
			ColorFondo= _square.ColorFondo;
		}
		

	}
}
