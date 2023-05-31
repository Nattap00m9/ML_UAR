using System.Data;

namespace ML_UAR.Models.Settings
{
    public class PositionMappingSystem
    {
        public string position_code { get; set; }
        public string position_name { get; set; }
        public string status { get; set; }
        public string depart_code { get; set; }
        public string depart_name { get; set; }
        public string section_code { get; set; }
        public string section_name { get; set; }

    }

    public class PositionMappingLine
    {
        public string position_code { get; set; }
        public string system_id { get; set; }
        public string system_name { get; set; }
        public string role_id { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string approve_master_id { get; set; }
        public string isoldlog { get; set; }
    }


    public class ddlSectionMasterList
    {
        public string section_code { get; set; }
        public string section_name { get; set; }
    }

    public class SystemList
    {
        public string system_id { get; set; }
        public string system_name { get; set; }

    }

    public class ddlRoleMasterList
    {
        public string role_id { get; set; }
        public string role_name { get; set; }
    }

    public class FormAddPostionMappingSystem
    {
        public string ddlposition_code { get; set; }
        public List<string> txtsystem_id { get; set; }
        public List<string> txtdescription { get; set; }
        public List<string> ddlrole_id { get; set; }
        public List<string> txtstatus { get; set; }
        public List<int> txtisUpdate { get; set; }
        public List<string> txtreason_code { get; set; }
        public List<string> txtremark { get; set; }


    }
}
