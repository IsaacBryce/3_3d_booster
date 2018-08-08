using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }
    private void ProcessInput()
    {
        thrusters();
        rotators();

    }
private void thrusters()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            print("thrusters engage");
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else { audioSource.Stop(); }
    }
    private void rotators()
    {
        rigidBody.freezeRotation = true;//take manual control of rotation
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
            transform.Rotate(Vector3.forward);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating Right");
            transform.Rotate(-Vector3.forward);
        }
        rigidBody.freezeRotation = false;//resume physics
    }

    
}