using UnityEngine;

public class BoostPowerUp : Collectible
{
    [SerializeField] float m_boostPower;
    protected override void OnCollected(Collider2D other)
    {
        if (!other.TryGetComponent(out Rigidbody2D rb)) return;
        Vector2 velocityDirection = rb.linearVelocity.normalized;
        rb.AddForce(velocityDirection * m_boostPower, ForceMode2D.Impulse);
    }
}
