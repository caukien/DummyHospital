using AutoMapper;
using Dummy1.Helper;
using Dummy1.HospitalDtos;
using Dummy1.Interfaces;
using Dummy1.IRepositories;
using Dummy1.Model;
using Dummy1.PantientDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dummy1.Service
{
    public class PantientService : CrudAppService<Patient, PantientDto, Guid, QueryObject, CreateUpdatePantientDto>, IPantientService
    {
        private readonly IMapper _mapper;
        private readonly IPatientRepo<Patient> _pantientRepo;
        private readonly IHospitalRepo<Hospital> _hospitalRepo;
        public PantientService(IRepository<Patient, Guid> repository, IMapper mapper, IPatientRepo<Patient> pantientRepo, IHospitalRepo<Hospital> hospitalRepo) : base(repository)
        {
            _mapper = mapper;
            _pantientRepo = pantientRepo;
            _hospitalRepo = hospitalRepo;
        }
        public override async Task<PagedResultDto<PantientDto>> GetListAsync(QueryObject input)
        {
            var hospital = await _hospitalRepo.GetHospitalByTenant();

            var items = await _pantientRepo.GetPatientByHospitalCode(input, hospital.Id);

            var totalCount = await _pantientRepo.GetTotalCount();

            var itemsDto = _mapper.Map<List<PantientDto>>(items);

            return new PagedResultDto<PantientDto>
            {
                TotalCount = totalCount,
                Items = (IReadOnlyList<PantientDto>)itemsDto
            };
        }

        public override async Task<PantientDto> CreateAsync(CreateUpdatePantientDto input)
        {
            var hospitalId = await _hospitalRepo.GetHospitalByTenant();
            input.HospitalId = hospitalId.Id;
            return await base.CreateAsync(input);
        }

        public override async Task<PantientDto> UpdateAsync(Guid id, CreateUpdatePantientDto input)
        {
            var hospitalId = await _hospitalRepo.GetHospitalByTenant();
            input.HospitalId = hospitalId.Id;
            return await base.UpdateAsync(id, input);
        }
    }
}
