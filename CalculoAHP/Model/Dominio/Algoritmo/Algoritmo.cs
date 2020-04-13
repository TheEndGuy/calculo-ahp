using CalculoAHP.Domain;
using CalculoAHP.Model.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Algoritmo
{
    public class Algoritmo
    {
        private Algoritmo()
        {
        }

        private static Algoritmo m_instance;

        public static Algoritmo Instance
        {
            get { return m_instance ?? (m_instance = new Algoritmo()); }
            set { m_instance = value; }
        }

        public void CalcularDecisao(Lancamento lancamentoDecisao)
        {
            // matriz dividida
            double[,] m_matriz = CalcularMatrizDividida(lancamentoDecisao.Modelo);

            // peso dos critérios
            Dictionary<string, double> m_pesoCriterios = new Dictionary<string, double>();

            // estruturação dos pesos dos critérios
            for (int i = 0; i < lancamentoDecisao.Modelo.Criterios.Count; i++)
            {
                double somaLinha = 0;

                // soma da linha
                for (int j = 0; j < lancamentoDecisao.Modelo.Criterios.Count; j++)
                    somaLinha += m_matriz[j, i];

                m_pesoCriterios.Add(lancamentoDecisao.Modelo.Criterios[i].Nome, (double)(somaLinha/ lancamentoDecisao.Modelo.Criterios.Count));
            }

            lancamentoDecisao.ItensDecisao.Clear();

            foreach (var elemento in lancamentoDecisao.Elementos)
                lancamentoDecisao.ItensDecisao.Add(new ItemDecisao(elemento.Nome, elemento.Criterios.Select(entry => (double)(entry.Importancia * m_pesoCriterios[entry.Criterio.Nome])).Sum()));
        }

        /// <summary>
        /// Calcula a matriz dividida, retornando uma matriz contendo a divisão de cada coluna pela soma do peso das colunas da matriz de julgamento
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        private double[,] CalcularMatrizDividida(Modelo modelo)
        {
            double[,] m_matriz = new double[modelo.Criterios.Count, modelo.Criterios.Count];
            double[,] m_matrizJulgamento = CalcularMatrizJulgamento(modelo);

            for (int i = 0; i < modelo.Criterios.Count; i ++)
            {
                double somaColuna = 0;

                // soma da coluna
                for (int j = 0; j < modelo.Criterios.Count; j++)
                    somaColuna += m_matrizJulgamento[j, i];

                // divisão de cada valor da coluna pela soma
                for (int j = 0; j < modelo.Criterios.Count; j++)
                    m_matriz[i, j] = (double)(m_matrizJulgamento[j, i] / somaColuna);
            }

            return m_matriz;
        }

        /// <summary>
        /// Calcula a matriz de julgamento, retornando uma matriz contendo a importância de cada critério em relação aos demais
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        private double[,] CalcularMatrizJulgamento(Modelo modelo)
        {
            double[,] m_matriz = new double[modelo.Criterios.Count, modelo.Criterios.Count];

            for(int i = 0; i < modelo.Criterios.Count; i++)
            {
                for(int j = 0; j < modelo.Criterios.Count; j++)
                {
                    // critério igual (linha - coluna)
                    if(i == j)
                    {
                        m_matriz[i, j] = 1;
                        continue;
                    }

                    if (m_matriz[i, j] > 0)
                        continue;

                    var criterioSet = modelo.CriteriosItems.FirstOrDefault(entry => entry.Equals(new CriterioItem(modelo.Criterios[i], modelo.Criterios[j], 1)));

                    if (criterioSet == null)
                        continue;

                    int selecionadoIndex = modelo.Criterios.IndexOf(modelo.Criterios.FirstOrDefault(entry => entry.Nome.Equals(criterioSet.CriterioSelecionado.Nome)));
                    int opcaoIndex = modelo.Criterios.IndexOf(modelo.Criterios.FirstOrDefault(entry => entry.Nome.Equals(criterioSet.CriterioOpcao.Nome)));

                    m_matriz[selecionadoIndex, opcaoIndex] = criterioSet.Importancia;
                    m_matriz[opcaoIndex, selecionadoIndex] = (double)(1.0/criterioSet.Importancia);
                }
            }

            return m_matriz;
        }
    }
}
