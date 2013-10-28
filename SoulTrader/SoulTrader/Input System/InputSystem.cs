using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SoulTrader.Input_System
{
    public static class InputSystem
    {
        public static InputAction RunRight = new InputAction();
        public static InputAction RunLeft = new InputAction();

        public static InputAction Action1 = new InputAction();
        public static InputAction Action2 = new InputAction();
        public static InputAction Action3 = new InputAction();
        public static InputAction Action4 = new InputAction();

        public static InputAction EnableEditor = new InputAction();

        private static List<InputAction> actions = new List<InputAction>();

        public static void Initialize()
        {
            actions.Add(RunRight);
            RunRight.RegisterInput(Keys.D);
            RunRight.RegisterInput(Buttons.DPadRight);
            RunRight.RegisterInput(Buttons.LeftThumbstickRight);

            actions.Add(RunLeft);
            RunLeft.RegisterInput(Keys.A);
            RunLeft.RegisterInput(Buttons.DPadLeft);
            RunLeft.RegisterInput(Buttons.LeftThumbstickLeft);

            actions.Add(Action1);
            Action1.RegisterInput(Keys.Space);
            Action1.RegisterInput(Buttons.A);

            actions.Add(Action2);
            Action2.RegisterInput(Keys.J);
            Action2.RegisterInput(Buttons.X);

            actions.Add(Action3);
            Action3.RegisterInput(Keys.I);
            Action3.RegisterInput(Buttons.Y);

            actions.Add(Action4);
            Action4.RegisterInput(Keys.L);
            Action4.RegisterInput(Buttons.B);

            actions.Add(EnableEditor);
            EnableEditor.RegisterInput(Keys.OemTilde);
        }

        static public void Update()
        {
            foreach (InputAction input in actions)
            {
                input.Update();
            }
        }
    }
}
