using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishController : MonoBehaviour
{
    [SerializeField] GameObject Cars, GameWinParticles;
    List<Transform> carList;
    private Animator finishAnimator;
    private bool finishBarCnotroller = false;
    void Start()
    {
        GameWinParticles.SetActive(false);
        finishAnimator = GetComponent<Animator>();

        carList = new List<Transform>();

        for (int i = 0; i < Cars.transform.childCount; i++)
        {
            carList.Add(Cars.transform.GetChild(i));
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        finishAnimator.SetTrigger("OpenTrig");
        finishBarCnotroller = true;
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("TriggerStay");
        finishBarCnotroller = true;
    }
    private void OnTriggerExit(Collider other)
    {
        finishBarCnotroller = false;

        Invoke("CloseFinishBar", 0.1f);

        if (other.gameObject.tag == "car")
        {
            Debug.Log("TriggerFinish");

            carList.Remove(other.gameObject.transform);
            Destroy(other.gameObject);

            if (carList.Count == 0)
            {
                Debug.Log("GAME WIN");
                StartCoroutine(GameWin());
            }
        }
    }

    private void CloseFinishBar()
    {
        if(!finishBarCnotroller)
            finishAnimator.SetTrigger("CloseTrig");
    }

    IEnumerator GameWin()
    {
        GameWinParticles.SetActive(true);

        yield return new WaitForSeconds(2);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        
    }
}
