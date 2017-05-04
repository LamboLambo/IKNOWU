using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;

namespace PatientIKNOWU.Models
{
    class Person
    {
        //Database Storage
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "relation")]
        public string Relation { get; set; }

        [JsonProperty(PropertyName = "isFamiliar")]
        public bool IsFamiliar { get; set; }

        [JsonProperty(PropertyName = "patientId")]
        public string PatientId { get; set; }

        [JsonProperty(PropertyName = "defaultImageAddress")]
        public string DefaultImageAddress { get; set; }

        [JsonProperty(PropertyName = "riskFactor")]
        public int RiskFactor { get; set; }


        //Tempory Storage
        public BitmapImage DefaultIcon { get; set; }









        public Person()
        {

        }

    }//end class
}
