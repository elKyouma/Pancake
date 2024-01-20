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

    private float reloadTime = 0.5f;
    private float reloadTimer = 0.0f;
    private Vector2 weaponDir;

    private bool CanShoot { get { return reloadTimer < 0f;  } }
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
        transform.LeanMoveLocal(weaponDir * distance * 0.1f, 0.06f).setOnComplete(() => transform.LeanMoveLocal(weaponDir * distance, 0.4f).setEaseOutBounce());

        GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.right, Vector3.up), null);
        go.GetComponent<Rigidbody>().velocity = weaponDir * bulletSpeed;

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
