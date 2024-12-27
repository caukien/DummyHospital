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
    public class PantientRepo : DapperRepository<Dummy1DbContext>, IPatientRepo<Patient>
    {
        public PantientRepo(IDbContextProvider<Dummy1DbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Patient>> GetAll(QueryObject input)
        {
            var dbConnection = await GetDbConnectionAsync();

            var orderByClause = string.IsNullOrWhiteSpace(input.Sorting) ? "ORDER BY PatientName" : $"ORDER BY {input.Sorting}";
            orderByClause += input.IsAscending ? " ASC" : " DESC";

            var limitClause = $"LIMIT {input.MaxResultCount} OFFSET {input.SkipCount}";

            var sqlQuery = $@"
            SELECT Id, Code, Name, ProvinceCode, DistrictCode, CommuneCode, Address, HospitalId, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeletionTime
                FROM Patients
                {orderByClause}
                {limitClause}";

            return (await dbConnection.QueryAsync<Patient>(
                sqlQuery,
                transaction: await GetDbTransactionAsync())
            ).ToList();
        }

        public Task<Patient> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Patient>> GetPatientByHospitalCode(QueryObject input, Guid hospitalCode)
        {
            var dbConnection = await GetDbConnectionAsync();

            var sqlQuery = @"
            SELECT Id, Code, Name, ProvinceCode, DistrictCode, CommuneCode, Address, HospitalId, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeletionTime
            FROM Patients where HospitalId = @HospitalId";

            if (input.MaxResultCount > 0)
            {
                sqlQuery += " ORDER BY Name LIMIT @MaxResultCount OFFSET @SkipCount";
            }

            var parameters = new
            {
                HospitalId = hospitalCode,
                SkipCount = input.SkipCount,
                MaxResultCount = input.MaxResultCount
            };

            var patients = await dbConnection.QueryAsync<Patient>(sqlQuery, parameters);
            return patients.ToList();
        }

        public async Task<int> GetTotalCount()
        {
            var dbConnection = await GetDbConnectionAsync();

            var sqlQuery = $@"
                SELECT COUNT(*)
                FROM Patients
            ";

            var totalCount = await dbConnection.ExecuteScalarAsync<int>(
                sqlQuery,
                transaction: await GetDbTransactionAsync()
            );

            return totalCount;
        }
    }
}
