using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventSourcingTutorial.Events
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(StudentCreated), nameof(StudentCreated))]
    [JsonDerivedType(typeof(StudentUpdated), nameof(StudentUpdated))]
    [JsonDerivedType(typeof(StudentEnrolled), nameof(StudentEnrolled))]
    [JsonDerivedType(typeof(StudentUnEnrolled), nameof(StudentUnEnrolled))]
    public abstract class Event
    {
        public abstract Guid StreamId { get; }
        public DateTime CreatedAtUtc { get; set; }        
        [JsonPropertyName("pk")] public string Pk => StreamId.ToString();        
        [JsonPropertyName("sk")] public string Sk => CreatedAtUtc.ToString("yyyyMMddTHHmmss");
    }
}
