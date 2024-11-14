using System.Collections;
using System.Collections.Generic;
using Gameplay.Controllers;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private EnemySpawnController.Settings enemySpawnerSettings;
    [SerializeField] private BulletSpawnController.BulletSpawnSettings bulletSettings;
    [SerializeField] private PlayerMovementController.PlayerMovementSettings playerMovementSettings;
    [SerializeField] private GameStatsController.Settings gameStatsSettings;
    
    public override void InstallBindings()
    {
        Container.BindInstance(enemySpawnerSettings);
        Container.BindInstance(bulletSettings);
        Container.BindInstance(playerMovementSettings);
        Container.BindInstance(gameStatsSettings);
    }
}
