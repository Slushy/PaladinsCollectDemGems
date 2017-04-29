using PaladinsCollectDemGems.tools.native;
using System.Drawing;
using System.Windows.Forms;
using MouseEvent = PaladinsCollectDemGems.tools.native.WinCursorInterop.MouseEvent;

namespace PaladinsCollectDemGems.tools
{
	/// <summary>
	/// A virtual mouse which delegates all actions to the desktop mouse
	/// </summary>
	public static class Mouse
	{
		/// <summary>
		/// Performs a left click at the current cursor position
		/// </summary>
		public static void LeftClick()
		{
			Point currPos = GetMousePosition();
			LeftClick(currPos.X, currPos.Y);
		}

		/// <summary>
		/// Performs a left click at the specified cursor position
		/// </summary>
		/// <param name="xPos">x position in screen coordinates</param>
		/// <param name="yPos">y position in screen coordinates</param>
		/// <param name="retainClickPosition">determines if the cursor should stay at the click position, or reset to the starting before the click</param>
		public static void LeftClick(int xPos, int yPos, bool retainClickPosition = false)
		{
			ExecuteMouseEvents(xPos, yPos, retainClickPosition, MouseEvent.LeftClickDown, MouseEvent.LeftClickUp);
		}

		/// <summary>
		/// Performs a right click at the current cursor position
		/// </summary>
		public static void RightClick()
		{
			Point currPos = GetMousePosition();
			RightClick(currPos.X, currPos.Y);
		}

		/// <summary>
		/// Performs a right click at the specified cursor position
		/// </summary>
		/// <param name="xPos">x position in screen coordinates</param>
		/// <param name="yPos">y position in screen coordinates</param>
		/// <param name="retainClickPosition">determines if the cursor should stay at the click position, or reset to the starting before the click</param>
		public static void RightClick(int xPos, int yPos, bool retainClickPosition = false)
		{
			ExecuteMouseEvents(xPos, yPos, retainClickPosition, MouseEvent.RightClickDown, MouseEvent.RightClickUp);
		}

		/// <summary>
		/// Moves the mouse position to the specified xy position in screen coordinates
		/// </summary>
		/// <param name="xPos">x position in screen coordinates</param>
		/// <param name="yPos">y position in screen coordinates</param>
		public static void MoveTo(int xPos, int yPos)
		{
			WinCursorInterop.SetCursorPosition(xPos, yPos);
		}

		/// <summary>
		/// Retrieves the current mouse position
		/// </summary>
		/// <returns>Point struct containing current x-y cursor position in screen coordinates</returns>
		public static Point GetMousePosition()
		{
			return Control.MousePosition;
		}

		/// <summary>
		/// Executes a set of mouse events at a specific position on the screen
		/// </summary>
		/// <param name="xPos">x position in screen coordinates</param>
		/// <param name="yPos">y position in screen coordinates</param>
		/// <param name="retainNewPosition">determines if the cursor should stay at the event position, or reset to the starting before the events</param>
		/// <param name="mouseEvents">the mouse events to execute</param>
		private static void ExecuteMouseEvents(int xPos, int yPos, bool retainNewPosition, params MouseEvent[] mouseEvents)
		{
			Point currPos = GetMousePosition();

			// First move the cursor (must be at the position before executing the events)
			MoveTo(xPos, yPos);

			// Send all of the mouse events
			foreach (MouseEvent mEvent in mouseEvents) {
				WinCursorInterop.SendMouseEvent(mEvent, xPos, yPos);
			}

			if (!retainNewPosition)
			{
				// Reset to original mouse position before the events
				MoveTo(currPos.X, currPos.Y);
			}
		}
	}
}
