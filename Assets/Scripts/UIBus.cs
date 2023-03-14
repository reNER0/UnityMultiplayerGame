using System;

namespace Assets.Scripts
{
    public static class UIBus
    {
        public static Action<bool> OnCounterShown;
        public static Action<int> OnCounterUpdate;
    }
}
