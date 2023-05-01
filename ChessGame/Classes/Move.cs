using System;
namespace ChessGame.Classes
{
	public class Move
	{
		public Position Position { get; set; }
		public MoveType IsBeatMove { get; set; }
		public Move(Position Position, MoveType IsBeatMove)
		{
			this.Position = Position;
            this.IsBeatMove = IsBeatMove;
        }
	}
}

