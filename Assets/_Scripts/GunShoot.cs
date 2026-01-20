using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunShoot : MonoBehaviour
{
    [Header("References")]
    public Transform muzzle;
    public GameObject bulletPrefab;
    public Rigidbody2D playerRb;

    [Header("Shooting")]
    public float fireRate = 0.15f;
    private float nextFireTime;

    [Header("Recoil")]
    public float recoilForce = 6f;

    [Header("Ammo & Reload")]
    public int maxAmmo = 3;
    public float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloading = false;
    private float reloadTimer = 0f;   // <- IMPORTANTE para UI

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    // =======================
    // DISPARO
    // =======================
    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // No disparar mientras recarga
        if (isReloading) return;

        // Fire rate
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        // Si ya no hay balas → recarga automática
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // ----- DISPARO REAL -----
        Shoot();
    }

    private void Shoot()
    {
        currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
        Vector2 shootDir = muzzle.right;
        bullet.GetComponent<BulletMove>().SetDirection(shootDir);

        // Recoil
        Vector2 recoilDir = -shootDir;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.AddForce(recoilDir * recoilForce, ForceMode2D.Impulse);
    }

    // =======================
    // RECARGA MANUAL (R)
    // =======================
    public void OnReload(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // Solo recarga si falta al menos 1 bala
        if (currentAmmo < maxAmmo && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    // =======================
    // CORUTINA DE RECARGA
    // =======================
    private IEnumerator Reload()
    {
        isReloading = true;
        reloadTimer = 0f;

        while (reloadTimer < reloadTime)
        {
            reloadTimer += Time.deltaTime;
            yield return null;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    // =======================
    // GETTERS PARA UI
    // =======================
    public int GetAmmo()
    {
        return currentAmmo;
    }

    // PROGRESO REAL de la barra (0 → 1)
    public float GetReloadProgress()
    {
        if (!isReloading) return 1f;

        return Mathf.Clamp01(reloadTimer / reloadTime);
    }

    public bool IsReloading()
    {
        return isReloading;
    }
}
