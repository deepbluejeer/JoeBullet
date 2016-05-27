using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public float timeToActivateGravity;
    public int cannonPower;
    GameObject currentCannon;
    GameObject firePoint;
    Rigidbody player;
    bool insideCannon;

    void Start()
    {
        player = GetComponent<Rigidbody>();
        insideCannon = false;
    }

    void Update()
    {
        if (currentCannon != null)
        {
            player.transform.position = firePoint.transform.position;
            player.transform.rotation = currentCannon.transform.rotation;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && insideCannon)
            StartCoroutine(fireCannon());
    }

    void enterCannon()
    {
        insideCannon = true;
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        player.velocity = Vector3.zero;
        player.angularVelocity = Vector3.zero;
        player.useGravity = false;
    }

    IEnumerator fireCannon()
    {
        if (insideCannon)
        {
            insideCannon = false;
            this.GetComponent<Renderer>().enabled = true;
            this.GetComponent<Collider>().enabled = true;
            player.AddForce(transform.right * cannonPower, ForceMode.VelocityChange);
            currentCannon = null;
            firePoint = null;

            yield return new WaitForSeconds(timeToActivateGravity);

            player.useGravity = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cannon")
        {
            currentCannon = other.gameObject;
            firePoint = other.transform.GetChild(0).gameObject;
            enterCannon();
        }
    }
}
