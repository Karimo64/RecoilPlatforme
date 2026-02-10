using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AmmoUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunShoot gun;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletsContainer;

    [Header("Colors")]
    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color emptyColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    List<Image> bullets = new List<Image>();
    int lastMaxAmmo = -1;

    void Update()
    {
        if (gun.maxAmmo != lastMaxAmmo)
            RebuildUI();

        UpdateAmmo();
    }

    void RebuildUI()
    {
        foreach (Transform child in bulletsContainer)
            Destroy(child.gameObject);

        bullets.Clear();

        for (int i = 0; i < gun.maxAmmo; i++)
        {
            GameObject b = Instantiate(bulletPrefab, bulletsContainer);
            bullets.Add(b.GetComponent<Image>());
        }

        lastMaxAmmo = gun.maxAmmo;
    }

    void UpdateAmmo()
    {
        int currentAmmo = gun.GetAmmo();

        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].color = i < currentAmmo ? activeColor : emptyColor;
        }
    }
}
