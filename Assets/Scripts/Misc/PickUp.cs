using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private enum PickUpTypes
    {
        Coin,
        Health,
        Stamina
    }

    [SerializeField] private PickUpTypes pickUpType;
    [SerializeField] private float pickUpDistance = 4.0f;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float accelerationRate = 0.4f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1.0f;

    private Vector3 moveDirection;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AnimationCurveSpawnRoutine());
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            moveDirection = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        }
        else
        {
            moveDirection = Vector3.zero;
            moveSpeed = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        rigidBody.linearVelocity = moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            DetectPickupType();

            Destroy(gameObject);
        }
    }

    private IEnumerator AnimationCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;

        float randomX = transform.position.x + Random.Range(-2.0f, 2.0f);
        float randomY = transform.position.y + Random.Range(-1.0f, 1.0f);
        Vector2 endPoint = new Vector2(randomX, randomY);

        float timePassed = 0.0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animationCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0.0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0.0f, height);
            yield return null;
        }
    }

    private void DetectPickupType()
    {
        switch (pickUpType)
        {
            case PickUpTypes.Coin:
                Debug.Log("Coin+");

                break;

            case PickUpTypes.Health:
                Debug.Log("Health+");
                PlayerHealth.Instance.HealPlayer();

                break;

            case PickUpTypes.Stamina:
                Debug.Log("Stamina+");

                break;
        }
    }
}
