using UnityEngine;

public class Bow : MonoBehaviour, InterfaceWeapon
{
    [SerializeField] private WeaponDetails weaponDetails;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    private Animator animator;
    // should improve performance with hash instead of string
    readonly int attackHash = Animator.StringToHash("Fire");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public WeaponDetails GetWeaponDetails()
    {
        return weaponDetails;
    }

    public void Attack()
    {
        animator.SetTrigger(attackHash);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponDetails(weaponDetails);
    }
}
