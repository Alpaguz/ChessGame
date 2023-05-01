using System;
using System.Linq;
using ChessGame.Classes;
using ChessGame.Classes.Pieces;

namespace ChessGame
{
	public static class Helper
	{
        //Deneme
        #region General
        private static int WhiteScore = 0;
        private static int BlackScore = 0;
        private static Color Turn = Color.White;
        private static Position Marker = new Position(0, 0);
        #endregion

        public static string[] Letters = { "ⓐ", "ⓑ", "ⓒ", "ⓓ", "ⓔ", "ⓕ", "ⓖ", "ⓗ" };
        public static string[] Numbers = { "⑧", "⑦", "⑥", "⑤", "④", "③", "②", "①" };

        public static int Page = 0;
        public static int PerPage = 10;
        public static bool IsPaginationActive = false;
        public static ConsoleColor BorderColor = ConsoleColor.Cyan;

        public static List<List<Square>> Rows = new List<List<Square>>();

        public static void NewGame()
		{
			WhiteScore = 0;
            BlackScore = 0;
            Turn = Color.White;
            ResetMarker();

            Rows = new List<List<Square>>();
            for (int Y = 0; Y < 8; Y++)
            {
                var Row = new List<Square>();
                for (int X = 0; X < 8; X++)
                {
                    if (Y % 2 == 0 && X % 2 == 0 || Y % 2 != 0 && X % 2 != 0)
                    {
                        Row.Add(new Square(Color.White));
                    }
                    else
                    {
                        Row.Add(new Square(Color.Black));
                    }
                }
                Rows.Add(Row);
            }

            for (int Y = 0; Y < 8; Y++)
            {
                for (int X = 0; X < 8; X++)
                {
                    if (Y != 0 && Y != 7 && Y != 1 && Y != 6)
                    {
                        continue;
                    }
                    Color Color = Y == 6 || Y == 7 ? Color.White : Color.Black;
                    Piece? Piece = null;
                    if (Y == 1 || Y == 6)
                    {
                        Piece = new Pawn(Color);
                    }
                    else
                    {
                        if (X == 0 || X == 7)
                        {
                            Piece = new Rook(Color);
                        }else if (X == 1 || X == 6)
                        {
                            Piece = new Knight(Color);
                        }else if (X == 2 || X == 5)
                        {
                            Piece = new Bishop(Color);
                        }
                        else if (X == 3)
                        {
                            Piece = new Queen(Color);
                        }
                        else
                        {
                            Piece = new King(Color);
                        }
                    }

                    Rows[Y][X].Piece = Piece;
                }
            }
            Rows[4][0].Piece = new Queen(Color.White);
        }

        public static void ResetMarker()
        {
            Marker.X = 4;
            Marker.Y = 4;
        }

        public static void ClearScreen()
        {
            Console.Clear();
            Console.Write("\f\u001bc\x1b[3J");
        }

        public static void ResetPagination()
        {
            IsPaginationActive = false;
            Page = 0;
        }

        public static void DrawGame()
        {
            var _Moves = GetAffectedMoves();
            var Moves = GetAffectedMoves();
            if (Moves.Count > PerPage)
            {
                IsPaginationActive = true;
                Moves = Moves.Skip(Page * PerPage).Take((Page + 1) * PerPage).ToList();
            }
            Write(delegate
            {
                Console.WriteLine("  " + String.Join("", Letters));
            }, BorderColor);
            for (int Y = 0; Y < Rows.Count; Y++)
            {
                Write(delegate
                {
                    Console.Write(Numbers[Y] + " ");
                }, BorderColor);
                var Row = Rows[Y];
                for (int X = 0; X < Row.Count; X++)
                {
                    var Square = Row[X];
                    if (_Moves.Any(x=>x.Position.X == X && x.Position.Y == Y)) {
                        var Index = _Moves.FindIndex(x => x.Position.X == X && x.Position.Y == Y);
                        var Index2 = Moves.FindIndex(x => x.Position.X == X && x.Position.Y == Y);
                        Write(() =>
                        {
                            Console.Write(_Moves[Index].IsKillMove() ? Square.View : Index2 == -1 ? Square.View : Index2);
                        }, _Moves[Index].IsKillMove() ? ConsoleColor.Red : ConsoleColor.Green);
                        continue;
                    }
                    else if (Marker.X == X && Marker.Y == Y)
                    {
                        Write(() =>{
                            Console.Write(Square.View);
                        }, ConsoleColor.Blue);
                        continue;
                    }
                    Console.Write(Square.View);
                }
                Write(delegate
                {
                    Console.Write(" " + Numbers[Y]);
                }, BorderColor);
                Console.Write(Environment.NewLine);
            }
            Write(delegate
            {
                Console.WriteLine("  " + String.Join("", Letters));
            }, BorderColor);
        }

