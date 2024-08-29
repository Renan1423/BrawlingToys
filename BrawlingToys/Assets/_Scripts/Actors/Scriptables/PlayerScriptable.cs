using UnityEngine;

[CreateAssetMenu(fileName ="New Player SO", menuName ="Player Stats")]
public class PlayerScriptable : ScriptableObject
{
    [Header("Player")]
    public float healthPoints;
    public float speed;
    public float dashForce;
    public float dashCooldown;
    //VFX to play
    //Sound to play
    [Header("Weapon")]
    public float fireRate;
    public float ammo;
    public float reloadTime;
    
}
