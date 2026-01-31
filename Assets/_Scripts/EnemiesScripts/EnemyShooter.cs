using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 3f;

    private float timer;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isDead) return;

        timer += Time.deltaTime;
        if (timer >= shootCooldown)
        {
            Shoot();
            timer = 0f;
        }
    }

    private void Shoot()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<EnemyProjectile>().SetDirection(direction);
    }
}