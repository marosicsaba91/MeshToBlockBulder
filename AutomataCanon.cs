using System.Collections;
using UnityEngine;

public class AutomataCanon : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Vector3 spawnOffset = new(0, 0, 1);
    
    [SerializeField] float bulletSpeed = 50f;
    [SerializeField] float fireRate = 5f;
    
    [SerializeField] float rotationSpeed = 30f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            StartCoroutine(Shoot());
         
        Vector3 horizontal = new(0, Input.GetAxis("Horizontal") * rotationSpeed);
        horizontal *= Time.deltaTime;

        Vector3 vertical = new(-Input.GetAxis("Vertical") * rotationSpeed, 0);
        vertical *= Time.deltaTime;

        transform.Rotate(horizontal, Space.World);
        transform.Rotate(vertical, Space.Self);

    }
    Vector3 SpawnPoint => transform.TransformPoint(spawnOffset);

    IEnumerator Shoot()
    {
        while (Input.GetButton("Fire1"))
        { 
            GameObject newBullet = Instantiate(bullet, SpawnPoint, transform.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            yield return new WaitForSeconds(1/fireRate);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(SpawnPoint, transform.forward * 10f);
    }
}
