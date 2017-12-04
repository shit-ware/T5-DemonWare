using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

using MongoDB.Bson;
using MongoDB.Driver;

namespace DWServer
{
    class Database
    {
        public static string connectionString =
                "mongodb://127.0.0.1";

        public static MongoServer Server { get; set; }
        public static MongoDatabase ADatabase { get; set; }
        public static MongoCollection<GroupUser> AGroupUser { get; set; }
        public static MongoCollection<GroupUserCache> AGroupUserCache { get; set; }
        public static MongoCollection<DWProfiles.PublicProfile> APublicProfile { get; set; }
        public static MongoCollection<DWStorage.File> AFiles { get; set; }
        public static MongoCollection<DWEventLog.BinaryEvent> ABlobEvents { get; set; }
        public static MongoCollection<DWAuther.ServerKey> AServerKeys { get; set; }

        public static void Initialize()
        {
            Server = MongoServer.Create(connectionString);

            var credentials = new MongoCredentials("repz", "repz");
            ADatabase = Server.GetDatabase("repzdw_db", credentials);
            

            if (Program.Game == TitleID.T5)
            {
                // clear data and load collections
                AGroupUser = ADatabase.GetCollection<GroupUser>("groupUsers");
                AGroupUserCache = ADatabase.GetCollection<GroupUserCache>("groupUsersCache");
                APublicProfile = ADatabase.GetCollection<DWProfiles.PublicProfile>("publicProfiles");
            }
            else if (Program.Game == TitleID.IW5)
            {
                // clear data and load collections
                AGroupUser = ADatabase.GetCollection<GroupUser>("groupUsersIW5");
                AGroupUserCache = ADatabase.GetCollection<GroupUserCache>("groupUsersCacheIW5");
                APublicProfile = ADatabase.GetCollection<DWProfiles.PublicProfile>("publicProfilesIW5");
            }

            AGroupUser.RemoveAll();
            AGroupUser.EnsureIndex("groupID");
            AGroupUser.EnsureIndex("userID");

            AGroupUserCache.RemoveAll();
            AGroupUserCache.EnsureIndex("groupID");

            APublicProfile.EnsureIndex("user_id");

            AFiles = ADatabase.GetCollection<DWStorage.File>("files");
            AFiles.EnsureIndex("filename");

            ABlobEvents = ADatabase.GetCollection<DWEventLog.BinaryEvent>("blobEvents");

            AServerKeys = ADatabase.GetCollection<DWAuther.ServerKey>("serverKeysIW5");
            AServerKeys.EnsureIndex("keyHash");
        }

        
    }
}
