namespace ML_UAR.Models.ApproveMaster
{
    public class FormApproveMaster
    {
    }
    public class FormAddApproveMaster
    {
        //approve master
        public int approve_master_id { get; set; }
        public string txtApproveName { get; set; }
        // approve flow
        public List<string> txtApproveLevel { get; set; }
        public List<string> txtPersonnelCode { get; set; }
        public List<string> txtEmail { get; set; }
        public List<string> txtBackUPEmail { get; set; }
        public List<string> txtStatus { get; set; }
        public List<int> txtApproveTypeID { get; set; }
        public List<string> txtisFinal { get; set; }
    }
    public class FormEditApproveMaster
    {
        //ApproveMaster
        public int txtApproveMasterID { get; set; }
        public string txtApproveName { get; set; }
        public string raiApproveMasterStatus { get; set; }
        //ApproveFlow 
        
        public List<int> txtApproveFlowID { get; set; }
        public List<int> txtisDelete { get; set; }
        public List<int> txtisNewFlow { get; set; }
        public List<string> txtApproveLevel { get; set; }
        public List<string> txtPersonnelCode { get; set; }
        public List<string> txtApproveTypeID { get; set; }
        public List<string> txtBackUPEmail { get; set; }
        public List<string> txtStatus { get; set; }
        public List<string> txtReasonCode { get; set; }
        public List<string> txtRemark { get; set; }
        public List<string> txtisFinal { get; set; }
    }
}
