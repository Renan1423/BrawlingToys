using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadVisual : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _head;

    private void Start()
    {
        _player.OnUpdateAimRotation += Player_OnUpdateAimRotation;
    }

    private void Player_OnUpdateAimRotation(object sender, Vector3 lookDirection)
    {
        _head.forward = lookDirection;
    }
}
