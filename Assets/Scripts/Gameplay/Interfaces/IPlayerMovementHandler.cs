using UnityEngine;

public interface IPlayerMovementHandler
{
    /// <summary>
    /// Define the player's position and speed
    /// </summary>
    /// <param name="position">Global position of player in scene</param>
    /// <param name="speed">Speed of the player - as reference for animation, range is 0..1</param>
    public void SetPosition(Vector3 position, Vector2 speed);
}
