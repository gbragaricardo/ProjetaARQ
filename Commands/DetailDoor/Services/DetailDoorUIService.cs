using ProjetaARQ.Commands.DetailDoor.Views;
using ProjetaARQ.Core.UI;

namespace ProjetaARQ.Commands.DetailDoor.Services
{
    public class DetailDoorUIService : IUIService<IDetailDoorResult>
    {
        private readonly DetailDoorView _detailDoorview;
        public DetailDoorUIService(DetailDoorView detailDoorView)
        {
            _detailDoorview = detailDoorView;
        }
        public IDetailDoorResult ShowDialog()
        {

            if (_detailDoorview.ShowDialog() == true)
                return (IDetailDoorResult)_detailDoorview.DataContext;
            
            return null;
        }
    }
}
