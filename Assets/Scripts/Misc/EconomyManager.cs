using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text coinText;

    // make sure string is exact in the hierarchy
    private const string CoinAmount = "CoinAmount";
    private int currentAmount = 0;

    public void UpdateCurrentCoin()
    {
        currentAmount += 1;

        if (coinText == null)
        {
            coinText = GameObject.Find(CoinAmount).GetComponent<TMP_Text>();
        }

        //D3 - format with 3 digits
        coinText.text = currentAmount.ToString("D3");
    }
}
