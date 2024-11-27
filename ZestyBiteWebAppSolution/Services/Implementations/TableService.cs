using Microsoft.AspNetCore.Identity;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _repository;

        public TableService(ITableRepository tableRepository)
        {
            _repository = tableRepository;
        }
        public async Task<TableDTO> CreateTableAsync(TableDTO dto)
        {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto), "Table cannot be null");
            }
        }
    }
}
