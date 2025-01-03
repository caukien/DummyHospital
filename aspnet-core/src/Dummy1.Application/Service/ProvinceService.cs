using AutoMapper;
using Dummy1.Area;
using Dummy1.EntityFrameworkCore;
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
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;


namespace Dummy1.Service
{
    //public class ProvinceService : ApplicationService, IProvinceService
    //{
    //    private readonly Dummy1DbContext _context;
    //    private readonly IMapper _mapper;
    //    private readonly IProvinceRepo _provinceRepo;

    //    public ProvinceService(IMapper mapper,
    //        IProvinceRepo provinceRepo,
    //        Dummy1DbContext context)
    //    {
    //        _mapper = mapper;
    //        _provinceRepo = provinceRepo;
    //        _context = context;
    //    }

    //    public async Task Create(ProvinceDto province)
    //    {
    //        _mapper.Map<Province>(province);
    //        _context.Add(province);
    //        await _context.SaveChangesAsync();
    //    }

    //    public Task Delete(string id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<List<Province>> GetAllProvince()
    //    {
    //        try
    //        {
    //            var items = await _provinceRepo.GetAll();
    //            return items;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex);
    //            throw new Exception("An error occurred while retrieving provinces.", ex);
    //        }
    //    }

    //    public Task<Province> GetOne()
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public Task Update(ProvinceDto province)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task UploadFile(IFormFile file)
    //    {
    //        ExcelPackage.LicenseContext = LicenseContext.Commercial;

    //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    //        if (file == null) throw new ArgumentNullException();

    //        var fileExtension = Path.GetExtension(file.FileName);
    //        if (fileExtension != ".xlsx" && fileExtension != ".xls")
    //        {
    //            throw new InvalidDataException("File is not a valid Excel file.");
    //        }

    //        if (file.Length > 5 * 1024 * 1024) throw new Exception("File size should be less than 5MB.");

    //        try
    //        {
    //            using (var stream = new MemoryStream())  // Khai báo và khởi tạo stream ở đây
    //            {
    //                await file.CopyToAsync(stream);  // Lưu file vào memory stream

    //                using (var package = new ExcelPackage(stream))  // Sử dụng EPPlus để đọc Excel
    //                {
    //                    var worksheet = package.Workbook.Worksheets[0];  // Lấy worksheet đầu tiên
    //                    var rowCount = worksheet.Dimension.Rows;
    //                    var provinceList = new List<Province>();

    //                    for (int row = 2; row <= rowCount; row++)  // Bắt đầu từ dòng 2 nếu dòng 1 là header
    //                    {
    //                        var provinceDto = new CreateUpdateProvinceDto
    //                        {
    //                            // Đọc các cột trong file và ánh xạ vào DTO
    //                            Code = int.Parse(worksheet.Cells[row, 1].Text),  // Cột 1: Mã
    //                            ProvinceName = worksheet.Cells[row, 2].Text,  // Cột 2: Tên
    //                            ProvinceLevel = (ProvinceLevel)ParseProvinceLevel(worksheet.Cells[row, 4].Text) // Cột 4: Cấp
    //                        };

    //                        // Ánh xạ từ CreateUpdateProvinceDto sang Province
    //                        //var province = _mapper.Map<Province>(provinceDto);

    //                        provinceList.Add(new Province
    //                        {
    //                            Code = provinceDto.Code,
    //                            ProvinceName = provinceDto.ProvinceName,
    //                            ProvinceLevel = provinceDto.ProvinceLevel,
    //                        });
    //                    }

    //                    // Lưu danh sách province vào cơ sở dữ liệu nếu cần
    //                    await _context.AddRangeAsync(provinceList);
    //                    await _context.SaveChangesAsync();
    //                }
    //            }
    //        }catch(Exception ex)
    //        {
    //            throw new Exception("", ex);
    //        }
    //    }

