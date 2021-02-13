using System;
using System.IO;

namespace amulware.Graphics.ShaderManagement
{
    /// <summary>
    /// This class can be used to watch a file for modifications by checking its last write time stamp.
    /// Stolen from https://github.com/beardgame/utilities/blob/master/Bearded.Utilities/IO/FileModifiedWatcher.cs
    /// </summary>
    internal sealed class FileModifiedWatcher
    {
        private DateTime? lastModified;

        private readonly string path;

        public static FileModifiedWatcher FromPath(string path) => new FileModifiedWatcher(path);

        private FileModifiedWatcher(string path)
        {
            this.path = path;
            Reset();
        }

        private DateTime? getLastWriteTime()
        {
            return File.Exists(path)
                ? (DateTime?)File.GetLastWriteTime(path)
                : null;
        }

        public void Reset()
        {
            lastModified = getLastWriteTime();
        }

        public bool WasModified(bool resetModified = true)
        {
            var modified = getLastWriteTime();

            if (Nullable.Equals(modified, lastModified))
                return false;

            if (resetModified)
                lastModified = modified;

            return true;
        }
    }
}
