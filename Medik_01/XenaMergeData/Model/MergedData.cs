using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenaMergeData.Model
{
    public class MergedData
    {
        [BsonId]
        public long Id { get; set; }

        [BsonElement("bcr_patient_barcode")]
        public string PatientId { get; set; }

        [BsonElement("DSS")]
        public string DSS { get; set; }

        [BsonElement("OS")]
        public string OS { get; set; }

        [BsonElement("clinical_stage")]
        public string ClinicalStage { get; set; }

        [BsonElement("CancerCohort")]
        public string CancerCohort { get; set; }
        [BsonElement("GeneExpressionValues")]
        public Dictionary<string, double> GeneExpressionValues { get; set; }
    }
}
