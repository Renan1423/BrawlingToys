using UnityEngine;

public class PushBulletCommand : ICommand {
    private Transform _firePoint;

    public PushBulletCommand(Transform firePoint) {
        _firePoint = firePoint;
    }

    public void Execute() {
        Debug.Log("PushBullet");
        // KillBullet Logic
    }
}
