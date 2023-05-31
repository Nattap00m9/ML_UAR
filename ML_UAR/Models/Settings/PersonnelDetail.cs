namespace ML_UAR.Models.Settings
{
    public class PersonnelDetail
    {
        public string personnel_code { get; set; }
        public string thname { get; set; }
        public string personnel_name_TH { get; set; }
        public string personnel_name_EN { get; set; }
        public string personnel_last_TH { get; set; }
        public string personnel_last_EN { get; set; }
        public string startdate { get; set; }
        public string telephone { get; set; }
        public string e_mail { get; set; }
        public string off_phone { get; set; }
        public string ad_account { get; set; }
        public string position_level_code { get; set; }
        public string position_code { get; set; }
        public string position { get; set; }
        public string depart_code { get; set; }
        public string department { get; set; }
        public string branch { get; set; }
        public string abbreviation { get; set; }
        public string section_code { get; set; }
        public string branch_code { get; set; }
        public string section { get; set; }
        public string area_no { get; set; }
    }
    public class ddlPersonnelList
    {
        public string personnel_code { get; set; }
        public string ddlpersonnelcode { get; set; }
    }
}
