using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoulTrader.SceneSystem
{
    static public class ActiveScene
    {
        static public Scene Scene = null;
        static public SceneEditor Editor = null;

        static public Camera Camera = null;
        static public Player Player = null;
    }
}
