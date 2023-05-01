using System;
using System.Linq;

namespace ChessGame.Classes.Pieces
{
    public class Pawn : Piece
    {
        private bool FirstMove = true;
        private Move[] Abilities = new Move[] {
            new Move(new Position(0, -1), MoveType.Normal),
            new Move(new Position(1, -1), MoveType.BeatMove),
            new Move(new Position(-1, -1), MoveType.BeatMove)
            };
        public Pawn(Color Color)
        {
            Views = new string[] { "♙", "♟" };
            _Color = Color;
        }

        public override void Move(Move Target)
        {
            if (FirstMove) FirstMove = false;
            _Move(Target);
        }

        public override List<Move> Moves()
        {
            var Pos = this.GetPosition();
            var Output = Abilities!.Select(x =>
            {
                var X = x.Position.X + Pos.X;
                var Y = x.Position.Y + Pos.Y;
                return new Move(new Position(X, Y), x.IsBeatMove);
            }).Where(x => !Helper.IsOutOfBoard(x.Position)).Where(x =>
            {
                var Sq = Helper.Rows[x.Position.Y][x.Position.X];
                return (x.IsBeatMove == MoveType.Normal && Sq.Piece == null) || ((x.IsBeatMove == MoveType.BeatMove || x.IsBeatMove == MoveType.Both) && Sq.Piece != null && Sq.Piece.Color() != Color());
            }).ToList();
            var Double = new Move(new Position(Pos.X + 0, Pos.Y + -2), MoveType.Normal);
            if (FirstMove && !Helper.IsOutOfBoard(Double.Position) && Output.Any(x=>x.IsBeatMove == MoveType.Normal) && Double.Square().Piece == null)
            {
                Output.Add(Double);
            }
            return Output;
        }
    }
}

