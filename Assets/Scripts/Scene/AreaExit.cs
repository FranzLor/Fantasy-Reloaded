using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour 
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private float playerFadeDuration = 0.4f;

    private float waitToLoad = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);

            StartCoroutine(FadePlayer(other.gameObject));

            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        while (waitToLoad >= 0)
        {
            waitToLoad -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);

    }

    private IEnumerator FadePlayer(GameObject player)
    {
        SpriteRenderer[] sprites = player.GetComponentsInChildren<SpriteRenderer>();

        float elapsedTime = 0.0f;

        while (elapsedTime < playerFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / playerFadeDuration);

            foreach (SpriteRenderer sprite in sprites)
            {
                if (sprite != null)
                {
                    Color color = sprite.color;
                    color.a = alpha;
                    sprite.color = color;
                }
            }

            yield return null;
        }

        // disable player
        player.SetActive(false);
    }
}
