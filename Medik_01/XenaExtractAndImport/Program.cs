using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Diagnostics;

string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe";
string gzFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaScrapping\\Downloads\\XENA\\";
string outputFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\XENA\\Extracted\\";

if (!Directory.Exists(outputFolder))
{
    Directory.CreateDirectory(outputFolder);
}

if (Directory.GetFiles(outputFolder).Length > 0)
{
    Console.WriteLine("Output folder is not empty.");
    var filez = Directory.GetFiles(outputFolder);
    foreach (var file in filez)
    {
        File.Delete(file);
    }
}

if (!Directory.Exists(gzFilePath))
{
    Console.WriteLine("No .gz to extract from.");
    return;
}

string[] gzFiles = Directory.GetFiles(gzFilePath, "*.gz");

if (gzFiles.Length == 0)
{
    Console.WriteLine("No .gz found.");
    return;
}


foreach (string gzFile in gzFiles)
{
    string originalFileName = Path.GetFileNameWithoutExtension(gzFile.Split("%2F")[1]);
    string cancerCohort = Path.GetFileNameWithoutExtension(gzFile.Split("%2F")[0].Split('.')[1].Split('.')[0]);

    ProcessStartInfo psi = new ProcessStartInfo(sevenZipPath);
    psi.Arguments = $"x \"{gzFile}\" -o\"{outputFolder}\" -r";
    psi.WindowStyle = ProcessWindowStyle.Hidden;
    Process p = Process.Start(psi);
    p.WaitForExit();

    string extractedFile = Path.Combine(outputFolder, originalFileName);
    if (File.Exists(extractedFile))
    {
        string newFile = $"{cancerCohort}_{originalFileName}_{Guid.NewGuid()}.tsv";

        File.Move(extractedFile, Path.Combine(outputFolder, newFile));
        Console.WriteLine($"Renamed: {extractedFile} -> {newFile}");
    }
}

Console.WriteLine("Completed extract!");


const string ENDPOINT = "regoch.net:9000";
const string ACCESS_KEY = "minioAdmin";
const string SECRET_KEY = "supersecretpassword";
string bucketName = "ldpbucket";

var minioClient = new MinioClient()
                        .WithEndpoint(ENDPOINT)
                        .WithCredentials(ACCESS_KEY, SECRET_KEY)
                        .Build();

bool found = false;
try
{
    found = await minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName));
}
catch (MinioException ex)
{
    Console.WriteLine($"{ex.Message}");
}


Console.WriteLine($"{(found ? "Found" : "Not found")}");

string[] files = Directory.GetFiles(outputFolder);

foreach (string file in files)
{
    var response = await minioClient.PutObjectAsync(
        new PutObjectArgs()
            .WithBucket(bucketName)
            .WithFileName(file)
            .WithContentType("text/tab-separated-values")
            .WithObject($"{file.Split("Extracted\\")[1]}")
    );
    Console.WriteLine($"uploaded: {response.ObjectName}");
}

var objects = minioClient.ListObjectsEnumAsync(
        new ListObjectsArgs().WithBucket(bucketName)
    );

var firstObj = objects.ToBlockingEnumerable().ToList().ElementAt(0);

await foreach (var obj in objects)
{
    Console.WriteLine($"{obj.Key} | {obj.Size}B | {obj.ContentType}");
}