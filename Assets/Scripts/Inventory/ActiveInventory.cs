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
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        // exit if no weapon in slot
        if (!transform.GetChild(activeSlotIndex).GetComponentInChildren<InventorySlot>())
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        // kind of ugly but it works for now
        GameObject weaponToSpawn = transform
            .GetChild(activeSlotIndex)
            .GetComponentInChildren<InventorySlot>()
            .GetWeaponDetails().weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

        // fixes rotation issue with mouse following weapon - script
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
