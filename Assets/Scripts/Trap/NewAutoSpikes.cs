using UnityEngine;

public class NewAutoSpikes : MonoBehaviour
{
    public float activeDuration = 2f;

    Animator anim;
    AudioSource audioSource;
    int activeParamID = Animator.StringToHash("Active");
    float deactivationTime;
    bool playerInRange;
    bool trapActive;
    int playerLayer;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (trapActive && Time.time >= deactivationTime) //&& !playerInRange 
        {
            Invoke("ChangeValue", 1.5f);
            anim.SetBool(activeParamID, false);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            trapActive = true;
            anim.SetBool(activeParamID, true);
            audioSource.Play();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(trapActive == false && playerInRange)
        {
            ActiveTrap();
            Debug.Log("sss");
        }
    }

    void ActiveTrap()
    {
        deactivationTime = Time.time + activeDuration;
        trapActive = true;
        anim.SetBool(activeParamID, true);
        audioSource.Play();
    }

    void ChangeValue()
    {
        trapActive = false;
    }
}