using CommunityToolkit.Mvvm.ComponentModel;
using DamasNamas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNamas.Models
{
	partial class ObjetoSignalR : ObservableObject
	{

		#region Atributes
		String idJugador;
		Square huecoInicial;
		Square huecoFinal;
		EstadosJuego estadoJuego;

		#endregion
		#region Properties
		public String IdJugador
		{
			get
			{
				return idJugador;
			}

			set
			{
				idJugador = value;
			}
		}

		public Square HuecoInicial
		{
			get
			{
				return huecoInicial;
			}

			set
			{
				huecoInicial = value;
			}
		}


		public Square HuecoFinal
		{
			get
			{
				return huecoFinal;
			}

			set
			{
				huecoFinal = value;
			}
		}

		public EstadosJuego EstadoJuego
		{
			get
			{
				return estadoJuego;
			}
			set
			{ 
				estadoJuego = value; 
			}
		}
		#endregion

		#region Constructors
		public ObjetoSignalR()
		{

		}

		public ObjetoSignalR(string idJugador, Square huecoInicial, Square huecoFinal, EstadosJuego estadoJuego)
		{
			IdJugador=idJugador;
			HuecoInicial=huecoInicial;
			HuecoFinal=huecoFinal;
			EstadoJuego=estadoJuego;
		}


		#endregion

	}
}
