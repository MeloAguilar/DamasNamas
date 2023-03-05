﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DamasNamas.Models;
using DamasNamas.ViewModels.Utilidades;
using Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

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

		String relojMostrado;

		TimeSpan reloj;

		#region Properties

		public Color ColorTurnoArriba
		{
			get
			{
				return colorTurnoArriba;
			}
			set
			{
				colorTurnoArriba = value;
				OnPropertyChanged(nameof(ColorTurnoArriba));
			}
		}

		public String RelojMostrado
		{
			get
			{
				return relojMostrado;
			}
			set
			{
				relojMostrado = value;
				OnPropertyChanged(nameof(RelojMostrado));
			}
		}



		public Color ColorTurnoAbajo
		{
			get
			{
				return colorTurnoAbajo;
			}
			set
			{
				colorTurnoAbajo = value;
				OnPropertyChanged(nameof(ColorTurnoAbajo));
			}
		}
		Color colorTurnoArriba;

		Color colorTurnoAbajo;
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
			ColorTurnoArriba = Colors.LightGreen;
			ColorTurnoAbajo = Colors.LightGray;
			PosiblesComidas = new List<Square>();
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


		/// <summary>
		/// Método que se encarga de establecer lo que se hará tras cada tick del Timer.
		/// 
		/// 
		/// </summary>
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
						actualizarRelojMostrado();
					};
					timer.Start();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		/// <summary>
		/// Método que se encarga de quitar la hora al timespan convirtiendolo en un string que solo contiene la 
		/// informacion visual de este
		/// <pre>nada</pre>
		/// <post>nada</post>
		/// </summary>
		void actualizarRelojMostrado()
		{

			var mins = Reloj.Minutes.ToString();
			var secs = Reloj.Seconds.ToString();
			if (Reloj.Minutes< 10)
			{
				mins= $"0{mins}";
			}
			if (Reloj.Seconds < 10)
			{
				secs = $"0{secs}";
			}
			RelojMostrado =  $"{mins} : {secs}";
		}
		/// <summary>
		/// Método que se encarga de inicializar el temporizador en el hilo principal
		/// </summary>
		/// <returns></returns>
		private async Task PutTiempo()
		{
			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				PutTimer();
			});
		}




		/// <summary>
		/// Método que comprueba si una pieza que ha sido guardada en la lista de posibles
		/// movimientos de otra pero contiene una pieza del color contrario a la que se está moviendo
		/// tiene una casilla vacía en la diagonal que corresponde.
		/// <pre>nada</pre>
		/// <post>nada</post>
		/// </summary>
		/// <param name="hueco">objeto Square que va a compararse con el huecoSeleccionado para mover</param>
		/// <returns>
		///		==null: si hay una pieza tras la que se quiere comer y no hay hueco vacío tras ella
		///		!=null: si hay hueco vacío tras la pieza que se quiere comer
		/// </returns>
		private Square Huecomida(Square hueco)
		{
			//Comenzaremos a buscar la pieza en la posicion del hueco llegado como parámetro
			var posibleX = hueco.PosX;
			var posibleY = hueco.PosY;
			//Si el hueco que mandamos está más a la derecha que la pieza que queremos mover
			//se sumará uno a la posicion en Y que vamos a comprobar
			if (hueco.PosY > HuecoSeleccinado.PosY)
			{
				posibleY +=1;
			}
			else
			{
				posibleY -=1;
			}

			//Si la pieza seleccionada anteriormente es blanca se sumará uno, sino, se restará a la 
			//posicion que buscaremos en x
			if (!HuecoSeleccinado.EsReina)
			{
				if (HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Blanca))
					posibleX += 1;
				else
					posibleX -= 1;
			}
			else
			{
				if (hueco.PosX > HuecoSeleccinado.PosX)
				{
					posibleX +=1;
				}
				else
				{
					posibleX -=1;
				}
			}
			//Recogemos el hueco del tablero
			if (posibleX >= 0 && posibleX < 8 && posibleY >= 0 && posibleY < 8)
			{
				var huecomprobar = HuecosTablero.Where(x => x.PosY == posibleY && x.PosX == posibleX).First();
				if (huecomprobar.TipoPieza.Equals(ColorPieza.None))
				{
					return huecomprobar;
				}
				else return null;
			}
			else
			{
				return null;
			}
		}



		private List<Square> PosiblesComidas {   get; set;  }

		private ObservableCollection<Square> GetMovimientoReina()
		{

			int fila = HuecoSeleccinado.PosX;
			int columna = HuecoSeleccinado.PosY;
			//var posibleXmayor = fila+1;
			//var posibleXmenor = fila-1;
			//var posibleYmayor = columna+1;
			//var posibleYmenor = columna-1;





			//Recogemos los cuatro huecos diagonales





			ColorPieza colorPieza = HuecoSeleccinado.TipoPieza;
			var listaPosibles = new ObservableCollection<Square>();

			//Recorro las dos diagonales de la reina
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					//Inicio en la posicion fila-1 y columna-1
					int row = fila + i;
					int col = columna + j;
					//Mientras que no se salga del tablero y no encuentre una pieza
					//la añadiré al listado de posibles movimientos
					while (row >= 0 && row < 8 && col >= 0 && col < 8)
					{

						var hueco = HuecosTablero.Where(x => x.PosX == row && x.PosY == col).First();
						listaPosibles.Add(hueco);
						if (!hueco.TipoPieza.Equals(ColorPieza.None))
						{
							break;
						}
						//Siempre que se compruebe un
						row += i;
						col += j;
					}

				}
			

			}

			//Vacío la lista en una backup para eliminar de esta las que no vaya a marcar
			var backupList = new List<Square>(listaPosibles);


			foreach (var hueco in listaPosibles)
			{


				//Comer pieza si la pieza del hueco es del otro color
				if (!hueco.TipoPieza.Equals(colorPieza) && !hueco.TipoPieza.Equals(ColorPieza.None))
				{
					//Llamo al método que me devolverá un Square si se puede realizar la accion y null si no 
					var huequitoacomprobar = Huecomida(hueco);

					if (huequitoacomprobar != null)
					{
						backupList.Add(huequitoacomprobar);
						PosiblesComidas.Add(hueco);
					}
						

					backupList.Remove(hueco);
				}
				//Se elimina la pieza si es del mismo color
				else if (hueco.TipoPieza.Equals(colorPieza))
				{
					backupList.Remove(hueco);
				}
				//Si la casilla está vacía se seguirá tomando como opción

			}






			listaPosibles = new ObservableCollection<Square>(backupList);

			return listaPosibles;
		}







		/// <summary>
		/// Método que obtiene el movimiento posible de una pieza
		/// </summary>
		/// <param name="fila"></param>
		/// <param name="columna"></param>
		/// <param name="colorPieza"></param>
		/// <returns></returns>
		private List<Square> GetMovimientoPieza()
		{

			//Recojo los unicos dos movimientos posibles (en principio) en las Damas
			var posibleX = HuecoSeleccinado.PosX;
			var PosibleYmayor = HuecoSeleccinado.PosY+1;
			var posibleYmenor = HuecoSeleccinado.PosY-1;
			//Si la pieza es blanca, sumaré 1 a la fila, si es negra restaré 1
			ColorPieza colorPieza = HuecoSeleccinado.TipoPieza;
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


				//Comer pieza si la pieza del hueco es del otro color
				if (!hueco.TipoPieza.Equals(colorPieza) && !hueco.TipoPieza.Equals(ColorPieza.None))
				{
					//Llamar a otro método que compruebe si la casilla siguiente está vacía
					//var posibleComida = Tablero.Huecos.Find(x => x.PosX == posibleX + 1 && x.PosY == PosibleYmayor + 1 || x.PosY == posibleYmenor - 1);

					var huequitoacomprobar = Huecomida(hueco);

					if (huequitoacomprobar != null)
						backupList.Add(huequitoacomprobar);
					backupList.Remove(hueco);
				}
				//Se elimina la pieza si es del mismo color
				else if (hueco.TipoPieza.Equals(colorPieza))
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
			if (HuecoAnterior == null && (HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Blanca) || HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Negra)))
			{


				if ((HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Blanca) && Estado.Equals(EstadosJuego.TurnoBlancas)) || (HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Negra) && Estado.Equals(EstadosJuego.TurnoNegras)))
				{
					if (!HuecoSeleccinado.EsReina)
					{
						ListaPosiblesHuecos = new ObservableCollection<Square>(GetMovimientoPieza());



					}
					else
					{
						try
						{
							ListaPosiblesHuecos = new ObservableCollection<Square>(GetMovimientoReina());
						}
						catch (Exception ex)
						{
							throw ex;
						}


					}
					ModificarColorHuecos("selected");

					HuecoAnterior = new(HuecoSeleccinado);
				}


			}
			//Si el hueco anterior no es null, significa que ya se ha seleccionado una pieza
			//por lo que si el hueco que acabamos de seleccionar nu contiene ninguna pieza
			//se moverá la pieza del hueco anterior a este
			else if (HuecoAnterior != null && (HuecoAnterior.TipoPieza.Equals(ColorPieza.Blanca) || HuecoAnterior.TipoPieza.Equals(ColorPieza.Negra)) && !(HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Blanca) || HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Negra)))
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

				}


			}
			//Si el hueco anterior no es null, significa que ya se ha seleccionado una pieza
			//Por lo que la borraremos y haremos una llamada recursiva 
			//ya que ahora si entrará en el primer 'if'
			else if ((HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Blanca) || HuecoSeleccinado.TipoPieza.Equals(ColorPieza.Negra)) && (HuecoAnterior.TipoPieza.Equals(ColorPieza.Blanca) || HuecoAnterior.TipoPieza.Equals(ColorPieza.Negra)))
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
			if (Tablero.PiezasBlancas == 0)
			{
				Estado = EstadosJuego.NegroGana;
			}
			else if (Tablero.PiezasNegras == 0)
			{
				Estado = EstadosJuego.BlancoGana;

			}
			else if (Estado.Equals(EstadosJuego.TurnoBlancas))
			{
				Estado = EstadosJuego.TurnoNegras;
				ColorTurnoAbajo = Colors.LightGreen;
				ColorTurnoArriba = Colors.LightGray;
			}
			else if (Estado.Equals(EstadosJuego.TurnoNegras))
			{
				Estado = EstadosJuego.TurnoBlancas;
				ColorTurnoArriba = Colors.LightGreen;
				ColorTurnoAbajo = Colors.LightGray;
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
				if (!(huec.Pieza.Equals("piezanegra") || huec.Pieza.Equals("piezablanca") || huec.Pieza.Equals("reinanegra") || huec.Pieza.Equals("reinablanca")))
				{
					huec.Pieza = color;
				}
				huec.ColorFondo = color;
			}
		}

		int selectedIndex = -1;
		public int SelectedIndex
		{
			get => selectedIndex;
			set
			{
				if (selectedIndex != value)
				{
					selectedIndex = value;
					OnPropertyChanged("SelectedIndex");
				}
			}
		}





		/// <summary>
		/// 
		/// </summary>
		private async void MovePieza()
		{
			try
			{
				var hueco = ListaPosiblesHuecos
					.Where(x => x.PosX == HuecoSeleccinado.PosX && x.PosY == HuecoSeleccinado.PosY)
					.First();
				hueco.Pieza = HuecoAnterior.Pieza;
				hueco.EsReina = HuecoAnterior.EsReina;


				HuecosTablero
			   .Where(x => x.PosX == HuecoAnterior.PosX && x.PosY == HuecoAnterior.PosY)
			   .First().Pieza = "lightynone";

				TransformarReina();

				//Comerpieza
				if (!Comer() || (ListaPosiblesHuecos.Count == 0))
				{
					SetTurno();
				}

				//Busco el hueco seleccionado entre la lista de posibles movimientos y, solo si existe
				//en esta lista, se añadirá una pieza a este y se eliminara del hueco de los huecos del tablero
				//la pieza del hueco que coincida con PiezaAnterior






			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("", "Hubo un error inesperado", "Ok");
			}
		}



		/// <summary>
		/// Método que se encarga de, en caso de que una pieza llegue al final del tablero,
		/// establecer la imagen como la reina perteneciente y establecer el booleano esReina 
		/// a verdadero.
		/// <pre>ninguna</pre>
		/// <post>ninguna</post>
		/// </summary>
		private void TransformarReina()
		{
			//Una blanca no será reina hasta que no llegue a la posicion 7
			if (Estado.Equals(EstadosJuego.TurnoBlancas) && HuecoSeleccinado.PosX == 7)
			{
				HuecoSeleccinado.Pieza = "reinablanca";
				HuecoSeleccinado.EsReina = true;
			}
			//Una negra np será reina hasta que llegue a la posición 0
			else if (Estado.Equals(EstadosJuego.TurnoNegras) && huecoSeleccionado.PosX == 0)
			{
				HuecoSeleccinado.Pieza = "reinanegra";
				HuecoSeleccinado.EsReina = true;
			}
		}


		

		Square RealizarComida()
		{
			var difY = Math.Sign(HuecoSeleccinado.PosY - HuecoAnterior.PosY);
			var difX = Math.Sign(HuecoSeleccinado.PosX - HuecoAnterior.PosX);
			Square huecoAComer = null;

			if (difY > 0 && difX > 0)
			{
				huecoAComer = HuecosTablero.Where(x => x.PosX == (HuecoSeleccinado.PosX - 1) && x.PosY == (HuecoSeleccinado.PosY - 1)).First();

			}else if (difY > 0 && difX < 0)
			{
				huecoAComer = HuecosTablero.Where(x => x.PosX == (HuecoSeleccinado.PosX - 1) && x.PosY == (HuecoSeleccinado.PosY + 1)).First();
			}
			else if(difY < 0 && difX > 0)
			{
				huecoAComer=HuecosTablero.Where(x => x.PosX == (HuecoSeleccinado.PosX + 1) && x.PosY == (HuecoSeleccinado.PosY - 1)).First();
			}
			else if(difY < 0 && difX < 0) 
			{
				huecoAComer = HuecosTablero.Where(x => x.PosX == (HuecoSeleccinado.PosX + 1) &&x.PosY == (HuecoSeleccinado.PosY + 1)).First();
			}
			return huecoAComer;
		}
		/// <summary>
		/// Método que se encarga de comprobar si una pieza se cruza en el camino de otra,
		/// después de esto, comprueba la casilla siguiente a la pieza que se ha comido,
		/// si ahi no se encuentra ninguna pieza, se elimina la pieza comida y se resta del contador de
		/// piezas del jugador contrario.
		/// <pre>nada</pre>
		/// <post>nada</post>
		/// </summary>
		/// <returns>
		/// true -> se ha comido la pieza
		/// false -> no se ha comido la pieza
		/// </returns>
		private bool Comer()
		{
			//Comprobamos si la diferencia entre los huecos es negativa o positiva.
			var signoDifY = Math.Sign(HuecoSeleccinado.PosY - HuecoAnterior.PosY);
			var haComido = false;
			Square huecomido = null;
			//Si no es reina se comprueba solo el movimiento hacia delante
			if (!HuecoSeleccinado.EsReina)
			{
				if (Estado.Equals(EstadosJuego.TurnoBlancas) && (HuecoSeleccinado.PosX > HuecoAnterior.PosX+1))
				{

					huecomido = HuecosTablero
						.Where(
						x =>
						((x.PosX > HuecoAnterior.PosX) && (x.PosX < HuecoSeleccinado.PosX))
						&&
						(x.PosY == HuecoAnterior.PosY+signoDifY))
						.First();
					huecomido.Pieza = "lightynone";

					Tablero.PiezasNegras--;
					haComido = true;
				}
				else if (Estado.Equals(EstadosJuego.TurnoNegras) && (HuecoSeleccinado.PosX < HuecoAnterior.PosX-1))
				{


					huecomido = HuecosTablero
						.Where(
						x =>
						((x.PosX < HuecoAnterior.PosX) && (x.PosX > HuecoSeleccinado.PosX))
						&&
						(x.PosY == HuecoAnterior.PosY + signoDifY))
						.First();
					huecomido.Pieza = "lightynone";

					Tablero.PiezasBlancas--;
					haComido = true;

				}
			}
			//Si es reina se comprueba la diagonal completa
			else
			{
				
				var difAbs = Math.Abs(HuecoSeleccinado.PosY - HuecoAnterior.PosY);
				//Si la diferencia absoluta dentre la posicion de los huecos es 1,
				//significa que no habrá comido ninguna pieza.
				if (difAbs == 2)
				{
					
					if (signoDifY > 0 && (HuecoSeleccinado.PosX < HuecoAnterior.PosX))
					{

						huecomido = HuecosTablero
							.Where(
							x =>
							((x.PosX < HuecoAnterior.PosX) && (x.PosX > HuecoSeleccinado.PosX))
							&&
							(x.PosY == HuecoAnterior.PosY+signoDifY))
							.First();
					}

					else if (signoDifY > 0 && (HuecoSeleccinado.PosX > HuecoAnterior.PosX))
					{

						huecomido = HuecosTablero
							.Where(
							x =>
							((x.PosX > HuecoAnterior.PosX) && (x.PosX < HuecoSeleccinado.PosX))
							&&
							(x.PosY == HuecoAnterior.PosY+signoDifY))
							.First();

					}
					else if (signoDifY < 0 && (HuecoSeleccinado.PosX < HuecoAnterior.PosX))
					{

						huecomido = HuecosTablero
							.Where(
							x =>
							((x.PosX < HuecoAnterior.PosX) && (x.PosX > HuecoSeleccinado.PosX))
							&&
							(x.PosY == HuecoAnterior.PosY+signoDifY))
							.First();
					}
					else if (signoDifY < 0 && (HuecoSeleccinado.PosX > HuecoAnterior.PosX))
					{
						huecomido = HuecosTablero
						   .Where(
						   x =>
						   ((x.PosX > HuecoAnterior.PosX) && (x.PosX < HuecoSeleccinado.PosX))
						   &&
						   (x.PosY == HuecoAnterior.PosY+signoDifY))
						   .First();
					}
					
				}
				else if(difAbs > 2)
				{
					try
					{
						huecomido = RealizarComida();
					}
					catch { huecomido = null; }
					
				}
				if (huecomido == null)
				{
					return haComido;
				}
				else if(huecomido.TipoPieza.Equals(ColorPieza.None))
				{

					return haComido;
				}

				huecomido.Pieza = "lightynone";
				if (Estado.Equals(EstadosJuego.TurnoBlancas))
					Tablero.PiezasNegras--;
				if (Estado.Equals(EstadosJuego.TurnoNegras))
					Tablero.PiezasBlancas--;

				haComido = true;


			}
			return haComido;
		}
	}



	#region Comandos

	#endregion

	public enum EstadosJuego
	{
		TurnoBlancas,
		TurnoNegras,
		NegroGana,
		BlancoGana
	}
}
