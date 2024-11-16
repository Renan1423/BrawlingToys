using Unity.Netcode;
using UnityEngine;
using BrawlingToys.Core;

namespace BrawlingToys.Actors
{
    public class Bullet : NetworkBehaviour
    {
        public float Speed { get => _constSpeed; }
        public Rigidbody Rb { get => _rb; }

        [SerializeField] private float _constSpeed = 1f; 
        [SerializeField] private float _buffSpeed = 2f;

        [SerializeField] private float _gravityScale = 0.5f;

        private Rigidbody _rb;
        private float _bulletPower = 1f;
        private Vector3 _direction;
        private Player _bulletOwner;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _direction = transform.forward;
        }

        private void FixedUpdate()
        {
            MoveBehaviour();
        }

        public void Initialize(Player bulletOwner, float bulletPower)
        {
            _bulletOwner = bulletOwner;
            _bulletPower = bulletPower;
        }

        private void MoveBehaviour()
        {
            if (IsOwner) _rb.velocity = (GetVelocity() * _direction) + _gravityScale * Physics.gravity;
        }

        private float GetVelocity()
        {
            return _constSpeed + (_buffSpeed * _bulletPower); 
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Hitable hitable) && ValidCollision(other))
            {
                hitable.GetHit(_bulletOwner.gameObject, _bulletOwner.Stats.CurrentHitEffect);
            }

            Debug.Log($"Bullet Collision: {other.CompareTag("Ground") || other.CompareTag("Wall")}");
            if (other.CompareTag("Ground") || other.CompareTag("Wall")) 
            {
                Debug.Log($"passou");
                DestroyBulletServerRpc();
            }
        } 

        protected bool ValidCollision(Collider other)
        {
            if (other.gameObject == _bulletOwner.gameObject) return false;

            return true; 
        }

        [ServerRpc(RequireOwnership = false)]
        protected void DestroyBulletServerRpc()
        {
            Debug.Log($"Server");
            DestroyBulletClientRpc();
        }

        [ClientRpc]
        protected void DestroyBulletClientRpc()
        {
            Debug.Log($"cliente");
            Destroy(gameObject);
        }
    }
}