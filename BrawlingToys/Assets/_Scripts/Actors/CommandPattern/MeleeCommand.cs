using BrawlingToys.Actors;
using UnityEngine;

public class MeleeCommand : ICommand {
    private Transform _firePoint;
    private float _meleeRadius;

    public MeleeCommand(Transform firePoint, float radius) {
        _firePoint = firePoint;
        _meleeRadius = radius;
    }

    public void Execute() {
        if (Physics.SphereCast(_firePoint.position, _meleeRadius, _firePoint.forward, out RaycastHit raycastHit, 2.0f)) {
            if(raycastHit.collider.TryGetComponent(out IDamageable hit))
                hit.Knockback();
        }
    }
}
