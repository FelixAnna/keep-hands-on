namespace PokerServer.Poker
{
    public interface IPokerService
    {
        void Initial();
        string GetWinner();
        bool IsFinished();
        bool Pick(string userName, int row, int count);
        string Print();
    }
}