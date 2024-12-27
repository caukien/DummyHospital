using Dapper;
using Dummy1.EntityFrameworkCore;
using Dummy1.Helper;
using Dummy1.IRepositories;
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
    public class ProvinceDapperRepo : DapperRepository<Dummy1DbContext>, IDapperBasicRepo<Province>
    {
        public ProvinceDapperRepo(IDbContextProvider<Dummy1DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<List<Province>> GetAll(QueryObject input)
        {

            var dbConnection = await GetDbConnectionAsync();

            var orderByClause = string.IsNullOrWhiteSpace(input.Sorting) ? "ORDER BY Code" : $"ORDER BY {input.Sorting}";
            orderByClause += input.IsAscending ? " ASC" : " DESC";

            // Xây dựng phân trang SQL
            var limitClause = $"LIMIT {input.MaxResultCount} OFFSET {input.SkipCount}";

            // Kết hợp các phần trên trong query
            var sqlQuery = $@"
                SELECT id, Code, ProvinceName, ProvinceEnName, ProvinceLevel, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeletionTime
                FROM Provinces
                {orderByClause}
                {limitClause}
            ";

            // Thực thi truy vấn
            return (await dbConnection.QueryAsync<Province>(
                sqlQuery,
                transaction: await GetDbTransactionAsync())
            ).ToList();
        }

        public async Task<Province> GetOne(Guid id)
        {
            var dbConnection = await GetDbConnectionAsync();

            var sqlQuery = $@"
                SELECT id, Code, ProvinceName, ProvinceEnName, ProvinceLevel, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeletionTime
                FROM Provinces
                WHERE Id = @Id
            ";
            var province = await dbConnection.QueryFirstOrDefaultAsync<Province>(
                sqlQuery,
                new { Id = id },
                transaction: await GetDbTransactionAsync()
            );

            return province!;

        }

        public async Task<int> GetTotalCount()
        {
            var dbConnection = await GetDbConnectionAsync();

            var sqlQuery = $@"
                SELECT COUNT(*)
                FROM Provinces
            ";

            var totalCount = await dbConnection.ExecuteScalarAsync<int>(
                sqlQuery,
                transaction: await GetDbTransactionAsync()
            );

            return totalCount;
        }
    }
}
