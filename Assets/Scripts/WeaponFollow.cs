using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponFollow : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    private float distance = 2f;

    [SerializeField]
    private float bulletSpeed = 20f;

    [SerializeField]
    private int magazineSize = 4;

    private float shootCount = 0;
    private float reloadTime = 0.5f;
    private float reloadTimer = 0.0f;
    private Vector2 weaponDir;

    [SerializeField]
    private AudioClip clip;

    private AudioSource audio;

    private bool CanShoot { get { return reloadTimer < 0f;  } }

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void OnWeaponMovement(InputAction.CallbackContext ctx)
    {
        weaponDir = ctx.ReadValue<Vector2>();
    }

    public void OnMouseMovement(InputAction.CallbackContext ctx)
    {
        if (Camera.main)
        {
            weaponDir = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>()) - transform.parent.position;
            weaponDir.Normalize();
        }
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() < 0.5f || !CanShoot) return;

        SpecialEffects.Instance.ScreenShake(0.3f, 20f);
        StartCoroutine(Vibrate());
        audio.PlayOneShot(clip);
        transform.LeanMoveLocal(weaponDir * distance * 0.1f, 0.06f).setOnComplete(() => transform.LeanMoveLocal(weaponDir * distance, 0.4f).setEaseOutBounce());

        GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity, null);
        go.GetComponent<Rigidbody2D>().velocity = weaponDir * bulletSpeed;
        shootCount++;
        if(shootCount == magazineSize)
        {
            reloadTime = 2.0f;
            shootCount = 0;
        }
        else
        {
            reloadTime = 0.5f;
        }

        reloadTimer = reloadTime;
    }

    private IEnumerator Vibrate()
    {
        Gamepad.current.SetMotorSpeeds(0.2f,0.2f);
        yield return new WaitForSeconds(0.3f);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

    private void Update()
    {
        if (CanShoot)
            transform.localPosition = weaponDir * distance;
        else
            reloadTimer -= Time.deltaTime;
    }
}
