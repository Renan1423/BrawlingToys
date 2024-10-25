using Unity.Netcode;
using UnityEngine;
using BrawlingToys.Core;
using System.Linq;

namespace BrawlingToys.Actors
{
    public class Bullet : NetworkBehaviour
    {
        public float Speed { get => _speed; }
        public Rigidbody Rb { get => _rb; }

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
            MoveBehaviour();
        }

        public void Initialize(Player bulletOwner)
        {
            _bulletOwner = bulletOwner;
        }

        private void MoveBehaviour()
        {
            // PlayerWeapon possui Stats e avisa como a bala deve se mover
            // if (IsOwner) _bulletOwner.Weapon.MoveBehaviour();

            if (IsOwner) _rb.velocity = _speed * _direction;
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

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Hitable hitable) && ValidCollision(other))
            {
                hitable.GetHit(_bulletOwner.gameObject, _bulletOwner.Stats.CurrentHitEffector);
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