//using MongoDB.Driver;
//using XenaImportToMongoDb.Model;

//public class Program
//{
//    private static void Main(string[] args)
//    {
//        const string CONNECTION_STRING = "mongodb+srv://ldposavec:Pa$$w0rd@cluster0.xhqtb9b.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

//        var mongoClient = new MongoClient(CONNECTION_STRING);
//        var db = mongoClient.GetDatabase("MedikDatabase");
//        var genesCollection = db.GetCollection<GeneExpression>("genes");

//        var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaExtractAndImport\\XENA\\Extracted";
//        //TransposeData(filePath);
//        //TransformData(filePath);
//        //DeleteLines(filePath);
//        var geneExpressions = ReadTSVFiles(filePath);

//        genesCollection.InsertMany(geneExpressions);
//    }

//    private static void DeleteLines(string filePath)
//    {
//        var files = Directory.GetFiles(filePath, "*.tsv");
//        var cancerCohorts = Directory.GetFiles(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaScrapping\\Downloads\\XENA\\", "*.gz").Select(Path.GetFileNameWithoutExtension).Select(s => s.Split('.')[1]).Select(s => s.Split('.')[0]).ToList();
//        for (int i = 0; i < files.Count(); i++)
//        {
//            var file = files[i];
//            var lines = File.ReadAllLines(file);
//            var newLines = new List<string>();

//            var header = lines[0].Split('\t').ToList();

//            var filteredHeader = header.Where(h => GenesOfInterest.Contains(h)).ToList();
//            filteredHeader.Insert(0, "patient_id");
//            filteredHeader.Insert(1, "cancer_cohort");
//            newLines.Add(string.Join("\t", filteredHeader));

//            for (int j = 1; j < lines.Length; j++)
//            {
//                var columns = lines[j].Split('\t').ToList();
//                var filteredColumns = columns.Where((c, index) => GenesOfInterest.Contains(header[index])).ToList();
//                filteredColumns.Insert(0, j.ToString());
//                filteredColumns.Insert(1, cancerCohorts[i]);
//                newLines.Add(string.Join("\t", filteredColumns));
//            }

//            //foreach (var line in lines)
//            //{
//            //    if (!line.Contains("??"))
//            //    {
//            //        newLines.Add(line);
//            //    }
//            //}
//            //newLines.RemoveAt(1);

//            //var header = newLines[0].Split('\t').ToList();

//            File.WriteAllLines(file, newLines);
//        }
//    }

//    private static void TransformData(string filePath)
//    {
//        var files = Directory.GetFiles(filePath, "*.tsv");
//        //var cancerCohorts = Directory.GetFiles(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaScrapping\\Downloads\\XENA\\", "*.gz").Select(Path.GetFileNameWithoutExtension).Select(s => s.Split('.')[1]).Select(s => s.Split('.')[0]).ToList();
//        var cancerCohorts = Directory.GetFiles(filePath).Select(Path.GetFileNameWithoutExtension).Select(s => s.Split('_')[0]).ToList();
//        for (int i = 0; i < files.Count(); i++)
//        {
//            var file = files[i];
//            var lines = File.ReadAllLines(file);
//            var newLines = new List<string>();

//            var header = lines[0].Split('\t').ToList();
//            header.Insert(0, "patient_id");
//            header.Insert(1, "cancer_cohort");
//            newLines.Add(string.Join("\t", header));

//            for (int j = 0; j < lines.Length; j++)
//            {
//                var columns = lines[j].Split('\t').ToList();
//                columns.Insert(0, j.ToString());
//                columns.Insert(1, cancerCohorts[i]);
//                //var geneExpressionValues = columns.Skip(2).ToList();
//                //newLines.Add($"{patientId}\t{cancerCohort}\t{string.Join("\t", geneExpressionValues)}");
//                newLines.Add(string.Join("\t", columns));
//            }
//            File.WriteAllLines(file, newLines);
//        }
//    }

//    private static void TransposeData(string filePath)
//    {
//        var files = Directory.GetFiles(filePath, "*.tsv");
//        foreach (var file in files)
//        {
//            var lines = File.ReadAllLines(file);
//            var columnsList = new List<List<string>>();

