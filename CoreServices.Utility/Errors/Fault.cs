using CoreServices.Utility.Faults;
using FluentValidation.Results;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CoreServices.Utility.Errors
{
    public class Fault
    {
        public bool ErrorOccured { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(EmitDefaultValue = false)]
        public string FaultDescription { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        public Faultkey Faultkey { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(EmitDefaultValue = false)]
        public string Details { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> ValidationErrors { get; set; }

        public Fault()
        {

        }

        public Fault(string message)
        {
            FaultDescription = message;
            ErrorOccured = true;

        }

        public Fault(IEnumerable<ValidationFailure> errors)
        {
            if (errors == null) return;
            ErrorOccured = true;
            ValidationErrors = errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
        }
    }
}
