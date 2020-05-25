using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CodeMonkey;




public class Bird : MonoBehaviour
{
    public const float Jump_Amount = 5f;
    private Rigidbody2D BirdRigidbody2D;
    private Animator anim;

    public event EventHandler Ondied;
    public event EventHandler OnStartPlaying;
    public event EventHandler OnPauseMode;
    private State state;
    private enum State
    {
        WaitingToStart,
        Playing,
        Pause,
        Dead,
    }

    private static Bird instance;
    public static Bird GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        BirdRigidbody2D = GetComponent<Rigidbody2D>();
        anim  = GetComponent<Animator>();
        BirdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }

    // Update is called once per frame
    private void Update()
    {   
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    BirdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (OnStartPlaying != null) OnStartPlaying(this, EventArgs.Empty);
                    
                }                
                
                break;

            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                }
                else
                {
                   
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        state = State.Pause;
                        BirdRigidbody2D.bodyType = RigidbodyType2D.Static;
                        if (OnPauseMode != null) OnPauseMode(this, EventArgs.Empty);
                    }
                   

                }


                break;

            case State.Pause:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    BirdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    transform.eulerAngles = new Vector3(0, 0, BirdRigidbody2D.velocity.y * 2f);
                    if (OnStartPlaying != null) OnStartPlaying(this, EventArgs.Empty);
                }
               
                   
               
                break;
            case State.Dead:
               
                //SoundManager.PlaySound(SoundManager.Sound.Mute);
                break;
        }

        
    }

    private void Jump()
    {
        BirdRigidbody2D.velocity = Vector2.up * Jump_Amount;
        SoundManager.PlaySound(SoundManager.Sound.BirdJump);
        Debug.Log("GameHandler.JUMP " + state);
        anim.SetTrigger("Fly");
    }

    // détecter collision entre deux collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        state = State.Dead;
        BirdRigidbody2D.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        SoundManager.PlaySound(SoundManager.Sound.Mute);



        if (Ondied != null) Ondied(this, EventArgs.Empty);
    }

}
