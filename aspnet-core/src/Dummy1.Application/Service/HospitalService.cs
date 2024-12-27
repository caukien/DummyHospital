using AutoMapper;
using Dummy1.CommuneDtos;
using Dummy1.Helper;
using Dummy1.HospitalDtos;
using Dummy1.Interfaces;
using Dummy1.IRepositories;
using Dummy1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Dummy1.Service
{
    public class HospitalService : CrudAppService<
            Hospital,
            HospitalDto,
            Guid,
            QueryObject,
            CreateUpdateHospitalDto
        >, IHospitalService
    {
        private readonly IMapper _mapper;
        private readonly IHospitalRepo<Hospital> _dapperBasicRepo;
        private readonly ICurrentTenant _currentTenant;
        private readonly ICurrentUser _currentUser;
        public HospitalService(IRepository<Hospital, Guid> repository, IMapper mapper, IHospitalRepo<Hospital> dapperBasicRepo, ICurrentTenant currentTenant, ICurrentUser currentUser) : base(repository)
        {
            _mapper = mapper;
            _dapperBasicRepo = dapperBasicRepo;
            _currentTenant = currentTenant;
            _currentUser = currentUser;
        }
        public override async Task<PagedResultDto<HospitalDto>> GetListAsync(QueryObject input)
        {
            var items = await _dapperBasicRepo.GetAll(input);

            var totalCount = await _dapperBasicRepo.GetTotalCount();

            var itemsDto = _mapper.Map<List<HospitalDto>>(items);

            return new PagedResultDto<HospitalDto>
            {
                TotalCount = totalCount,
                Items = (IReadOnlyList<HospitalDto>)itemsDto
            };
        }

        public override async Task<HospitalDto> CreateAsync(CreateUpdateHospitalDto input)
        {
            if(_currentUser.TenantId != null)
            {
                throw new BusinessException("Chỉ được tạo 1 bệnh viện duy nhất");
            }
            var hospital = ObjectMapper.Map<CreateUpdateHospitalDto, Hospital>(input);

            // Set the current tenant id
            hospital.TenantId = _currentTenant.Id ?? Guid.Empty;

            // Save the hospital entity
            await Repository.InsertAsync(hospital);

            // Return the created HospitalDto
            return MapToGetOutputDto(hospital);
        }
    }
}
