using BrawlingToys.Actors;
using UnityEngine;

public class TemporaryTrain : MonoBehaviour {
    private Rigidbody rb;
    private Animator anim;

    [SerializeField] private float screenXLimit = -30f;
    [SerializeField] private float speed = 250.0f;
    [SerializeField] private float maxSpeed = 3000f;
    [SerializeField][Range(1.0f, 1.25f)] private float speedMultiplierFactor = 1.10f;

    private Vector3 direction;
    private Vector3 startPosition;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start() {
        startPosition = transform.position;

        anim.SetBool("Moving", true);
    }

    private void Update() {
        direction = transform.forward;

        if (transform.position.x <= screenXLimit) {
            transform.position = startPosition;

            if (speed < maxSpeed) {
                speed *= speedMultiplierFactor;
                anim.speed *= speedMultiplierFactor;
            } else {
                speed = maxSpeed;
            }
        }
    }

    private void FixedUpdate() {
        rb.velocity = speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out IDamageable hit)) {
            hit.Damage();
        }
    }
}
