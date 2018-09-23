using System;
using System.IO;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class FilepathManager
    {
        private FilepathManager() { }
        public FilepathManager(string rootDirectory)
        {
            if (String.IsNullOrWhiteSpace(rootDirectory))
                throw new ArgumentNullException(nameof(rootDirectory));

            RootDirectory = rootDirectory;

            // --- Root --- //
            CreateDirectoryIfNotExists(RootDirectory);

            // --- Temporary --- //
            CreateDirectoryIfNotExists(TemporaryDirectory);
            CreateFileIfNotExists(TestFile);

            // --- Images --- //
            CreateDirectoryIfNotExists(ImagesDirectory);

            // --- Database --- //
            CreateDirectoryIfNotExists(DatabaseDirectory);
            // CreateFileIfNotExists(PersistentDatabase); // Created on DB generation
            // CreateFileIfNotExists(UserDatabase); // Created on DB generation
        }

        // --- Root --- //
        public string RootDirectory { get; private set; }

        // -- Temporary -- //
        public string TemporaryDirectory { get { return Path.Combine(RootDirectory, "Temporary"); } }
        public string TestFile { get { return Path.Combine(TemporaryDirectory, "Testfile.txt"); } }

        // -- Images -- //
        public string ImagesDirectory { get { return Path.Combine(RootDirectory, "Images"); } }

        // -- Database -- //
        public string DatabaseDirectory { get { return Path.Combine(RootDirectory, "Database"); } }
        public string PersistentDatabase { get { return Path.Combine(DatabaseDirectory, "Persistent.db"); } }
        public string UserDatabase { get { return Path.Combine(DatabaseDirectory, "User.db"); } }

        private static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        private static void CreateFileIfNotExists(string path)
        {
            if (!File.Exists(path))
                using (File.Create(path)) { }
        }
    }
}
