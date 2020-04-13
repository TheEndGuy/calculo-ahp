using CalculoAHP.Domain;
using CalculoAHP.Model.Modelos.Registro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Dominio
{
    public class CriterioItem
    {
        [JsonConstructor]
        public CriterioItem(Criterio criterioSelecionado, Criterio criterioOpcao, int importancia)
        {
            CriterioSelecionado = criterioSelecionado;
            CriterioOpcao = criterioOpcao;
            Importancia = importancia;
        }

        public Criterio CriterioSelecionado
        {
            get;
            set;
        }

        public Criterio CriterioOpcao
        {
            get;
            set;
        }

        public int Importancia
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (!(obj is CriterioItem) && !(obj is CriterioChoiceItem))
                return false;

            if (obj is CriterioItem)
            {
                if ((obj as CriterioItem).CriterioSelecionado.Nome.Equals(CriterioOpcao.Nome) && (obj as CriterioItem).CriterioOpcao.Nome.Equals(CriterioSelecionado.Nome))
                    return true;

                if ((obj as CriterioItem).CriterioOpcao.Nome.Equals(CriterioOpcao.Nome) && (obj as CriterioItem).CriterioSelecionado.Nome.Equals(CriterioSelecionado.Nome))
                    return true;
            }

            else
            {
                if ((obj as CriterioChoiceItem).PrimeiroCriterio.Equals(CriterioOpcao.Nome) && (obj as CriterioChoiceItem).SegundoCriterio.Equals(CriterioSelecionado.Nome))
                    return true;

                if ((obj as CriterioChoiceItem).SegundoCriterio.Equals(CriterioOpcao.Nome) && (obj as CriterioChoiceItem).PrimeiroCriterio.Equals(CriterioSelecionado.Nome))
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
