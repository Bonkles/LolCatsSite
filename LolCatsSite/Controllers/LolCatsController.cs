using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcTestApp.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using DataCore;
using DataImporters; 

namespace MvcTestApp.Controllers
{
    /// <summary>
    /// This controller governs the lolCat database operations. 
    /// </summary>
    public class LolCatsController : ApiController
    {
        RavenDB lolCatsDB;
        //
        // GET: /lolCats/

        /// <summary>
        /// Constructor - creates a new RavenDB instance, and connects to the server. 
        /// </summary>
        public LolCatsController()
        {
            lolCatsDB = new RavenDB();
            lolCatsDB.Connect(); //First, connect to the database. Kinda important. 
            //Create a new ImageImporter. We'll use this to add any new pictures to the database. 
            DataImporters.ImageImporter imgImporter = new DataImporters.ImageImporter();

            //Consult the image importer. Do we have any new lolCats in our inbox? 
            imgImporter.lookThroughInbox();

            //Supposing that we DO have new pictures to import... 
            if (imgImporter.NewPics)
            {
                //Time to create some new database images! 
                //Establish a session with the database, so that we can do useful
                //things with it. 
                using (var session = lolCatsDB.DocStore.OpenSession())
                {
                    //Now, we want to create one DB entry for each lolCat in our inbox. 
                    foreach (string imgPathString in imgImporter.LolCatFilePaths)
                    {
                        //Check to see if we already have a LolCat of this filename in the database. 
                        //If it exists already, we should not add it to the database. 
                        var lolCatExists = session.Load<LolCat>(LolCat.fileNameFromFullPath(imgPathString));

                        //If the document does not exist- then we can add it safely! 
                        if (lolCatExists == null)
                        {
                            var lolCatEntity = new LolCat(imgPathString);
                            session.Store(lolCatEntity);
                        }
                    }

                    //Be sure to save changes outside the foreach, so that the DB can cache all the add item requests. 
                    //Hooray for Performance! 
                    session.SaveChanges();
                }

            }


            
        }


        //This method gets all of the lolCats. What else do you need to know? 
        /// <summary>
        /// This method is run when the server is queried with the following string: 
        /// /api/lolCats/
        /// </summary>
        /// <returns>An IEnumerable of LolCats that it received from the database.</returns>
        public IEnumerable<LolCat> GetAllLolCats()
        {
            using (var session = lolCatsDB.DocStore.OpenSession())
            {
                //Use some LINQ, grab some lolCats. 
                var lolCatResults = from lolCat in session.Query<LolCat>()
                                    select lolCat;

                //Supposing we didn't actually find any lolCats- a travesty, I know- we should 404 them! 
                AssertNonEmptyCollection(lolCatResults);

                return lolCatResults;
            }
        }


        /// <summary>
        /// This API obtains a lolCat at random from the database. Note that it does NOT fetch the entire DB and 
        /// then select from that collection- RavenDB performs the randomization for us. 
        /// </summary>
        /// <param name="Random">Unused parameter (for now).</param>
        /// <returns>A random lolCat from the database.</returns>
        public LolCat GetlolCatByRandom(int Random)
        {
            using (var session = lolCatsDB.DocStore.OpenSession())
            {
                //Tell the DB to grab a random lolCat. MAKE SURE that the database doesn't return every single element! 
                var lolCatResults = session.Query<LolCat>().Customize(x => x.RandomOrdering()).Take(1);
                AssertNonEmptyCollection(lolCatResults); 
                return lolCatResults.FirstOrDefault();
            }
        }

        /// <summary>
        /// Searches the database for a lolCat of the given Id (fileName). 
        /// </summary>
        /// <param name="Id">The fileName for the lolCat in question.</param>
        /// <returns>the lolCat, if found.</returns>
        public LolCat GetlolCatById(string Id)
        {
            using (var session = lolCatsDB.DocStore.OpenSession())
            {
             var lolCatResults = from lolCat in session.Query<LolCat>()
                                 where lolCat.Id.Equals(Id)
                                 select lolCat;

             AssertNonEmptyCollection(lolCatResults); 

             return lolCatResults.FirstOrDefault(); 
            }      
        }
        
        /// <summary>
        /// Allow our users to search by rating threshold.
        /// </summary>
        /// <param name="Rating">The minimum rating that the lolCat must achieve.</param>
        /// <returns>An IEnumerable of lolCats that meet the minimum Rating.</returns>
        public IEnumerable<LolCat> GetlolCatsByRating(decimal Rating)
        {
            using (var session = lolCatsDB.DocStore.OpenSession())
            {
                var lolCatResults = from lolCat in session.Query<LolCat>()
                                    where lolCat.Rating > Rating
                                    select lolCat;
                AssertNonEmptyCollection(lolCatResults); 
                return lolCatResults;
            }
        }
        
        /// <summary>
        /// Allow our users to search by Cuteness threshold.  
        /// </summary>
        /// <param name="Cuteness">The minimum cuteness criteria for any lolCats returned.</param>
        /// <returns>An IEnumerable of LolCats that meet or exceed the Cuteness parameter.</returns>
        public IEnumerable<LolCat> GetlolCatsByCuteness(decimal Cuteness)
        {
            using (var session = lolCatsDB.DocStore.OpenSession())
            {
                var lolCatResults = from lolCat in session.Query<LolCat>()
                                    where lolCat.Cuteness >= Cuteness
                                    select lolCat;
                AssertNonEmptyCollection(lolCatResults);
                return lolCatResults;
            }
        }

        //Not an action method. 
        private void AssertNonEmptyCollection(IEnumerable<LolCat> lolCatResults)
        {
                if (lolCatResults == null)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                    throw new HttpResponseException(resp);
                }
        }
    }
}
