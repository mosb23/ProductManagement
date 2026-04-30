namespace ProductManagement_V2.Application.Common.Constants
{
    public static class AppRoles
    {
        public const string ProjectManager = "ProjectManager";
        public const string Supervisor = "Supervisor";
        public const string WarehouseManager = "WarehouseManager";

        public static readonly string[] All =
        {
            ProjectManager,
            Supervisor,
            WarehouseManager
        };
    }
}