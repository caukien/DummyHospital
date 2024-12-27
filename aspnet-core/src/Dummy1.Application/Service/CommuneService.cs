using AutoMapper;
using Dummy1.Area;
using Dummy1.CommuneDtos;
using Dummy1.DistrictDtos;
using Dummy1.Helper;
using Dummy1.Interfaces;
using Dummy1.IRepositories;
using Dummy1.Model;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dummy1.Service
{
    public class CommuneService : CrudAppService<
            Commune,
            CommuneDto,
            Guid,
            QueryObject,
            CreateUpdateCommuneDto
        >, ICommuneService
    {
        private readonly IMapper _mapper;
        private readonly ICommuneRepo<Commune> _dapperBasicRepo;
        public CommuneService(IRepository<Commune, Guid> repository, IMapper mapper, ICommuneRepo<Commune> dapperBasicRepo) : base(repository)
        {
            _mapper = mapper;
            _dapperBasicRepo = dapperBasicRepo;
        }
        public override async Task<PagedResultDto<CommuneDto>> GetListAsync(QueryObject input)
        {
            var items = await _dapperBasicRepo.GetAll(input);

            var totalRecord = await _dapperBasicRepo.GetTotalCount();

            var itemsDto = _mapper.Map<List<CommuneDto>>(items);

            return new PagedResultDto<CommuneDto>
            {
                TotalCount = totalRecord,
                Items = (IReadOnlyList<CommuneDto>)itemsDto
            };
        }

        public override async Task<CommuneDto> CreateAsync(CreateUpdateCommuneDto input)
        {
            await CheckExistsAsync(input.CommuneCode);
            return await base.CreateAsync(input);
        }

        private async Task CheckExistsAsync(int code)
        {
            var exists = await Repository.AnyAsync(item => item.CommuneCode == code);
            if (exists)
            {
                throw new BusinessException("already exists.");
            }
        }

        public async Task ImportFile(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length < 0) throw new ArgumentNullException();

            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xlsx" && fileExtension != ".xls")
            {
                throw new InvalidDataException("File is not a valid Excel file.");
            }

            if (file.Length > 5 * 1024 * 1024) throw new Exception("File size should be less than 5MB.");

            try
            {
                using (var stream = new MemoryStream())  // Khai báo và khởi tạo stream ở đây
                {
                    await file.CopyToAsync(stream);  // Lưu file vào memory stream

                    using (var package = new ExcelPackage(stream))  // Sử dụng EPPlus để đọc Excel
                    {
                        var worksheet = package.Workbook.Worksheets[0];  // Lấy worksheet đầu tiên
                        var rowCount = 5;
                        var communeList = new List<Commune>();

                        for (int row = 2; row <= rowCount; row++)  // Bắt đầu từ dòng 2 nếu dòng 1 là header
                        {
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text) || // Code
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text) || // DistrictName
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 4].Text) || // DistrictLevel
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 5].Text) // ProvinceCode
                                )
                            {
                                continue; // Skip this row
                            }

                            int code = int.Parse(worksheet.Cells[row, 1].Text);

                            // Check if the code already exists in the database
                            if (await Repository.AnyAsync(c => c.CommuneCode == code))
                            {
                                continue;
                            }

                            var communeDto = new CreateUpdateCommuneDto
                            {
                                // Đọc các cột trong file và ánh xạ vào DTO
                                CommuneCode = code,  // Cột 1: Mã
                                CommuneName = worksheet.Cells[row, 2].Text,  // Cột 2: Tên
                                CommuneEnName = string.IsNullOrEmpty(worksheet.Cells[row, 3].Text)
                                    ? string.Empty
                                    : worksheet.Cells[row, 3].Text,  // Cột 2: Tên Tiếng Anh
                                CommuneLevel = (CommuneLevel)ParseLevel(worksheet.Cells[row, 4].Text), // Cột 4: Cấp
                                DistrictCode = int.Parse(worksheet.Cells[row, 5].Text),  // Cột 5: Mã Quận/Huyện
                            };

                            // Ánh xạ từ CreateUpdateProvinceDto sang Province
                            var commune = _mapper.Map<Commune>(communeDto);

                            communeList.Add(commune);
                        }

                        // Lưu danh sách province vào cơ sở dữ liệu nếu cần
                        await Repository.InsertManyAsync(communeList);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        private int ParseLevel(string level)
        {
            return level switch
            {
                "Thị trấn" => 3,
                "Xã" => 2,
                "Phường" => 1,
                _ => 0,
            };
        }

        public async Task<List<CommuneDto>> GetCommuneByDistrictCode(int districtCode)
        {
            var items = await _dapperBasicRepo.GetCommuneByDistrictCode(districtCode);
            var itemsDto = _mapper.Map<List<CommuneDto>>(items);
            return itemsDto;
        }
    }
}
