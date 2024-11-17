using System;
using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class Bullet : NetworkBehaviour
    {
        public float Speed { get => _constSpeed; }
        public Rigidbody Rb { get => _rb; }

        [SerializeField] private float _constSpeed = 1f; 
        [SerializeField] private float _buffSpeed = 2f;

        [SerializeField] private float _gravityScale = 0.5f;

        [Header("Clients Sync")]

        [SerializeField] private GameObject _fakeBullet; 

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

            if (other.CompareTag("Ground") || other.CompareTag("Wall")) 
            {
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
            SpawnFakeBulletClientRpc(); 

            var no = GetComponent<NetworkObject>(); 
            no.Despawn(); 
        }

        [ClientRpc]
        private void SpawnFakeBulletClientRpc()
        {
            if(!IsHost)
            {
                var instancePosition = transform.position; 
                var instanceRotation = transform.rotation; 

                var fakeBullet = Instantiate(_fakeBullet, instancePosition, instanceRotation)
                    .GetComponent<FakeBullet>(); 

                fakeBullet.Init(GetVelocity(), _gravityScale); 
            }
        }
    }
}