using System.Collections;
using UnityEngine;

public class BomberProjectile : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 1.0f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject projectileShadow;
    [SerializeField] private float shadowDistance = -0.43f;
    [SerializeField] private float explosionHeight = 1.0f;

    private MageProjectile mageProjectile;
    private Animator bombAnimator;
    private GameObject bombShadow;

    private void Start()
    {
        mageProjectile = GetComponent<MageProjectile>();
        bombAnimator = GetComponent<Animator>();

        mageProjectile.DisableDestructionOnLand();
        mageProjectile.OnArcComplete += HandleArcComplete;
    }

    private void HandleArcComplete()
    {
        if (bombAnimator != null)
        {
            bombAnimator.SetBool("StopSpinning", true);
        }

        if (projectileShadow != null)
        {
            bombShadow = Instantiate(projectileShadow, transform.position + new Vector3(0, shadowDistance, 0), Quaternion.identity);
        }

        StartCoroutine(ExplosionDelayRoutine());
    }

    private IEnumerator ExplosionDelayRoutine()
    {
        yield return new WaitForSeconds(explosionDelay);

        Instantiate(explosionPrefab, transform.position + new Vector3(0, explosionHeight, 0), Quaternion.identity);

        if (bombShadow != null)
        {
            Destroy(bombShadow);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (mageProjectile != null)
        {
            mageProjectile.OnArcComplete -= HandleArcComplete;
        }
    }

}
