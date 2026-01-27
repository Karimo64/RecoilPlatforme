using UnityEngine;
using System.Collections;

// Plataforma que se activa y desactiva en ciclos de tiempo
// HEREDA de PlatformBase
public class TimedPlatform : PlatformBase
{
    [Header("Initial State")]
    [Tooltip("¿La plataforma empieza activa al iniciar el nivel?")]
    public bool startActive = true;

    [Header("Timing")]
    [Tooltip("Tiempo que permanece activa")]
    public float activeTime = 1.5f;

    [Tooltip("Tiempo que permanece desactivada")]
    public float inactiveTime = 1.5f;

    private void Start()
    {
        // Estado inicial definido por el diseñador
        SetActive(startActive);

        //  Comienza el ciclo
        StartCoroutine(TimedCycle());
    }

    //  Ciclo infinito de activación / desactivación
    IEnumerator TimedCycle()
    {
        while (true)
        {
            // ⏸ Espera según el estado actual
            yield return new WaitForSeconds(startActive ? activeTime : inactiveTime);

            // Cambia estado
            startActive = !startActive;
            SetActive(startActive);
        }
    }
}
