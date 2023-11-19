using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMovement : MonoBehaviour
{
    float speed = 50;
    bool movedLeft = false;
    bool movedRight = true;
    bool WallRunSection = false;
    public void MoveLeftFoot()
    {
        if (!WallRunSection && !movedLeft) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            movedLeft = true;
            movedRight = false;
        }
    }

    public void MoveRightFoot()
    {
        if (!WallRunSection && !movedRight)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            movedLeft = false;
            movedRight = true;
        }
    }

    public void JumpInput()
    {
        if (!WallRunSection) { }
    }

    public void CrouchInput()
    {
        if (!WallRunSection) { }
    }
}
