using UnityEngine;

public class CameraController : MonoBehaviour, IPlayerMovementHandler
{
    public Rect cameraBounds
    {
        get
        {
            float screenRatio = Screen.width / (float)Screen.height;
            Rect result = new Rect
            {
                position = transform.position,
                width = camComponent.orthographicSize * screenRatio * 2f,
                height = camComponent.orthographicSize * 2f
            };
            return result;
        }
    }
    private Camera camComponent;
    
    private void Awake()
    {
        camComponent = GetComponent<Camera>();
    }

    public void SetPosition(Vector3 position, Vector2 speed)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
