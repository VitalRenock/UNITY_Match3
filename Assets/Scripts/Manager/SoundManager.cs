using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SoundManager : MonoBehaviour
{
    #region Variables

    private AudioSource _audioSource;
    public AudioClip _inGameMusic;
    public AudioClip _selectPieceSound;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.clip = _inGameMusic;
        _audioSource.Play();
        FindObjectOfType<GameplayManager>().EventOnSoundTrigger += PlaySound;
    }

    #endregion

    #region Custom Methods

    private void PlaySound(object sender, GameplayManager.OnSoundTriggerEventArgs e)
    {
        switch (e.sound)
        {
            case "Selected":
                _audioSource.PlayOneShot(_selectPieceSound);
                break;
            default:
                break;
        }
    }

    #endregion
}
