using UnityEngine;

public class Shotgun : MouseFollow, InterfaceWeapon
{
    [SerializeField] private WeaponDetails weaponDetails;
    [SerializeField] private GameObject shotgunPellets;
    [SerializeField] private Transform shotgunPelletSpawnPoint;
    [SerializeField] private float knockbackForce = 20.0f;

    private SpriteRenderer weaponSprite;
    private Animator animator;

    readonly int attackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        // should grab shotgun sprite automatically
        weaponSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();


        if (weaponSprite == null)
        {
            Debug.LogWarning("Couldn't find Shotgun Sprite.");
        }
    }

    public WeaponDetails GetWeaponDetails()
    {
        return weaponDetails;
    }

    public void Attack()
    {
        animator.SetTrigger(attackHash);
    }

    public void SpawnPelletsAnimationEvent()
    {
        GameObject pellets = Instantiate(shotgunPellets, shotgunPelletSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        pellets.GetComponent<Projectile>().UpdateProjectileRange(weaponDetails.weaponRange);

        ApplyKnockback();
    }

    protected override void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        if (mousePosition.x < playerScreenPoint.x)
        {
            weaponSprite.flipY = true;
        }
        else
        {
            weaponSprite.flipY = false;
        }
    }

    private void ApplyKnockback()
    {
        Vector2 knockbackDirection = -transform.right;
        PlayerController.Instance.ApplyKnockback(knockbackDirection, knockbackForce);
    }

}
