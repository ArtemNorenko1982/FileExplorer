using System;
using System.IO;

namespace Explorer
{
    class GetFiles
    {
        DirectoryInfo directoryInfo;
        FileInfo[] files;
        public GetFiles(string strPath)
        {
            directoryInfo = new DirectoryInfo(strPath);
            files = directoryInfo.GetFiles();
        }
        public FileInfo[] GetAllFiles { get { return files; } }
    }
}
