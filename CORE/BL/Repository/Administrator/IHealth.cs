using BeicipFranLabERP.ViewModel.HealthCheckup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.BL.Repository.Administrator
{
    public interface IHealth
    {
        Task<List<GetHealthCheckUpVM>> GetBrokenLinks(int id);
        Task<List<GetHealthCheckUpVM>> GetSpecialCharacters(int id);
        Task<List<GetHealthCheckUpVM>> GetDocumentWithoutContent(int id);
        Task<List<GetHealthCheckUpVM>> GetDuplicateDocuments(int id);
        Task<List<GetHealthCheckUpVM>> GetTooLongPaths(int id);
    }
}
