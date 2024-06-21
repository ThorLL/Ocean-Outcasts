namespace _Project.Scripts.Model
{
    public class Ship
    {
        public const float RESOURCE_MIN_VALUE = 0.0f;
        public const float RESOURCE_MAX_VALUE = 100.0f;
        
        public float Speed { set; get; }
        
        public float EngineIntegrity { set; get; }
        public float Oxygen { set; get; }
        public float Navigation { set; get; }
        public float Ammunition { set; get; }
        public float Altimeter { get; set; }
    }
}