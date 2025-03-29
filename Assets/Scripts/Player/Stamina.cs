using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    [SerializeField] private Sprite fullStaminaSprite, emptyStaminaSprite;
    [SerializeField] private int timeBetweenStaminaRestore = 6;

    // make sure string is exact in the hierarchy
    private const string StaminaText = "Stamina Container";

    private int startingStamina = 4;
    private int maxStamina;
    private Transform staminaContainer;

    public int CurrentStamina { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(StaminaText).transform;
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImage();
    }

    public void RestoreStamina()
    {
        if (CurrentStamina < maxStamina)
        {
            CurrentStamina++;
        }
        UpdateStaminaImage();
    }

    private IEnumerator RestoreStaminaSlowlyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRestore);
            RestoreStamina();
        }
    }

    private void UpdateStaminaImage()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            if (i <= CurrentStamina - 1)
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaSprite;
            }
            else
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = emptyStaminaSprite;
            }
        }

        if (CurrentStamina < maxStamina)
        {
            // call this to stop overlap, only one instance will run
            StopAllCoroutines();

            StartCoroutine(RestoreStaminaSlowlyRoutine());
        }
    }
}
