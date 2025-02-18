using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndex = 0;


    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        // in player action input map, put scale processors based off which slot is selected

        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

        // instantiate sword as active weapon upon start
        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int number)
    {
        // sub 1 to match index, starting at 0
        ToggleActiveHighlight(number - 1);
    }

    private void ToggleActiveHighlight(int index)
    {
        activeSlotIndex = index;

        // turn off all highlights first
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(activeSlotIndex).GetChild(0).gameObject.SetActive(true);

        SwitchActiveWeapon();
    }

    private void SwitchActiveWeapon()
    {

        Transform childTransform = transform.GetChild(activeSlotIndex);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();

        if (inventorySlot == null || inventorySlot.GetWeaponDetails() == null)
        {

            if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
            {
                Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
            }

            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        WeaponDetails weaponDetails = inventorySlot.GetWeaponDetails();
        GameObject weaponToSpawn = weaponDetails.weaponPrefab;

        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

        // fixes rotation issue with mouse following weapon - script
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
