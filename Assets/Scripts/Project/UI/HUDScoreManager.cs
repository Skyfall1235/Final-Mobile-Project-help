using System;
using MatrixUtils.GenericDatatypes;
using UnityEngine;
[Serializable]
public class HUDScoreManager : IScoreManager
{
    [field:SerializeField] public Observer<uint> Score { get; private set; }
    public void AddScore(uint scoreToAdd)
    {
        Score.Value += scoreToAdd;
    }

    public void RemoveScore(uint scoreToRemove)
    {
        Score.Value = scoreToRemove > Score ? 0 : Score - scoreToRemove;
    }

    public void ClearCurrentScore()
    {
        Score.Value = 0;
    }
}
