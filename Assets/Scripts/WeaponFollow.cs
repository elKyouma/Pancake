using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    private int shootCount = 0;
    private float reloadTime = 0.5f;
    private float reloadTimer = 0.0f;

    private bool useMouse = true;

    private Vector2 weaponDir;
    private Vector2 mousePos;


    [SerializeField]
    private AudioClip clip;

    private AudioSource audioSource;

    private SpriteRenderer sprite;

    [SerializeField]
    private Text ammoText;

    private Transform graphic;

    [SerializeField]
    private UIDetectedCutscene cutscene;

    private bool CanShoot { get { return reloadTimer < 0f; } }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        graphic = GetComponentsInChildren<Transform>()[1];
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnWeaponMovement(InputAction.CallbackContext ctx)
    {
        weaponDir = ctx.ReadValue<Vector2>();
        useMouse = false;
    }

    public void OnMouseMovement(InputAction.CallbackContext ctx)
    {
        if (Camera.main)
        {
            mousePos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
            useMouse = true;
        }
    }

    private bool wasNotAngry = true;
    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() < 0.5f || !CanShoot) return;

        if (wasNotAngry)
        {
            foreach (var p in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                p.GetComponent<Shooting>().MakeAngry();
            }
            wasNotAngry = false;
        }

        SpecialEffects.Instance.ScreenShake(0.3f, 15f);
        StartCoroutine(Vibrate());
        audioSource.PlayOneShot(clip);
        graphic.LeanMoveLocal(Vector3.left * distance * 0.5f, 0.08f).setOnComplete(() => graphic.LeanMoveLocal(Vector3.zero, 0.4f).setEaseOutBounce());

        GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity, null);
        go.GetComponent<Rigidbody2D>().velocity = weaponDir * bulletSpeed;
        shootCount++;

        if (shootCount % magazineSize == 0)
            reloadTime = 2.0f;
        else
            reloadTime = 0.5f;
        cutscene.Activate();
        reloadTimer = reloadTime;
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {

            ammoText.text = $"Ammo: {magazineSize - shootCount}/{magazineSize}";
        }
    }

    private IEnumerator Vibrate()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.2f, 0.2f);
            yield return new WaitForSeconds(0.3f);
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }

    private void Update()
    {
        if (CanShoot)
        {
            if (shootCount % magazineSize == 0)
            {
                shootCount = 0;
                UpdateAmmoText();
            }
        }
        else
            reloadTimer -= Time.deltaTime;

        if (useMouse)
        {
            weaponDir = (Vector3)mousePos - transform.parent.position;
            weaponDir.Normalize();
        }
        WeaponRotate();

        transform.localPosition = (Vector3)weaponDir * distance + Vector3.up * 0.5f;
    }
    public void WeaponRotate()
    {
        Vector2 dir = weaponDir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle < 0 || angle > 180)
            sprite.sortingOrder = -1;
        else
            sprite.sortingOrder = 1;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (angle > 90 || angle < -90)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
