using MatrixUtils.GenericDatatypes;

public interface IScoreManager
{
    Observer<uint> Score { get; }
    void AddScore(uint scoreToAdd);
    void RemoveScore(uint scoreToRemove);
    void ClearCurrentScore();
}