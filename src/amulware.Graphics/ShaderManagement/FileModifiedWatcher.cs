using System;
using System.IO;

namespace amulware.Graphics.ShaderManagement
{
    /// <summary>
    /// This class can be used to watch a file for modifications by checking its last write time stamp.
    /// Stolen from https://github.com/beardgame/utilities/blob/develop/src/IO/FileModifiedWatcher.cs
    /// </summary>
    class FileModifiedWatcher
    {
        private readonly string path;
        private readonly string fileName;
        private DateTime? lastModified;

        /// <summary>
        /// Gets the path of the file watched.
        /// </summary>
        public string Path { get { return this.path; } }

        /// <summary>
        /// Gets the filename, without directories, of the file watched.
        /// </summary>
        public string FileName { get { return this.fileName; } }

        /// <summary>
        /// Creates a new <see cref="FileModifiedWatcher"/> watching the specified file.
        /// </summary>
        /// <param name="path">The file to watch. Must be valid path.
        /// Otherwise, behaviour is undefined and exceptions may be thrown.</param>
        public FileModifiedWatcher(string path)
        {
            this.path = path;
            this.fileName = System.IO.Path.GetFileName(path);

            this.Reset();
        }

        private DateTime? getLastWriteTime()
        {
            return File.Exists(this.path)
                ? (DateTime?)File.GetLastWriteTime(this.path)
                : null;
        }

        /// <summary>
        /// Resets this watcher to ignore all past modifications to file.
        /// </summary>
        public void Reset()
        {
            this.lastModified = this.getLastWriteTime();
        }

        /// <summary>
        /// Checks whether the file was changed since the last reset of the watcher.
        /// </summary>
        /// <param name="resetModified">Whether to reset the watcher after checking for changes.</param>
        /// <returns>True, if the file has a different last-write time stamp than when the watcher was reset last.
        /// True if the file was created or deleted since the last reset.
        /// False otherwise.</returns>
        public bool WasModified(bool resetModified = true)
        {
            var modified = this.getLastWriteTime();

            if (Nullable.Equals(modified, this.lastModified))
                return false;

            if (resetModified)
                this.lastModified = modified;
            return true;
        }
    }
}