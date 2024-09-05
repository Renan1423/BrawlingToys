using BrawlingToys.Actors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownDebufArea : MonoBehaviour
{
    [SerializeField] BasicModifierSO BasicModifierSO;

    private RaycastHit hitInfo;
    private StatModifier basicStatModifier;
    private Player playerTarget;

    [Header("Gizmos Parameters")]
    [SerializeField] private Color color;
    [Range(1f, 10f)]
    [SerializeField] private float radius = 1f;

    private void Start()
    {
        basicStatModifier = BasicModifierSO.CreateModifier();
    }

    private void Update()
    {
        if(Physics.SphereCast(transform.position, radius, Vector3.one, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                Player playerHit = hitInfo.collider.GetComponent<Player>();
                if (playerTarget == null)
                    playerTarget = playerHit;
                else if (playerTarget == playerHit)
                    return;
                
                playerTarget._stats.Mediator.AddModifier(basicStatModifier);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position + Vector3.one, radius);
    }
}
