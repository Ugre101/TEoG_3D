using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameEvents.UI {
   public sealed class GameEventManger : MonoBehaviour {
      [SerializeField] Image image;

      GameBaseEvent currentEvent;
      public void StartEvent(GameBaseEvent baseEvent) {
         switch (baseEvent) 
         {
            case ImageEvent imageEvent:
               image.sprite = imageEvent.Image;
               break;
         
         }

         currentEvent = baseEvent;
      }

      public void InvokeEvent() {
         if (currentEvent != null)
            currentEvent.Invoke();
      }
   }
}
