using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PkMn.Game
{
    public static class InputManager
    {
        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;
        private static GamePadState currentPadState;
        private static GamePadState previousPadState;
        
        public static bool KeyPressed(Keys key)
        {
            if (currentKeyState != null && previousKeyState != null)
                if (previousKeyState.IsKeyUp(key) && currentKeyState.IsKeyDown(key))
                    return true;

            return false;
        }

        public static bool ButtonPressed(Buttons button)
        {
            if (currentPadState != null && previousPadState != null)
                if (previousPadState.IsButtonUp(button) && currentPadState.IsButtonDown(button))
                    return true;

            return false;
        }

        public static void Update()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            previousPadState = currentPadState;
            currentPadState = GamePad.GetState(PlayerIndex.One);
        }

    }
}
