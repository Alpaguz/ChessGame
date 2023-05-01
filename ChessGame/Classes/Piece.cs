using System;
namespace ChessGame.Classes
{
	public abstract class Piece
	{
        private protected string[]? Views { get; set; }
        public string View => Views![(int)_Color];
        private protected Color _Color { get; set; }

        public Color Color()
        {
            return _Color;
        }

        public abstract List<Move> Moves();

        public virtual void _Move(Move Target)
        {
            var Marker = this.GetPosition();
            Helper.Rows[Marker.Y][Marker.X].Piece = null;
            Helper.Rows[Target.Position.Y][Target.Position.X].Piece = this;
        }

        public virtual void Move(Move Target)
        {
            _Move(Target);
        }
    }
}

