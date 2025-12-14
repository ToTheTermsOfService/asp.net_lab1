using API.Protos;
using Core.Entities;
using Core.Interfaces;
using Grpc.Core;
using TenantService = API.Protos.TenantService;

namespace API.Services;

public class GrpcTenantService: TenantService.TenantServiceBase
{
    private readonly ITenantServiceRepository _repository;
    private readonly ILogger<GrpcTenantService> _logger;

    public GrpcTenantService(
        ITenantServiceRepository repository,
        ILogger<GrpcTenantService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public override async Task<TenantsResponse> GetAllTenants(Empty request, ServerCallContext context)
    {
        var tenants = _repository.GetAllTenantsWithServices();

        var response = new TenantsResponse();
        foreach (var tenant in tenants)
        {
            response.Tenants.Add(MapToTenantResponse(tenant));
        }

        return await Task.FromResult(response);
    }

    public override async Task<TenantResponse> GetTenantById(
            GetTenantRequest request,
            ServerCallContext context)
    {
        var tenant = _repository.GetTenantById(request.Id);
        if (tenant == null)
        {
            throw new RpcException(new Status(
                StatusCode.NotFound,
                $"Tenant with ID {request.Id} not found"));
        }

        return await Task.FromResult(MapToTenantResponse(tenant));
    }

    public override async Task<TenantResponse> CreateTenant(
            CreateTenantRequest request,
            ServerCallContext context)
    {
        var tenant = new Tenant
        {
            LastName = request.LastName,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            Address = request.Address,
            PersonalAccount = request.PersonalAccount,
            ResidentsCount = request.ResidentsCount,
            ApartmentArea = (decimal)request.ApartmentArea
        };

        var created = _repository.AddTenant(tenant);

        return await Task.FromResult(MapToTenantResponse(created));
    }

    public override async Task<TenantResponse> UpdateTenant(
            UpdateTenantRequest request,
            ServerCallContext context)
    {
        var tenant = new Tenant
        {
            Id = request.Id,
            LastName = request.LastName,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            Address = request.Address,
            PersonalAccount = request.PersonalAccount,
            ResidentsCount = request.ResidentsCount,
            ApartmentArea = (decimal)request.ApartmentArea
        };

        var updated = _repository.UpdateTenant(request.Id, tenant);
        if (updated == null)
        {
            throw new RpcException(new Status(
                StatusCode.NotFound,
                $"Tenant with ID {request.Id} not found"));
        }

        return await Task.FromResult(MapToTenantResponse(updated));
    }
    public override async Task<DeleteResponse> DeleteTenant(
            DeleteTenantRequest request,
            ServerCallContext context)
    {
        var deleted = _repository.DeleteTenant(request.Id);

        return await Task.FromResult(new DeleteResponse { Success = deleted });
    }

    public override async Task<Empty> AddServiceToTenant(
        AddServiceRequest request,
        ServerCallContext context)
    {
        _repository.AddServiceToTenant(request.TenantId, request.ServiceId);

        return await Task.FromResult(new Empty());
    }

    private TenantResponse MapToTenantResponse(Core.Dto.TenantWithServices tenant)
    {
        var response = new TenantResponse
        {
            Id = tenant.Id,
            LastName = tenant.LastName,
            FirstName = tenant.FirstName,
            MiddleName = tenant.MiddleName ?? string.Empty,
            Address = tenant.Address,
            PersonalAccount = tenant.PersonalAccount,
            ResidentsCount = tenant.ResidentsCount,
            ApartmentArea = (double?)tenant.ApartmentArea ?? 0
        };

        foreach (var service in tenant.Services)
        {
            response.Services.Add(new ServiceResponse
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                BillingType = service.BillingType,
                Tariff = (double)service.Tariff,
                CalculatedAmount = (double)(service.CalculatedAmount ?? 0)
            });
        }

        return response;
    }

    private TenantResponse MapToTenantResponse(Tenant tenant)
    {
        return new TenantResponse
        {
            Id = tenant.Id,
            LastName = tenant.LastName,
            FirstName = tenant.FirstName,
            MiddleName = tenant.MiddleName ?? string.Empty,
            Address = tenant.Address,
            PersonalAccount = tenant.PersonalAccount,
            ResidentsCount = tenant.ResidentsCount,
            ApartmentArea = (double)tenant.ApartmentArea
        };
    }
}
