using UnityEngine;

public class GroundController : MonoBehaviour, IPlayerMovementHandler
{
    [SerializeField] private Transform[] tilesets;
    [SerializeField] private Vector2Int tilesetSize;

    private Vector3[] initialPositions;

    private void Start()
    {
        initialPositions = new Vector3[tilesets.Length];
        for (int i = 0; i < tilesets.Length; i++)
        {
            initialPositions[i] = tilesets[i].position;
        }
    }

    public void SetPosition(Vector3 position, Vector2 speed)
    {
        for (int i = 0; i < tilesets.Length; i++)
        {
            var tilesetPos = tilesets[i].position;
            tilesetPos.x = Mathf.RoundToInt(position.x / tilesetSize.x) * tilesetSize.x + initialPositions[i].x;
            tilesetPos.y = Mathf.RoundToInt(position.y / tilesetSize.y) * tilesetSize.y + initialPositions[i].y;
            tilesets[i].position = tilesetPos;
        }
    }
}
