using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasNamas.Models
{
	public class clsPieza
	{

		public String ImageSrc { get; set; }
		public ColorPieza colorPieza { get; set; }
		public Tipo tipoPieza { get; set; }

		public clsPieza(ColorPieza colorP, Tipo tipo)
		{
			colorPieza = colorP;
			tipoPieza = tipo;
			if (colorPieza.Equals(ColorPieza.Blanco))
			{
				ImageSrc = "piezablanca.png";
			}
			else
			{
				ImageSrc = "piezanegra.png";
			}
		}

		public clsPieza(ColorPieza colorP)
		{
			colorPieza = colorP;
			tipoPieza = Tipo.Normal;
			if (colorPieza.Equals(ColorPieza.Blanco))
			{
				ImageSrc = "piezablanca.png";
			}
			else
			{
				ImageSrc = "piezanegra.png";
			}
		}

	}

	public enum ColorPieza
	{
		Blanco,
		Negro
	}
	public enum Tipo
	{
		Normal,
		Reina
	}
}
