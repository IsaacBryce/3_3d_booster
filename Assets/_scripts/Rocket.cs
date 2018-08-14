using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField]float rcsThrust = 100f;
    [SerializeField]float Boosters = 100f;


    enum State {Alive, Dying, Trancending }
    State state = State.Alive;


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
        if (state == State.Alive)
        {
        thrusters();
        rotators();
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive){ return;}//ignore collisions after death
        
        print("Collideded");
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("ok");
                break;
            case "Finish":
                print("Fin");
                state = State.Trancending;
                Invoke("LoadNextScene",1f);
                break;
            default:
                print("ded");
                state = State.Dying;
                Invoke("ReloadLevel_1",1f);

                break;

        }
    }

    private void ReloadLevel_1()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
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