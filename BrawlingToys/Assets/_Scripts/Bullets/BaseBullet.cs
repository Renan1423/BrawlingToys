using BrawlingToys.Actors;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Bullets
{
    public abstract class BaseBullet : NetworkBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float lifespan = 1f;
        [SerializeField] private float range = 1f;

        [SerializeField] private LayerMask ground;
        [SerializeField] private LayerMask PlayerMask;

        private float timer;
        private Rigidbody rb;
        private Vector3 direction;
        protected Player _bulletOwner;

        public int Health { get; set; }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            Initialize();
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

        public void Initialize()
        {
            Debug.DrawRay(transform.position, -transform.forward, Color.red, .5f);
            Physics.Raycast(transform.position, -transform.forward, out RaycastHit hitInfo, range, PlayerMask);
            if(hitInfo.collider != null)
            {
                if(hitInfo.transform.TryGetComponent(out Player bulletOwner))
                {
                    _bulletOwner = bulletOwner;
                }
            }
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
            //if(!IsHost) return; // Just the host machine will manage the collision  
        }

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

