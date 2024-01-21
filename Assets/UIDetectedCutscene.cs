using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.Animations;
using UnityEditor.Animations;

public class UIDetectedCutscene : MonoBehaviour
{
    [SerializeField]
    private GameObject DetectedText;
    [SerializeField]
    private CinemachineVirtualCamera vc;

    [SerializeField]
    private AnimatorController newAnimator;
    [SerializeField]
    private GameObject player;

    private AudioSource source;

    private bool activated = false;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void Activate()
    {
        if (activated) return;

        activated = true;
        StartCoroutine(nameof(CutscenePlayer));
    }

    private void Update()
    {
        //Plz dont delete, important
        LeanTween.update();
    }

    IEnumerator CutscenePlayer()
    {
        source.Play();
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.deltaTime;
        yield return new WaitForSecondsRealtime(5f);

        LeanTween.value(vc.gameObject, UpdateOrthoSize, vc.m_Lens.OrthographicSize, 2, 0.05f);

        var yPos = DetectedText.GetComponent<RectTransform>().localPosition.y;
        LeanTween.moveY(DetectedText.GetComponent<RectTransform>(), 0, .2f).setEaseOutBack();
        yield return new WaitForSecondsRealtime(0.2f);

        ChangeClothes();

        yield return new WaitForSecondsRealtime(5f);
        LeanTween.moveY(DetectedText.GetComponent<RectTransform>(), yPos, .8f).setEaseInBack();
        LeanTween.value(vc.gameObject, UpdateOrthoSize, vc.m_Lens.OrthographicSize, 10, 0.05f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void UpdateOrthoSize(float newSize)
    {
        vc.m_Lens.OrthographicSize = newSize;
    }


    private void ChangeClothes()
    {
        Animator animator = player.GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = newAnimator;
    }
}
