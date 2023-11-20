using Entities;

namespace Businnes.Interfaces
{
    public interface IToDoBusiness
    {
        bool Insert(TaskToDo todo);
        bool Delete(int id);
        bool Update(TaskToDo todo);
        IEnumerable<TaskToDo> Get();
        IEnumerable<TaskToDo> GetById(int id);
    }
}
