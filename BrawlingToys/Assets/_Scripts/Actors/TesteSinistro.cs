using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Actors
{
    public class TesteSinistro : MonoBehaviour
    {
        public float moveSpeed = 5f; 

        private Rigidbody rb;
        private Vector3 movement;

        void Start()
        {
            // Pega o componente Rigidbody
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Captura o input das teclas WASD
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Cria um vetor de movimento baseado no input
            movement = new Vector3(moveX, 0f, moveZ);
        }

        void FixedUpdate()
        {
            // Move o jogador usando o Rigidbody
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
