using Dummy1.Books;
using Dummy1.Helper;
using Dummy1.Model;
using Dummy1.ProvinceDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Dummy1.Interfaces
{
    //public interface IProvinceService : ITransientDependency
    //{
    //    Task<List<Province>> GetAllProvince();
    //    Task<Province> GetOne();
    //    Task Create(ProvinceDto province);
    //    Task Update(ProvinceDto province);
    //    Task Delete(string id);
    //    Task UploadFile(IFormFile file);
    //    Task CreateTemp(Province province);
    //}

    public interface IProvinceService :
    ICrudAppService< //Defines CRUD methods
        ProvinceDto, //Used to show books
        Guid, //Primary key of the book entity
        QueryObject, //Used for paging/sorting
        CreateUpdateProvinceDto> //Used to create/update a book
    {
        Task ImportFile(IFormFile file);
    }
}
