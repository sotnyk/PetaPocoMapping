namespace Models
{
    public enum EventTypes
    {
        PatientActivated,
        PatientDeactivated,
    };

    public class JsonEnumMap
    {
        public long Id { get; set; }

        public string Jsonb { get; set; }

        public EventTypes Event { get; set; }
    }
}
