using Unity.Netcode;
using UnityEngine;
using BrawlingToys.Core;

namespace BrawlingToys.Actors
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _lifespan = 1f;

        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _hitableMask;

        private CountdownTimer _timer;
        private Rigidbody _rb;
        private Vector3 _direction;
        private Player _bulletOwner;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            _timer = new CountdownTimer(_lifespan);
            _timer.Start();
        }

        private void Update()
        {
            _direction = transform.forward;

            _timer.Tick(Time.deltaTime);

            if (!_rb.useGravity && _timer.IsFinished)
            {
                EnableGravity();
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Initialize(Player bulletOwner)
        {
            _bulletOwner = bulletOwner;
        }

        private void Move()
        {
            if(IsOwner) _rb.velocity = _speed * Time.deltaTime * _direction;
        }

        private void EnableGravity()
        {
            _rb.useGravity = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IHitable hitable) && ValidCollision(other))
            {
                hitable.GetHit(_bulletOwner.gameObject, _bulletOwner.Stats.CurrentHitEffector);
            }
        } 

        protected bool ValidCollision(Collider other)
        {
            if (!IsOwner) return false; 
            Debug.Log($"Is the same: {other.gameObject == _bulletOwner.gameObject} - Collision: {other.gameObject} - Owner: {_bulletOwner.gameObject}");
            if (other.gameObject == _bulletOwner.gameObject) return false;

            return true; 
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