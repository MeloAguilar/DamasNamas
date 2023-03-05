using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNamas.Models
{
	public class clsJugadorConPartidas : clsJugador
	{
		public List<clsSala> partidas { get; set; }



		public clsJugadorConPartidas(String nombre, String password, List<clsSala> partidasJugadas)
		{
			base.idJugador = idJugador;
			base.nombre=nombre;
			base.password=password;
			partidas=partidasJugadas;
		}
	}
}
