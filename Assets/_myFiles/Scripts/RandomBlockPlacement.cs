using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBlockPlacement : MonoBehaviour
{
    [SerializeField] GameObject block;

    private void Start()
    {
        float xPos = Random.Range(-3,3);
        block.transform.position = new Vector3(block.transform.position.x + xPos, block.transform.position.y, block.transform.position.z);
    }
}
