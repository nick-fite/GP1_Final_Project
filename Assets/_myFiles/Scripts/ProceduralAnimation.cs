using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//I got this from here https://github.com/lchaumartin/HumanoidProceduralAnimation/blob/main/Scripts/ProceduralAnimation.cs
public class ProceduralAnimation : MonoBehaviour
{

    static Vector3[] CastOnSurface(Vector3 point, float halfRange, Vector3 up)
    {
        Vector3[] res = new Vector3[2];
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(point.x, point.y + halfRange, point.z), -up);

        if (Physics.Raycast(ray, out hit, 2f * halfRange))
        {
            res[0] = hit.point;
            res[1] = hit.normal;
        }
        else
        {
            res[0] = point;
        }
        return res;
    }


    [SerializeField] private Transform leftFootTarget;
    [SerializeField] private Transform rightFootTarget;
    [SerializeField] private Transform leftFootTargetRig;
    [SerializeField] private Transform rightFootTargetRig;
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform scaler;

    [SerializeField] private float smoothness = 2f;
    [SerializeField] private float stepHeight = 0.2f;
    [SerializeField] private float stepLength = 1f;
    [SerializeField] private float angularSpeed = 0.1f;
    [SerializeField] private float velocityMultiplier = 80f;
    [SerializeField] private float bounceAmplitude = 0.05f;
    [SerializeField] private bool running = false;

    private Vector3 initLeftFootPos;
    private Vector3 initRightFootPos;

    private Vector3 lastLeftFootPos;
    private Vector3 lastRightFootPos;

    private Vector3 lastBodyPos;
    private Vector3 initBodyPos;

    private Vector3 velocity;
    private Vector3 lastVelocity;

    void Start()
    {
        initLeftFootPos = leftFootTarget.localPosition;
        initRightFootPos = rightFootTarget.localPosition;

        lastLeftFootPos = leftFootTarget.position;
        lastRightFootPos = rightFootTarget.position;

        lastBodyPos = transform.position;
        initBodyPos = transform.localPosition;
    }

    void FixedUpdate()
    {
        velocity = transform.position - lastBodyPos;
        velocity *= velocityMultiplier;
        velocity = (velocity + smoothness * lastVelocity) / (smoothness + 1f);

        if (velocity.magnitude < 0.000025f * velocityMultiplier)
            velocity = lastVelocity;
        lastVelocity = velocity;

        int sign = (Vector3.Dot(velocity.normalized, transform.forward) < 0 ? -1 : 1);

        scaler.localScale = new Vector3(scaler.localScale.x, stepHeight * 2f * velocity.magnitude, stepLength * velocity.magnitude);
        scaler.rotation = Quaternion.LookRotation(sign * velocity.normalized, Vector3.up);
        pivot.Rotate(Vector3.right, sign * angularSpeed, Space.Self);

        Vector3 desiredPositionLeft = leftFootTarget.position;
        Vector3 desiredPositionRight = rightFootTarget.position;

        Vector3[] posNormLeft = CastOnSurface(desiredPositionLeft, 2f, Vector3.up);
        if (posNormLeft[0].y > desiredPositionLeft.y)
        {
            leftFootTargetRig.position = posNormLeft[0];
        }
        else
        {
            leftFootTargetRig.position = desiredPositionLeft;
        }
        if (posNormLeft[1] != Vector3.zero)
        {
            leftFootTargetRig.rotation = Quaternion.LookRotation(sign * velocity.normalized, posNormLeft[1]);
        }

        Vector3[] posNormRight = CastOnSurface(desiredPositionRight, 2f, Vector3.up);
        if (posNormRight[0].y > desiredPositionRight.y)
        {
            rightFootTargetRig.position = posNormRight[0];
        }
        else
        {
            rightFootTargetRig.position = desiredPositionRight;
        }
        if (posNormRight[1] != Vector3.zero)
        {
            rightFootTargetRig.rotation = Quaternion.LookRotation(sign * velocity.normalized, posNormRight[1]);
        }

        lastLeftFootPos = leftFootTargetRig.position;
        lastRightFootPos = rightFootTargetRig.position;
        float feetDistance = Mathf.Clamp01(Mathf.Abs(leftFootTargetRig.localPosition.z - rightFootTargetRig.localPosition.z) / (stepLength / 4f));

        float heightReduction = (running ? Mathf.Clamp01(velocity.magnitude) * bounceAmplitude - bounceAmplitude * Mathf.Clamp(velocity.magnitude, 0f, 10f) 
            * feetDistance : bounceAmplitude * Mathf.Clamp(velocity.magnitude, 0f, 10f) * feetDistance);
        transform.localPosition = initBodyPos - heightReduction * Vector3.up;
        scaler.localPosition = new Vector3(0f, heightReduction, 0f);

        lastBodyPos = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(leftFootTarget.position, 0.2f);
        Gizmos.DrawWireSphere(rightFootTarget.position, 0.2f);
    }
}
