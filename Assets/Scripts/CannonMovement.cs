using UnityEngine;
using System.Collections;

public class CannonMovement : MonoBehaviour
{
    public GameObject position1;
    public GameObject position2;
    public float rotation;
    GameObject destination;

    public bool isRotating;
    bool hasPlayer;

    public int cannonSpeed;

    private Rigidbody cannon;

    public enum Axial : int { RIGHT=0, LEFT=180, UP=90, DOWN=270};
    public Axial initialAxis;
    public Axial finalAxis;

    // Use this for initialization
    void Start()
    {
        cannon = GetComponent<Rigidbody>();
        destination = position1;
    }

    void cannonFacing(Axial currentAxis)
    {
        Axial axis = currentAxis;
        Quaternion actualRotation;
        actualRotation = transform.rotation;

        switch (axis)
        {
            case Axial.RIGHT:
                transform.rotation = Quaternion.Lerp(actualRotation, Quaternion.Euler(0, 0, (float)Axial.RIGHT), 10f * Time.deltaTime);
                break;

            case Axial.LEFT:
                transform.rotation = Quaternion.Lerp(actualRotation, Quaternion.Euler(0, 0, (float)Axial.LEFT), 10f * Time.deltaTime);
                break;

            case Axial.UP:
                transform.rotation = Quaternion.Lerp(actualRotation, Quaternion.Euler(0, 0, (float)Axial.UP), 10f * Time.deltaTime);
                break;

            case Axial.DOWN:
                transform.rotation = Quaternion.Lerp(actualRotation, Quaternion.Euler(0, 0, (float)Axial.DOWN), 10f * Time.deltaTime);
                break;
        }
    }

    void waitingPlayer()
    {
        //The cannon should face the direction the player is (i.e. left, up, down). When the player gets inside the cannon, it should rotate to the correct angle.
        if (!hasPlayer && !isRotating)
            cannonFacing(initialAxis);
        else if (hasPlayer && !isRotating)
            cannonFacing(finalAxis);
    }

    void MoveCannon()
    {
        //Move cannon between two predefined positions.
        if (destination != null)
        {
            Vector3 direction = (destination.transform.position - transform.position).normalized;
            cannon.MovePosition(transform.position + direction * cannonSpeed * Time.deltaTime);

            if (Vector3.Distance(destination.transform.position, transform.position) <= 0.2f)
                destination = (destination != position1) ? position1 : position2;
        }
    }

    void rotateCannon(float rot)
    {


        if (rot == 360)
            transform.Rotate(Vector3.forward * 360.0f * Time.deltaTime);
    }

    void Update()
    {
        waitingPlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCannon();
        rotateCannon(rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hasPlayer = true;
        }
    }
}