    //    private int ParseProvinceLevel(string level)
    //    {
    //        // Kiểm tra giá trị cột ProvinceLevel và trả về giá trị tương ứng
    //        return level switch
    //        {
    //            "Thành phố Trung ương" => 2,
    //            "Tỉnh" => 1,
    //            _ => 0,  // Nếu không khớp với bất kỳ giá trị nào thì là Undefined
    //        };
    //    }

    //    public async Task CreateTemp(Province province)
    //    {
    //        await _context.Provinces.AddAsync(province);
    //    }


    //}


    public class ProvinceService : CrudAppService<
        Province,
        ProvinceDto,
        Guid,
        QueryObject,
        CreateUpdateProvinceDto
        >, IProvinceService
    {
        //private readonly IMapper _mapper;
        private readonly IDapperBasicRepo<Province> _dapperBasicRepo;
        private readonly Dummy1DbContext _context;
        private readonly IObjectMapper _objectMapper;

        public ProvinceService(IRepository<Province, Guid> repository, IDapperBasicRepo<Province> dapperBasicRepo, Dummy1DbContext context, IObjectMapper objectMapper) : base(repository)
        {
            //_mapper = mapper;
            _dapperBasicRepo = dapperBasicRepo;
            _context = context;
            _objectMapper = objectMapper;
        }

        //public ProvinceService(IRepository<Province, Guid> repository) : base(repository)
        //{

        //}
        public override async Task<ProvinceDto> CreateAsync(CreateUpdateProvinceDto input)
        {
            await CheckDistrictCodeExistsAsync(input.Code);
            return await base.CreateAsync(input);
        }

        public override async Task<PagedResultDto<ProvinceDto>> GetListAsync(QueryObject input)
        {
            var items = await _dapperBasicRepo.GetAll(input);

            var totalRecord = await _dapperBasicRepo.GetTotalCount();

            var itemsDto = _objectMapper.Map<List<Province>, List<ProvinceDto>>(items);

            return new PagedResultDto<ProvinceDto>
            {
                TotalCount = totalRecord,
                Items = itemsDto
            };
        }
        private async Task CheckDistrictCodeExistsAsync(int code)
        {
            var exists = await Repository.AnyAsync(district => district.Code == code);
            if (exists)
            {
                throw new BusinessException("District code already exists.");
            }
        }

        public override async Task<ProvinceDto> GetAsync(Guid id)
        {
            var item = _objectMapper.Map<Province, ProvinceDto>(await _dapperBasicRepo.GetOne(id));

            return item;
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
                        var provinceList = new List<Province>();

                        for (int row = 2; row <= rowCount; row++)  // Bắt đầu từ dòng 2 nếu dòng 1 là header
                        {
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text) || // Code
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text) || // ProvinceName
                                string.IsNullOrWhiteSpace(worksheet.Cells[row, 4].Text))  // ProvinceLevel
                            {
                                continue; // Skip this row
                            }

                            int code = int.Parse(worksheet.Cells[row, 1].Text);

                            // Check if the code already exists in the database
                            if (await Repository.AnyAsync(province => province.Code == code))
                            {
                                continue;
                            }

                            var provinceDto = new CreateUpdateProvinceDto
                            {
                                // Đọc các cột trong file và ánh xạ vào DTO
                                Code = int.Parse(worksheet.Cells[row, 1].Text),  // Cột 1: Mã
                                ProvinceName = worksheet.Cells[row, 2].Text,  // Cột 2: Tên
                                ProvinceLevel = (ProvinceLevel)ParseProvinceLevel(worksheet.Cells[row, 4].Text) // Cột 4: Cấp
                            };

                            // Ánh xạ từ CreateUpdateProvinceDto sang Province
                            var province = _objectMapper.Map<CreateUpdateProvinceDto, Province>(provinceDto);

                            provinceList.Add(province);
                        }

                        // Lưu danh sách province vào cơ sở dữ liệu nếu cần
                        await Repository.InsertManyAsync(provinceList);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        private int ParseProvinceLevel(string level)
        {
            
            return level switch
            {
                "Thành phố Trung ương" => 2,
                "Tỉnh" => 1,
                _ => 0,
            };
        }
    }
}
