using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellBurstProjectile : MonoBehaviour
{
    public static event EventHandler OnAnySpellBurst;

    [SerializeField] private Transform spellBurstVfxPrefab;
    [SerializeField] private TrailRenderer spellBurstTrailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetPosition;
    private Action OnSpellBurstComplete;
    private float totalDistance;
    private Vector3 positionXZ;

    public void Setup(GridPosition targetGridPosition, Action OnSpellBurstComplete)
    {
        this.OnSpellBurstComplete = OnSpellBurstComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 1.5f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(40);
                }
                if (collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate))
                {
                    destructableCrate.Damage();
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
