using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab, healthPrefab, staminaPrefab;

    public void DropItems()
    {
        // leave 1 in 6 chance to drop nothing
        int randomNum = Random.Range(1, 6);

        if (randomNum == 1)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        if (randomNum == 2)
        {
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
        }

        if (randomNum == 3)
        {
            Instantiate(staminaPrefab, transform.position, Quaternion.identity);
        }

        if (randomNum == 4)
        {
            int randomAmountOfCoins = Random.Range(1, 4);
            for (int i = 0; i < randomAmountOfCoins; i++)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
