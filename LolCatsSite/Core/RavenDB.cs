using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client;

namespace DataCore
{

    /// <summary>
    /// The DataCore namespace contains the database configuration and connection 
    /// information that will be used by the site. If the site expands to use distributed 
    /// DBs, add the functionality here.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    /// <summary>
    /// The RavenDB class provides access to a document store through a property. It also allows connection capability.
    /// </summary>
    public class RavenDB
    {
        /// <summary>
        /// The document store in question. 
        /// </summary>
        private DocumentStore documentStore; 

        /// <summary>
        /// The Document Store that we are using on the server. 
        /// </summary>
        public DocumentStore DocStore
        {
            get { return documentStore; } 
        }

        /// <summary>
        /// Uses .Net 'Connection String' functionality to connect to a DB server IP, port, and Database name.
        /// See the appropiate string in the Web.config file.
        /// <remarks>You MUST change the connection string in the web.config file if you are 
        /// running the server on a separate machine, or with a different database name!!!</remarks>
        /// </summary>
        public void Connect()
        {

            documentStore = new DocumentStore { ConnectionStringName = "RavenDBServer" }; 
            documentStore.Initialize();
        }

    }
}
