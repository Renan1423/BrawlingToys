using BrawlingToys.Actors;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour, IDamageable {
    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifespan = 1f;
    [SerializeField] LayerMask ground;

    private float timer;
    private Rigidbody rb;
    private Vector3 direction;

    public int Health { get; set; }

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        direction = transform.forward;

        timer += Time.deltaTime;

        if (!rb.useGravity && timer >= lifespan) {
            EnableGravity();
        }
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void EnableGravity() {
        rb.useGravity = true;
    }

    public abstract void OnTriggerEnter(Collider other);

    public void Damage() { }

    public void Knockback(GameObject sender) { }
}
