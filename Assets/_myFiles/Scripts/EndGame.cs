using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<RhythmMovement>().SetMove(false);
        GameManager.m_Instance.GetGameOverCanvas().SetActive(true);
        StartCoroutine(WaitBeforeTitleScreen());
    }

    IEnumerator WaitBeforeTitleScreen()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