//            for (int i = 0; i < lines.Length; i++)
//            {
//                var columns = lines[i].Split('\t').ToList();
//                for (int j = 0; j < columns.Count; j++)
//                {
//                    if (columnsList.Count <= j)
//                    {
//                        columnsList.Add(new List<string>());
//                    }
//                    columnsList[j].Add(columns[j]);
//                }
//            }

//            var newLines = new List<string>();
//            foreach (var columns in columnsList)
//            {
//                newLines.Add(string.Join("\t", columns));
//            }

//            File.WriteAllLines(file, newLines);
//        }
//    }

//    private static readonly List<string> GenesOfInterest = new List<string>
//        {
//        "C6orf150", "CCL5", "CXCL10", "TMEM173", "CXCL9", "CXCL11", "NFKB1",
//        "IKBKE", "IRF3", "TREX1", "ATM", "IL6", "IL8"
//        };
//    private static int id = 0;

//    public static List<GeneExpression> ReadTSVFiles(string folderPath)
//    {
//        var geneExpressionDataList = new List<GeneExpression>();
//        var files = Directory.GetFiles(folderPath, "*.tsv");

//        foreach (var file in files)
//        {
//            var lines = File.ReadAllLines(file);
//            var header = lines[0].Split('\t').ToList();
//            var geneExpressionValues = header.Skip(2).ToList();
//            for (int i = 1; i < lines.Length; i++)
//            {
//                var columns = lines[i].Split('\t').ToList();
//                var patientId = long.Parse(columns[0]);
//                var cancerCohort = columns[1];
//                var geneExpression = new GeneExpression
//                {
//                    Id = ++id,
//                    PatientId = patientId,
//                    CancerCohort = cancerCohort,
//                    GeneExpressionValues = new Dictionary<string, double>()
//                };
//                for (int j = 0; j < geneExpressionValues.Count; j++)
//                {
//                    geneExpression.GeneExpressionValues.Add(geneExpressionValues[j], double.Parse(columns[j + 2]));
//                }
//                geneExpressionDataList.Add(geneExpression);
//            }
//        }

//        return geneExpressionDataList;
//    }
//}

using MongoDB.Driver;
using XenaImportToMongoDb.Model;

public class Program
{
    private static void Main(string[] args)
    {
        const string CONNECTION_STRING = "mongodb+srv://ldposavec:Pa$$w0rd@cluster0.xhqtb9b.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        var mongoClient = new MongoClient(CONNECTION_STRING);
        //var db = mongoClient.GetDatabase("MedikDatabase");
        var db = mongoClient.GetDatabase("MediqDatabase");
        var genesCollection = db.GetCollection<GeneExpression>("genes");

        var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaExtractAndImport\\XENA\\Extracted";
        TransposeData(filePath);
        //TransformData(filePath);
        DeleteLines(filePath);
        var geneExpressions = ReadTSVFiles(filePath);

        genesCollection.InsertMany(geneExpressions);
    }

    private static void DeleteLines(string filePath)
    {
        var files = Directory.GetFiles(filePath, "*.tsv");
        //var cancerCohorts = Directory.GetFiles(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaScrapping\\Downloads\\XENA\\", "*.gz").Select(Path.GetFileNameWithoutExtension).Select(s => s.Split('.')[1]).Select(s => s.Split('.')[0]).ToList();
        var cancerCohorts = Directory.GetFiles(filePath).Select(Path.GetFileNameWithoutExtension).Select(s => s.Split('_')[0]).ToList();
        for (int i = 0; i < files.Count(); i++)
        {
            var file = files[i];
            var lines = File.ReadAllLines(file);
            var newLines = new List<string>();

            var header = lines[0].Split('\t').ToList();

            var filteredHeader = header.Where(h => ColumnsOfInterest.Contains(h)).ToList();
            //filteredHeader.Insert(0, "patient_id");
            filteredHeader.Insert(1, "cancer_cohort");
            newLines.Add(string.Join("\t", filteredHeader));

            for (int j = 1; j < lines.Length; j++)
            {
                var columns = lines[j].Split('\t').ToList();
                var filteredColumns = columns.Where((c, index) => ColumnsOfInterest.Contains(header[index])).ToList();
                //filteredColumns.Insert(0, j.ToString());
                filteredColumns.Insert(1, cancerCohorts[i]);
                newLines.Add(string.Join("\t", filteredColumns));
            }

            //foreach (var line in lines)
            //{
            //    if (!line.Contains("??"))
            //    {
            //        newLines.Add(line);
            //    }
            //}
            //newLines.RemoveAt(1);

            //var header = newLines[0].Split('\t').ToList();

            File.WriteAllLines(file, newLines);
        }
    }

