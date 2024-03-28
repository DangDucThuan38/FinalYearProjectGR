using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.IServices;
using Microsoft.AspNetCore.Authorization;

namespace DangDucThuanFinalYear.Endpoints
{
    public static class Endpoints
    {
        public static IEndpointRouteBuilder MapCustomeEndPoint(this IEndpointRouteBuilder builder)
        {
            var staffAdminGroup = builder.MapGroup("/staff-admin").RequireAuthorization()
                .RequireAuthorization(authorizationPolicyBuilder => authorizationPolicyBuilder.RequireRole(RoleType.Admin.ToString(), RoleType.Staff.ToString()));

            staffAdminGroup.MapPost("/manage-amenitites/delete/{amenityId:int}",
                async (int amenityId, IAmenititesService amenitiesService) =>
                {
                    await amenitiesService.DeleteAmenitityAsyns(amenityId);
                    return TypedResults.LocalRedirect("~/staff-admin/manage-amenitites");
                });

            staffAdminGroup.MapPost("/manage-finances/delete/{financeId:int}",
               async (int financeId, IFinanceService financeService) =>
               {
                   await financeService.DeleteAmenitityAsyns(financeId);
                   return TypedResults.LocalRedirect("~/staff-admin/manage-finances");
               });
            return builder;


        }

    }
}
