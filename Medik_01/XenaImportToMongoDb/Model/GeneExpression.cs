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
        public long PatientId { get; set; }
        [BsonElement("CancerCohort")]
        public string CancerCohort { get; set; }
        [BsonElement("GeneExpressionValues")]
        public Dictionary<string, double> GeneExpressionValues { get; set; }

        
    }
}
