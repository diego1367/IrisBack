namespace Businnes
{
    using Businnes.Interfaces;
    using Entities;
    using Repository;
    public class TodoBusiness : IToDoBusiness
    {
        private IUnitOfWork _unit;
        public TodoBusiness(IUnitOfWork unit)
        {
            this._unit = unit;
        }
        public bool Update(TaskToDo toDo)
        {
            this._unit.GenericRepository<TaskToDo>().Update(toDo);
            return true;
        }

        public bool Delete(int id)
        {
            this._unit.GenericRepository<TaskToDo>().Delete(id);
            return true;
        }

        public IEnumerable<TaskToDo> GetById(int id)
        {
            return _unit.GenericRepository<TaskToDo>().Get(x => x.Id == id).AsEnumerable();
        }

        public bool Insert(TaskToDo toDo)
        {
            this._unit.GenericRepository<TaskToDo>().Insert(toDo);
            return true;
        }

        public IEnumerable<TaskToDo> Get()
        {
            return this._unit.GenericRepository<TaskToDo>().Get();
        }
    }
}