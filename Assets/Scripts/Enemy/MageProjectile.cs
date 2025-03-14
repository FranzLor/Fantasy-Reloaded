using System.Collections;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 4.0f;
    [SerializeField] private GameObject projectileShadow;
    [SerializeField] private float projectileShadowHeight = -0.4f;
    [SerializeField] private GameObject splatterPrefab;

    private void Start()
    {
        GameObject projectileShadow = 
            Instantiate(this.projectileShadow, transform.position + new Vector3(0, projectileShadowHeight, 0), Quaternion.identity);

        Vector3 playerPosition = PlayerController.Instance.transform.position;
        Vector3 projectileShadowStartPosition = projectileShadow.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPosition));

        StartCoroutine(MoveProjectileShadowRoutine
            (projectileShadow, projectileShadowStartPosition, playerPosition - new Vector3(0, 0.5f, 0)));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0.0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animationCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0.0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0.0f, height);

            yield return null;
        }

        Instantiate(splatterPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private IEnumerator MoveProjectileShadowRoutine(GameObject projectileShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0.0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;

            projectileShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            yield return null;
        }

        Destroy(projectileShadow);
    }
}
