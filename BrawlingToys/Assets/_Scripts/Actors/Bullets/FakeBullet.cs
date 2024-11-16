using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class FakeBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        
        private float _speed;
        private float _gravityScale;
        
        private Vector3 _direction;

        public void Init(float originBulletSpeed, float originGravityScale)
        {
            _speed = originBulletSpeed; 
            _gravityScale = originGravityScale;  
        }

        private void Update()
        {
            _direction = transform.forward;
        }

        private void FixedUpdate()
        {
            _rb.velocity = (_speed * _direction) + _gravityScale * Physics.gravity;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground") || other.CompareTag("Wall")) 
            {
                Destroy(gameObject); 
            }
        }
    }
}
