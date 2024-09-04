using UnityEngine;

public class MeleeCommand : ICommand {
    private Transform _firePoint;

    public MeleeCommand(Transform firePoint) {
        _firePoint = firePoint;
    }

    public void Execute() {
        if (Physics.SphereCast(_firePoint.position, 0.5f, _firePoint.forward, out RaycastHit raycastHit, 2.0f)) {
            Debug.Log("Melee hit: " + raycastHit.collider.gameObject.ToString());
        } else {
            Debug.Log("Melee missed!");
        }
    }
}
