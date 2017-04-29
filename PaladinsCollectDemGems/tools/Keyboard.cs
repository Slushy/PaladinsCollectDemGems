using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace PaladinsCollectDemGems.tools
{
	/// <summary>
	/// A virtual keyboard which allows you to send key events to the active window
	/// </summary>
	public static class Keyboard
	{
		#region Special Key Construction

		#region Enum Definitions

		/// <summary>
		/// Represents non-alphanumeric characters on the keyboard
		/// </summary>
		public enum SpecialKey {
			Backspace,
			Break,
			CapsLock,
			Delete,
			Down,
			End,
			Enter,
			Esc,
			Help,
			Home,
			Insert,
			Left,
			NumLock,
			[Description("PGDN")]
			PageDown,
			[Description("PGUP")]
			PageUp,
			[Description("PRTSC")]
			PrintScreen,
			Right,
			ScrollLock,
			Tab,
			Up,
			F1,
			F2,
			F3,
			F4,
			F5,
			F6,
			F7,
			F8,
			F9,
			F10,
			F11,
			F12,
			F13,
			F14,
			F15,
			F16,
			Add,
			Subtract,
			Multiply,
			Divide
			
		}

		/// <summary>
		/// Represents keyboard key mods that you can apply with other characters
		/// </summary>
		public enum SpecialKeyMod {
			[Description("+")]
			Shift,
			[Description("^")]
			Ctrl,
			[Description("%")]
			Alt
		}

		#endregion

		// holds the string representation of keyboard special keys
		private static Dictionary<SpecialKey, string> _specialKeyValues = new Dictionary<SpecialKey, string>();
		private static Dictionary<SpecialKeyMod, string> _specialKeyModValues = new Dictionary<SpecialKeyMod, string>();

		// Instantiated immediately upon class reference
		static Keyboard() {
			BuildKeyDictionary();
		}

		// Initializes the string representations of our non-alphanumeric keys into our mappings
		private static void BuildKeyDictionary()
		{
			foreach (SpecialKey key in Enum.GetValues(typeof(SpecialKey)))
				_specialKeyValues.Add(key, GetEnumStringRepresentation(key));
			
			foreach (SpecialKeyMod key in Enum.GetValues(typeof(SpecialKeyMod)))
				_specialKeyModValues.Add(key, GetEnumStringRepresentation(key));
		}

		// Helper that returns the string representation of the enum key
		private static string GetEnumStringRepresentation<T>(T key)
		{
			var fieldInfo = key.GetType().GetField(key.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			// Use the text from the "Description" field if the enum value has it, 
			// else just use its name
			string keyValue = string.Empty;
			if (attributes != null && attributes.Length > 0)
				keyValue = attributes[0].Description;
			else
				keyValue = Enum.GetName(typeof(T), key);
			
			return string.Format("{{{0}}}", keyValue).ToUpper();
		}

		#endregion

		/// <summary>
		/// Types the text into the keyboard character by character
		/// </summary>
		/// <param name="text">the text you want to send to the active window</param>
		public static void Type(string text)
		{
			SendKeys.SendWait(text);
		}

		/// <summary>
		/// Types the keyboard key represented by the special key passed in
		/// </summary>
		/// <param name="key">the special key to send to the active window</param>
		public static void Type(SpecialKey key) {
			Type(_specialKeyValues[key]);
		}

		/// <summary>
		/// Holds down a key mod (shift, alt, ctrl) at the same time pressing a set of characters on the keyboard
		/// </summary>
		/// <param name="keyMod">the key mod to hold down (shift, alt, ctrl)</param>
		/// <param name="letters">the letters to send to the keyboard with the key mod</param>
		public static void Type(SpecialKeyMod keyMod, params char[] letters) {
			Type(string.Format("{0}({1})", _specialKeyModValues[keyMod], letters.ToString()));
		}

		/// <summary>
		/// Holds down multiple key mods (shift, alt, ctrl) at the same time as pressing a character on the keyboard
		/// </summary>
		/// <param name="letter">the letter to send to the keyboard with the key mods held</param>
		/// <param name="keyMods">the key mods to hold down (shift, alt, ctrl)</param>
		public static void Type(char letter, params SpecialKeyMod[] keyMods) {
			string mods = string.Empty;
			foreach (SpecialKeyMod keyMod in keyMods)
				mods += _specialKeyModValues[keyMod];

			Type(string.Format("{0}{1}", mods, letter));
		}

	}
}
