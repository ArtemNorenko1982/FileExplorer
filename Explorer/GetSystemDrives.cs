using System;
using System.IO;


namespace Explorer
{
    class GetSystemDrives
    {
        DriveInfo[] driveInfo;

        public GetSystemDrives()
        {
            driveInfo = DriveInfo.GetDrives();
        }
            
        public DriveInfo[] GetDrives{ get { return driveInfo; }}
        
    }
}
