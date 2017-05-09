// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;

namespace Chromatap
{
	/// <summary>
	/// Handles playing SFX and music.
	/// </summary>

	public class SoundPlayer : MonoBehaviour
	{
		#region Fields

		// Singleton
		private static SoundPlayer instance;



		[Header("Settings:")]
		[SerializeField, Tooltip("Speed with which the music will adjust volume after being enabled/disabled in options.")]
		private float musicLerpSpeed = 5f;

		[Header("Audio Source references:")]
		[SerializeField, Tooltip("Source for generic feedback one-shot sounds.")]
		private AudioSource genericSource;
		[SerializeField, Tooltip("Source for music and looping type sounds.")]
		private AudioSource musicSource;

		[Header("Sounds:")]
		[SerializeField, Tooltip("Sound played when the player completes a circle perfectly.")]
		private AudioClip soundPerfect;
		[SerializeField, Tooltip("Sound played when the player completes a circle well.")]
		private AudioClip soundGood;
		[SerializeField, Tooltip("Sound played when the player completes a circle poorly.")]
		private AudioClip soundBad;
		[SerializeField, Tooltip("Sound played when the player fails to complete a circle.")]
		private AudioClip soundFailed;
		[SerializeField, Tooltip("Sound played when the heart streak is broken.")]
		private AudioClip soundHeartbroken;
		[SerializeField, Tooltip("Sound played when the heart is full.")]
		private AudioClip soundHeartFull;

		[Header("Music:")]
		[SerializeField, Tooltip("Looped music clip to play in the background.")]
		private AudioClip backgroundMusic;

		#endregion



		#region Unity and Events

		private void Awake ()
		{
			if (instance == null)
				instance = this;
			else Debug.Log("SingletonError: Multiple SoundPlayer components detected in scene.", this);

			// Music source setup
			musicSource.clip = backgroundMusic;
			musicSource.Play();
		}



		private void Update ()
		{
			// Lerp/adjust music volume according to options
			if (Options.Sound && musicSource.volume != 1)
			{
				musicSource.volume = Mathf.Lerp(musicSource.volume, 1f, musicLerpSpeed * Time.deltaTime);
				if (musicSource.volume > 0.99f)
				{
					musicSource.volume = 1f;
				}
			}
			else if (!Options.Sound && musicSource.volume != 0)
			{
				musicSource.volume = Mathf.Lerp(musicSource.volume, 0f, musicLerpSpeed * Time.deltaTime);
				if (musicSource.volume < 0.01f)
				{
					musicSource.volume = 0f;
				}
			}
		}

		#endregion



		#region Private Methods

		/// <summary>
		/// Play a sound from a predefined list of Audioclips.
		/// </summary>
		private void PlaySFX (Sound sound)
		{
			// Don't play if the player has disabled sounds
			if (!Options.Sound) return;

			switch (sound)
			{
				case Sound.Perfect:
					genericSource.PlayOneShot(soundPerfect);
					break;
				case Sound.Good:
					genericSource.PlayOneShot(soundGood);
					break;
				case Sound.Bad:
					genericSource.PlayOneShot(soundBad);
					break;
				case Sound.Failed:
					genericSource.PlayOneShot(soundFailed);
					break;
				case Sound.Heartbroken:
					genericSource.PlayOneShot(soundHeartbroken);
					break;
				case Sound.HeartFull:
					genericSource.PlayOneShot(soundHeartFull);
					break;
				default:
					Debug.LogError("SoundPlayer tried to play a sound that does not exist in its Sound enum.", this);
					break;
			}
		}

		#endregion



		#region Public Static Methods

		/// <summary>
		/// Play a sound from a predefined list of Audioclips.
		/// </summary>
		public static void Play (Sound sound)
		{
			instance.PlaySFX(sound);
		}

		#endregion
	}

}