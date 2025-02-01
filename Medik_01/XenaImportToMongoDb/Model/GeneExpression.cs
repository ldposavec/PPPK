//using MongoDB.Bson.Serialization.Attributes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace XenaImportToMongoDb.Model
//{
//    public class GeneExpression
//    {
//        [BsonId]
//        public long Id { get; set; }
//        [BsonElement("PatientId")]
//        public long PatientId { get; set; }
//        [BsonElement("CancerCohort")]
//        public string CancerCohort { get; set; }
//        [BsonElement("GeneExpressionValues")]
//        public Dictionary<string, double> GeneExpressionValues { get; set; }

//        public override string ToString()
//        {
//            return $"Id: {Id}, PatientId: {PatientId}, CancerCohort: {CancerCohort}, GeneExpressionValues: {string.Join(", ", GeneExpressionValues.Select(x => $"{x.Key}: {x.Value}"))}";
//        }

//    }
//}

using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenaImportToMongoDb.Model
{
    public class GeneExpression
    {
        [BsonId]
        public long Id { get; set; }
        [BsonElement("PatientId")]
        public string PatientId { get; set; }
        [BsonElement("CancerCohort")]
        public string CancerCohort { get; set; }
        [BsonElement("GeneExpressionValues")]
        public Dictionary<string, double> GeneExpressionValues { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, PatientId: {PatientId}, CancerCohort: {CancerCohort}, GeneExpressionValues: {string.Join(", ", GeneExpressionValues.Select(x => $"{x.Key}: {x.Value}"))}";
        }

    }
}

