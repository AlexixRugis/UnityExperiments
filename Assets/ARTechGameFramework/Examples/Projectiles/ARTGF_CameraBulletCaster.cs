using UnityEngine;

namespace ARTech.GameFramework.Examples
{
    public class ARTGF_CameraBulletCaster : MonoBehaviour
    {
        [SerializeField] private ARTGF_Projectile _projectilePrefab;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    ARTGF_Projectile projectile = Instantiate(_projectilePrefab, transform.position + new Vector3(0, 15, 0), Quaternion.identity);
                    projectile.Launch(hit.point - projectile.transform.position);
                }
            }
        }
    }
}