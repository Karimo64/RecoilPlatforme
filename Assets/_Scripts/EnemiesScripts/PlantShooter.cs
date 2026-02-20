using UnityEngine;

public class PlantShooter : EnemyBase
{
    [Header("Plant Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float cooldown = 8f;

    private float timer;

    private void Update()
    {
        if (!CanAct) return;

        // Solo dispara si está visible
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

        Vector2[] directions =
        {
            Vector2.up,
            new Vector2(0.5f, 1f).normalized,
            new Vector2(-0.5f, 1f).normalized
        };

        foreach (Vector2 dir in directions)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            EnemyProjectile projectile = bullet.GetComponent<EnemyProjectile>();
            if (projectile != null)
                projectile.SetDirection(dir);
        }
    }

    protected override void ResetState()
    {
        base.ResetState();
        timer = 0f;
    }
}