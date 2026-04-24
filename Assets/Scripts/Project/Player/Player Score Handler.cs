using MatrixUtils.DependencyInjection;
using UnityEngine;

public class PlayerScoreHandler : MonoBehaviour
{
    [Inject] IScoreManager m_scoreManager;
}
