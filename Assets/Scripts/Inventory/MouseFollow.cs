using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    protected virtual void Update()
    {
        FaceMouse();
    }
    protected virtual void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = transform.position - mousePosition;
        
        transform.right = -direction;
    }
}
