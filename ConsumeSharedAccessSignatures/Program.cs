using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ConsumeSharedAccessSignatures
{
    class Program
    {
        static void Main(string[] args)
        {
            string containerSAS = "https://sharedaccesssignature.blob.core.windows.net/sascontainer?sv=2014-02-14&sr=c&sig=5uVRDzqxmmu4XRjqofr4w2IBv0amUJYwsVx59jV6rj4%3D&se=2014-11-04T12%3A05%3A55Z&sp=wl";
            string blobSAS = "https://sharedaccesssignature.blob.core.windows.net/sascontainer?sv=2014-02-14&sr=c&sig=5uVRDzqxmmu4XRjqofr4w2IBv0amUJYwsVx59jV6rj4%3D&se=2014-11-04T12%3A05%3A55Z&sp=wl";
            string containerSASWithAccessPolicy = "https://sharedaccesssignature.blob.core.windows.net/sascontainer?sv=2014-02-14&sr=c&si=tutorialpolicy&sig=XXwnwB5cZkTbP%2FiYsKC9E3cEiP%2FFeQBEY8R%2BiY8vboE%3D";
            string blobSASWithAccessPolicy = "https://sharedaccesssignature.blob.core.windows.net/sascontainer/sasblobpolicy.txt?sv=2014-02-14&sr=b&si=tutorialpolicy&sig=kmHtOfws0Oqw9B7XrkhfsgARLCUwKtnbYklx1DPrjbY%3D";
            
           
        }
    }
}
