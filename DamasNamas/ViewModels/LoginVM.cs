using BL.Gestion;
using BL.Listados;
using CommunityToolkit.Mvvm.ComponentModel;
using DamasNamas.ViewModels.Utilidades;
using Entities;
using System.Collections.ObjectModel;

namespace DamasNamas.ViewModels
{
	partial class LoginVM : VMBase
	{
		#region Atributos

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(ComandoLogin))]
		[NotifyPropertyChangedFor(nameof(ComandoSignup))]
		String username;

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(ComandoLogin))]
		[NotifyPropertyChangedFor(nameof(ComandoSignup))]
		String password;

		clsJugador Jugador;

		private DelegateCommand _comandoLogin;
		private DelegateCommand _comandoSignup;

		#endregion

		#region Propiedades


		public DelegateCommand ComandoLogin
		{
			get
			{
				_comandoLogin = new DelegateCommand(ComandoLogin_Execute, ComandoLogin_CanExecute);
				return _comandoLogin;
			}
		}

		public DelegateCommand ComandoSignup
		{
			get
			{
				_comandoSignup = new DelegateCommand(ComandoSignup_Execute, ComandoSignup_CanExecute);
				return _comandoSignup;
			}
		}

		#endregion

		#region Constructores

		public LoginVM()
		{
			IsBusy = false;
			Username = "";
			Password = "";
		}

		#endregion

		#region Comandos

		/// <summary>
		/// Metodo que se acciona al pulsar el boton de Log in, cuando se pulse, navegaremos hacia la siguiente pagina
		/// </summary>
		private async void ComandoLogin_Execute()
		{

			if (await TestLogin())
			{

				GoTo();

			}
			else
			{
				await App.Current.MainPage.DisplayAlert("Usuario no registrado", "Lo siento, ese usuario no esta registrado", "Ok");

			}



		}




		private async void GoTo()
		{
			var dict = new Dictionary<string, object>();
			dict.Add("JugadorQueMando", Jugador);
			await Shell.Current.GoToAsync("///Main", true, dict);
		}

		/// <summary>
		/// Metodo que comprueba si el nombre y el usuario estan vacios para activar el boton de logeo o no
		/// </summary>
		/// <returns> bool </returns>
		private bool ComandoLogin_CanExecute()
		{
			bool botonLogin = true;

			if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
			{

				botonLogin = false;

			}

			return botonLogin;
		}

		/// <summary>
		/// Metodo que se acciona al pulsar el boton de Sign Up, cuando se pulse, navegaremos hacia la siguiente pagina
		/// </summary>
		private void ComandoSignup_Execute()
		{

			TestSignUp();

		}



		/// <summary>
		/// Metodo que comprueba si el nombre y el usuario estan vacios para activar el boton de logeo o no
		/// </summary>dddddd
		/// <returns> bool </returns>
		private bool ComandoSignup_CanExecute()
		{
			bool botonSignUp = true;

			if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
			{

				botonSignUp = false;

			}

			return botonSignUp;
		}

		#endregion

		#region Utilities

		private async Task<bool> TestLogin()
		{

			bool logeadoConExito = false;
			try{
			ObservableCollection<clsJugador> jugadores = await clsListadoJugadoresBL.getJugadoresBL();

			for (int i = 0; i < jugadores.Count && !logeadoConExito; i++)
			{
				if (jugadores[i].nombre.ToUpper().Equals(Username.ToUpper()) && jugadores[i].password.Equals(Password))
				{
					Jugador = jugadores[i];
					logeadoConExito = true;
					
				}
			}
			}
			catch (Exception e)
			{
				await Shell.Current.DisplayAlert("Ha petao", "Yo no se porqué, busca porahi", "po vale");
			}

			return logeadoConExito;
		}


		private async void TestSignUp()
		{
			bool existe = false;

			ObservableCollection<clsJugador> jugadores = await clsListadoJugadoresBL.getJugadoresBL();

			for (int i = 0; i < jugadores.Count; i++)
			{
				if (jugadores[i].nombre.ToUpper().Equals(Username.ToUpper()))
				{
					existe = true;
				}
			}

			if (existe)
			{
				await Shell.Current.DisplayAlert("Usuario ya existente", "Lo siento, ese usuario ya existe", "Ok");
			}
			else
			{
				clsJugador jugadorRegistrado = new clsJugador(Username, Password);
				clsGestionJugadoresBL.insertarJugadorBL(jugadorRegistrado);

				var dict = new Dictionary<string, object>();
				dict.Add("JugadorQueMando", jugadorRegistrado);


				await Shell.Current.GoToAsync("///Main", true, dict);
			}


		}
		#endregion
	}
}
