using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaregiverIKNOWU.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using CaregiverIKNOWU.Services;

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




        #region Get Methods

        public async static Task<ObservableCollection<Person>> GetPersonList(string patientId)
        {
            persons = await personTable
                .Where(personTable => personTable.Name != "stranger")
                .Where(personTable => personTable.PatientId == patientId).ToCollectionAsync();
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


        public async static Task<Person> GetOnePersonFromId(string personId)
        {
            persons = await personTable
                .Where(personTable => personTable.Id == personId).ToCollectionAsync();

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


        /// <summary>
        /// Get the Unfinished Warning List
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public async static Task<ObservableCollection<Warning>> GetUnfinishedWarningList(string patientId)
        {
            warnings = await warningTable
                .Where(warningTable => warningTable.IsFinished == false)
                .Where(warningTable => warningTable.PatientId == patientId)
                .OrderByDescending(warningTable => warningTable.CreatedAt)
                .ToCollectionAsync();

            ObservableCollection<Warning> unfinishedWarningList = new ObservableCollection<Warning>();
            foreach (Warning warning in warnings)
            {
                unfinishedWarningList.Add(warning);
            }
            return unfinishedWarningList;
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


        public async static Task<ObservableCollection<Face>> UploadFaceInfo(string personId, ObservableCollection<Face> addFaceList)
        {
            ObservableCollection<Face> newAddFaceList = new ObservableCollection<Face>();

            foreach (Face face in addFaceList)
            {
                JObject jo = new JObject();
                    jo.Add("id", face.Id);
                    jo.Add("imageAddress", face.ImageAddress);
                    jo.Add("imageToken", face.ImageToken);
                    jo.Add("isDefault", face.IsDefault);
                    jo.Add("personId", face.PersonId);
                    jo.Add("warningId", face.WarningId);
                try
                {
                    await faceTable.InsertAsync(jo);
                    //await faceTable.InsertAsync(face);
                }
                catch
                {
                    await faceTable.UpdateAsync(jo);
                    //await faceTable.UpdateAsync(face);
                }

                Face updatedFace = await UpdateFaceImageAddress(face);

                //Update Person's Default Image Icon
                if (face.IsDefault)
                {
                    await UpdatePersonDefaultImageAddress(personId, updatedFace);
                }

                //Add the face into newAddFaceList
                newAddFaceList.Add(updatedFace);

            }//end foreach

            return newAddFaceList;
        }


        public async static Task UpdateFaceIsDefault(string personId, ObservableCollection<Face> faces)
        {
            foreach (Face face in faces)
            {
                JObject jo = new JObject();
                jo.Add("id", face.Id);
                jo.Add("isDefault", face.IsDefault);
                jo.Add("personId", face.PersonId);
                await faceTable.UpdateAsync(jo);

                if (face.IsDefault == true)
                {
                    await UpdatePersonDefaultImageAddress(personId, face);
                }
            }
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

        public async static Task UpdatePersonInfoWithId(Person person)
        {
            JObject jo = new JObject();
            jo.Add("id", person.Id);
            jo.Add("name", person.Name);
            jo.Add("relation", person.Relation);
            jo.Add("isFamiliar", person.IsFamiliar);
            jo.Add("riskFactor", person.RiskFactor);
            await personTable.UpdateAsync(jo);
        }

        public async static Task UpdateWarningEnquiryWithId(Warning warning)
        {
            JObject jo = new JObject();
            jo.Add("id", warning.Id);
            jo.Add("isFinished", warning.IsFinished);
            jo.Add("isFamiliar", warning.IsFamiliar);
            await warningTable.UpdateAsync(jo);
        }



        #endregion


        #region Delete Methods
        public async static Task DeleteFace(Face face)
        {
            faces = await faceTable.ToCollectionAsync();
            JObject jo = new JObject();
            jo.Add("id", face.Id);
            await faceTable.DeleteAsync(jo);
        }

        public async static Task DeletePersonAndItsFaces(Person person)
        {
            //Delete its faces
            faces = await faceTable.Where(faceTable => faceTable.PersonId == person.Id).ToCollectionAsync();
            foreach (Face face in faces)
            {
                await faceTable.DeleteAsync(face);
            }

            //TODO: Delete blob images


            //Delete the person
            persons = await personTable.ToCollectionAsync();
            JObject jo = new JObject();
            jo.Add("id", person.Id);
            await personTable.DeleteAsync(jo);

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
