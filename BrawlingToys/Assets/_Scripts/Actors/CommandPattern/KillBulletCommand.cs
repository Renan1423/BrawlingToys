using UnityEngine;

public class KillBulletCommand : ICommand {
    private Transform _firePoint;

    public KillBulletCommand(Transform firePoint) {
        _firePoint = firePoint;
    }

    public void Execute() {
        Debug.Log("KillBullet");
        /* KillBullet Logic 
         *
         */
    }
}
