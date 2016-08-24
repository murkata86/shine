namespace Shine.Models
{
    using System;
    using System.IO;
    using Core;

    public class FileSystemChecker
    {
        private string path;
        private FileSystemWatcher watcher;

        public FileSystemChecker(string path)
        {
            this.Path = path;
            this.watcher = new FileSystemWatcher();
        }

        public string Path
        {
            get { return this.path; }
            private set { this.path = value; }
        }

        public void Run()
        {
            this.watcher.Path = this.Path;

            EventLogger.Logger(string.Format("Monitoring {0}", this.Path));

            this.watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess |
                                        NotifyFilters.FileName | NotifyFilters.DirectoryName;

            this.watcher.Filter = "*.*";

            this.watcher.Changed += new FileSystemEventHandler(OnChanged);
            this.watcher.Created += new FileSystemEventHandler(OnChanged);
            this.watcher.Deleted += new FileSystemEventHandler(OnChanged);
            this.watcher.Renamed += new RenamedEventHandler(OnRenamed);

            this.watcher.EnableRaisingEvents = true;

            while (Console.Read() != 'q');
        }

        public void Stop()
        {
            this.watcher.EnableRaisingEvents = false;
            this.watcher.Dispose();

            EventLogger.Logger("Monitoring stopped!");
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {

            Console.WriteLine(e.Name);
            EventLogger.Logger(string.Format("File: " + e.Name + " " + e.ChangeType));
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
           EventLogger.Logger(string.Format("File: {0} renamed to {1}", e.OldFullPath, e.FullPath));
        }

    }
}
