using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace GenerateSharedAccessSignatures
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parsing the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=sharedaccesssignature;AccountKey=qikQYOZPbpik/pxe1o6kyC8DnWD4CHXuxp55wPhYqXWeL1AlkwJlJ43hGgdUNIghvKGZTgr2px1NuWT1wv6Ntg==");

            //Creates reference to the bob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference("sascontainer");
            container.CreateIfNotExists();

            //Generates a SAS URI for the container, without a stored access policy.
            Console.WriteLine("Container SAS URI: " + GetContainerSasUri(container));
            Console.WriteLine();

            //Generates a SAS URI for a blob within the container, without a stored access policy.
            Console.WriteLine("Blob SAS URI: " + GetBlobSasUri(container));
            Console.WriteLine();

            //shared access signatures on the container and the blob.
            string sharedAccessPolicyName = "tutorialpolicy";
            CreateSharedAccessPolicy(blobClient, container, sharedAccessPolicyName);

            //Generates a SAS URI for the container, using a stored access policy to set constraints on the SAS.
             Console.WriteLine("Container SAS URI using stored access policy: " + GetContainerSasUriWithPolicy(container, sharedAccessPolicyName));
             Console.WriteLine();

            //Generate a SAS URI for a blob within the container, using a stored access policy to set constraints on the SAS.
            Console.WriteLine("Blob SAS URI using stored access policy: " + GetBlobSasUriWithPolicy(container, sharedAccessPolicyName));
            Console.WriteLine();

            //Requires user input before closing the console window.
            Console.ReadLine();
            }


            //method that generates the shared access signature for the container and returns the signature URI.
            static string GetContainerSasUri(CloudBlobContainer container1)
            {

                //Sets the expiry time and permissions for the container.
                //In this case no start time is specified, so the shared access signature becomes valid immediately.
                SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
                sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(4);
                sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List;



                //Generates the shared access signature on the container, setting the constraints directly on the signature.
                string sasContainerToken = container1.GetSharedAccessSignature(sasConstraints);

                //Return the URI string for the container, including the SAS token.
                return container1.Uri + sasContainerToken;
            }




            //Method that creates a new blob and writes some text to it, then generates a shared access signature and returns the signature URI.
            static string GetBlobSasUri(CloudBlobContainer container)
            {


            //Get a reference to a blob within the container.
            CloudBlockBlob blob = container.GetBlockBlobReference("sasblob.txt");

           //Upload text to the blob. If the blob does not yet exist, it will be created. 
           //If the blob does exist, its existing content will be overwritten.
            string blobContent = "This blob will be accessible to clients via a Shared Access Signature.";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
            ms.Position = 0;
            using (ms)
            {
                blob.UploadFromStream(ms);
                }

            //Set the expiry time and permissions for the blob.
            //In this case the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(4);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
}


            //New method that creates a new stored access policy and returns the name of the policy
            static void CreateSharedAccessPolicy(CloudBlobClient blobClient, CloudBlobContainer container, string policyName)
         {
            //Create a new stored access policy and define its constraints.
            SharedAccessBlobPolicy sharedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(10),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List
            };

            //Get the container's existing permissions.
            BlobContainerPermissions permissions = new BlobContainerPermissions();

            //Add the new policy to the container's permissions.
            permissions.SharedAccessPolicies.Clear();
            permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
            container.SetPermissions(permissions);
        }


            //New method to create a blob and generate a shared access signature
            static string GetBlobSasUriPolicy(CloudBlobContainer container, string policyName)
            {

                CloudBlockBlob blockBlob = container.GetBlockBlobReference("sasblobpolicy.txt");

                string blobContent = "This blob will be accessible to all clients via a shared access signature. "
                +
                "A stored access policy defines the constraints for this signature. ";

                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
                ms.Position = 0;

                using (ms)
                {

                    blockBlob.UploadFromStream(ms);
                }

                string sasBlobToken = blockBlob.GetSharedAccessSignature(null, policyName);

                return blockBlob.Uri + sasBlobToken;


            }

            static string GetContainerSasUriWithPolicy(CloudBlobContainer container, string policyName)
            {
                //Generate the shared access signature on the container. In this case, all of the constraints for the 
                //shared access signature are specified on the stored access policy.
                string sasContainerToken = container.GetSharedAccessSignature(null, policyName);

                //Return the URI string for the container, including the SAS token.
                return container.Uri + sasContainerToken;
            }

            static string GetBlobSasUriWithPolicy(CloudBlobContainer container, string policyName)
            {
                //Get a reference to a blob within the container.
                CloudBlockBlob blob = container.GetBlockBlobReference("sasblobpolicy.txt");

                //Upload text to the blob. If the blob does not yet exist, it will be created. 
                //If the blob does exist, its existing content will be overwritten.
                string blobContent = "This blob will be accessible to clients via a shared access signature. " +
                "A stored access policy defines the constraints for the signature.";
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
                ms.Position = 0;
                using (ms)
                {
                    blob.UploadFromStream(ms);
                }

                //Generate the shared access signature on the blob.
                string sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

                //Return the URI string for the container, including the SAS token.
                return blob.Uri + sasBlobToken;
            }
        

    }

}

