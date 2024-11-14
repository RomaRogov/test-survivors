using Gameplay.Controllers;
using Gameplay.Instances;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [Header("Player")]
    [SerializeField] private Player player;
    [SerializeField] private BulletSpawnController bulletSpawnController;
    [Header("User Input")]
    [SerializeField] private PlayerMovementController playerMovementController;
    [Header("Camera")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GroundController groundController;
    [Header("Enemies")]
    [SerializeField] private EnemySpawnController enemySpawnController;
    [Header("Interface")]
    [SerializeField] private GameStatsView gameStatsView;
    [SerializeField] private PlayerMovementView playerMovementView;
    
    public override void InstallBindings()
    {
        LoadingManager loadingManager = FindObjectOfType<LoadingManager>();
        Container.Bind<LoadingManager>().FromInstance(loadingManager).AsSingle();
        
        Container.Bind<IPlayerMovementHandler>().FromInstance(player).AsTransient();
        Container.Bind<IPlayerMovementHandler>().FromInstance(cameraController).AsTransient();
        Container.Bind<IPlayerMovementHandler>().FromInstance(groundController).AsTransient();
        Container.Bind<GameStatsView>().FromInstance(gameStatsView).AsSingle();
        Container.Bind<GameStatsController>().AsSingle();
        Container.Bind<BulletSpawnController>().FromInstance(bulletSpawnController).AsSingle();
        Container.Bind<Player>().FromInstance(player).AsSingle();
        Container.Bind<CameraController>().FromInstance(cameraController).AsSingle();
        Container.Bind<PlayerMovementView>().FromInstance(playerMovementView).AsSingle();
        Container.BindInstance(playerMovementController).AsSingle();
        Container.BindInstance(enemySpawnController).AsSingle();
        
        loadingManager.Hide();
    }
}