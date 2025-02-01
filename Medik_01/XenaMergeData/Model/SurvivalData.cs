using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenaMergeData.Model
{
    public class SurvivalData : IComparable<SurvivalData>
    {
        [BsonElement("bcr_patient_barcode")]
        public string PatientId { get; set; }

        [BsonElement("DSS")]
        public string DSS { get; set; }

        [BsonElement("OS")]
        public string OS { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("clinical_stage")]
        public string ClinicalStage { get; set; }

        public int CompareTo(SurvivalData? other)
        {
            if (other == null)
            {
                return 1;
            }
            return string.Compare(PatientId, other.PatientId, StringComparison.Ordinal);
        }
    }
}
