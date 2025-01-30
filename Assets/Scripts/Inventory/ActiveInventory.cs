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
    }
}
