using System;
using System.IO;

namespace Explorer
{
    class GetFolders
    {
        DirectoryInfo directoryInfo;
        DirectoryInfo[] directories;
        public GetFolders(string rootPath)
        {
            directoryInfo = new DirectoryInfo(rootPath);
            directories = directoryInfo.GetDirectories();
        }
        public DirectoryInfo[] GetDirectories{ get { return directories; }}
    }
}
