using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Controllers
{
    public class GameStatsController
    {
        public int playerHealth { get; private set; }
        public int enemiesKilled { get; private set; }
        
        private readonly GameStatsView gameStatsView;
        private readonly LoadingManager loadingManager;
        private readonly AudioController audioController;
        
        private Settings settings;
        private int experience;
        private int level = 1;
        private int targetExperience => level * settings.experiencePerLevelMultiplier;
        
        [Inject]
        public GameStatsController(Settings settings, GameStatsView gameStatsView, 
            LoadingManager loadingManager, AudioController audioController)
        {
            this.gameStatsView = gameStatsView;
            this.settings = settings;
            this.loadingManager = loadingManager;
            this.audioController = audioController;
            
            playerHealth = settings.playerHealth;
            enemiesKilled = 0;
            
            gameStatsView.SetHealth(1);
            gameStatsView.SetEnemiesKilled(enemiesKilled, level, 0);
        }

        public void PlayerHit(int damage)
        {
            if (playerHealth <= 0)
            {
                return;
            }
            
            playerHealth -= damage;
            if (playerHealth <= 0)
            {
                playerHealth = 0;
                audioController.PlaySound(AudioController.Sound.Lose);
                loadingManager.Show();
            }
            else
            {
                audioController.PlaySound(AudioController.Sound.PlayerHit);
            }
            gameStatsView.SetHealth(playerHealth / (float)settings.playerHealth);
        }
        
        public void EnemyKilled(int experience)
        {
            enemiesKilled++;
            this.experience += experience;
            if (this.experience >= targetExperience)
            {
                this.experience -= targetExperience;
                level++;
                audioController.PlaySound(AudioController.Sound.LevelUp);
            }
            gameStatsView.SetEnemiesKilled(enemiesKilled, level, this.experience / (float)targetExperience);
        }
        
        [Serializable]
        public class Settings
        {
            public int playerHealth;
            public int experiencePerLevelMultiplier;
        }
    }
}