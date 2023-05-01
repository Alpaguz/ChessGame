using ChessGame.Classes;
using ChessGame.Classes.Pieces;

namespace ChessGame;
class Program
{
    static void Main(string[] args)
    {
        Helper.NewGame();
        while (true)
        {
            Helper.ClearScreen();
            Helper.DrawGame();
            Helper.ListKillMoves();
            Helper.Pagination();
            Helper.ReadAction();
        }
    }
}

