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
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Dummy1.Repositories
{
    public class HospitalDapperRepo : DapperRepository<Dummy1DbContext>, IHospitalRepo<Hospital>, ITransientDependency
    {
        public HospitalDapperRepo(IDbContextProvider<Dummy1DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Hospital>> GetAll(QueryObject input)
        {
            var connection = await GetDbConnectionAsync();  // Get the DB connection
            var transaction = await GetDbTransactionAsync(); // Get the DB transaction

            // Kiểm tra và sử dụng thuộc tính Sorting nếu có giá trị
            var orderByClause = string.IsNullOrWhiteSpace(input.Sorting) ? "ORDER BY HospitalCode" : $"ORDER BY {input.Sorting}";
            orderByClause += input.IsAscending ? " ASC" : " DESC";

            // Start building the SQL query for MySQL (no need for TenantId)
            var sqlQuery = $@"
                SELECT Id, HospitalCode, HospitalName, ProvinceCode, DistrictCode, Address, CommuneCode, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeletionTime
                FROM Hospitals
                WHERE TenantId = @TenantId
                {orderByClause}";

            // Pagination logic for MySQL (using LIMIT and OFFSET)
            if (input.MaxResultCount > 0)
            {
                sqlQuery += " LIMIT @MaxResultCount OFFSET @SkipCount";
            }

            // Prepare parameters for the query
            var parameters = new
            {
                TenantId = CurrentTenant.Id,
                SkipCount = input.SkipCount,    // The number of rows to skip (pagination)
                MaxResultCount = input.MaxResultCount  // The max number of rows to return
            };

            // Execute the query and return the result
            var hospitals = await connection.QueryAsync<Hospital>(sqlQuery, parameters, transaction: transaction);

            return hospitals.ToList();
        }

        public async Task<Hospital> GetHospitalByTenant()
        {
            var dbConnection = await GetDbConnectionAsync();

            var sqlQuery = "SELECT Id, HospitalCode, HospitalName, ProvinceCode, DistrictCode, Address, CommuneCode, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeletionTime " +
               "FROM Hospitals where TenantId = @TenantId";
            var hospital = await dbConnection.QueryFirstOrDefaultAsync<Hospital>(
                sqlQuery,
                new { TenantId = CurrentTenant.Id },
                transaction: await GetDbTransactionAsync()
            );

            return hospital!;
        }

        public Task<Hospital> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalCount()
        {
            var dbConnection = await GetDbConnectionAsync();

            var sqlQuery = $@"
                SELECT COUNT(*)
                FROM Hospitals
            ";

            var totalCount = await dbConnection.ExecuteScalarAsync<int>(
                sqlQuery,
                transaction: await GetDbTransactionAsync()
            );

            return totalCount;
        }
    }
}
