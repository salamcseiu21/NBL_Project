namespace NblClassLibrary.Models
{
    public static class Generator
    {
        public static string GenerateAccountCode(string prefix, int lastSlNo)  
        {
            string subSubSubAccountCode = prefix + (lastSlNo + 1);
            return subSubSubAccountCode;
        }

    }
}