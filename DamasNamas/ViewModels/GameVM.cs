using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DamasNamas.Models;
using DamasNamas.ViewModels.Utilidades;
using Entities;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNamas.ViewModels
{
	[QueryProperty(nameof(JugadorAbajo), "JugadorQueMando")]
	partial class GameVM : VMBase
	{
		clsGameTablero tablero;

		clsJugador jugadorArriba;

		clsJugador jugadorAbajo;

		Square huecoSeleccionado;

		[ObservableProperty]
		private ObservableCollection<Square> huecosTablero;

		EstadosJuego estado;

		TimeSpan reloj;

		#region Properties


		public clsGameTablero Tablero
		{
			get
			{
				return tablero;
			}
			set
			{
				tablero = value;
				OnPropertyChanged("Tablero");
			}
		}



		public clsJugador JugadorArriba
		{
			get
			{
				return jugadorArriba;
			}
			set
			{
				jugadorArriba = value;
				OnPropertyChanged("JugadorArriba");
			}
		}



		public Square HuecoSeleccinado
		{
			get
			{
				return huecoSeleccionado;
			}
			set
			{
				huecoSeleccionado = value;
				GetPosiblePosicion();
				
				foreach (var hueco in HuecosTablero.Where(x => x.ColorFondo.Equals(Colors.LightPink)))
				{
					hueco.ColorFondo = Colors.RosyBrown;
				}
				OnPropertyChanged("HuecoSeleccionado");
				OnPropertyChanged(nameof(HuecosTablero));
			}
		}

		public clsJugador JugadorAbajo
		{
			get { return jugadorAbajo; }
			set { jugadorAbajo= value; OnPropertyChanged(nameof(JugadorAbajo)); }
		}

		public EstadosJuego Estado
		{
			get { return estado; }
			set { estado = value; OnPropertyChanged(nameof(Estado)); }
		}
		public TimeSpan Reloj
		{
			get { return reloj; }
			set { reloj = value; OnPropertyChanged(nameof(Reloj)); }
		}




		#endregion


		public GameVM()
		{

			tablero = new clsGameTablero();
			HuecosTablero = new ObservableCollection<Square>(Tablero.Huecos);
			jugadorArriba = new clsJugador();
			jugadorAbajo = new clsJugador();
			BeginMatch();
		}


		//Método que se encargue de mover la pieza
		//Debe comprobar que el hueco seleccionado es válido(tiene una pieza y es del color del jugador que está moviendo)
		//Debe comprobar si (Posx+1 & Psy+1) & (Posx+1 & PosY-1) están vacías
		//Si están vacías debe preguntarse al jugador a cual de las dos casillas quiere mover
		//Si no están vacías debe comprobarse si la pieza que hay en la casilla es del color contrario
		//Si es del color contrario debe comprobarse si (Posx+2 & Psy+2) & (Posx+2 & PosY-2) están vacías
		//Si una de ellas está vacía debe preguntarse al jugador si desea comer la pieza y moverse a esa casilla
		//Si está ocupada por una pieza del mismo color no se debe mostrar como opción
		//Si (Posx+1 & Psy+1) está ocupada por una pieza contraria pero (Posx+2 & Psy+2) está ocupada por cualquier pieza, no se debe mostrar como opción
		[RelayCommand]
		public void EleccionHueco()
		{
			List<Square> list = new List<Square>();
			if (HuecoSeleccinado != null)
				if (HuecoSeleccinado != null)
				{

				}

		}


		public void BeginMatch()
		{
			Tablero.Tiempo = 0;
			Estado = EstadosJuego.TurnoBlancas;
			Shell.Current.DisplayAlert("", "Blancas empiezan", "Ok");
			PutTiempo();
		}

		//public void EndMatch()
		//{
		//	if (Tablero.cantidadFichasAbajo == 0)
		//		Estado = EstadosJuego.BlancoGana;
		//	else if (Tablero.cantidadFichasArriba == 0)
		//		Estado = EstadosJuego.NegroGana;

		//}


		//	PutTiempo();
		//}

		public void ChangeTurn()
		{
			if (Estado.Equals(EstadosJuego.TurnoBlancas))
				Estado = EstadosJuego.TurnoNegras;
			else if (Estado.Equals(EstadosJuego.TurnoNegras))
				Estado = EstadosJuego.TurnoBlancas;
		}


		private void PutTimer()
		{
			try
			{
				//Si el estado del juego sigue siendo BlancoMueve o NegroMueve, el Timer seguirá corriendo
				if (!(Estado.Equals(EstadosJuego.BlancoGana) || Estado.Equals(EstadosJuego.NegroGana)))
				{
					//Iniciamos el Timer y le damos un intervalo de un segundo 
					var timer = Application.Current.Dispatcher.CreateTimer();
					timer.Interval = TimeSpan.FromMilliseconds(1000);
					//Le decimos al timer que con cada tick(cada 1000 milis) 
					//deberá cambiar el string del reloj 
					timer.Tick += (s, e) =>
					{
						Reloj = TimeSpan.FromMilliseconds(Tablero.Tiempo);
						Tablero.Tiempo+=1000;
					};
					timer.Start();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

		}


		private async void PutTiempo()
		{
			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				PutTimer();
			});
		}



		private Square GetHuecomida(int fila, int columna, Square posicionDespuesDeComer)
		{



			return null;
		}


		/// <summary>
		/// Método que obtiene el movimiento posible de una pieza
		/// </summary>
		/// <param name="fila"></param>
		/// <param name="columna"></param>
		/// <param name="colorPieza"></param>
		/// <returns></returns>
		private List<Square> GetMovimientoPieza(int fila, int columna, ColorPieza colorPieza)
		{
			//Recojo los unicos dos movimientos posibles (en principio) en las Damas
			var posibleX = fila;
			var PosibleYmayor = columna+1;
			var posibleYmenor = columna-1;
			//Si la pieza es blanca, sumaré 1 a la fila, si es negra restaré 1

			if (colorPieza.Equals(ColorPieza.Blanco))
				posibleX += 1;
			else
				posibleX -= 1;

			//Busco los huecos que coincidan con las posiciones posibles
			var listaPosibles = Tablero.Huecos.FindAll
					(
					x => 
					((x.PosX == posibleX))
					&&
					(x.PosY == PosibleYmayor || x.PosY == posibleYmenor)
					);
			var backupList = new List<Square>(listaPosibles);
			foreach (var hueco in listaPosibles)
			{
				//Si el hueco no está vacío habrá que comprobar si la pieza que hay en él es del color contrario
				//Si lo es, habrá que comprobar si la casilla siguiente está vacía
				//Si está vacía, habrá que preguntar al jugador si desea comer la pieza y moverse a esa casilla
				//Si no está vacía, no se mostrará como opción

				if (hueco.Pieza != null)
					//Comer pieza si la pieza del hueco es del otro color
					if (!hueco.Pieza.colorPieza.Equals(colorPieza))
					{
						//Llamar a otro método que compruebe si la casilla siguiente está vacía
						//var posibleComida = Tablero.Huecos.Find(x => x.PosX == posibleX + 1 && x.PosY == PosibleYmayor + 1 || x.PosY == posibleYmenor - 1);
					}
					//Se elimina la pieza si es del mismo color
					else
					{
						backupList.Remove(hueco);
					}
				//Si la casilla está vacía se seguirá tomando como opción


				listaPosibles = new List<Square>(backupList);

			}
			return listaPosibles;
		}


		
		
		public async void GetPosiblePosicion()
		{
			if(HuecoSeleccinado.Pieza.colorPieza.Equals(ColorPieza.Blanco) && Estado.Equals(EstadosJuego.TurnoBlancas)) 
			{
				var listaPosibles = GetMovimientoPieza(HuecoSeleccinado.PosX, HuecoSeleccinado.PosY, ColorPieza.Blanco);
				foreach (var huec in listaPosibles)
				{
					huec.ColorFondo = Colors.LightPink;
				}
				
			}
		}
		////Comprobamos que haya ficha en el hueco seleccionado
		////Comprobamos que la ficha sea blanca y que el turno sea de las blancas
		//if (HuecoSeleccinado != null)
		//{
		//	if (HuecoSeleccinado.Pieza != null)
		//	{
		//		if (HuecoSeleccinado.Pieza.colorPieza.Equals(ColorPieza.Blanco) && estado.Equals(EstadosJuego.TurnoBlancas))
		//		{
		//			//Devolvemos la lista de huecos posibles
		//			var list = GetMovimientoPieza(HuecoSeleccinado.PosX, HuecoSeleccinado.PosY, HuecoSeleccinado.Pieza.colorPieza);
		//			if (list.Count() == 1)
		//			{
		//				var eleccion = await Shell.Current.DisplayActionSheet("Mover a?", "Cancel", null, $"x:{list[0].PosX} y:{list[0].PosY}");
		//				if(eleccion.Equals($"x:{list[0].PosX} y:{list[0].PosY}"))
		//				{

		//					HuecosTablero.Where(x => x.PosX == list[0].PosX && x.PosY == list[0].PosY).First().Pieza = new clsPieza(HuecoSeleccinado.Pieza.colorPieza, HuecoSeleccinado.Pieza.esReina);

		//					HuecoSeleccinado.Pieza = null;
		//					HuecoSeleccinado = null;
		//					OnPropertyChanged(nameof(HuecosTablero));
		//					OnPropertyChanged(nameof(HuecoSeleccinado.Pieza));
		//					Estado = EstadosJuego.TurnoNegras;
		//				}

		//				//Mostramos al usuario las posibles elecciones
		//				//Llamamos a una funcion que modifique el tablero segun la eleccion del usuario
		//			}
		//			else if (list.Count() == 2)
		//			{
		//				var eleccion = Shell.Current.DisplayActionSheet("Mover a?", "Cancel", null, $"x:{list[0].PosX} y:{list[0].PosY}", $"x:{list[1].PosX} y:{list[1].PosY}");

		//				OnPropertyChanged(nameof(HuecosTablero));

		//			}
		//			else
		//			{
		//				await Shell.Current.DisplayAlert("Eres tonto?", "No ves que la ficha está encerrá?", "LO SIENTO");
		//				HuecoSeleccinado= null;
		//			}
		//		}



		//		else if (HuecoSeleccinado.Pieza.colorPieza.Equals(ColorPieza.Negro) && estado.Equals(EstadosJuego.TurnoNegras))
		//		{
		//			var list = GetMovimientoPieza(HuecoSeleccinado.PosX, HuecoSeleccinado.PosY, HuecoSeleccinado.Pieza.colorPieza);
		//			if (list.Count() == 1)
		//			{
		//				var eleccion = await Shell.Current.DisplayActionSheet("Mover a?", "Cancel", null, $"x:{list[0].PosX} y:{list[0].PosY}");
		//				if (eleccion.Equals($"x:{list[0].PosX} y:{list[0].PosY}"))
		//				{

		//					HuecosTablero.Where(x => x.PosX == list[0].PosX && x.PosY == list[0].PosY).First().Pieza = new clsPieza(HuecoSeleccinado.Pieza.colorPieza, HuecoSeleccinado.Pieza.esReina);
		//					HuecoSeleccinado.Pieza = null;
		//					HuecoSeleccinado = null;
		//					OnPropertyChanged(nameof(HuecosTablero));
		//					Estado = EstadosJuego.TurnoBlancas;
		//				}
		//				else if (eleccion.Equals($"x:{list[1].PosX} y:{list[1].PosY}"))
		//				{

		//					HuecosTablero.Where(x => x.PosX == list[1].PosX && x.PosY == list[1].PosY).First().Pieza = new clsPieza(HuecoSeleccinado.Pieza.colorPieza, HuecoSeleccinado.Pieza.esReina);
		//					HuecoSeleccinado.Pieza = null;
		//					HuecoSeleccinado = null;
		//					OnPropertyChanged(nameof(HuecosTablero));
		//					Estado = EstadosJuego.TurnoBlancas;
		//				}

		//				//Mostramos al usuario las posibles elecciones
		//				//Llamamos a una funcion que modifique el tablero segun la eleccion del usuario
		//			}
		//			else if (list.Count() == 2)
		//			{
		//				var eleccion = Shell.Current.DisplayActionSheet("Mover a?", "Cancel", null, $"x:{list[0].PosX} y:{list[0].PosY}", $"x:{list[1].PosX} y:{list[1].PosY}");
		//				if (eleccion.Equals($"x:{list[0].PosX} y:{list[0].PosY}"))
		//				{

		//					HuecosTablero.Where(x => x.PosX == list[0].PosX && x.PosY == list[0].PosY).First().Pieza = new clsPieza(HuecoSeleccinado.Pieza.colorPieza, HuecoSeleccinado.Pieza.esReina);
		//					HuecoSeleccinado.Pieza = null;
		//					HuecoSeleccinado = null;
		//					OnPropertyChanged(nameof(HuecosTablero));
		//					Estado = EstadosJuego.TurnoBlancas;
		//				}
		//				else if (eleccion.Equals($"x:{list[1].PosX} y:{list[1].PosY}"))
		//				{

		//					HuecosTablero.Where(x => x.PosX == list[1].PosX && x.PosY == list[1].PosY).First().Pieza = new clsPieza(HuecoSeleccinado.Pieza.colorPieza, HuecoSeleccinado.Pieza.esReina);
		//					HuecoSeleccinado.Pieza = null;
		//					HuecoSeleccinado = null;
		//					OnPropertyChanged(nameof(HuecosTablero));
		//					Estado = EstadosJuego.TurnoBlancas;
		//				}
		//				OnPropertyChanged(nameof(HuecosTablero));

		//			}
		//			else
		//			{
		//				await Shell.Current.DisplayAlert("Eres tonto?", "No ves que la ficha está encerrá?", "LO SIENTO");
		//				HuecoSeleccinado= null;
		//			}
		//		}
		//		else
		//		{
		//			await Shell.Current.DisplayAlert("Eres tonto?", "No toques lo que no es tuyo", "LO SIENTO");
		//			HuecoSeleccinado= null;
		//		}
		//	}
		//	else
		//	{
		//		await Shell.Current.DisplayAlert("Eres tonto?", "No ves que no hay ficha ?", "LO SIENTO");
		//		HuecoSeleccinado= null;
		//	}






	}



	#region Comandos

	#endregion

	public enum EstadosJuego
	{
		TurnoBlancas,
		TurnoNegras,
		BlancasMoviendo,
		NegrasMoviendo,
		NegroGana,
		BlancoGana
	}
}
