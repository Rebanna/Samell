using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semi_shotgun_FIre : MonoBehaviour
{
    public GameObject bulletprefab;
    public GameObject sight;
    public Transform fire_point;
    public Transform Aimpoint;
    public Animator anim;
    public Camera cam;
    public float spread;
    public int pershot;
    public float recoil;
    public float rate;
    public float reload;
    public float maxaoom;
    public float nowaoom;
    public float reloadtime;
    public float ratetime;
    private float finalrecoil = 1f;
    private float jumpexpand = 1f;


    private float distance;
    private float bullet_deflection;
    private float finalpointX, finalpointY;
    private Vector3 impactpoint;
    private Vector2 gunDirection;

    void Start()
    {
        ratetime = rate;
        nowaoom = maxaoom;
        reloadtime = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && nowaoom > 0 && reloadtime == 0)
        {
            ratetime += Time.deltaTime;
            if (ratetime - rate > 0)
            {
                anim.SetBool("fire_shotgun", true);
                for(int i = 0; i < pershot; i++)
                {
                    Shoot();
                }
            }
            ratetime = 0;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            ratetime = rate;
            anim.SetBool("fire_shotgun", false);
        }
        if (nowaoom == 0)
        {
            reloadtime += Time.deltaTime;
            anim.SetBool("reload_shotgun", true); anim.SetBool("fire_shotgun", false);
        }
        if (reloadtime > reload)
        {
            anim.SetBool("reload_shotgun", false); anim.SetBool("fire_shotgun", false);
            nowaoom = maxaoom;
            reloadtime = 0;
        }
        Expand();
        distance = (fire_point.position - Aimpoint.position).magnitude;//得出距离
        bullet_deflection = distance * spread * finalrecoil * jumpexpand;//扩圈范围=距离*扩散度*开火扩圈度*跳跃扩圈
        sight.transform.localScale = new Vector3(bullet_deflection / 3, bullet_deflection / 3, sight.transform.localScale.z);
    }

    void Shoot()
    {
        nowaoom -= 1;
        finalpointX = Random.Range(-bullet_deflection, bullet_deflection); finalpointY = Random.Range(-bullet_deflection, bullet_deflection);//随机一个在正负扩散度内的偏转值
        impactpoint = new Vector3(Aimpoint.transform.position.x + finalpointX, Aimpoint.transform.position.y + finalpointY, Aimpoint.transform.position.z);//把得到的偏转值加给鼠标周围的坐标*/
        gunDirection = (impactpoint - transform.position).normalized;//子弹的最终出膛角度
        float angle = Mathf.Atan2(gunDirection.y, gunDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        Instantiate(bulletprefab, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
        finalrecoil = finalrecoil + recoil;
    }
    void Expand()
    {
        if (finalrecoil > 1)
        {
            finalrecoil -= Time.deltaTime;
        }
        if (finalrecoil <= 1)
        {
            finalrecoil = 1;
        }
        if (finalrecoil > 2)
        {
            finalrecoil = 2;
        }
        if (PlayerController.isFalling == true)
        {
            jumpexpand = 1.25f;
        }
        else
        {
            jumpexpand = 1f;
        }
    }
}
