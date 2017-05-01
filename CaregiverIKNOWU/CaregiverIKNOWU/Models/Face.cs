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
    class Face
    {
        //Database Storage
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "imageAddress")]
        public string ImageAddress { get; set; }

        [JsonProperty(PropertyName = "personId")]
        public string PersonId { get; set; }

        [JsonProperty(PropertyName = "warningId")]
        public string WarningId { get; set; }

        //Tempory Storage
        public BitmapImage Image { get; set; }





        public Face()
        {

        }

    }//end face
}
