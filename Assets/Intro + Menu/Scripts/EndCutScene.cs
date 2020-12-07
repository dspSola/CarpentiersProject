using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using Cinemachine;

public class EndCutScene : MonoBehaviour
{
    [SerializeField] Animator _fade;
    [SerializeField] PlayableDirector _timeLine;
    [SerializeField] Transform _cam;
    [SerializeField] float _travelTime, _cutSceneTime;
    [SerializeField] TextMeshProUGUI _skipText;
    [SerializeField] GameObject _CMVCam2;
    [SerializeField] GameObject _CMVCam3;
    [SerializeField] TMP_Text _LastText;
    private bool hasStarted, hasFinished, quit;
    

    void Update()
    {
        if (!hasStarted)
        {
            StartCoroutine(WaitForCutScene());
            hasStarted = true;
        }
        else if (hasFinished)
        {
            if (Input.GetButton("Jump")&& !quit)
            {
                _timeLine.Stop();
                FinishCutScene();         
                quit = true;
            }
        }
    }

    private void FinishCutScene()
    {
        //ACTIVATION DU TRAVELLING !!
        _LastText.gameObject.SetActive(false);
        _CMVCam2.SetActive(false);
        _CMVCam3.SetActive(true);
        StartCoroutine(FadeOut());
    }

    private IEnumerator WaitForCutScene()
    {
        yield return new WaitForSeconds(_cutSceneTime);
        hasFinished = true;
        _skipText.gameObject.SetActive(true);
    }
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(_travelTime);
        _fade.SetBool("FadeOut", true);
        yield return new WaitForEndOfFrame();
        _fade.SetBool("FadeOut", false);
        Debug.Log("Loading Game Scene...");
        SceneManager.LoadScene(1);
    }
}

