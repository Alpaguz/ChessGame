using System;
using System.Linq;

namespace ChessGame.Classes.Pieces
{
    public class King : Piece
    {
        private Move[] Abilities = new Move[] {
            new Move(new Position(1, 0), MoveType.Both),
            new Move(new Position(-1, 0), MoveType.Both),
            new Move(new Position(0, 1), MoveType.Both),
            new Move(new Position(0, -1), MoveType.Both),
            new Move(new Position(1, 1), MoveType.Both),
            new Move(new Position(-1, -1), MoveType.Both),
            new Move(new Position(1, -1), MoveType.Both),
            new Move(new Position(-1, 1), MoveType.Both)
            };
        public King(Color Color)
        {
            Views = new string[] { "♔", "♚" };
            _Color = Color;
        }

        public override List<Move> Moves()
        {
            var Pos = this.GetPosition();
            return Abilities!.Select(x =>
            {
                var X = x.Position.X + Pos.X;
                var Y = x.Position.Y + Pos.Y;
                return new Move(new Position(X, Y), x.IsBeatMove);
            }).Where(x => !Helper.IsOutOfBoard(x.Position)).Where(x =>
            {
                var Sq = x.Square();
                return (x.IsBeatMove == MoveType.Normal && Sq.Piece == null)
                || (x.IsBeatMove == MoveType.BeatMove && Sq.Piece != null && Sq.Piece.Color() != Color())
                || (x.IsBeatMove == MoveType.Both && (Sq.Piece == null  || Sq.Piece != null && Sq.Piece.Color() != Color()));
            }).ToList();
        }
    }
}

