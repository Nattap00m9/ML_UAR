namespace ML_UAR.Models.ApproveMaster
{
    public class ApproveMaster
    {
        public string approve_master_id { get; set; }
        public string approve_master_name { get; set; }
        public string status { get; set; }
    }
    public class ApproveType
    {
        public int aprvtype_id { get; set; }
        public string aprvtype_name_en { get; set; }
        public string aprvtype_name_th { get; set; }
        public string isapprove { get; set; }
        public string isconfirm { get; set; }
        public string status { get; set; }
    }
}
