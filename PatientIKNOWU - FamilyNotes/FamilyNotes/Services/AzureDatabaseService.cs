using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyNotes.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;

namespace FamilyNotes.Services
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




        #region Get Methods

        public async static Task<ObservableCollection<Person>> GetPersonList(string patientId)
        {
            persons = await personTable.Where(personTable => personTable.PatientId == patientId).ToCollectionAsync();
            ObservableCollection<Person> personList = new ObservableCollection<Person>();
            foreach (Person person in persons)
            {
                if (person.DefaultImageAddress != null)
                {
                    person.DefaultIcon = await AzureBlobService.DisplayImageFile(person.DefaultImageAddress);
                }
                else
                {
                    person.DefaultIcon = MainPage.nullBitmapImage;
                }

                personList.Add(person);
            }
            return personList;
        }

        public async static Task<Person> GetPersonFromNameAndRelation(string personName, string personRelation, string patientId)
        {
            persons = await personTable
                .Where(personTable => personTable.PatientId == patientId)
                .Where(personTable => personTable.Name == personName)
                .Where(personTable => personTable.Relation == personRelation).ToCollectionAsync();

            if (persons.Count == 0)
            {
                return null;
            }
            else
            {
                return persons[0];
            }
        }

        public async static Task<ObservableCollection<Face>> GetFaceList(string personId)
        {
            faces = await faceTable.Where(faceTable => faceTable.PersonId == personId).ToCollectionAsync();
            ObservableCollection<Face> faceList = new ObservableCollection<Face>();
            faceList = faces;
            return faceList;
        }

        public async static Task<Face> GetFaceFromImageTokenAndPersonId(string imageToken, string personId)
        {
            faces = await faceTable
                .Where(faceTable => faceTable.PersonId == personId)
                .Where(faceTable => faceTable.ImageToken == imageToken).ToCollectionAsync();
            return faces[0];
        }



        #endregion



        #region Upload Methods

        /// <summary>
        /// Upload Person Information
        /// </summary>
        /// <param name="person"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public async static Task UploadPersonInfo(Person person)
        {
            persons = await personTable
                .Where(personTable => personTable.PatientId == person.PatientId)
                .Where(personTable => personTable.Id == person.Id).ToCollectionAsync();

            if (persons.Count > 0)
            {
                await personTable.UpdateAsync(person);
            }
            else
            {
                await personTable.InsertAsync(person);

            }
        }


        public async static Task UploadFaceInfo(string personId, ObservableCollection<Face> addFaceList)
        {
            foreach (Face face in addFaceList)
            {
                JObject jo = new JObject();
                try
                {
                    jo.Add("id", face.Id);
                    jo.Add("imageAddress", face.ImageAddress);
                    jo.Add("imageToken", face.ImageToken);
                    jo.Add("isDefault", face.IsDefault);
                    jo.Add("personId", face.PersonId);
                    jo.Add("warningId", face.WarningId);
                    await faceTable.InsertAsync(jo);
                    //await faceTable.InsertAsync(face);
                }
                catch
                {
                    jo.Add("id", face.Id);
                    jo.Add("imageAddress", face.ImageAddress);
                    jo.Add("imageToken", face.ImageToken);
                    jo.Add("isDefault", face.IsDefault);
                    jo.Add("personId", face.PersonId);
                    jo.Add("warningId", face.WarningId);
                    await faceTable.UpdateAsync(jo);
                    //await faceTable.UpdateAsync(face);
                }

                Face updatedFace = await UpdateFaceImageAddress(face);

                //Update Person's Default Image Icon
                if (face.IsDefault)
                {
                    await UpdatePersonDefaultImageAddress(personId, updatedFace);
                }

            }//end foreach
        }






        #endregion


        #region Update Methods

        public async static Task<Face> UpdateFaceImageAddress(Face face)
        {
            Face updatedFace = await GetFaceFromImageTokenAndPersonId(face.ImageToken, face.PersonId);
            await AzureBlobService.UploadImageFromImageToken(face.ImageToken, updatedFace.Id + ".jpg");
            JObject jo = new JObject();
            jo.Add("id", updatedFace.Id);
            jo.Add("imageAddress", AzureBlobService.GetImageAddress(updatedFace.Id + ".jpg"));
            //updatedFace.ImageAddress = AzureBlobService.GetImageAddress(updatedFace.Id + ".jpg");
            await faceTable.UpdateAsync(jo);
            return updatedFace;
        }

        public async static Task UpdatePersonDefaultImageAddress(string personId, Face face)
        {
            JObject jo = new JObject();
            jo.Add("id", personId);
            jo.Add("defaultImageAddress", AzureBlobService.GetImageAddress(face.Id + ".jpg"));
            await personTable.UpdateAsync(jo);
        }



        #endregion


        #region Delete Methods
        public async static Task DeleteFace(Face face)
        {
            JObject jo = new JObject();
            jo.Add("id", face.Id);
            await faceTable.DeleteAsync(jo);
        }




        #endregion


        #region isEmpty

        public async static Task<bool> isPersonTableEmpty(string patientId)
        {
            persons = await personTable.Where(personTable => personTable.PatientId == patientId).ToCollectionAsync();

            if (persons.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async static Task<bool> isFaceTableEmpty(string personId)
        {
            faces = await faceTable.Where(faceTable => faceTable.PersonId == personId).ToCollectionAsync();

            if (faces.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        #endregion




    }//end class
}
