using BrawlingToys.Actors;
using UnityEngine;

public class PushBullet : BaseBullet {
    public override void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out IDamageable hit))
            hit.Knockback();

        Destroy(gameObject);
    }
}
