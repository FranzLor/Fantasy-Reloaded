using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxEffect = -0.2f;

    private Camera camera;
    private Vector2 startPosition;
    private Vector2 travel => (Vector2)camera.transform.position - startPosition;


    private void Awake()
    {
        camera = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.position = startPosition + travel * parallaxEffect;
    }
}
