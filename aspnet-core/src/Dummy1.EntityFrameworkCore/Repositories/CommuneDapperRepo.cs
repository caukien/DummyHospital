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
    public class CommuneDapperRepo : DapperRepository<Dummy1DbContext>, ICommuneRepo<Commune>
    {
        public CommuneDapperRepo(IDbContextProvider<Dummy1DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Commune>> GetAll(QueryObject input)
        {
            var dbConnection = await GetDbConnectionAsync();

            var orderByClause = string.IsNullOrWhiteSpace(input.Sorting) ? "ORDER BY CommuneCode" : $"ORDER BY {input.Sorting}";
            orderByClause += input.IsAscending ? " ASC" : " DESC";

            // Xây dựng phân trang SQL
            var limitClause = $"LIMIT {input.MaxResultCount} OFFSET {input.SkipCount}";

            // Kết hợp các phần trên trong query
            var sqlQuery = $@"
                SELECT Id, CommuneCode, CommuneName, CommuneEnName, Communelevel, DistrictCode, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeletionTime
                FROM Communes
                {orderByClause}
                {limitClause}
            ";

            // Thực thi truy vấn
            return (await dbConnection.QueryAsync<Commune>(
                sqlQuery,
                transaction: await GetDbTransactionAsync())
            ).ToList();
        }

        public async Task<List<Commune>> GetCommuneByDistrictCode(int districtCode)
        {
            var dbConnection = await GetDbConnectionAsync();

            var orderByClause = "ORDER BY CommuneCode";

            // Kết hợp các phần trên trong query
            var sqlQuery = $@"
                SELECT Id, CommuneCode, CommuneName, CommuneEnName, Communelevel, DistrictCode
                FROM Communes
                WHERE DistrictCode = @DistrictCode
                {orderByClause}
            ";

            // Thực thi truy vấn
            return (await dbConnection.QueryAsync<Commune>(
                sqlQuery,
                new { DistrictCode = districtCode },
                transaction: await GetDbTransactionAsync())
            ).ToList();
        }

        public Task<Commune> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalCount()
        {
            var dbConnection = await GetDbConnectionAsync();

            var sqlQuery = $@"
                SELECT COUNT(*)
                FROM Communes
            ";

            var totalCount = await dbConnection.ExecuteScalarAsync<int>(
                sqlQuery,
                transaction: await GetDbTransactionAsync()
            );

            return totalCount;
        }
    }
}
