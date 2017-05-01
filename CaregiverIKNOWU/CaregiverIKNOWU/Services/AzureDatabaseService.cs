using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaregiverIKNOWU.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;


namespace CaregiverIKNOWU.Services
{
    class AzureDatabaseService
    {
        #region Attributes
        //Preparing for Uploading data 
        private static IMobileServiceTable<Face> faceTable = App.MobileService.GetTable<Face>();
        private static IMobileServiceTable<Person> personTable = App.MobileService.GetTable<Person>();
        private static IMobileServiceTable<Warning> warningTable = App.MobileService.GetTable<Warning>();

        //Preparing for Loading data 
        public static MobileServiceCollection<Face, Face> faces;
        public static MobileServiceCollection<Person, Person> persons;
        public static MobileServiceCollection<Warning, Warning> warnings;

        #endregion


        #region Upload Methods


        #endregion


        #region Update Methods


        #endregion


        #region Delete Methods


        #endregion






    }//end class
}
