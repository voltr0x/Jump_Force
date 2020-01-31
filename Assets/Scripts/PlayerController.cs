using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtSplatter;

    public AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    private float jumpForce = 15;
    private float gravityMod = 3;
    private bool isOnGround = true;
    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        //Amount of gravity
        Physics.gravity *= gravityMod;
    }    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            //Jump trigger
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            dirtSplatter.Stop();
            playerAudio.PlayOneShot(jumpSound);

            //Jump animation
            playerAnim.SetTrigger("Jump_trig");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtSplatter.Play();
        }else if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Death animation
            playerAnim.SetInteger("DeathType_int", 1);
            playerAnim.SetBool("Death_b", true);

            //Explosion effect
            explosionParticle.Play();
            dirtSplatter.Stop();
            playerAudio.PlayOneShot(crashSound);

            //Console message
            Debug.Log("GAME OVER!");
            gameOver = true;
        }
    }
}
