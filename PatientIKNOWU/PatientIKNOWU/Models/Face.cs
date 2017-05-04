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
    class Face
    {
        //Database Storage
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "imageAddress")]
        public string ImageAddress { get; set; }

        [JsonProperty(PropertyName = "imageToken")]
        public string ImageToken { get; set; }

        [JsonProperty(PropertyName = "isDefault")]
        public bool IsDefault { get; set; } = false;

        [JsonProperty(PropertyName = "personId")]
        public string PersonId { get; set; }

        [JsonProperty(PropertyName = "warningId")]
        public string WarningId { get; set; }

        //Tempory Storage
        public BitmapImage Image { get; set; } = null;





        public Face()
        {
            
        }

    }//end face
}
