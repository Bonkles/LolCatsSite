LolCatsSite
===========

A simple web site that loads/fetches images from a database. Demonstrates use of WebAPI, .NET C#, LINQ, RavenDB, and jQuery.


Created using Visual Studio 2010 SP1 using the MVC with SQL Database template. 
Tested with RavenDB Server build 960 and the Raven Lightweight client. 

Overview: 
A simple MVC App that imports images to a RavenDB Database, and then dynamically fetches them for display using jQuery. 

API Documentation: 
See /Help/Index.html for documentation created from the sources of this project. 

Highlights: 
* Linq Queries are featured prominently in the LolCatController.cs (Controllers/LolCatController.cs)
* The LolCat document template is located at Models/LolCat.cs
* The RavenDB server setup is governed by the file Core/RavenDB.cs
* The images that get stored in the DB are automatically loaded by the Core/ImageImporter.cs
* Index.cshtml contains some simple jQuery calls to write database data to the screen, and load the lolCat Images
 so the user can have a nice chuckle. 



To test:
1) Run a RavenDB Server executable as a separate instance. Tested with build 960. 
   (Make sure the server .exe is running in elevated mode)
2) Create a database on the server called 'lolCats'. 
3) Copy any directories filled with lolcat images into the LolCatsSite/lolcat_inbox directory. 
4) Open the Solution file (LolCatsSite.sln) and hit f5 to 'debug' it. 


Notes

Upon server startup, the following occurs: 
* The ImageImporter Recursively iterates over any directories that you placed inside lolcat_inbox, 
  isolating any .jpg, .gif, or .png files. 
* The ImageImporter then stores each file on the RavenDB server using the filename as an ID. 
  (The imageImporter is scrupulous about making sure no duplicate images are stored in the database, don't worry)
* Starts the server, which displays a single lolcat image with a search bar, search button, and 'random' button. 
  * Clicking the 'random' button will instruct the DB Server to fetch a random image URL from the DB and return it. 
  * jQuery then takes that image URL and dynamically updates the image source (see index.cshtml). 
  * Click the 'search' button won't do anything, unless the exact filename (Id) of an image is used for searching, 
    in which case the image is updated just as if the random button was clicked. 


Known Bugs: 
* Only 'happy path' testing has been performed. 
* Currently there is no way to set the Cuteness or Rating properties of the documents- though, there are working 
LINQ queries written to fetch lolcats of a minimum rating and/or Cuteness threshold. 



