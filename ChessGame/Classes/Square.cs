using System;
namespace ChessGame.Classes
{
	public class Square
	{
		private string[] Colors = { "□", "■" };
		public Color Color { get; set; }
		public string View => Piece == null ? Colors[(int)Color] : Piece.View;
		public Piece? Piece { get; set; }
		public Square(Color Color, Piece? Piece = null)
		{
			this.Color = Color;
			this.Piece = Piece;
		}
	}
}

