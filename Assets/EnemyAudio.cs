using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] AudioSource attackAudio;
    [SerializeField] AudioSource dashAudio;
    [SerializeField] AudioSource jumpAudio;

    private void playDashSound()
    {
        dashAudio.Play();
    }

    private void playAttackSound()
    {
        attackAudio.Play();
    }

    private void playJumpSound()
    {
        jumpAudio.Play();
    }
}
