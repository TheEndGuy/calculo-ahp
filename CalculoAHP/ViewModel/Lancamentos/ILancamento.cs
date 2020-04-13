using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.ViewModel.Lancamentos
{
    public interface ILancamento
    {
        void Save(Lancamento lancamento);

        void Load(Lancamento lancamento);
    }
}
