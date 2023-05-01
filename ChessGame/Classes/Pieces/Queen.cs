using System;
using System.Linq;

namespace ChessGame.Classes.Pieces
{
    public class Queen : Piece
    {
        private Move[] Abilities = new Move[] {
            new Move(new Position(0, -1), MoveType.Normal),
            new Move(new Position(1, -1), MoveType.BeatMove),
            new Move(new Position(-1, -1), MoveType.BeatMove)
            };
        public Queen(Color Color)
        {
            Views = new string[] { "♕", "♛" };
            _Color = Color;
        }

        public override List<Move> Moves()
        {
            List<Move> Moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                List<Move> Temporary = new List<Move>();
                var Pos = this.GetPosition();
                while (true)
                {
                    switch (i)
                    {
                        case 0:
                            Pos.X++;
                            Pos.Y++;
                            break;
                        case 1:
                            Pos.X--;
                            Pos.Y--;
                            break;
                        case 2:
                            Pos.X--;
                            Pos.Y++;
                            break;
                        case 3:
                            Pos.X++;
                            Pos.Y--;
                            break;
                        case 4:
                            Pos.X++;
                            break;
                        case 5:
                            Pos.X--;
                            break;
                        case 6:
                            Pos.Y++;
                            break;
                        case 7:
                            Pos.Y--;
                            break;
                    }
                    Square? Target = null;
                    if (Helper.IsOutOfBoard(Pos, ref Target!) || (Target.Piece != null && Target.Piece.Color() == this.Color() || (Temporary.Count() > 0 && Temporary.Last().Square().Piece != null)))
                    {
                        break;
                    }
                    Moves.Add(new Move(new Position(Pos.X, Pos.Y), MoveType.Both));
                    Temporary.Add(new Move(new Position(Pos.X, Pos.Y), MoveType.Both));
                }
            }
            return Moves;
        }
    }
}

