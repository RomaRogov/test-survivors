using System;
using System.Collections;
using DG.Tweening;
using Gameplay.Controllers;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Instances
{
    public class Player : MonoBehaviour, IPlayerMovementHandler
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform weapon;
        [SerializeField] private Transform leftSideWeaponContainer;
        [SerializeField] private Transform rightSideWeaponContainer;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private SpriteRenderer shadowSpriteRenderer;
    
        private EnemySpawnController enemySpawnController;
        private BulletSpawnController bulletSpawnController;
        private GameStatsController gameStatsController;
        
        private static readonly int isRunning = Animator.StringToHash("Running");
        private Vector3 playerScale = Vector3.one;
        private Vector3 weaponScale = Vector3.one;
        private int currentSide;
        private Enemy nearestEnemy;
        private SpriteRenderer characterSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;
        private Tween damageTween;
        private Coroutine fireRoutine;
    
        [Inject]
        public void Initialize(
            EnemySpawnController enemySpawnController, 
            BulletSpawnController bulletSpawnController, 
            GameStatsController gameStatsController)
        {
            this.enemySpawnController = enemySpawnController;
            this.bulletSpawnController = bulletSpawnController;
            this.gameStatsController = gameStatsController;
            
            fireRoutine = StartCoroutine(FireRoutine());

            characterSpriteRenderer = animator.GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = weapon.GetComponent<SpriteRenderer>();
        }

        public void SetPosition(Vector3 position, Vector2 speed)
        {
            transform.position = position;
            if (speed.sqrMagnitude > 0)
            {
                if (speed.x != 0 && currentSide != (int)Mathf.Sign(speed.x))
                {
                    currentSide = (int)Mathf.Sign(speed.x);
                    playerScale.x = currentSide;
                    animator.transform.localScale = playerScale;
                    weapon.transform.SetParent(currentSide > 0 ? rightSideWeaponContainer : leftSideWeaponContainer);
                    weapon.transform.localPosition = Vector3.zero;
                }
            
                animator.SetBool(isRunning, true);
                animator.speed = 1 + speed.magnitude;
            }
            else
            {
                animator.SetBool(isRunning, false);
                animator.speed = 1;
            }
        }

        public void Hit(int damage)
        {
            damageTween?.Kill(true);
            damageTween = characterSpriteRenderer.DOColor(Color.red, .2f).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo);
            
            gameStatsController.PlayerHit(damage);
        }

        private IEnumerator FireRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(bulletSpawnController.currentFireRate);
                if (nearestEnemy != null)
                    bulletSpawnController.Fire(bulletSpawnPoint.position, weapon.right);
            }
        }

        private void Update()
        {
            nearestEnemy = enemySpawnController.FindNearest(transform.position);
            if (nearestEnemy != null)
            {
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (angle < 0)
                    angle += 360;
                weaponScale.y = angle > 90 && angle < 270 ? -1 : 1;
                weapon.rotation = Quaternion.Euler(0, 0, angle);
                weapon.localScale = weaponScale;
            }
            
            characterSpriteRenderer.sortingOrder = -(int) (transform.position.y * 100);
            weaponSpriteRenderer.sortingOrder = characterSpriteRenderer.sortingOrder - 1;
            shadowSpriteRenderer.sortingOrder = characterSpriteRenderer.sortingOrder - 2;
        }

        private void OnDisable()
        {
            StopCoroutine(fireRoutine);
            damageTween?.Kill(true);
        }
    }
}