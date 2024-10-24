using BrawlingToys.Actors;
using UnityEngine;

[CreateAssetMenu(fileName ="New Player SO", menuName ="Player Base Stats")]
public class BaseStats : ScriptableObject
{
    [Header("Player Stats")]
    public float moveSpeed = 10;
    public float meleeCooldown = 10;
    public float dashCooldown = 10;
    public float dashAmount = 1;
    //VFX to play
    //Sound to play

    [Header("Weapon Stats")]
    public float reloadTime = 10;
    public float fireRate = 10;
    public float bulletAmount = 1;
    public HitCommand defaultHitEffect = new KillCommand();
}
