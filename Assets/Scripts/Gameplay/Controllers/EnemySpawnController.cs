using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Instances;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Controllers
{
    public class EnemySpawnController : MonoBehaviour
    {
        private Settings settings;
        private CameraController cameraController;
        private GameStatsController gameStatsController;
        
        private DiContainer diContainer;
        private List<Coroutine> enemyRoutines;
        private List<Enemy> enemies;
        
        [Inject]
        public void Initialize(DiContainer diContainer, 
            CameraController cameraController, GameStatsController gameStatsController, Settings settings)
        {
            this.diContainer = diContainer;
            this.settings = settings;
            this.cameraController = cameraController;
            this.gameStatsController = gameStatsController;

            enemies = new List<Enemy>();
            enemyRoutines = new List<Coroutine>();
            foreach (var enemyType in settings.enemyTypes)
            {
                enemyRoutines.Add(StartCoroutine(SpawnEnemy(enemyType)));
            }
        }

        public Enemy FindNearest(Vector3 fromPos)
        {
            if (enemies.Count == 0)
                return null;

            Enemy nearest = null;
            float minDistance = float.MaxValue;
            foreach (var enemy in enemies)
            {
                float sqrDist = (enemy.transform.position - fromPos).sqrMagnitude;
                if (sqrDist < settings.detectRadius * settings.detectRadius && sqrDist < minDistance)
                {
                    minDistance = (enemy.transform.position - fromPos).sqrMagnitude;
                    nearest = enemy;
                }
            }

            return nearest;
        }

        private void OnDisable()
        {
            foreach (var routine in enemyRoutines)
            {
                StopCoroutine(routine);
            }
        }

        private IEnumerator SpawnEnemy(Enemy.EnemyTypeSettings enemyTypeSettings)
        {
            while (true)
            {
                yield return new WaitForSeconds(enemyTypeSettings.spawnRate);
                var spawnPos = Random.insideUnitCircle * settings.spawnRadius;
                var cameraBounds = cameraController.cameraBounds;
                //Mirror out spawned enemy if it is inside camera bounds
                if (cameraBounds.Contains(spawnPos + cameraBounds.position + cameraBounds.size / 2f))
                {
                    var borderX = cameraBounds.width / 2f * Mathf.Sign(spawnPos.x);
                    var borderY = cameraBounds.height / 2f * Mathf.Sign(spawnPos.y);
                    spawnPos.x += (borderX - spawnPos.x) * 2f;
                    spawnPos.y += (borderY - spawnPos.y) * 2f;
                }
                
                spawnPos += cameraBounds.position;
                var enemy = diContainer.InstantiatePrefabForComponent<Enemy>
                    (enemyTypeSettings.enemyPrefab, spawnPos, Quaternion.identity, transform);
                enemy.SetStats(enemyTypeSettings, EnemyDied);
                enemies.Add(enemy);
            }
        }

        private void EnemyDied(Enemy enemy)
        {
            enemies.Remove(enemy);
            gameStatsController.EnemyKilled(enemy.experience);
        }

        [Serializable]
        public class Settings
        {
            public Enemy.EnemyTypeSettings[] enemyTypes;
            public float spawnRadius;
            public float detectRadius;
        }
    }
}