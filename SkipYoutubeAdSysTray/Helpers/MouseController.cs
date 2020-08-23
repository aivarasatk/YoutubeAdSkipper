using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipYoutubeAdSysTray.Helpers
{
    public class MouseController
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        private void SetMousePosition(int a, int b)
        {
            SetCursorPos(a, b);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

        private System.Drawing.Point GetCursorPosition()
        {
            GetCursorPos(out System.Drawing.Point lpPoint);
            return lpPoint;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        /// <summary>
        /// Simulates mouse left click on specified position and returns cursor to its original position.
        /// </summary>
        public void SimulateClick(System.Drawing.Point position)
        {
            var orgMousePosition = GetCursorPosition();

            SetMousePosition(position.X, position.Y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            SetMousePosition(orgMousePosition.X, orgMousePosition.Y);

        }
    }
}
