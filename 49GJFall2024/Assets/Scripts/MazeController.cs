using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeController : MonoBehaviour
{
    public Rigidbody localRigidbody;

    public float movementSpeed = 1.0f;
    public float horizontalRotationSpeed = 2.0F;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
 	    Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = Vector3.zero;
        if (Input.GetKey("w"))
        {
            moveVector = moveVector + Vector3.forward * movementSpeed;
        }
        if (Input.GetKey("s"))
        {
            moveVector = moveVector + Vector3.back * movementSpeed;
        }
        if (Input.GetKey("a"))
        {
            moveVector = moveVector + Vector3.left * movementSpeed;
        }
        if (Input.GetKey("d"))
        {
            moveVector = moveVector + Vector3.right * movementSpeed;
        }
        //moveVector = Quaternion.AngleAxis(transform.rotation.y, Vector3.up) * moveVector;
        localRigidbody.linearVelocity = transform.rotation.normalized * moveVector;

        transform.Rotate(0, horizontalRotationSpeed * Input.GetAxis("Mouse X"), 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
 	    Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
