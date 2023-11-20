using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRoomEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RhythmMovement movement;
        if (other.TryGetComponent(out movement))
        {
            movement.SetWallRunSection(false);
        }
    }
}
