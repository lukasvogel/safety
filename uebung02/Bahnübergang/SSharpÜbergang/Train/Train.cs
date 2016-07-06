namespace SSharpÜbergang.Train
{
    using SafetySharp.Modeling;

    class Train : Component
    {

        const int START_POSITION = 0;
        const int END_POSITION = 120;

        const int MAX_SPEED = 5;

        [Range(START_POSITION, END_POSITION, OverflowBehavior.Clamp)]
        public int Position { get; private set; } = START_POSITION;

        [Range(0, MAX_SPEED, OverflowBehavior.Clamp)]
        public int Speed { get; private set; } = MAX_SPEED;

        public extern int Acceleration { get; }

        public override void Update()
        {
            Position += Speed;
            Speed += Acceleration;
        }
    }
}
