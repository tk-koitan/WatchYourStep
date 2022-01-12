using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTrail : MonoBehaviour
{
    LineRenderer lineRenderer;
    Vector3[] poses;
    [SerializeField]
    Transform target;
    private void Awake()
    {
        TryGetComponent(out lineRenderer);
        poses = new Vector3[100];
        for (int i = 0; i < poses.Length; i++)
        {
            poses[i] = target.position;
        }
        lineRenderer.positionCount = poses.Length;
        lineRenderer.SetPositions(poses);
    }

    private void FixedUpdate()
    {
        for (int i = poses.Length - 2; i >= 0; i--)
        {
            poses[i + 1] = poses[i] + Vector3.left * RunManager.AthleticSpeed * Time.deltaTime;
        }
        poses[0] = target.position;
        lineRenderer.SetPositions(poses);
    }
}
