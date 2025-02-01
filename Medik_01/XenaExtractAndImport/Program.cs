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

    ProcessStartInfo psi = new ProcessStartInfo(sevenZipPath);
    psi.Arguments = $"x \"{gzFile}\" -o\"{outputFolder}\" -r";
    psi.WindowStyle = ProcessWindowStyle.Hidden;
    Process p = Process.Start(psi);
    p.WaitForExit();

    string extractedFile = Path.Combine(outputFolder, originalFileName);
    if (File.Exists(extractedFile))
    {
        //gzFile.ToString() = $"{gzFile.ToString()}_{Guid.NewGuid()}.tsv";
        string newFile = $"{originalFileName}_{Guid.NewGuid()}.tsv";

        File.Move(extractedFile, Path.Combine(outputFolder, newFile));
        Console.WriteLine($"Renamed: {extractedFile} -> {newFile}");
    }
}

Console.WriteLine("Completed extract!");


const string ENDPOINT = "regoch.net:9000";
const string ACCESS_KEY = "minioAdmin"; // tu bi trebao biti ACCESS_KEY
const string SECRET_KEY = "supersecretpassword"; // tu bi trebao biti SECRET_KEY
string bucketName = "ldpbucket";

var minioClient = new MinioClient()
                        .WithEndpoint(ENDPOINT)
                        .WithCredentials(ACCESS_KEY, SECRET_KEY)
                        .Build();

// provjeravam postoji li bucket imena bucket-test


// operacije s bucketima: napraviti bucket, izbrisati bucket,
//  izlistati objekte iz bucketa, provjeriti postoji li bucket

//operacije s objektima: prenijeti objekt (put), obrisati objekt, dohvatiti objekt,
//    provjeriti detalje objekta


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

// napravite svoj novi bucket preko MinioClienta
//try
//{

//    Task makeTask
//        = minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket("lukas-deni"));
//    await makeTask;
//}
//catch (MinioException ex)
//{
//    Console.WriteLine($"{ex.Message}");
//}

//Console.WriteLine("bucket created");


// upload i download objekta putem object keya

//string filepath = @"D:\PPPK\Exercises07\MinioExample\test.txt";
string[] files = Directory.GetFiles(outputFolder);

foreach (string file in files)
{
    //var changeFile = file.Split("Extracted\\")[1];
    var response = await minioClient.PutObjectAsync(
        new PutObjectArgs()
            .WithBucket(bucketName)
            .WithFileName(file)
            .WithContentType("text/tab-separated-values")
            .WithObject($"{file.Split("Extracted\\")[1]}")
    );
    Console.WriteLine($"uploaded: {response.ObjectName}");
}
//var response = await minioClient.PutObjectAsync(
//        new PutObjectArgs()
//            .WithBucket(bucketName)
//            .WithFileName(filepath)
//            .WithContentType("application/text")
//            .WithObject($"{Guid.NewGuid()}_{filepath}")
//    );

//Console.WriteLine($"uploaded: {response.ObjectName}");


// izlistati sve objekte iz bucketa bucket-novi

var objects = minioClient.ListObjectsEnumAsync(
        new ListObjectsArgs().WithBucket(bucketName)
    );

var firstObj = objects.ToBlockingEnumerable().ToList().ElementAt(0);

await foreach (var obj in objects) // === objects.ToBlockingEnumerable();
{
    Console.WriteLine($"{obj.Key} | {obj.Size}B | {obj.ContentType}");
}

// download prvog objekta
//try
//{

//    var stats = await minioClient.GetObjectAsync(
//            new GetObjectArgs()
//                .WithBucket(bucketName)
//                .WithObject(firstObj.Key)
//                //.WithFile("./downloaded.txt")
//                .WithCallbackStream((stream) =>
//                {
//                    // stream.CopyTo(File.Create($"./downloaded_{firstObj.Key}"));
//                    //Console.WriteLine("\nSadrzaj:");
//                    //stream.CopyTo(Console.OpenStandardOutput());
//                    using (var fileStream = new FileStream($"./downloaded_new.txt", FileMode.Create, FileAccess.Write))
//                        stream.CopyTo(fileStream);
//                })
//        );
//}
//catch (Exception ex)
//{

//    Console.WriteLine(ex.Message);
//}