// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using System.Collections.Generic;
using System;

namespace Chromatap
{
	/// <summary>
	/// ScriptableObject for storing text strings used for hints.
	/// </summary>

	[CreateAssetMenu(fileName = "Hints_", menuName = "/Scriptables/Hints")]
	public class HintsContainer : ScriptableObject
	{
		#region Fields

		// Exposed:
		[Header("Add exactly one of each hint!")]
		[Header("(Excluding 0 = None)")]
		[SerializeField, Tooltip("List of Hint-types and corresponding strings.")]
		private List<HintCombo> Hints = new List<HintCombo>();

		#endregion



		#region Public Methods

		/// <summary>
		/// Given an Hint type, returns the string text for that hint.
		/// </summary>
		public string GetHintText (Hint hint)
		{
			return Hints.Find(x => x.Hint == hint).String;
		}



		/// <summary>
		/// Returns a random hint string.
		/// </summary>
		public string GetRandomHintText ()
		{
			int i = UnityEngine.Random.Range(1, Enum.GetNames(typeof(Hint)).Length);    // Excluding 0 = Hint.None
			return GetHintText((Hint) i);
		}

		#endregion



		#region Utility Classes

		[Serializable]
		private struct HintCombo
		{
			public Hint Hint;
			[TextArea]
			public string String;
		}

		#endregion
	}

}