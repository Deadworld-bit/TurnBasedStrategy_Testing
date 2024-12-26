using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellBurstProjectile : MonoBehaviour
{
    public static event EventHandler OnAnySpellBurst;

    [SerializeField] private Transform spellBurstVfxPrefab;
    [SerializeField] private TrailRenderer spellBurstTrailRenderer;

    private Vector3 targetPosition;
    private Action OnSpellBurstComplete;

    public void Setup(GridPosition targetGridPosition, Action OnSpellBurstComplete)
    {
        this.OnSpellBurstComplete = OnSpellBurstComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(40);
                }
            }

            OnAnySpellBurst?.Invoke(this, EventArgs.Empty);

            spellBurstTrailRenderer.transform.parent = null;
            Instantiate(spellBurstVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            
            Destroy(gameObject);
            OnSpellBurstComplete();
        }
    }
}
