using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    public Transform muzzle;
    public GameObject bulletPrefab;

    public float fireRate = 0.15f;
    private float nextFireTime;

    [Header("Recoil Settings")]
    public float recoilForce = 6f;
    public Rigidbody2D playerRb;

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        
        //Shoot 
        if(Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
        Vector2 shootDir = muzzle.right;
        bullet.GetComponent<BulletMove>().SetDirection(shootDir);

        //Recoil
        Vector2 recoilDir = -shootDir;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.AddForce(recoilDir * recoilForce, ForceMode2D.Impulse); 
    }
}
