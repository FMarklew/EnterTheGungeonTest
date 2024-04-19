using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for projectiles - can be inherited and extended for any projectile logic required
/// </summary>
public abstract class ProjectileBase : MonoBehaviour
{
    public Rigidbody2D rb;

    public abstract void Init();
    public abstract void Fire(Vector2 position, Vector2 direction, float velocity);
    public abstract void Destruct();
}
