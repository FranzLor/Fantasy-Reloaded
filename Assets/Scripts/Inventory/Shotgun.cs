using UnityEngine;

public class Shotgun : MouseFollow, InterfaceWeapon
{
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
    public void Attack()
    {
        Debug.Log("POW");
        ActiveWeapon.Instance.ToggleIsAttacking(false);

    }

    protected override void FaceMouse()
    {
        base.FaceMouse();

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
