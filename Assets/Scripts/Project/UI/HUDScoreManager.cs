using System;
using MatrixUtils.Attributes;
using UnityEngine;
[Serializable]
public class HUDScoreManager : IScoreManager
{
    [field:SerializeField, ReadOnly] public uint Score { get; private set; }
    public void AddScore(uint scoreToAdd)
    {
        Score += scoreToAdd;
    }

    public void RemoveScore(uint scoreToRemove)
    {
        Score = scoreToRemove > Score ? 0 : Score - scoreToRemove;
    }

    public void ClearCurrentScore()
    {
        Score = 0;
    }
}
