using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;

namespace CaregiverIKNOWU.Models
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

        //Tempory Storage
        public BitmapImage Image { get; set; }









        public Person()
        {

        }

    }//end class
}
