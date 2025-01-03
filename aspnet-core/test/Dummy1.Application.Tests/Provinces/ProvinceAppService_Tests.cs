using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using AutoMoq;
using Dapper;
using Dummy1.Area;
using Dummy1.EntityFrameworkCore;
using Dummy1.Helper;
using Dummy1.Interfaces;
using Dummy1.IRepositories;
using Dummy1.Model;
using Dummy1.ProvinceDtos;
using Dummy1.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Dummy1.Provinces
{
    public class ProvinceAppService_Tests
    {
        private readonly Mock<IRepository<Province, Guid>> _mockRepository;
        private readonly Mock<IObjectMapper> _mockMapper;
        private readonly Mock<IDapperBasicRepo<Province>> _mockDapperRepo;
        private readonly ProvinceService _provinceService;

        public ProvinceAppService_Tests()
        {
            _mockRepository = new Mock<IRepository<Province, Guid>>();
            _mockMapper = new Mock<IObjectMapper>();
            _mockDapperRepo = new Mock<IDapperBasicRepo<Province>>();

            _provinceService = new ProvinceService(
                _mockRepository.Object,
                
                _mockDapperRepo.Object,
                new Dummy1DbContext(new DbContextOptions<Dummy1DbContext>()),
                _mockMapper.Object
                
            );
        }

        [Fact]
        public async Task GetListAsync_Should_Return_Paged_Result()
        {
            // Arrange
            var queryObject = new QueryObject { SkipCount = 1, MaxResultCount = 10 };
            var provinces = new List<Province>
            {
                new Province { Code = 101, ProvinceName = "Sample Province 1", ProvinceLevel = ProvinceLevel.Undefined },
                new Province { Code = 102, ProvinceName = "Sample Province 2", ProvinceLevel = ProvinceLevel.Tinh }
            };

            _mockDapperRepo.Setup(repo => repo.GetAll(queryObject))
                .ReturnsAsync(provinces);

            _mockDapperRepo.Setup(repo => repo.GetTotalCount())
                .ReturnsAsync(provinces.Count);

            var provinceDtos = provinces.Select(p => new ProvinceDto { Code = p.Code, ProvinceName = p.ProvinceName, ProvinceLevel = p.ProvinceLevel }).ToList();

            _mockMapper.Setup(mapper => mapper.Map<List<Province>,List<ProvinceDto>>(provinces))
                .Returns(provinceDtos);

            // Act
            var result = await _provinceService.GetListAsync(queryObject);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(provinceDtos.Count, result.Items.Count);
            Assert.Equal(provinces.Count, result.TotalCount);
        }

        [Fact]
        public async Task GetAsync_Should_Return_Province_By_Id()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedEntity = new Province { Code = 101, ProvinceName = "Sample Province", ProvinceLevel = ProvinceLevel.Tinh };
            var expectedDto = new ProvinceDto { Id = id, Code = 101, ProvinceName = "Sample Province", ProvinceLevel = ProvinceLevel.Undefined };

            _mockDapperRepo.Setup(repo => repo.GetOne(id))
                .ReturnsAsync(expectedEntity);

            _mockMapper.Setup(mapper => mapper.Map<Province, ProvinceDto>(expectedEntity))
                .Returns(expectedDto);

            // Act
            var result = await _provinceService.GetAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Code, result.Code);
            Assert.Equal(expectedDto.ProvinceName, result.ProvinceName);
        }

        [Fact]
        public async Task ImportFile_Should_ThrowException_When_FileIsInvalid()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidDataException>(() => _provinceService.ImportFile(fileMock.Object));
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Province_By_Id()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.DeleteAsync(id, false, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _provinceService.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(id, false, It.IsAny<CancellationToken>()), Times.Once);
        }

    }


    //public class ProvinceAppService_Tests
    //{
    //    private readonly IFixture _fixture;
    //    private readonly Mock<IRepository<Province, Guid>> _mockRepository;
    //    private readonly Mock<IObjectMapper> _mockMapper;
    //    private readonly Mock<IDapperBasicRepo<Province>> _mockDapperRepo;
    //    private readonly Mock<Dummy1DbContext> _mockContext;
    //    private readonly IProvinceService _provinceService;

    //    public ProvinceAppService_Tests()
    //    {
    //        _fixture = new Fixture().Customize(new AutoMoqCustomization());

    //        _mockRepository = _fixture.Freeze<Mock<IRepository<Province, Guid>>>();
    //        _mockMapper = _fixture.Freeze<Mock<IObjectMapper>>();
    //        _mockDapperRepo = _fixture.Freeze<Mock<IDapperBasicRepo<Province>>>();
    //        _mockContext = _fixture.Freeze<Mock<Dummy1DbContext>>();

    //        _provinceService = new ProvinceService(
    //            _mockRepository.Object,
    //            _mockDapperRepo.Object,
    //            _mockContext.Object,
    //            _mockMapper.Object
    //        );
    //    }



    //    [Fact]
    //    public async Task GetListAsync_ShouldReturnPagedResult()
    //    {
    //        // Arrange
    //        var query = _fixture.Create<QueryObject>();
    //        var provinces = _fixture.CreateMany<Province>(5).AsList();
    //        var totalCount = provinces.Count;

    //        _mockDapperRepo
    //            .Setup(repo => repo.GetAll(query))
    //            .ReturnsAsync(provinces);

    //        _mockDapperRepo
    //            .Setup(repo => repo.GetTotalCount())
    //            .ReturnsAsync(totalCount);

    //        _mockMapper
    //            .Setup(m => m.Map<List<Province>, List<ProvinceDto>>(It.IsAny<List<Province>>()))
    //            .Returns(_fixture.CreateMany<ProvinceDto>(5).AsList());

    //        // Act
    //        var result = await _provinceService.GetListAsync(query);

    //        // Assert
    //        Assert.NotNull(result);
    //        Assert.Equal(totalCount, result.TotalCount);
    //        Assert.Equal(5, result.Items.Count);
    //    }

    //    [Fact]
    //    public async Task CreateAsync_ShouldThrowException_WhenCodeExists()
    //    {
    //        var input = _fixture.Create<CreateUpdateProvinceDto>();
    //        input.Code = 123;

    //        var newProvince = new Province
    //        {
    //            Code = input.Code,
    //            ProvinceName = input.ProvinceName,
    //            ProvinceLevel = ProvinceLevel.Undefined
    //        };

    //        _mockRepository.Setup(repo => repo.InsertAsync(newProvince, true, default))
    //            .ReturnsAsync(newProvince);
    //        _mockMapper
    //            .Setup(mapper => mapper.Map<CreateUpdateProvinceDto, Province>(input))
    //            .Returns(newProvince);


    //        // Act & Assert
    //        await Assert.ThrowsAsync<BusinessException>(() => _provinceService.CreateAsync(input));

    //        //var res = await _provinceService.CreateAsync(input);

    //        //res.ShouldNotBeNull();

    //    }
    //}
}
