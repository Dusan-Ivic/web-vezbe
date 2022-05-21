using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak3.Models
{
    public class UploadedFile
    {
        private string _fileName;
        private string _directoryPath;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; }
        }

        public UploadedFile(string fileName, string directoryPath)
        {
            FileName = fileName;
            DirectoryPath = directoryPath;
        }
    }
}