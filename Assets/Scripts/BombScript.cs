using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField]
    public int damage;
    [SerializeField]
    private Gun gun;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger is ON!");
            gun.Triggers(damage);
            Destroy(gameObject, 2f);
        }
    }
}
