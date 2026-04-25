using MatrixUtils.GenericDatatypes;
using UnityEngine.Events;
public interface IScoreManager
{
    Observer<uint> Score { get; }
    UnityEvent<uint> OnBonusEarned { get; }
    void AddScore(uint scoreToAdd);
    void AddAirtimeBonus(uint scoreToAdd);
    void RemoveScore(uint scoreToRemove);
    void ClearCurrentScore();
}