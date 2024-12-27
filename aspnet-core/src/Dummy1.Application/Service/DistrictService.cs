using AutoMapper;
using Dummy1.Area;
using Dummy1.DistrictDtos;
using Dummy1.Helper;
using Dummy1.Interfaces;
using Dummy1.IRepositories;
using Dummy1.Model;
using Dummy1.ProvinceDtos;
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
    public class DistrictService : CrudAppService<
        District, 
        DistrictDto, 
        Guid, 
        QueryObject, 
        CreateUpdateDistrictDto>, IDistrictService
    {
        private readonly IMapper _mapper;
        private readonly IDistrictRepo<District> _dapperBasicRepo;
        public DistrictService(IRepository<District, Guid> repository, IDistrictRepo<District> dapperBasicRepo, IMapper mapper) : base(repository)
        {
            _dapperBasicRepo = dapperBasicRepo;
            _mapper = mapper;
        }
        private async Task CheckExistsAsync(int code)
        {
            var exists = await Repository.AnyAsync(item => item.Code == code);
            if (exists)
            {
                throw new BusinessException("already exists.");
            }
        }
        public override async Task<DistrictDto> CreateAsync(CreateUpdateDistrictDto input)
        {
            await CheckExistsAsync(input.Code);
            return await base.CreateAsync(input);
        }

        public override async Task<PagedResultDto<DistrictDto>> GetListAsync(QueryObject input)
        {
            var items = await _dapperBasicRepo.GetAll(input);

            var totalRecord = await _dapperBasicRepo.GetTotalCount();

            var itemsDto = _mapper.Map<List<DistrictDto>>(items);

            return new PagedResultDto<DistrictDto>
            {
                TotalCount = totalRecord,
                Items = (IReadOnlyList<DistrictDto>)itemsDto
            };
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
                        var rowCount = worksheet.Dimension.Rows;
                        var districtList = new List<District>();

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
                            if (await Repository.AnyAsync(d => d.Code == code))
                            {
                                continue;
                            }

                            var districtDto = new CreateUpdateDistrictDto
                            {
                                // Đọc các cột trong file và ánh xạ vào DTO
                                Code = int.Parse(worksheet.Cells[row, 1].Text),  // Cột 1: Mã
                                DistrictName = worksheet.Cells[row, 2].Text,  // Cột 2: Tên
                                DistrictEnName = string.IsNullOrEmpty(worksheet.Cells[row, 3].Text)
                                    ? string.Empty
                                    : worksheet.Cells[row, 3].Text,  // Cột 2: Tên Tiếng Anh
                                DistrictLevel = (DistrictLevel)ParseLevel(worksheet.Cells[row, 4].Text), // Cột 4: Cấp

                                ProvinceCode = int.Parse(worksheet.Cells[row, 5].Text)  // Cột 5: Mã Tỉnh/Thành phố
                            };

                            // Ánh xạ từ CreateUpdateProvinceDto sang Province
                            var district = _mapper.Map<District>(districtDto);

                            districtList.Add(district);
                        }

                        // Lưu danh sách province vào cơ sở dữ liệu nếu cần
                        await Repository.InsertManyAsync(districtList);
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
                "Thành phố" => 4,
                "Thị xã" => 3,
                "Huyện" => 2,
                "Quận" => 1,
                _ => 0,
            };
        }

        public async Task<List<DistrictDto>> GetDistrictByProvinceCode(int provinceCode)
        {
            var districts = await _dapperBasicRepo.GetDistrictByProvinceCode(provinceCode);
            var itemsDto = _mapper.Map<List<DistrictDto>>(districts);
            return itemsDto;
        }
    }
}
