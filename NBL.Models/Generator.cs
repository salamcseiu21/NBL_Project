namespace NBL.Models
{
    public static class Generator
    {
        public static string GenerateAccountCode(string prefix, int lastSlNo)  
        {
            string subSubSubAccountCode = prefix + (lastSlNo + 1);
            return subSubSubAccountCode;
        }

        public static string GenerateEmployeeNo(Employee employee,int lastSn)
        {
            string empNo = $"NBL-{employee.EmployeeTypeId:D2}{employee.DepartmentId:D2}{employee.DesignationId:D2}{lastSn+1:D3}";
            return empNo;
        }
    }
}