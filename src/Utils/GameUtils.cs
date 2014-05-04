using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pathfinder
{
    public static class GameUtils
    {
        private static GameServiceContainer container;
        public static GameServiceContainer Instance
        {
            get
            {
                if (container == null)
                    container = new GameServiceContainer();
                return container;
            }
        }

        public static T GetUtil<T>()
        {
            return (T)Instance.GetService(typeof(T));
        }

        public static void AddUtil<T>(T Util)
        {
            Instance.AddService(typeof(T), Util);
        }

        public static void RemoveUtil<T>()
        {
            Instance.RemoveService(typeof(T));
        }
    }
}
