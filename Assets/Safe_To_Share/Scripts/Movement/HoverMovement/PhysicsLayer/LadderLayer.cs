namespace Safe_To_Share.Scripts.Movement.HoverMovement.PhysicsLayer {
    public class LadderLayer : BaseLayer {
        public override void OnEnter(Movement mover) {
            //   mover.StartClimbing();
        }

        public override void OnExit(Movement mover) {
            // mover.StopClimbing();
        }

        public override void OnFixedUpdate(Movement movement) {
            // Maybe handle exit here? if dir is away
        }
    }
}