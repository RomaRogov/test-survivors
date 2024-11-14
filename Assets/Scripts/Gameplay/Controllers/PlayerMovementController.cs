using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Instances;
using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour
{
    private List<IPlayerMovementHandler> movementHandlers;

    private Vector3? mouseClickPos;
    private Vector3 playerPos;
    private Vector3 joystickValue;
    private PlayerMovementSettings settings;
    private PlayerMovementView view;
    
    [Inject]
    public void Initialize(List<IPlayerMovementHandler> movementHandlers, 
        Player player, PlayerMovementView view, PlayerMovementSettings settings)
    {
        this.movementHandlers = movementHandlers;
        this.settings = settings;
        this.view = view;
        playerPos = player.transform.position;
        
        view.InitWithSize(settings.joystickSensitivity);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClickPos = Input.mousePosition;
            view.Show();
        }
        
        if (mouseClickPos.HasValue)
        {
            joystickValue = Vector3.ClampMagnitude(
                (Input.mousePosition - mouseClickPos.Value) / Screen.width / settings.joystickSensitivity, 1);
            playerPos += joystickValue * (settings.playerSpeed * Time.deltaTime);
            foreach (var movementHandler in movementHandlers)
                movementHandler.SetPosition(playerPos, joystickValue);
            view.SetValue(joystickValue);
        }
        
        if (Input.GetMouseButtonUp(0) || (mouseClickPos.HasValue && !Input.GetMouseButton(0)))
        {
            mouseClickPos = null;
            joystickValue = Vector3.zero;
            foreach (var movementHandler in movementHandlers)
                movementHandler.SetPosition(playerPos, Vector2.zero);
            view.Hide();
        }
    }
    
    [Serializable]
    public class PlayerMovementSettings
    {
        public float playerSpeed;
        public float joystickSensitivity;
    }
}
