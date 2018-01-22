using System;

namespace c4
{

public class Piece
{
    public string Color {get;set;}
    public Piece(string col)
    {
        Color = col;
    }


}

public class Player
{
    public int playerNumber;
       

}

public class Board{
    public Piece[,] playArea = new Piece[6,7];

    




}
class MainClass
{
    public static Main()
    {
        Piece testPiece = new Piece("red");    
        Console.WriteLine("I'm alive " + testPiece.Color);
    }
}
}
