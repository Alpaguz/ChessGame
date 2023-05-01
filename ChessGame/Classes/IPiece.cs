using System;
namespace ChessGame.Classes
{
	public interface IPiece
	{
        public Color Color();
		public List<Move> Moves();
	}
}

