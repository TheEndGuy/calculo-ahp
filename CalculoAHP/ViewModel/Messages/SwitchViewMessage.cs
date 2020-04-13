using CalculoAHP.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.ViewModel.Messages
{
    public class SwitchViewMessage
    {
        public RegistroModeloEnum ModeloViewModel
        {
            get;
            set;
        }

        public RegistroLancamentoEnum LancamentoViewModel
        {
            get;
            set;
        }
    }
}
