using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HitScanWeapon : MonoBehaviour
{
    private InputManager playerInput;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float distance = 30f;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.onFootActions.Shoot.triggered)
        {
            fireWeapon();
        }
    }

    private void fireWeapon()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            if (hit.collider.gameObject.CompareTag("Target"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
