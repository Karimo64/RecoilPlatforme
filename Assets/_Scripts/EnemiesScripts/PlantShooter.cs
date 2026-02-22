using UnityEngine;

public class PlantShooter : EnemyBase
{
    [Header("Plant Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float cooldown = 8f;
    [SerializeField] private float spreadAngle = 25f; // Ángulo del cono

    private float timer;

    private void Update()
    {
        if (!CanAct) return;

        // Solo dispara si está visible en cámara
        if (!IsInsideCamera())
            return;

        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            FireProjectiles();
            timer = 0f;
        }
    }

    private void FireProjectiles()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        // Dirección base = hacia donde apunta el firePoint
        Vector2 baseDirection = firePoint.up;

        // Creamos el cono relativo a esa dirección
        Vector2[] directions =
        {
            baseDirection,
            Quaternion.Euler(0, 0, spreadAngle) * baseDirection,
            Quaternion.Euler(0, 0, -spreadAngle) * baseDirection
        };

        foreach (Vector2 dir in directions)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            EnemyProjectile projectile = bullet.GetComponent<EnemyProjectile>();
            if (projectile != null)
                projectile.SetDirection(dir.normalized);
        }
    }

    protected override void ResetState()
    {
        base.ResetState();
        timer = 0f;
    }
}