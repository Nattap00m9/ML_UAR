namespace ML_UAR.Models.Settings
{
    public class ReasonMaster
    {
       public string reason_code { get; set; }
       public string reason_description { get; set; }
       public string reason_status_code { get; set; }
       public string status { get; set; }
    }

    public class ReasonStatusMaster 
    {
        public string reason_status_code { get; set; }
        public string reason_status_description { get; set; }
        public string status { get; set; }
    }

    public class ddlReasonList
    { 
        public string reason_code { get; set; }
        public string reason_description { get; set; }
        
    }

}
