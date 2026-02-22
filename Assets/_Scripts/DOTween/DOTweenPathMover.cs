using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
public class DOTweenPathMover : MonoBehaviour
{
    [Header("Path")]
    public List<Transform> waypoints = new List<Transform>();

    [Header("Timing")]
    [Tooltip("If true duration is calculated from distance / speed.")]
    public bool distanceBasedDuration = true;
    public float speed = 2f;
    public float fixedDuration = 1f;

    [Header("Loop / Ease")]
    public int loops = -1; // -1 = infinite
    public LoopType loopType = LoopType.Yoyo;
    public Ease ease = Ease.Linear;

    [Header("Behaviour")]
    public bool startOnAwake = true;
    public bool destroyOnFinish = false;
    public bool useRigidbody2D = false;
    public Rigidbody2D rb2D;

    private Sequence sequence;

    private void Start()
    {
        if (startOnAwake)
            Play();
    }

    public void Play()
    {
        BuildSequence();
        sequence?.Play();
    }

    public void Pause() => sequence?.Pause();

    public void Kill()
    {
        sequence?.Kill();
        sequence = null;
    }

    private void BuildSequence()
    {
        if (sequence != null)
        {
            sequence.Kill();
            sequence = null;
        }

        if (waypoints == null || waypoints.Count == 0)
            return;

        if (useRigidbody2D && rb2D == null)
            rb2D = GetComponent<Rigidbody2D>();

        sequence = DOTween.Sequence();

        Vector3 startPos = transform.position;
        Transform previous = null;

        foreach (Transform wp in waypoints)
        {
            if (wp == null) continue;

            float duration = fixedDuration;

            if (distanceBasedDuration)
            {
                Vector2 from = (previous == null) ? startPos : previous.position;
                float distance = Vector2.Distance(from, wp.position);
                duration = Mathf.Max(0.01f, distance / Mathf.Max(0.01f, speed));
            }

            if (useRigidbody2D && rb2D != null)
                sequence.Append(rb2D.DOMove(wp.position, duration).SetEase(ease));
            else
                sequence.Append(transform.DOMove(wp.position, duration).SetEase(ease));

            previous = wp;
        }

        sequence.SetLoops(loops, loopType);

        if (destroyOnFinish && loops != -1)
            sequence.OnComplete(() => Destroy(gameObject));
    }

    private void OnDisable()
    {
        Kill();
    }
}