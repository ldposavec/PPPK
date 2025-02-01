
//using MongoDB.Driver;
//using XenaImportToMongoDb.Model;
//using XenaMergeData.Model;

//public class Program
//{
//    private static void Main(string[] args)
//    {
//        const string CONNECTION_STRING = "mongodb+srv://ldposavec:Pa$$w0rd@cluster0.xhqtb9b.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
//        var mongoClient = new MongoClient(CONNECTION_STRING);
//        var db = mongoClient.GetDatabase("MedikDatabase");
//        var genes = db.GetCollection<GeneExpression>("genes");

//        var geneExpressions = genes.Find(FilterDefinition<GeneExpression>.Empty).ToList();

//        var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaMergeData\\ToMerge\\TCGA_clinical_survival_data.tsv";

//        var survivalData = ReadTSVFile(filePath);

//        survivalData.Sort((a, b) => a.Type == b.Type ? string.Compare(a.PatientId, b.PatientId, StringComparison.Ordinal) : string.Compare(a.Type, b.Type, StringComparison.Ordinal));

//        var merged = MergeGenesWithSurvivalData(geneExpressions, survivalData);

//        var mergedCollection = db.GetCollection<MergedData>("merged");

//        mergedCollection.InsertMany(merged);

//    }

//    private static int _id = 0;
//    private static List<MergedData> MergeGenesWithSurvivalData(List<GeneExpression> geneExpressions, List<SurvivalData> survivalData)
//    {
//        var merged = new List<MergedData>();

//        var survivalCounter = 0;
//        for (int i = 0; i< geneExpressions.Count; i++)
//        {
//            var mergedData = new MergedData();
//            mergedData.Id = ++_id;
//            mergedData.CancerCohort = geneExpressions[i].CancerCohort;
//            mergedData.GeneExpressionValues = geneExpressions[i].GeneExpressionValues;
//            if (survivalCounter < survivalData.Count && geneExpressions[i].CancerCohort == survivalData[survivalCounter].Type)
//            {
//                    mergedData.PatientId = survivalData[survivalCounter].PatientId;
//                    mergedData.DSS = survivalData[survivalCounter].DSS;
//                    mergedData.OS = survivalData[survivalCounter].OS;
//                    mergedData.ClinicalStage = survivalData[survivalCounter].ClinicalStage;
//                    survivalCounter++;
//            }
//            else
//            {
//                int foundData = survivalData.FindIndex(s => s.Type == geneExpressions[i].CancerCohort);

//                if (foundData != -1)
//                {
//                    survivalCounter = foundData;
//                    mergedData.PatientId = survivalData[foundData].PatientId;
//                    mergedData.DSS = survivalData[foundData].DSS;
//                    mergedData.OS = survivalData[foundData].OS;
//                    mergedData.ClinicalStage = survivalData[foundData].ClinicalStage;
//                    survivalCounter++;
//                }
//                else
//                {
//                    mergedData.PatientId = geneExpressions[i].PatientId.ToString();
//                    mergedData.DSS = "Uknown";
//                    mergedData.OS = "Unknown";
//                    mergedData.ClinicalStage = "Unknown";
//                }
//            }
//            merged.Add(mergedData);
//        }

//        return merged;
//    }

//    private static List<SurvivalData> ReadTSVFile(string filePath)
//    {
//        var lines = File.ReadAllLines(filePath);
//        var header = lines[0].Split('\t').ToList();
//        var filteredHeader = header.Where(h => h == "bcr_patient_barcode" || h == "DSS" || h == "OS" || h == "clinical_stage").ToList();
//        var survivalData = new List<SurvivalData>();

//        for (int i = 1; i < lines.Length; i++)
//        {
//            var columns = lines[i].Split('\t').ToList();
//            var filteredColumns = columns.Where((c, index) => header[index] == "bcr_patient_barcode" || header[index] == "type" || header[index] == "DSS" || header[index] == "OS" || header[index] == "clinical_stage").ToList();
//            var survival = new SurvivalData();
//            survival.PatientId = filteredColumns[0];
//            survival.Type = filteredColumns[1];
//            survival.ClinicalStage = filteredColumns[2];
//            survival.DSS = filteredColumns[3].ToString();
//            survival.OS = filteredColumns[4].ToString();
//            survivalData.Add(survival);
//        }

//        return survivalData;
//    }
//}

using MongoDB.Driver;
using XenaImportToMongoDb.Model;
using XenaMergeData.Model;

