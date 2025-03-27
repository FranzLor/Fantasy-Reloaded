using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;

    public void DropItems()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);


    }
}
