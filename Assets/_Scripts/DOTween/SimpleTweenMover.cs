using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class SimpleTweenMover : MonoBehaviour
{
    [Header("Path Points (orden importa)")]
    public List<Transform> pathPoints = new List<Transform>();

    [Header("Movement Settings")]
    public float speed = 2f; // 🔥 unidades por segundo
    public Ease ease = Ease.Linear;
    public int loops = -1;
    public LoopType loopType = LoopType.Restart;
    public bool destroyOnComplete = false;

    private Tween tween;
    private Transform currentRider;

    private void Start()
    {
        PlayPath();
    }

    private void PlayPath()
    {
        if (pathPoints.Count < 2)
            return;

        Vector3[] path = new Vector3[pathPoints.Count];

        for (int i = 0; i < pathPoints.Count; i++)
        {
            if (pathPoints[i] != null)
                path[i] = pathPoints[i].position;
        }

        float totalDistance = CalculateTotalDistance(path);
        float duration = totalDistance / speed;

        tween = transform.DOPath(path, duration, PathType.Linear)
                         .SetEase(ease)
                         .SetLoops(loops, loopType)
                         .SetUpdate(UpdateType.Fixed);

        if (destroyOnComplete && loops == 0)
            tween.OnComplete(() => Destroy(gameObject));
    }

    private float CalculateTotalDistance(Vector3[] path)
    {
        float distance = 0f;

        for (int i = 0; i < path.Length - 1; i++)
        {
            distance += Vector3.Distance(path[i], path[i + 1]);
        }

        return distance;
    }

    // 🚀 Transporte del jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            currentRider = collision.transform;
            currentRider.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (collision.transform.CompareTag("Player"))
        {
            RemoveRider();
        }
    }

    private void OnDisable()
    {
        RemoveRider();

        if (tween != null && tween.IsActive())
            tween.Kill();
    }

    private void RemoveRider()
    {
        if (currentRider != null)
        {
            currentRider.SetParent(null);
            currentRider = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count < 2)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            if (pathPoints[i] != null && pathPoints[i + 1] != null)
            {
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
            }
        }
    }
#endif
}