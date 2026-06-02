using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ProjetaARQ.Commands.WordExport.MVVM.Views
{
    public partial class RuleCard : UserControl
    {
        public RuleCard()
        {
            InitializeComponent(); 
        }

        //private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    // 'this' se refere à instância do RuleCard que está sendo arrastada
        //    double newHeight = this.ContentBorder.ActualHeight + e.VerticalChange;
        //    if (newHeight >= this.ContentBorder.MinHeight && newHeight <= this.ContentBorder.MaxHeight)
        //    {
        //        this.ContentBorder.Height = newHeight;
        //    }
        //}
    }
}