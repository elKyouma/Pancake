using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bag : MonoBehaviour
{

    void Start()
    {
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + 0.5f, 0.5f)
                  .setLoopPingPong()
                .setEase(LeanTweenType.easeInOutCubic);
    }
}
