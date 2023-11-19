using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private IKFootSolver otherFoot;
    [SerializeField] private float stepDistance, stepHeight, stepLength, footspacing, speed;
    [SerializeField] private Transform body;
    [SerializeField] private float bodyOffset;
    [SerializeField] private Vector3 footOffset;

    private Vector3 oldPosition, newPosition, currentPosition;
    private Vector3 oldNormal, newNormal, currentNormal;
    float lerp;

    void Start()
    {
        footspacing = 0;
        oldPosition = newPosition = currentPosition = transform.position;
        oldNormal = newNormal = currentNormal = transform.up;
        lerp = 1;
        bodyOffset = 1;
    }

    private void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        Ray ray = new Ray( new Vector3(body.position.x, body.position.y + bodyOffset, body.position.z) + (body.right * footspacing), Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer))
        {
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = hit.point + body.forward * stepLength * direction + footOffset;
                newNormal = hit.normal;
            }
        }
        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = tempPos;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    public bool IsMoving() { return lerp < 1; }
}
