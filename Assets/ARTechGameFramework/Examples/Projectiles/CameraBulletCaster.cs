using UnityEngine;

namespace ARTech.GameFramework.Examples
{
    public class CameraBulletCaster : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    Projectile projectile = Instantiate(_projectilePrefab, transform.position + new Vector3(0, 15, 0), Quaternion.identity);
                    projectile.Launch(hit.point - projectile.transform.position);
                }
            }
        }
    }
}