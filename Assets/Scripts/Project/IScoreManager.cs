public interface IScoreManager
{
    uint Score { get; }
    void AddScore(uint scoreToAdd);
    void RemoveScore(uint scoreToRemove);
    void ClearCurrentScore();
}