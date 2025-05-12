namespace Service.Services.Constants;

public static class RoleConstants
{
    // Define roles here as constants
    public const string Admin = "Admin";
    public const string Employee = "Employee";
    public const string Customer = "Customer";

    // Optional: If you need normalized versions of the roles
    public const string AdminNormalized = "ADMIN";
    public const string EmployeeNormalized = "EMPLOYEE";
    public const string CustomerNormalized = "CUSTOMER";

    // You can also define Role IDs if needed
    public static readonly string AdminRoleId = "b09cf6dc-a3aa-4cdd-b1e9-f22a1d5d3149";
    public static readonly string EmployeeRoleId = "cbf2fd6c-60ba-4bb7-a066-b04cb9b5ea85";
    public static readonly string CustomerRoleId = "70f7c142-4ba0-43bb-966a-d4b8f7ea4eb6";

    public static readonly string[] AllRoles = { Admin, Customer, Employee };
}