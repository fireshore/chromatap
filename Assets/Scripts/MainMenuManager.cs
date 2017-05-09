// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using UnityEngine.UI;
using System;

namespace Chromatap
{
	/// <summary>
	/// Handles the main menu UI interactions.
	/// </summary>

	public class MainMenuManager : MonoBehaviour
	{
		#region Fields

		// Exposed:
		[Header("UI References:")]
		[SerializeField, Tooltip("References to Main Menu UI elements.")]
		private MainMenuContent mainMenuUI;

		#endregion



		#region Unity and Events

		private void Start ()
		{
			// Options should already have been loaded, but make sure.
			Options.Load();

			ShowMainMenu();
		}



		private void Update ()
		{
			// Check Android back button input.
			if (Input.GetKey(KeyCode.Escape))
			{
				// If credits panel is up, hide it.
				if (mainMenuUI.CreditsPanel.activeInHierarchy)
				{
					ShowMainMenu();
				}
				// Otherwise, exit the game.
				else
				{
					Application.Quit();
				}
			}
		}

		#endregion



		#region Private Methods

		/// <summary>
		/// Shows the main menu screen, ensures everything is set correctly.
		/// </summary>
		private void ShowMainMenu ()
		{
			mainMenuUI.Panel.SetActive(true);
			mainMenuUI.CreditsPanel.SetActive(false);
			if (PlayerScore.Highscore > 0)
			{
				mainMenuUI.HighscoreText.text = PlayerScore.Highscore.ToString();
				mainMenuUI.HighscoreText.gameObject.SetActive(true);
			}
			else
			{
				mainMenuUI.HighscoreText.gameObject.SetActive(false);
			}

			UpdateOptionButtons();
		}



		/// <summary>
		/// Enables/disables option buttons' images on the main menu screen according to current options.
		/// </summary>
		private void UpdateOptionButtons ()
		{
			if (Options.Sound)
			{
				mainMenuUI.SoundEnabledImage.SetActive(true);
				mainMenuUI.SoundDisabledImage.SetActive(false);
			}
			else
			{
				mainMenuUI.SoundEnabledImage.SetActive(false);
				mainMenuUI.SoundDisabledImage.SetActive(true);
			}

			if (Options.Vibration)
			{
				mainMenuUI.VibrationEnabledImage.SetActive(true);
				mainMenuUI.VibrationDisabledImage.SetActive(false);
			}
			else
			{
				mainMenuUI.VibrationEnabledImage.SetActive(false);
				mainMenuUI.VibrationDisabledImage.SetActive(true);
			}
		}

		#endregion



		#region UI Button Methods -- Do not call from code

		public void Button_Start ()
		{
			Events.GameplaySceneRequest();
		}



		public void Button_ToggleCredits ()
		{
			if (mainMenuUI.Panel.activeInHierarchy)
			{
				mainMenuUI.Panel.SetActive(false);
				mainMenuUI.CreditsPanel.SetActive(true);
			}
			else
			{
				ShowMainMenu();
			}
		}



		public void Button_ToggleSound ()
		{
			Options.Sound = !Options.Sound;
			Options.Save();

			UpdateOptionButtons();
		}



		public void Button_ToggleVibration ()
		{
			Options.Vibration = !Options.Vibration;
			Options.Save();

			UpdateOptionButtons();
		}

		#endregion



		#region Utility Classes

		[Serializable]
		public class MainMenuContent
		{
			[Header("Objects:")]
			[Tooltip("Parent panel for all Main Menu content.")]
			public GameObject Panel;
			[Tooltip("Parent panel for the Credits content.")]
			public GameObject CreditsPanel;

			[Header("Images:")]
			[Tooltip("Image shown when sound is enabled.")]
			public GameObject SoundEnabledImage;
			[Tooltip("Image shown when sound is disabled.")]
			public GameObject SoundDisabledImage;
			[Tooltip("Image shown when vibration is enabled.")]
			public GameObject VibrationEnabledImage;
			[Tooltip("Image shown when vibration is disabled.")]
			public GameObject VibrationDisabledImage;

			[Header("Texts:")]
			[Tooltip("Text component that shows the player's best highscore, if one exists.")]
			public Text HighscoreText;
		}

		#endregion
	}

}