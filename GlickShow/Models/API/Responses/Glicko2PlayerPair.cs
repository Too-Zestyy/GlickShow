public class Glicko2PlayerPair
{
    public Glicko2Player PlayerOne { get; set; }
    public Glicko2Player PlayerTwo { get; set; }

    public Glicko2PlayerPair(Glicko2Player playerOne, Glicko2Player playerTwo)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
    }
}