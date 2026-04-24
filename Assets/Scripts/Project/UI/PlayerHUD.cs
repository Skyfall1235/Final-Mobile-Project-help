using JetBrains.Annotations;
using MatrixUtils.Attributes;
using MatrixUtils.DependencyInjection;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour, IDependencyProvider
{
    [Provide, UsedImplicitly] IScoreManager GetScoreManager() => m_scoreManager;
    [SerializeField, RequiredField] TMP_Text m_scoreText;
    [SerializeField] string m_scorePrefix;
    [ClassSelector, SerializeReference] IScoreManager m_scoreManager;
}