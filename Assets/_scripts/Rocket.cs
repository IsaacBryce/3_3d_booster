using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField]float rcsThrust = 100f;
    [SerializeField]float Boosters = 100f;
    [SerializeField]float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip transcendAudio;
    [SerializeField] AudioClip deathAudio;

    [SerializeField] ParticleSystem mainThruster;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem successParticle;

    enum State { Alive, Dying, Trancending }
    State state = State.Alive;

    bool collisionsEnabled = true;

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
        if (Debug.isDebugBuild)
        {
            respondToDebugKeys();
        }
    }
    private void respondToDebugKeys()
        {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //print("load Next level");

            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            print("Collisions toggled");
            collisionsEnabled = !collisionsEnabled; 
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive|| !collisionsEnabled) { return; }

        print("Collideded");
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("ok");
                break;
            case "Finish":
                successSequence();
                break;
            default:
                deathSequence();
                break;

        }
    }

    private void deathSequence()
    {
        print("ded");
        state = State.Dying;
        audioSource.Stop();
        deathParticle.Play();
        mainThruster.Stop();
        audioSource.PlayOneShot(deathAudio);
        Invoke("ReloadLevel_1", levelLoadDelay);
    }

    private void successSequence()
    {
        print("Fin");
        state = State.Trancending;
        audioSource.Stop();
        mainThruster.Stop();
        successParticle.Play();
        audioSource.PlayOneShot(transcendAudio);
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void ReloadLevel_1()
    {
        SceneManager.LoadScene(0);

    }

    private void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        //print(currentScene);
        int nextScene = currentScene + 1;
        if (nextScene == SceneManager.sceneCountInBuildSettings) {
            ReloadLevel_1();
        }
        else SceneManager.LoadScene(nextScene);

    }

    private void thrusters()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            applyThrusting();
        }
        else
        {
            audioSource.Stop();
            mainThruster.Stop();
        }
    }

    private void applyThrusting()
    {
        print("thrusters engage");
        float BoostThis = Boosters * Time.deltaTime;

        rigidBody.AddRelativeForce(Vector3.up * BoostThis);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainThruster.Play();
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