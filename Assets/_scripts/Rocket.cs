using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField]float rcsThrust = 100f;
    [SerializeField]float Boosters = 100f;
    

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        thrusters();
        rotators();

    }
    private void OnCollisionEnter(Collision collision)
    {
        print("Collideded");
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("ok");
                break;
            case "Fuel":
                print("refueled");
                break;
            default:
                print("ded");
                break;

        }
    }

    private void thrusters()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            print("thrusters engage");
            float BoostThis = Boosters * Time.deltaTime;

            rigidBody.AddRelativeForce(Vector3.up * BoostThis);
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


        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
            
            transform.Rotate(Vector3.forward * rotationThisFrame);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating Right");
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;//resume physics
    } 
}