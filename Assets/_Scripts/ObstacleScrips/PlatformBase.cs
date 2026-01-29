using UnityEngine;

//  CLASE BASE (HERENCIA)
public class PlatformBase : MonoBehaviour
{
    protected Collider2D col;
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
        sr  = GetComponent<SpriteRenderer>();
    }

    public virtual void SetActive(bool state)
    {
        col.enabled = state;
        sr.enabled  = state;
    }
}
