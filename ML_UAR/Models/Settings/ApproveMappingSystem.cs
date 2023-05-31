namespace ML_UAR.Models.Settings
{
    public class ApproveMappingSystem
    {
        public string system_id { get; set; }
        public string system_name { get; set; }
        public int approve_master_id { get; set; }
        public string approve_master_name { get; set; }
        public int isstandard { get; set; }


    }

    public class ddlSystemList
    {
        public string system_id { get; set; }
        public string system_name { get; set; }

    }

    public class FormApproveMappingSystem
    {
        public string ddlSystem_id { get; set; }
        public int ddlApprove_master_id { get; set; }
        public int isstandard { get; set; }
        public int isUpdate { get; set; }
    }

}