    private static void TransformData(string filePath)
    {
        var files = Directory.GetFiles(filePath, "*.tsv");
        //var cancerCohorts = Directory.GetFiles(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaScrapping\\Downloads\\XENA\\", "*.gz").Select(Path.GetFileNameWithoutExtension).Select(s => s.Split('.')[1]).Select(s => s.Split('.')[0]).ToList();
        var cancerCohorts = Directory.GetFiles(filePath).Select(Path.GetFileNameWithoutExtension).Select(s => s.Split('_')[0]).ToList();
        for (int i = 0; i < files.Count(); i++)
        {
            var file = files[i];
            var lines = File.ReadAllLines(file);
            var newLines = new List<string>();

            var header = lines[0].Split('\t').ToList();
            header.Insert(0, "patient_id");
            header.Insert(1, "cancer_cohort");
            newLines.Add(string.Join("\t", header));

            for (int j = 0; j < lines.Length; j++)
            {
                var columns = lines[j].Split('\t').ToList();
                columns.Insert(0, j.ToString());
                columns.Insert(1, cancerCohorts[i]);
                //var geneExpressionValues = columns.Skip(2).ToList();
                //newLines.Add($"{patientId}\t{cancerCohort}\t{string.Join("\t", geneExpressionValues)}");
                newLines.Add(string.Join("\t", columns));
            }
            File.WriteAllLines(file, newLines);
        }
    }

    private static void TransposeData(string filePath)
    {
        var files = Directory.GetFiles(filePath, "*.tsv");
        foreach (var file in files)
        {
            var lines = File.ReadAllLines(file);
            var columnsList = new List<List<string>>();

            for (int i = 0; i < lines.Length; i++)
            {
                var columns = lines[i].Split('\t').ToList();
                for (int j = 0; j < columns.Count; j++)
                {
                    if (columnsList.Count <= j)
                    {
                        columnsList.Add(new List<string>());
                    }
                    columnsList[j].Add(columns[j]);
                }
            }

            var newLines = new List<string>();
            foreach (var columns in columnsList)
            {
                newLines.Add(string.Join("\t", columns));
            }

            File.WriteAllLines(file, newLines);
        }
    }

    private static readonly List<string> ColumnsOfInterest = new List<string>
        {
        "sample", "C6orf150", "CCL5", "CXCL10", "TMEM173", "CXCL9", "CXCL11", "NFKB1",
        "IKBKE", "IRF3", "TREX1", "ATM", "IL6", "IL8"
        };
    private static int id = 0;

    public static List<GeneExpression> ReadTSVFiles(string folderPath)
    {
        var geneExpressionDataList = new List<GeneExpression>();
        var files = Directory.GetFiles(folderPath, "*.tsv");

        foreach (var file in files)
        {
            var lines = File.ReadAllLines(file);
            var header = lines[0].Split('\t').ToList();
            var geneExpressionValues = header.Skip(2).ToList();
            for (int i = 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split('\t').ToList();
                var patientId = columns[0];
                var cancerCohort = columns[1];
                var geneExpression = new GeneExpression
                {
                    Id = ++id,
                    PatientId = patientId,
                    CancerCohort = cancerCohort,
                    GeneExpressionValues = new Dictionary<string, double>()
                };
                for (int j = 0; j < geneExpressionValues.Count; j++)
                {
                    geneExpression.GeneExpressionValues.Add(geneExpressionValues[j], double.Parse(columns[j + 2]));
                }
                geneExpressionDataList.Add(geneExpression);
            }
        }

        return geneExpressionDataList;
    }
}