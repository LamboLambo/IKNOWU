﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CaregiverIKNOWU.Models
{
    class Warning
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "isFamiliar")]
        public bool IsFamiliar { get; set; }

        [JsonProperty(PropertyName = "timeSpentSeconds")]
        public string TimeSpentSeconds { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "personId")]
        public string PersonId { get; set; }

        [JsonProperty(PropertyName = "patientId")]
        public string PatientId { get; set; }

        [JsonProperty(PropertyName = "isFinished")]
        public bool IsFinished { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; }








        public Warning()
        {

        }


    }//end class
}
