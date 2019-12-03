using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{

    #region Singleton
    public static ProjectilePool instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ProjectilePool found!");
            return;
        }
        instance = this;
    }

    #endregion


    public Projectile pooledProjectile;
    private bool notEnoughProjectilesInPool = true;


    private List<Projectile> projectiles;

    // Start is called before the first frame update
    void Start()
    {
        projectiles = new List<Projectile>();
    }

    public Projectile GetProjectile()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (!projectiles[i].gameObject.activeInHierarchy)
            {
                return projectiles[i];
            }
        }

        if (notEnoughProjectilesInPool)
        {
            Projectile newProjectile = Instantiate(pooledProjectile);
            newProjectile.gameObject.SetActive(false);
            newProjectile.gameObject.transform.SetParent(gameObject.transform);
            projectiles.Add(newProjectile);
            return newProjectile;
        }
        return null;
    }
}
