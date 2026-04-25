using System;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    Action m_onCollected;
    public void Initialize(Action onCollected)
    {
        m_onCollected = onCollected;
    }
    
    protected abstract void OnCollected(Collider2D other);
    void OnTriggerEnter2D(Collider2D other)
    {
        OnCollected(other);
        m_onCollected?.Invoke();
    }
}
