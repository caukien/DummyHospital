using Dapper;
using Dummy1.EntityFrameworkCore;
using Dummy1.Helper;
using Dummy1.IRepositories;
using Dummy1.Migrations;
using Dummy1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Dummy1.Repositories
{
    public class DistrictDapperRepo : DapperRepository<Dummy1DbContext>, IDistrictRepo<District>
    {
        public DistrictDapperRepo(IDbContextProvider<Dummy1DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<District>> GetAll(QueryObject input)
        {
            var dbConnection = await GetDbConnectionAsync();

            var orderByClause = string.IsNullOrWhiteSpace(input.Sorting) ? "ORDER BY Code" : $"ORDER BY {input.Sorting}";
            orderByClause += input.IsAscending ? " ASC" : " DESC";

            // Xây dựng phân trang SQL
            var limitClause = $"LIMIT {input.MaxResultCount} OFFSET {input.SkipCount}";

            // Kết hợp các phần trên trong query
            var sqlQuery = $@"
                SELECT Id, Code, DistrictName, DistrictEnName, Districtlevel, ProvinceCode, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeletionTime
                FROM Districts
                {orderByClause}
                {limitClause}
            ";

            // Thực thi truy vấn
            return (await dbConnection.QueryAsync<District>(
                sqlQuery,
                transaction: await GetDbTransactionAsync())
            ).ToList();
        }

        public async Task<List<District>> GetDistrictByProvinceCode(int provinceCode)
        {
            var dbConnection = await GetDbConnectionAsync();

            var orderByClause = "ORDER BY Code";

            // Kết hợp các phần trên trong query
            var sqlQuery = $@"
                SELECT Id, Code, DistrictName, DistrictEnName, Districtlevel, ProvinceCode
                FROM Districts
                WHERE ProvinceCode = @ProvinceCode
                {orderByClause}
            ";

            // Thực thi truy vấn
            return (await dbConnection.QueryAsync<District>(
                sqlQuery,
                new { ProvinceCode = provinceCode },
                transaction: await GetDbTransactionAsync())
            ).ToList();
        }

        public Task<District> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalCount()
        {
            var dbConnection = await GetDbConnectionAsync();

            var sqlQuery = $@"
                SELECT COUNT(*)
                FROM Districts
            ";

            var totalCount = await dbConnection.ExecuteScalarAsync<int>(
                sqlQuery,
                transaction: await GetDbTransactionAsync()
            );

            return totalCount;
        }
    }
}
