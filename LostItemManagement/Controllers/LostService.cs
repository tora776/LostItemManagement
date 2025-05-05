using LostItemManagement.Models;

namespace LostItemManagement.Controllers
{
    public class LostService
    {
        private readonly LostRepository _repository;

        public LostService(LostRepository repository)
        {
            _repository = repository;
        }

        public List<Lost> SelectLostService(string lostItem, string lostPlace, string lostDetailedPlace)
        {
            return _repository.SelectLostRepository(lostItem, lostPlace, lostDetailedPlace);
        }
        public void InsertLostService(Lost lost)
        {
            int maxLostId = _repository.GetMaxLostId();
            if (maxLostId == 0)
            {
                // TODO:LostIdが0の場合例外処理を作成する
            }
            else
            {
                maxLostId++;
                _repository.InsertLostRepository(lost, maxLostId);
            }
        }
        public void UpdateLostService(Lost lost)
        {
            _repository.UpdateLostRepository(lost);
        }
        public void DeleteLostService(int id)
        {
            _repository.DeleteLostRepository(id);
        }
    }
}
