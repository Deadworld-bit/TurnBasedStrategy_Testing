using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;
    //[SerializeField] private float delayBeforeRagdoll = 1f;

    private HealthPointSystem healthPointSystem;

    private void Awake()
    {
        healthPointSystem = GetComponent<HealthPointSystem>();

        healthPointSystem.OnUnitDown += HealthPointSystem_OnUnitDown;
    }

    private void HealthPointSystem_OnUnitDown(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        Ragdoll ragdoll = ragdollTransform.GetComponent<Ragdoll>();
        ragdoll.Setup(originalRootBone);
    }
}