        static List<Move> GetAffectedMoves()
        {
            var Square = Rows[Marker.Y][Marker.X];
            if (Square.Piece != null && Square.Piece.Color() == Turn)
            {
                return Square.Piece.Moves();
            }

            return new List<Move>();
        }

        public static void Pagination()
        {
            if (!IsPaginationActive) return;
            Console.WriteLine($"{Page + 1}/{Math.Ceiling((double)GetAffectedMoves().Count() / PerPage)}");
        }

        public static void ReadAction()
        {
            var Key = Console.ReadKey();
            var ConsoleKey = Key.Key;
            switch (ConsoleKey)
            {
                case ConsoleKey.UpArrow:
                    Marker.Y--;
                    ResetPagination();
                    break;
                case ConsoleKey.DownArrow:
                    Marker.Y++;
                    ResetPagination();
                    break;
                case ConsoleKey.RightArrow:
                    Marker.X++;
                    ResetPagination();
                    break;
                case ConsoleKey.LeftArrow:
                    Marker.X--;
                    ResetPagination();
                    break;
                case ConsoleKey.N:
                    Page++;
                    break;
                case ConsoleKey.P:
                    Page--;
                    break;
                default:
                    MoveOrAttack(Key);
                    ResetPagination();
                    break;
            }
            Marker.X = Marker.X == 8 ? 0 : Marker.X == -1 ? 7 : Marker.X;
            Marker.Y = Marker.Y == 8 ? 0 : Marker.Y == -1 ? 7 : Marker.Y;
            var TotalPages = Math.Ceiling((double)GetAffectedMoves().Count() / PerPage);
            Page = Page == -1 ? (int)TotalPages - 1 : Page == TotalPages ? 0 : Page;
        }

        public static void MoveOrAttack(ConsoleKeyInfo FirstKey)
        {
            if (!char.IsDigit(FirstKey.KeyChar)) return;
            string? Result = FirstKey.KeyChar.ToString();
            while (true)
            {
                var Key = Console.ReadKey();
                if (!char.IsDigit(Key.KeyChar) && Key.Key != ConsoleKey.Enter) return;
                else if (Key.Key == ConsoleKey.Enter) break;
                Result += Key.KeyChar.ToString();
            }
            var Moves = GetAffectedMoves();
            if (Moves.Count > PerPage)
            {
                Moves = Moves.Skip(Page * PerPage).Take((Page + 1) * PerPage).ToList();
            }
            int I = Convert.ToInt32(Result);
            if (I < 0 || I >= Moves.Count)
            {
                return;
            }
            var Target = Moves[I];
            Target.Square().Piece!.Move(Target);
            ToggleTurn();
            ResetMarker();
        }

        public static void ListKillMoves()
        {
            var _Moves = GetAffectedMoves();
            var Moves = GetAffectedMoves();
            if (Moves.Count > PerPage)
            {
                Moves = Moves.Skip(Page * PerPage).Take((Page + 1) * PerPage).ToList();
            }
            Console.WriteLine(string.Join(",", Moves.Select((x, i) => new { x, i }).Where(x => x.x.IsKillMove()).ToList().Select(x => x.i + "=" + Rows[x.x.Position.Y][x.x.Position.X].View)));
        }

        public static bool IsOutOfBoard(Position Pos)
        {
            return Pos.X > 7 || Pos.X < 0 || Pos.Y > 7 || Pos.Y < 0;
        }

        public static bool IsOutOfBoard(Position Pos, ref Square Reference)
        {
            var Result = Pos.X > 7 || Pos.X < 0 || Pos.Y > 7 || Pos.Y < 0;
            if (!Result)
            {
                Reference = Rows[Pos.Y][Pos.X];
            }
            return Result;
        }

        public static void Write(Action Action, ConsoleColor Color)
        {
            ConsoleColor DefaultColor = Console.ForegroundColor;

            Console.ForegroundColor = Color;

            Action.Invoke();

            Console.ForegroundColor = DefaultColor;
        }

        public static void ToggleTurn()
        {
            Turn = Turn == Color.White ? Color.Black : Color.White;
            //Rows.Reverse(); //tahtayı döndürmesek daha iyi olur
            //Rows.ForEach(x =>
            //{
            //    x.Reverse();
            //});
        }

        #region Extension Methods
        public static Position GetPosition(this Piece MyPiece)
        {
            int X, Y;
            for (Y = 0; Y < 8; Y++)
            {
                for (X = 0; X < 8; X++)
                {
                    if (Rows[Y][X].Piece == MyPiece)
                    {
                        return new Position(X, Y);
                    }
                }
            }
            throw new Exception("Taş Bulunamadı!");
        }

        public static Square Square(this Move Move)
        {

            return Rows[Move.Position.Y][Move.Position.X];
        }

        public static bool IsKillMove(this Move Move)
        {
            return Move.IsBeatMove != MoveType.Normal && Rows[Move.Position.Y][Move.Position.X].Piece != null;
        }
        #endregion

    }
}

