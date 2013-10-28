using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SoulTrader
{
    public class InputAction
    {
        private abstract class Input
        {
            protected enum InputState { DOWN, UP };
            private InputState oldState = InputState.UP;
            private InputState newState = InputState.UP;

            protected abstract bool GetDown();

            private InputState GetState()
            {
                if (GetDown())
                {
                    return InputState.DOWN;
                }
                return InputState.UP;
            }

            public void Update()
            {
                oldState = newState;
                newState = GetState();
            }

            public bool IsDown()
            {
                return newState == InputState.DOWN;
            }

            public bool IsUp()
            {
                return newState == InputState.UP;
            }

            public bool IsReleased()
            {
               return newState == InputState.UP && oldState == InputState.DOWN;
            }

            public bool IsPressed()
            {
                return newState == InputState.DOWN && oldState == InputState.UP;
            }
        }

        private class ButtonInput : Input
        {
            private Buttons button;
            public ButtonInput(Buttons button)
            {
                this.button = button;
            }

            protected override bool GetDown()
            {
                return GamePad.GetState(PlayerIndex.One).IsButtonDown(button);
            }
        }

        private class KeyInput : Input
        {
            private Keys key;

            public KeyInput(Keys key)
            {
                this.key = key;
            }

            protected override bool GetDown()
            {
                return Keyboard.GetState(PlayerIndex.One).IsKeyDown(key);
            }
        }

        private List<Input> inputList = new List<Input>();

        public void RegisterInput(Keys key)
        {
            inputList.Add(new KeyInput(key));
        }

        public void RegisterInput(Buttons button)
        {
            inputList.Add(new ButtonInput(button));
        }

        public bool IsDown()
        {
            foreach (Input input in inputList)
            {
                if (input.IsDown())
                {
                    return true;
                }
            }
            return false;
        }

        public bool isUp()
        {
            foreach (Input input in inputList)
            {
                if (input.IsUp())
                {
                    return true;
                }
            }
            return false;
        }

        public bool isPressed()
        {
            foreach (Input input in inputList)
            {
                if (input.IsPressed())
                {
                    return true;
                }
            }
            return false;
        }

        public bool isReleased()
        {
            foreach (Input input in inputList)
            {
                if (input.IsReleased())
                {
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            foreach (Input input in inputList)
            {
                input.Update();
            }
        }
    }
}
