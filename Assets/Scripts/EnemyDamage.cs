using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    public int damage;
    [SerializeField]
    private Gun gun;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger is ON!");
            gun.Triggers(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger is OFF!");
            damage = 0;
            gun.Triggers(damage);
        }
    }
}
