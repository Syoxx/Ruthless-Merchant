namespace RuthlessMerchant
{
    public class Warrior : Fighter
    {
        public override void Start()
        {
            base.Start();
            if (PatrolActive)
            {
                PatrolActive = false;
                Patrol();
            }
        }
    }
}
