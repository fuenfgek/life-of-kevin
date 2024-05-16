namespace Sopra.Logic.Achievements
{
    internal sealed class Achievement
    {
        public string Name { get; }
        public float TriggerValue { get; }
        public bool Achieved { get; set; }

        public Achievement(string name, float trigger)
        {
            Name = name;
            TriggerValue = trigger;
            Achieved = false;
        }
    }
}
