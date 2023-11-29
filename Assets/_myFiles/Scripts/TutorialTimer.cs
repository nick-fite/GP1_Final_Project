using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTimer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] TextToDisable;
    void Start()
    {
        StartCoroutine(WaitBeforeDisable());
    }

    IEnumerator WaitBeforeDisable()
    {
        yield return new WaitForSeconds(20f);

        foreach (GameObject obj in TextToDisable)
        {
            obj.SetActive(false);
        }
    }
}
