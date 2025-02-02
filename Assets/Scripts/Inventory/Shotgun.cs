using UnityEngine;

public class Shotgun : MouseFollow, InterfaceWeapon
{
    [SerializeField] private WeaponDetails weaponDetails;

    private SpriteRenderer weaponSprite;

    private void Awake()
    {
        // should grab shotgun sprite automatically
        weaponSprite = GetComponentInChildren<SpriteRenderer>();

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
        Debug.Log("POW");
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

}
