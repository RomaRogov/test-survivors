using System;
using System.Collections.Generic;
using Gameplay.Instances;
using UnityEngine;
using Zenject;

namespace Gameplay.Controllers
{
    public class BulletSpawnController : MonoBehaviour
    {
        public float currentFireRate => settings.bulletSettings.fireRate;
        
        private DiContainer container;
        private Queue<Bullet> bulletsPool;

        private BulletSpawnSettings settings;

        [Inject]
        public void Initialize(DiContainer container, BulletSpawnSettings settings)
        {
            this.container = container;
            bulletsPool = new Queue<Bullet>();
            this.settings = settings;
            
            for (int i = 0; i < settings.bulletSettings.initialCountInPool; i++)
                bulletsPool.Enqueue(CreateBullet());
        }

        public void Fire(Vector3 position, Vector3 direction)
        {
            Bullet bullet = bulletsPool.Count > 0 ? bulletsPool.Dequeue() : CreateBullet();
            bullet.Fire(position, direction);
        }
        
        private Bullet CreateBullet()
        {
            Bullet bullet = container.InstantiatePrefabForComponent<Bullet>(settings.bulletSettings.bulletPrefab, transform);
            bullet.Initialize(settings.bulletSettings.lifetime, settings.bulletSettings.speed, Release);
            return bullet;
        }

        private void Release(Bullet bullet)
        {
            bulletsPool.Enqueue(bullet);
        }
        
        [Serializable]
        public class BulletSpawnSettings
        {
            //Can also be an array if improvements with pooling of bullets of different types are needed
            public BulletSettings bulletSettings;
        }
        
        [Serializable]
        public class BulletSettings
        {
            public Bullet bulletPrefab;
            public float fireRate;
            public float speed;
            public float lifetime;
            public int initialCountInPool;
        }
    }
}