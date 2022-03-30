using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;

public class CameraBulletCaster : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.point);
                BulletFactory.CreateBullet(_bulletPrefab, new Vector3(0, 15, 0), hit.point, 10f);

            }
        }
    }
}
