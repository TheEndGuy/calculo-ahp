using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.ViewModel.Modelos
{
    public interface IModel
    {
        void Save(Modelo model);

        void Load(Modelo model);
    }
}
