namespace NblClassLibrary.Models
{
    public static class Status
    {
        public static string GetOrderStatus(int value)
        {

            switch (value)
            {
                case 0:
                    return "<p class='text-danger'>Pending</p>";

                case 1:
                    return "Approved By NSM";
                case 2:
                    return "Approved By Accounts";
                case 3:
                    return "Partially Delivered";
                case 4:
                    return "Delivered";
            }

            return "<p class='text-danger'>Pending</p>";
        }
    }
}