public class Program
{
    private static void Main(string[] args)
    {
        const string CONNECTION_STRING = "mongodb+srv://ldposavec:Pa$$w0rd@cluster0.xhqtb9b.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
        var mongoClient = new MongoClient(CONNECTION_STRING);
        var db = mongoClient.GetDatabase("MediqDatabase");
        var genes = db.GetCollection<GeneExpression>("genes");

        var geneExpressions = genes.Find(FilterDefinition<GeneExpression>.Empty).ToList();

        var filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\XenaMergeData\\ToMerge\\TCGA_clinical_survival_data.tsv";

        var survivalData = ReadTSVFile(filePath);

        survivalData.Sort((a, b) => a.Type == b.Type ? string.Compare(a.PatientId, b.PatientId, StringComparison.Ordinal) : string.Compare(a.Type, b.Type, StringComparison.Ordinal));

        var merged = MergeGenesWithSurvivalData(geneExpressions, survivalData);

        var mergedCollection = db.GetCollection<MergedData>("merged");

        mergedCollection.InsertMany(merged);

    }

    private static int _id = 0;
    private static List<MergedData> MergeGenesWithSurvivalData(List<GeneExpression> geneExpressions, List<SurvivalData> survivalData)
    {
        var merged = new List<MergedData>();

        //var survivalCounter = 0;
        //for (int i = 0; i< geneExpressions.Count; i++)
        //{
        //    var mergedData = new MergedData();
        //    mergedData.Id = ++_id;
        //    mergedData.CancerCohort = geneExpressions[i].CancerCohort;
        //    mergedData.GeneExpressionValues = geneExpressions[i].GeneExpressionValues;
        //    if (survivalCounter < survivalData.Count && geneExpressions[i].CancerCohort == survivalData[survivalCounter].Type)
        //    {
        //            mergedData.PatientId = survivalData[survivalCounter].PatientId;
        //            mergedData.DSS = survivalData[survivalCounter].DSS;
        //            mergedData.OS = survivalData[survivalCounter].OS;
        //            mergedData.ClinicalStage = survivalData[survivalCounter].ClinicalStage;
        //            survivalCounter++;
        //    }
        //    else
        //    {
        //        int foundData = survivalData.FindIndex(s => s.Type == geneExpressions[i].CancerCohort);

        //        if (foundData != -1)
        //        {
        //            survivalCounter = foundData;
        //            mergedData.PatientId = survivalData[foundData].PatientId;
        //            mergedData.DSS = survivalData[foundData].DSS;
        //            mergedData.OS = survivalData[foundData].OS;
        //            mergedData.ClinicalStage = survivalData[foundData].ClinicalStage;
        //            survivalCounter++;
        //        }
        //        else
        //        {
        //            mergedData.PatientId = geneExpressions[i].PatientId.ToString();
        //            mergedData.DSS = "Uknown";
        //            mergedData.OS = "Unknown";
        //            mergedData.ClinicalStage = "Unknown";
        //        }
        //    }
        foreach (var gene in geneExpressions)
        {
            var mergedData = new MergedData();
            mergedData.Id = ++_id;
            mergedData.CancerCohort = gene.CancerCohort;
            mergedData.GeneExpressionValues = gene.GeneExpressionValues;
            gene.PatientId = gene.PatientId.Substring(0, gene.PatientId.LastIndexOf('-'));
            mergedData.PatientId = gene.PatientId;
            var foundData = survivalData.Find(s => s.PatientId == gene.PatientId);
            if (foundData != null && gene.PatientId == foundData.PatientId)
            {
                mergedData.DSS = foundData.DSS;
                mergedData.OS = foundData.OS;
                mergedData.ClinicalStage = foundData.ClinicalStage;
            }
            else
            {
                mergedData.DSS = "Unknown";
                mergedData.OS = "Unknown";
                mergedData.ClinicalStage = "Unknown";
            }
            merged.Add(mergedData);
        }

        return merged;
    }

    private static List<SurvivalData> ReadTSVFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var header = lines[0].Split('\t').ToList();
        var filteredHeader = header.Where(h => h == "bcr_patient_barcode" || h == "DSS" || h == "OS" || h == "clinical_stage").ToList();
        var survivalData = new List<SurvivalData>();

        for (int i = 1; i < lines.Length; i++)
        {
            var columns = lines[i].Split('\t').ToList();
            var filteredColumns = columns.Where((c, index) => header[index] == "bcr_patient_barcode" || header[index] == "type" || header[index] == "DSS" || header[index] == "OS" || header[index] == "clinical_stage").ToList();
            var survival = new SurvivalData();
            survival.PatientId = filteredColumns[0];
            survival.Type = filteredColumns[1];
            survival.ClinicalStage = filteredColumns[2];
            survival.DSS = filteredColumns[3].ToString();
            survival.OS = filteredColumns[4].ToString();
            survivalData.Add(survival);
        }

        return survivalData;
    }
}