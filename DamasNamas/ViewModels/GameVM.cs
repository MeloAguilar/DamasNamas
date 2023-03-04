using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DamasNamas.Models;
using DamasNamas.ViewModels.Utilidades;
using Entities;
using System.Collections.ObjectModel;

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


		public async void BeginMatch()
		{
			Tablero.Tiempo = 0;
			Estado = EstadosJuego.TurnoBlancas;
			await Shell.Current.DisplayAlert("", "Blancas empiezan", "Ok");
			await PutTiempo();
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


		private async Task PutTiempo()
		{
			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				PutTimer();
			});
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

			if (colorPieza.Equals(ColorPieza.Blanca))
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
					if (!hueco.TipoPieza.Equals(colorPieza))
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



		/// <summary>
		/// <p>
		/// Método que comprueba la posición de la pieza seleccionada,
		/// comprueba que esta tenga pieza, si es así se volcará en PiezaAnterior 
		/// una vez se compruebe que es su turno.
		/// </p>
		/// <pre>nada</pre>
		/// <post>nada</post>
		/// </summary>
		public void GetPosiblePosicion()
		{

			//Si ya había huecos en la lista de posibles huecos, se volverán a poner de color normal
			if (ListaPosiblesHuecos != null)
				ModificarColorHuecos("lightynone");
			//Si HuecoAnterior es null sé que es la primera selección de hueco
			//Si es la primera selección de hueco, se comprueba que el hueco seleccionado tenga pieza
			if (HuecoAnterior == null && (HuecoSeleccinado.Pieza == "piezablanca" || HuecoSeleccinado.Pieza == "piezanegra"))
			{
				if ((HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Blanca) && Estado.Equals(EstadosJuego.TurnoBlancas)) || (HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Negra) && Estado.Equals(EstadosJuego.TurnoNegras)))
				{
					ListaPosiblesHuecos = new ObservableCollection<Square>(GetMovimientoPieza(HuecoSeleccinado.PosX, HuecoSeleccinado.PosY, HuecoSeleccinado.TipoPieza));

					ModificarColorHuecos("selected");

					HuecoAnterior = new(HuecoSeleccinado);

				}
			}
			//Si el hueco anterior no es null, significa que ya se ha seleccionado una pieza
			//por lo que si el hueco que acabamos de seleccionar nu contiene ninguna pieza
			//se moverá la pieza del hueco anterior a este
			else if((HuecoAnterior.Pieza == "piezablanca" || HuecoAnterior.Pieza == "piezanegra") && !(HuecoSeleccinado.Pieza == "piezablanca" || HuecoSeleccinado.Pieza == "piezanegra"))
			{
				bool esPosible;
				try
				{
					var huecomprobar = ListaPosiblesHuecos.Where(x => x.PosX == HuecoSeleccinado.PosX && x.PosY == HuecoSeleccinado.PosY).First();
					esPosible = true;
				}
				catch (Exception e)
				{
					esPosible = false;
				}
				if (esPosible)
				{ 
				MovePieza();
				SetTurno();
				}


			}
			//Si el hueco anterior no es null, significa que ya se ha seleccionado una pieza
			//Por lo que la borraremos y haremos una llamada recursiva 
			//ya que ahora si entrará en el primer 'if'
			else if((HuecoSeleccinado.Pieza == "piezablanca" || HuecoSeleccinado.Pieza == "piezanegra") && (HuecoAnterior.Pieza == "piezablanca" || HuecoAnterior.Pieza == "piezanegra"))
			{
				if ((HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Blanca) && Estado.Equals(EstadosJuego.TurnoBlancas)) || (HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Negra) && Estado.Equals(EstadosJuego.TurnoNegras)))
				{
					HuecoAnterior = null;
					GetPosiblePosicion();
				}
			}
		}


		/// <summary>
		/// Método que comprueba el turno actual.
		/// Primero comprueba que queden piezas en las listas de ambos jugadores,
		/// si no es así se establece el estado final del juego,
		/// si es así, el turno cambiará al jugador contrario.
		/// 
		/// <pre>ninguna</pre>
		/// <post>ninguna</post>
		/// </summary>
		private void SetTurno()
		{
			if (Tablero.PiezasBlancas == null)
			{
				Estado = EstadosJuego.NegroGana;
			}
			else if (Tablero.PiezasNegras == null)
			{
				Estado = EstadosJuego.BlancoGana;
			}
			else if (Estado.Equals(EstadosJuego.TurnoBlancas))
			{
				Estado = EstadosJuego.TurnoNegras;
			}
			else if (Estado.Equals(EstadosJuego.TurnoNegras))
			{
				Estado = EstadosJuego.TurnoBlancas;
			}
		}




		private Square HuecoAnterior { get; set; }
		private ObservableCollection<Square> ListaPosiblesHuecos { get; set; }

		/// <summary>
		/// Método que se encarga de modificar el color de los huecos posibles
		/// </summary>
		/// <param name="color"></param>
		private void ModificarColorHuecos(String color)
		{
			foreach (var huec in ListaPosiblesHuecos)
			{
				if(!(huec.Pieza.Equals("piezanegra") || huec.Pieza.Equals("piezablanca"))) { 
				huec.Pieza = color;
				}
				huec.ColorFondo = color;
			}
		}


		private async void MovePieza()
		{
			try
			{
				//Busco el hueco seleccionado entre la lista de posibles movimientos y, solo si existe
				//en esta lista, se añadirá una pieza a este y se eliminara del hueco de los huecos del tablero
				//la pieza del hueco que coincida con PiezaAnterior
				 HuecosTablero
				.Where(x => x.PosX == HuecoAnterior.PosX && x.PosY == HuecoAnterior.PosY)
				.First().Pieza = "lightynone";




				ListaPosiblesHuecos
					.Where(x => x.PosX == HuecoSeleccinado.PosX && x.PosY == HuecoSeleccinado.PosY)
					.First()
					.Pieza = HuecoAnterior.Pieza;

			}
			catch(Exception ex) 
			{
				await Shell.Current.DisplayAlert("","Seleciona uno de los huecos resaltados","Ok");
			}
		}

		




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
