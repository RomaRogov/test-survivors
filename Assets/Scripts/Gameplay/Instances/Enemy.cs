using System;
using System.Collections;
using DG.Tweening;
using Gameplay.Controllers;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Gameplay.Instances
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : MonoBehaviour
    {
        private static readonly int hit = Animator.StringToHash("Hit");
        private static readonly int dead = Animator.StringToHash("Dead");
        
        public int experience => settings.experience;
        
        [SerializeField] private SpriteRenderer shadowSpriteRenderer;
        
        private EnemyTypeSettings settings;
        private Player player;
        private AudioController audioController;
        
        private Animator animator;
        private Rigidbody2D rb;
        private Collider2D col;
        private bool died;
        private SpriteRenderer spriteRenderer;
        private Action<Enemy> onDied;
        private float attackCooldown;
        private Vector3 initialScale;
        private Vector3 currentScale;

        private int health;

        [Inject]
        public void Initialize(Player player, AudioController audioController)
        {
            this.player = player;
            this.audioController = audioController;
            
            died = false;
            
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();

            initialScale = transform.localScale;
            currentScale = initialScale;
        }
        
        public void SetStats(EnemyTypeSettings settings, Action<Enemy> onDied)
        {
            this.settings = settings;
            this.onDied = onDied;

            health = this.settings.health;
        }

        public void Hit()
        {
            if (died)
                return;
            
            health--;

            if (health > 0)
            {
                audioController.PlaySound(AudioController.Sound.EnemyHit);
                animator.SetTrigger(hit);
            }
            else
            {
                audioController.PlaySound(AudioController.Sound.EnemyDied);
                died = true;
                col.enabled = false;
                onDied?.Invoke(this);
                animator.SetBool(dead, true);
                spriteRenderer.DOFade(0, 2).SetEase(Ease.InExpo).OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            }
        }
        
        private void FixedUpdate()
        {
            if (died)
                return;

            Vector3 delta = player.transform.position - transform.position;
            if (attackCooldown <= 0 && delta.sqrMagnitude < Mathf.Abs(transform.localScale.x) * .5f)
            {
                attackCooldown = settings.attackRate;
                player.Hit(settings.damage);
            }
            attackCooldown -= Time.fixedDeltaTime;

            if (delta.x != 0)
            {
                currentScale.x = initialScale.x * Mathf.Sign(delta.x);
                transform.localScale = currentScale;
            }
            
            rb.MovePosition(Vector3.MoveTowards(transform.position, 
                player.transform.position, settings.speed * Time.deltaTime));
            
            spriteRenderer.sortingOrder = -(int) (transform.position.y * 100);
            shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        }
        
        [Serializable]
        public class EnemyTypeSettings
        {
            public Enemy enemyPrefab;
            public float spawnRate;
            public int health;
            public int damage;
            public float speed;
            public float attackRate;
            public int experience;
        }
    }
}