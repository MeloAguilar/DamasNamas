using CommunityToolkit.Mvvm.ComponentModel;

namespace DamasNamas.Models
{
	partial class clsPieza : ObservableObject
	{
		private String imageSrc;
		public ColorPieza colorPieza { get; set; }
		public bool esReina { get; set; }

		public String ImageSrc
		{
			get => imageSrc;
			set { imageSrc = value; OnPropertyChanged(nameof(ImageSrc)); }
		}


		public clsPieza(ColorPieza colorP, bool tipo)
		{
			colorPieza = colorP;
			esReina = tipo;
			if (colorPieza.Equals(ColorPieza.Blanco) && !tipo)
			{
				ImageSrc = "piezablanca.png";
			}
			else if (colorPieza.Equals(ColorPieza.Negro) && !tipo)
			{
				ImageSrc = "piezanegra.png";
			}
			else if (colorPieza.Equals(ColorPieza.Blanco) && tipo)
			{
				ImageSrc = "reinablanca.png";
			}
			else if (colorPieza.Equals(ColorPieza.Negro) && tipo)
			{
				ImageSrc = "reinanegra.png";
			}
		}

		public clsPieza(ColorPieza colorP)
		{
			colorPieza = colorP;
			esReina = false;
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
