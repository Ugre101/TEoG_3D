using UnityEngine.InputSystem;

namespace InputSystems
{
    public class IInputNotOverUI : IInputInteraction
    {
        public void Process(ref InputInteractionContext context)
        {
            if (context.timerHasExpired)
            {
                context.Canceled();
                return;
            }
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}
