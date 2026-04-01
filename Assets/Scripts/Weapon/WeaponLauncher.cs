using UnityEngine;

public class WeaponLauncher : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField, Tooltip("Point d'apparition du projectile. Laisser vide pour utiliser l'objet courant.")]
    private Transform spawnPoint;

    [Header("Lancement")]
    [SerializeField, Tooltip("Vitesse initiale (m/s) appliqu�e au projectile.")]
    private float launchSpeed = 20f;
    [SerializeField, Tooltip("Dur�e avant destruction automatique du projectile (s).")]
    private float projectileLifetime = 5f;
    [SerializeField, Tooltip("D�calage depuis le spawn pour �viter de collisionner avec l'�metteur.")]
    private float spawnOffset = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Launch();
        }
    }

    private void Launch()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError($"Projectile Prefab is missing on GameObject '{gameObject.name}'! Please assign it in the Inspector.", this);
            return;
        }

        Transform origin = spawnPoint != null ? spawnPoint : transform;
        Vector3 spawnPosition = origin.position + origin.forward * spawnOffset;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, origin.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(origin.forward * launchSpeed, ForceMode.VelocityChange);
        }

        Destroy(projectile, projectileLifetime);
    }
}
