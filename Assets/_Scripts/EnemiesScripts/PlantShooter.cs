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

        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            FireProjectiles();
            timer = 0f;
        }
    }

    private void FireProjectiles()
    {
        Vector2[] directions =
        {
            Vector2.up,
            new Vector2(0.5f, 1f).normalized,
            new Vector2(-0.5f, 1f).normalized,
            new Vector2(0f, 1f)
        };

        foreach (Vector2 dir in directions)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyProjectile>().SetDirection(dir);
        }
    }
}
