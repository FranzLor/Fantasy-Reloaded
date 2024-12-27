using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particleSystem && !particleSystem.IsAlive())
        {
            DestroySelf();
        }
    }

        public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
