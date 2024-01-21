using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UIDetectedCutscene : MonoBehaviour
{
    [SerializeField]
    private GameObject DetectedText;
    [SerializeField]
    private CinemachineVirtualCamera vc;

    private bool activated = false;
    public void Activate()
    {
        if (activated) return;

        activated = true;
        StartCoroutine(nameof(CutscenePlayer));
    }

    IEnumerator CutscenePlayer()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.deltaTime;
        yield return new WaitForSecondsRealtime(1f);
        LeanTween.value(vc.gameObject, UpdateOrthoSize, vc.m_Lens.OrthographicSize, 2, 0.05f);

        var yPos = DetectedText.GetComponent<RectTransform>().localPosition.y;
        LeanTween.moveY(DetectedText.GetComponent<RectTransform>(), 0, .2f).setEaseOutBack();

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
}
 