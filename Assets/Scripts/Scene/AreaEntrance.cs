using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEntrance : MonoBehaviour
{

    [SerializeField] private string transitionName;
    [SerializeField] private float playerUnfadeDuration = 0.4f;


    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();

            UIFade.Instance.FadeToClear();

            StartCoroutine(UnfadePlayer(PlayerController.Instance.gameObject));
        }
    }

    private IEnumerator UnfadePlayer(GameObject player)
    {
        player.SetActive(true);

        SpriteRenderer[] sprites = player.GetComponentsInChildren<SpriteRenderer>();

        float elapsedTime = 0.0f;

        while (elapsedTime < playerUnfadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / playerUnfadeDuration);

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

        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a = 1f;
                sprite.color = color;
            }
        }
    }
}
