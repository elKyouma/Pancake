using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond;
    // [SerializeField] private bool loop;

    void Start()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        int frameIndex = 0;

        while (true)
        {
            spriteRenderer.sprite = frames[frameIndex];
            frameIndex = (frameIndex + 1) % frames.Length;
            yield return new WaitForSeconds(1f / framesPerSecond);
        }
    }
}
