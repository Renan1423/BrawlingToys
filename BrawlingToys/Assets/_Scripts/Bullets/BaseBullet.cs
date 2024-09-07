using BrawlingToys.Actors;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Bullets
{
    public abstract class BaseBullet : NetworkBehaviour, IDamageable
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float lifespan = 1f;
        [SerializeField] private LayerMask ground;

        private float timer;
        private Rigidbody rb;
        private Vector3 direction;

        public int Health { get; set; }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            direction = transform.forward;

            timer += Time.deltaTime;

            if (!rb.useGravity && timer >= lifespan)
            {
                EnableGravity();
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            transform.position += speed * Time.deltaTime * direction;
        }

        private void EnableGravity()
        {
            rb.useGravity = true;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if(!IsHost) return; // Just the host machine will manage the collision  
        }

        public void Damage() { }

        public void Knockback(GameObject sender) { }

        /// <summary>
        /// Server call to destroy the bullet on all connected clients
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        protected void DestroyBulletServerRpc()
        {
            DestroyBulletClientRpc(); 
        }

        /// <summary>
        /// Destroy the bullet in current client
        /// </summary>
        [ClientRpc]
        protected void DestroyBulletClientRpc()
        {
            Destroy(gameObject); 
        }
    }
}

