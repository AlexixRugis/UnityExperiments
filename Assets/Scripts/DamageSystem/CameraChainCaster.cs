using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mobs;
using ARTech.GameFramework;

public class CameraChainCaster : MonoBehaviour
{
    [SerializeField] private ChainDamageEntity _chainPrefab;
    [SerializeField] private float _maxRadius;
    [SerializeField] private int _maxDamagablesCount;
    [SerializeField] private int _damage;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                var damagable = hit.transform.GetComponent<ARTGF_IDamageable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(_damage);
                    ChainDamageFactory.Spawn(_chainPrefab,
                        transform.position, 
                        hit.transform.position, 
                        _maxRadius, 
                        _damage, 
                        _maxDamagablesCount, 
                        new List<ARTGF_IDamageable>() { damagable }
                    );
                }

            }
        }
    }
}
