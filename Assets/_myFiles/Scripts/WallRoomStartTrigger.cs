using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRoomStartTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RhythmMovement movement;
        if (other.TryGetComponent(out movement))
        {
            Debug.Log("fuck");
            movement.SetWallRunSection(true);
        }
    }
}