using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform arrowHitVfxPrefab;

    private Vector3 targetPosition;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeShoot = Vector3.Distance(transform.position, targetPosition);
        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float distanceAfterShoot = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeShoot < distanceAfterShoot)
        {
            //Not over shoot the target
            transform.position = targetPosition;
            //make the trail visible after destroy the arrow
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            //Arrow hit Vfx
            Instantiate(arrowHitVfxPrefab, targetPosition, Quaternion.identity);
        }
    }
}