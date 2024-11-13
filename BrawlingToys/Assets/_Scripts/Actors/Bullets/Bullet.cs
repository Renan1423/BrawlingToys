using Unity.Netcode;
using UnityEngine;
using BrawlingToys.Core;

namespace BrawlingToys.Actors
{
    public class Bullet : NetworkBehaviour
    {
        public float Speed { get => _speed; }
        public Rigidbody Rb { get => _rb; }

        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _gravityScale = 0.5f;

        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _hitableMask;

        private Rigidbody _rb;
        private float _bulletPower = 1f;
        private Vector3 _direction;
        private Player _bulletOwner;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            //_timer = new CountdownTimer(_lifespan);
            //_timer.Start();
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
            // PlayerWeapon possui Stats e avisa como a bala deve se mover
            // if (IsOwner) _bulletOwner.Weapon.MoveBehaviour();

            if (IsOwner) _rb.velocity = (_speed * _bulletPower * _direction) + _gravityScale * Physics.gravity;
        }

        private void DestroyEffect()
        {
            // PlayerWeapon possui Stats e avisa qual efeito a bala deve ter quando for destruida
            // if (IsOwner) _bulletOwner.Weapon.DestroyEffect();
        }

        private void EnableGravity()
        {
            _rb.useGravity = true;
        }

        public void Parry(Player player) {
            _bulletOwner = player;
            transform.forward = player.transform.forward;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Hitable hitable) && ValidCollision(other))
            {
                hitable.GetHit(_bulletOwner.gameObject, _bulletOwner.Stats.CurrentHitEffect);
            }

            if (other.CompareTag("Ground"))
            {
                DestroyBulletServerRpc();
            }
        } 

        protected bool ValidCollision(Collider other)
        {
            //if (!IsOwner) return false; 
            if (other.gameObject == _bulletOwner.gameObject) return false;

            return true; 
        }

        [ServerRpc(RequireOwnership = false)]
        protected void DestroyBulletServerRpc()
        {
            DestroyBulletClientRpc();
        }

        [ClientRpc]
        protected void DestroyBulletClientRpc()
        {
            DestroyEffect();
            Destroy(gameObject);
        }
    }
}