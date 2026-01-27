using UnityEngine;
using System.Collections;

//  Herencia: FallingPlatform ← PlatformBase
//  Interfaz: FallingPlatform IMPLEMENTA IFallable
public class FallingPlatform : PlatformBase, IFallable
{
    // =============================
    //  CONFIGURACIÓN
    // =============================

    [Header("Fall Settings")]
    public float delay = 0.5f;           //  Tiempo antes de caer
    public float shakeDuration = 0.3f;   //  Duración del shake
    public float shakeStrength = 0.05f;  //  Intensidad del shake

    [Header("Respawn Settings")]
    public float respawnTime = 6f;        //  Tiempo antes de reaparecer
    public float blinkDuration = 1f;      //  Feedback visual

    // =============================
    //  VARIABLES INTERNAS
    // =============================

    private Rigidbody2D rb;
    private Vector3 startPos;

    private bool hasBeenTriggered; //  ya fue pisada
    private bool isRespawning;     //  está reapareciendo

    // =============================
    // CICLO DE VIDA
    // =============================

    protected override void Awake()
    {
        //  Llamamos al Awake del padre
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        // Empieza sólida e inmóvil
        rb.bodyType = RigidbodyType2D.Static;
    }

    // =============================
    //  INTERACCIÓN CON EL JUGADOR
    // =============================

    //  Unity llama esto cuando algo colisiona
    void OnCollisionEnter2D(Collision2D collision)
    {
        //  Evitamos múltiples activaciones
        if (hasBeenTriggered || isRespawning)
            return;

        if (collision.collider.CompareTag("Player"))
        {
            hasBeenTriggered = true;

            //  Feedback antes de caer
            StartCoroutine(ShakeThenFall());
        }
    }

    // =============================
    //  SHAKE + CAÍDA
    // =============================

    IEnumerator ShakeThenFall()
    {
        Vector3 originalPos = transform.position;
        float timer = 0f;

        //  Shake visual
        while (timer < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeStrength;
            transform.position = originalPos + new Vector3(offsetX, 0f, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;

        //  Espera antes de caer
        yield return new WaitForSeconds(delay);

        Fall();
    }

    //  Cambia a dinámica para que caiga
    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        //  Evita rotaciones por empujes
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // =============================
    //  DEADZONE (INTERFAZ)
    // =============================

    //  Llamado por Deadzone (NO por Unity)
    public void OnFellIntoDeadzone()
    {
        if (!isRespawning)
            StartCoroutine(RespawnRoutine());
    }

    // =============================
    //  RESPAWN
    // =============================

    IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        //  Desaparece
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // Reset completo
        transform.position = startPos;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;

        hasBeenTriggered = false;

        //  Feedback visual
        yield return StartCoroutine(Blink());

        isRespawning = false;
    }

    // =============================
    //  BLINK DE SEGURIDAD
    // =============================

    IEnumerator Blink()
    {
        float timer = 0f;
        col.enabled = false;

        while (timer < blinkDuration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        sr.enabled = true;
        col.enabled = true;
    }
}
