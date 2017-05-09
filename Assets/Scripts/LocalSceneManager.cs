// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chromatap
{
	/// <summary>
	/// Handles all scene management between the main menu and gameplay scene.
	/// </summary>

	public class LocalSceneManager : MonoBehaviour
	{
		#region Fields

		// Constants:
		private const string SCENENAME_MAIN_MENU = "mainmenu";
		private const string SCENENAME_GAMEPLAY = "gameplay";
		private const string SCENENAME_BACKGROUND = "background";


		// Static:
		public static bool hasLoadedStaticObjects = false;              // Has the empty scene with the static, background objects been loaded yet?

		#endregion



		#region Unity and Events

		private void Awake ()
		{
			SetupListeners();

			// These have to be loaded first thing no matter what scene is loaded into, always.
			Options.Load();
			PlayerScore.Load();

			if (!hasLoadedStaticObjects)
			{
				LoadStaticObjects();
				hasLoadedStaticObjects = true;
			}
		}



		private void OnDestroy ()
		{
			RemoveListeners();
		}



		private void SetupListeners ()
		{
			Events.onMainMenuRequest += OnMainMenuRequest;
			Events.onGameplaySceneRequest += OnGameplaySceneRequest;
		}



		private void RemoveListeners ()
		{
			Events.onMainMenuRequest -= OnMainMenuRequest;
			Events.onGameplaySceneRequest -= OnGameplaySceneRequest;
		}



		private void OnMainMenuRequest ()
		{
			SceneManager.LoadScene(SCENENAME_MAIN_MENU);
		}



		private void OnGameplaySceneRequest ()
		{
			SceneManager.LoadScene(SCENENAME_GAMEPLAY);
		}

		#endregion



		#region Private Methods

		/// <summary>
		/// Load the Background scene into current scene.
		/// </summary>
		private void LoadStaticObjects ()
		{
			SceneManager.LoadScene(SCENENAME_BACKGROUND, LoadSceneMode.Additive);
		}

		#endregion
	}

}