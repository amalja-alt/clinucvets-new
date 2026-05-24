using ClinicVets.Models;

namespace ClinicVets.Repositories;

public interface IVisitRepository
{
    Visit Add(Visit visit);
}
