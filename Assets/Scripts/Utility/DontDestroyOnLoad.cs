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
	/// Component that will ensure the parent GameObject is kept between scenes.
	/// </summary>

	public class DontDestroyOnLoad : MonoBehaviour
	{
		#region Fields

		[SerializeField, Tooltip("If enabled, this GameObject and its children will not be destroyed between scenes.")]
		private bool Enabled = true;

		#endregion



		#region Unity and Events

		private void Awake ()
		{
			if (Enabled) DontDestroyOnLoad(this.gameObject);
		}

		#endregion
	}
}
