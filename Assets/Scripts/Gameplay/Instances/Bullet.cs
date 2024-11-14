using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Instances
{
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private Vector3 direction;
        private float speed;
        private Action<Bullet> onRelease;
        private Coroutine lifetimeRoutine;
        private float lifetime;
        private bool active;
        private Collider2D myCollider;
    
        public void Initialize(float lifetime, float speed, Action<Bullet> onRelease)
        {
            myCollider = GetComponent<Collider2D>();
            
            this.onRelease = onRelease;
            spriteRenderer.enabled = false;
            myCollider.enabled = false;
            this.lifetime = lifetime;
            this.speed = speed;
            active = false;
        }

        public void Fire(Vector3 position, Vector3 direction)
        {
            transform.position = position;
            this.direction = direction.normalized;
            spriteRenderer.enabled = true;
            myCollider.enabled = true;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg);
            active = true;
            lifetimeRoutine = StartCoroutine(WaitForLifetime());
        }

        private IEnumerator WaitForLifetime()
        {
            yield return new WaitForSeconds(lifetime);
            Release();
        }
    
        private void FixedUpdate()
        {
            if (!active)
                return;
            
            transform.position += direction * (speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Hit();
                Release();
            }
        }

        private void Release()
        {
            spriteRenderer.enabled = false;
            myCollider.enabled = false;
            StopCoroutine(lifetimeRoutine);
            active = false;
            onRelease?.Invoke(this);
        }
    }
}
