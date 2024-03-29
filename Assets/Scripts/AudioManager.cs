using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource attackAudio;
    [SerializeField] AudioSource dashAudio;
    [SerializeField] AudioSource jumpAudio;
    [SerializeField] AudioSource walkAudio;
    [SerializeField] AudioSource slamAudio;
    [SerializeField] AudioSource castAudio;
    [SerializeField] AudioSource lightAttackAudio;
    [SerializeField] AudioSource heavyAttackAudio;

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

    private void playWalkSound()
    {
        walkAudio.Play();
    }

    private void playSlamSound()
    {
        slamAudio.Play();
    }

    private void playCastSound()
    {
        castAudio.Play();
    }

    private void playHeavyAttackSound()
    {
        heavyAttackAudio.Play();
    }

    private void playLightAttackSound()
    {
        lightAttackAudio.Play();
    }
}
