using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace CaregiverIKNOWU.Models
{
    class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "isPatient")]
        public bool IsPatient { get; set; }

        [JsonProperty(PropertyName = "bindingUserId")]
        public string BindingUserId { get; set; }









    }//end class
}
